using BeastsGym.DAL.Contexts;
using BeastsGym.DAL.DataSeeds;
using BeastsGym.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BeastsGym
{
    public static class ProgramExtentions
    {
        public static async Task MigrateAndSeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<BeastsGymDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var Configurations = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var PendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

            if (PendingMigrations.Any())
            {
                logger.LogInformation($"Applying {PendingMigrations.Count()} Pending Migrations...");
                await dbContext.Database.MigrateAsync();
                logger.LogInformation("Migrations Applied Successfully");
            }
            else
            {
                logger.LogInformation("No Pending Migrations Found");
            }

            //Seed
            var seedPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeed.SeedAsync(dbContext, seedPath, logger);
            await IdentityDataSeed.SeedAsync(RoleManager, UserManager, logger);
        }
    }
}