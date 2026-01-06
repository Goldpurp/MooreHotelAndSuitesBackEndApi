using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MooreHotelAndSuites.Domain.Entities; 

using System.Linq;

namespace MooreHotelAndSuites.Infrastructure.Identity
{
    public static class IdentitySeeder
    {
        // Default roles
        private static readonly string[] Roles = new[] { "Admin", "Manager", "Receptionist" };

        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            // Resolve required services for ApplicationUser
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var config = serviceProvider.GetRequiredService<IConfiguration>();

            // Get admin credentials from configuration
            var adminSection = config.GetSection("AdminSeed");
            var adminUsername = adminSection.GetValue<string>("Username") ?? "MooreHotel_Admin";
            var adminEmail = adminSection.GetValue<string>("Email") ?? "admin@moorehotel.local";
            var adminPassword = adminSection.GetValue<string>("Password") ?? "Moore123!";

            // Create roles if they don't exist
            foreach (var role in Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Check if admin user exists
            var adminUser = await userManager.FindByNameAsync(adminUsername)
                            ?? await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                // Create admin user as ApplicationUser
                adminUser = new ApplicationUser
                {
                    FullName = "Moore Hotel Administrator",
                    UserName = adminUsername,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create admin user: {errors}");
                }
            }
        }
    }
}
