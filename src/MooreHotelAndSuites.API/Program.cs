using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MooreHotelAndSuites.Infrastructure.Data;
using MooreHotelAndSuites.Infrastructure.Identity;
using MooreHotelAndSuites.Infrastructure.Auth;
using MooreHotelAndSuites.Application.Services;
using MooreHotelAndSuites.Infrastructure.Repositories;
using MooreHotelAndSuites.Domain.Interfaces;
using MooreHotelAndSuites.API.Mappings;
using Microsoft.OpenApi.Models;
using MooreHotelAndSuites.Domain.Entities; // 👈 THIS IS REQUIRED
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

// Add controllers and AutoMapper
services.AddControllers();
services.AddAutoMapper(typeof(AutoMapperProfile));

// Configure EF Core with PostgreSQL
services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// JWT Authentication
var jwtKey = configuration.GetValue<string>("Jwt:Key") ?? "ReplaceWithASecretKeyOfAtLeast32Chars!";
var key = Encoding.UTF8.GetBytes(jwtKey);

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true
    };
});

// Register repositories
services.AddScoped<IHotelRepository, HotelRepository>();
services.AddScoped<IRoomRepository, RoomRepository>();
services.AddScoped<IBookingRepository, BookingRepository>();
services.AddScoped<IGuestRepository, GuestRepository>();

// Register services
services.AddScoped<HotelService>();
services.AddScoped<RoomService>();
services.AddScoped<BookingService>();
services.AddScoped<GuestService>();
services.AddScoped<IJwtTokenService, JwtTokenService>();

// Swagger setup
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MooreHotelAndSuites.API", Version = "v1" });

    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'"
    };

    c.AddSecurityDefinition("bearer", jwtScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtScheme, new string[] { } } });
});

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;
    var db = sp.GetRequiredService<AppDbContext>();

    await DbInitializer.Initialize(db, sp);   // Seed roles
    await IdentitySeeder.SeedAsync(sp);       // Seed default users
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MooreHotelAndSuites.API v1"));
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
