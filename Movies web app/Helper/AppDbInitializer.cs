using Microsoft.AspNetCore.Identity;

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
    }
}
