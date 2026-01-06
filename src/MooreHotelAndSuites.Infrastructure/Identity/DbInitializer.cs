using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MooreHotelAndSuites.Infrastructure.Data;

namespace MooreHotelAndSuites.Infrastructure.Identity
{
    public static class DbInitializer
    {
        public static async Task Initialize(AppDbContext context, IServiceProvider serviceProvider)
        {
            context.Database.EnsureCreated(); // Or use context.Database.Migrate();

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = new[] { "Admin", "Staff", "Guest" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
