using Microsoft.AspNetCore.Identity;
using MooreHotelAndSuites.Domain.Constants;

namespace MooreHotelAndSuites.Infrastructure.Identity
{
    public static class RoleSeeder
{
    public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
    {
        foreach (var role in Roles.AllRoles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
}
