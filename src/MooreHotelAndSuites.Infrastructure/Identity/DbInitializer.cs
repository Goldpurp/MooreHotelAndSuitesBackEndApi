using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MooreHotelAndSuites.Infrastructure.Data;

namespace MooreHotelAndSuites.Infrastructure.Identity
{
   public static class DbInitializer
{
    public static async Task Initialize(
        AppDbContext context)
    {
      await context.Database.MigrateAsync();

    }
}

}
