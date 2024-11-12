using Dashboard.DAL.Models.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Dashboard.DAL.Data.Initializer
{
    public static class DataSeeder
    {
        public static async void SeedData(this IApplicationBuilder builder)
        {
            using (var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

                if (!roleManager.Roles.Any())
                {
                    var adminRole = new Role
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = Settings.AdminRole
                    };

                    var userRole = new Role
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = Settings.UserRole
                    };

                    await roleManager.CreateAsync(adminRole);
                    await roleManager.CreateAsync(userRole);
                }

                if (!userManager.Users.Any())
                {
                    var admin = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = "admin@gmail.com",
                        UserName = "admin",
                        EmailConfirmed = true,
                        FirstName = "Admin",
                        LastName = "Dashboard"
                    };

                    var user = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = "user@gmail.com",
                        UserName = "user",
                        EmailConfirmed = true,
                        FirstName = "User",
                        LastName = "Dashboard"
                    };

                    var user2 = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = "user2@gmail.com",
                        UserName = "galactic_warrior",
                        EmailConfirmed = true,
                        FirstName = "Leia",
                        LastName = "Organa"
                    };

                    var user3 = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = "user3@gmail.com",
                        UserName = "iron_man",
                        EmailConfirmed = true,
                        FirstName = "Tony",
                        LastName = "Stark"
                    };

                    var user4 = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = "user4@gmail.com",
                        UserName = "captain_marvel",
                        EmailConfirmed = true,
                        FirstName = "Carol",
                        LastName = "Danvers"
                    };

                    await userManager.CreateAsync(admin);
                    await userManager.CreateAsync(user);
                    await userManager.CreateAsync(user2);
                    await userManager.CreateAsync(user3);
                    await userManager.CreateAsync(user4);

                    await userManager.AddToRoleAsync(admin, Settings.AdminRole);
                    await userManager.AddToRoleAsync(user, Settings.UserRole);
                    await userManager.AddToRoleAsync(user2, Settings.UserRole);
                    await userManager.AddToRoleAsync(user3, Settings.UserRole);
                    await userManager.AddToRoleAsync(user4, Settings.UserRole);
                }
            }
        }
    }
}
