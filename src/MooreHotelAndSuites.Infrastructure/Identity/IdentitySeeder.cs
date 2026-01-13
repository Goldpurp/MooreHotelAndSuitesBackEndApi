using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MooreHotelAndSuites.Domain.Entities; 

using System.Linq;

namespace MooreHotelAndSuites.Infrastructure.Identity
{
   public static class IdentitySeeder
{
    private static readonly string[] Roles =
        { "Admin", "Manager", "Receptionist" };

    public static async Task SeedAsync(IServiceProvider sp)
    {
        var userManager = sp.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = sp.GetRequiredService<RoleManager<IdentityRole>>();
        var config = sp.GetRequiredService<IConfiguration>();

        foreach (var role in Roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminSection = config.GetSection("AdminSeed");
        var adminUsername = adminSection.GetValue<string>("Username")
            ?? throw new InvalidOperationException("AdminSeed:Username missing");
        var adminEmail = adminSection.GetValue<string>("Email")
            ?? throw new InvalidOperationException("AdminSeed:Email missing");
        var adminPassword = adminSection.GetValue<string>("Password")
            ?? throw new InvalidOperationException("AdminSeed:Password missing");

        var adminUser =
            await userManager.FindByNameAsync(adminUsername)
            ?? await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                FullName = "Moore Hotel Administrator",
                UserName = adminUsername,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ",
                    result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
        }

        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

}
