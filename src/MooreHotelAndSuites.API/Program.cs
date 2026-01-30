using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MooreHotelAndSuites.API.Mappings;
using MooreHotelAndSuites.Application.Interfaces.Repositories;
using MooreHotelAndSuites.Application.Interfaces.Services;
using MooreHotelAndSuites.Application.Interfaces.Auditing;
using MooreHotelAndSuites.Application.Services;
using MooreHotelAndSuites.Infrastructure.Data;
using MooreHotelAndSuites.Infrastructure.Identity;
using MooreHotelAndSuites.Infrastructure.Persistence.Repositories;
using MooreHotelAndSuites.Infrastructure.Auth;
using MooreHotelAndSuites.Infrastructure.Auditing;
using MooreHotelAndSuites.Domain.Abstractions;
using MooreHotelAndSuites.Domain.Events;
using MooreHotelAndSuites.Application.Interfaces.Events;
using MooreHotelAndSuites.Infrastructure.Events;







using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;


services.AddControllers();
services.AddAutoMapper(typeof(AutoMapperProfile));


services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Moore Hotel & Suites API",
        Version = "v1"
    });

   
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token like this: Bearer {your token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));


services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();
 
services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});


var jwtSettings = configuration.GetSection("Jwt");

var jwtKey = jwtSettings.GetValue<string>("Key")
    ?? throw new InvalidOperationException("JWT Key is missing");

var key = Encoding.UTF8.GetBytes(jwtKey);

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),

        ClockSkew = TimeSpan.Zero
    };
});





services.AddScoped<IHotelRepository, HotelRepository>();
services.AddScoped<IRoomRepository, RoomRepository>();
services.AddScoped<IBookingRepository, BookingRepository>();
services.AddScoped<IGuestRepository, GuestRepository>();
services.AddScoped<IAuditLogWriter, AuditLogWriter>();
services.AddScoped<IAuditAnalyticsRepository, AuditAnalyticsRepository>();
services.AddScoped<IBookingReadRepository, BookingReadRepository>();
services.AddScoped<IAuditLogReadRepository, AuditLogReadRepository>();



services.AddScoped<IRoomCommandService, RoomCommandService>();
services.AddScoped<IRoomQueryService, RoomQueryService>();
services.AddScoped<IHotelService, HotelService>();
services.AddScoped<IBookingService, BookingService>();
services.AddScoped<IGuestService, GuestService>();
services.AddScoped<IJwtTokenService, JwtTokenService>();
services.AddScoped<IImageStorageService, CloudinaryImageStorageService>();
services.AddScoped<IUserManagementService, UserManagementService>();
services.AddScoped<IAuditAnalyticsService, AuditAnalyticsService>();
services.AddScoped<IOperationsService, OperationsService>();
services.AddScoped<
    IDomainEventHandler<BookingCreatedEvent>,
    BookingCreatedAuditHandler>();
services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;
    var db = sp.GetRequiredService<AppDbContext>();

    await DbInitializer.Initialize(db);
    await IdentitySeeder.SeedAsync(sp);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Moore Hotel & Suites API v1");
        c.RoutePrefix = "swagger";
    });
}



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<AuditLogMiddleware>();
app.MapControllers();

app.MapGet("/", () => "Moore Hotel & Suites API is running");

app.Run();
