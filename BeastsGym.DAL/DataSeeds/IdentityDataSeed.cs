using BeastsGym.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeastsGym.DAL.DataSeeds
{
    public static class IdentityDataSeed
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger logger, CancellationToken ct=default)
        {
            try
            {
                bool HasUsers = userManager.Users.Any();
                bool HasRoles = roleManager.Roles.Any();

                if (HasUsers && HasRoles) return;
                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>()
                    {
                        new IdentityRole{Name="SuperAdmin"},
                        new IdentityRole{Name="Admin"},
                    };
                    foreach (var role in Roles.Select(r => r.Name))
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            var RoleResult = await roleManager.CreateAsync(new IdentityRole(role));

                            if (!RoleResult.Succeeded)
                            {
                                logger.LogError($"Failed to create role: {role}.");
                            }
                        }
                    }

                }
                if (!HasUsers)
                {
                    var MainUser = new ApplicationUser()
                    {
                        FirstName = "Sherif",
                        LastName = "Shamel",
                        UserName = "SherifShamel",
                        Email = "sherif@gmail.com",
                        PhoneNumber = "01273216464"
                    };
                    var userResult = await userManager.CreateAsync(MainUser, "P@ssw0rd");
                    await userManager.AddToRoleAsync(MainUser, "SuperAdmin");

                    if (!userResult.Succeeded)
                    {
                        logger.LogError("Failed to seed users");
                        return;
                    }

                }
                return;

            }
            catch (Exception ex)
            {
                logger.LogError("Failed to seed identity data");
            }
        }
    }
}
