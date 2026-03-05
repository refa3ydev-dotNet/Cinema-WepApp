using Core.Entities;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace Movies_web_app.Helper
{
    public static class AppDbInitializer
    {
        public static async Task TaskSeedRoleAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync("Admin"))await roleManager.CreateAsync(new IdentityRole("Admin"));
                if (!await roleManager.RoleExistsAsync("CinemaAgent"))await roleManager.CreateAsync(new IdentityRole("CinemaAgent"));
                if (!await roleManager.RoleExistsAsync("Customer"))await roleManager.CreateAsync(new IdentityRole("Customer"));

            }
        }
    public static async Task TaskSeedAdminUserAsync(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var Config = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>();
                string adminEmail = Config["AdminSetup:Email"];
                string adminPassword = Config["AdminSetup:Password"];

                if(string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
                {
                    return;
                }
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        FirstName = "Super",
                        LastName = "Admin",
                        UserName = "admin_flickit",
                        Email = adminEmail,
                        EmailConfirmed = true,
                        IsApproved = true,
                        Gender = "Male",
                    };
                var result = await userManager.CreateAsync(newAdminUser, adminPassword);
                if (result.Succeeded)
                {
                                       await userManager.AddToRoleAsync(newAdminUser, "Admin");
                }
                }

            }
    }
    }
}
