using Microsoft.AspNetCore.Identity;

namespace ShopApp.WebUI.Identity
{
    public static class SeedIdentity
    {
        public static async Task Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if(await userManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));

                ApplicationUser user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin@shopapp.com";
                user.EmailConfirmed = true;
                user.FullName = "Admin User";

                var result = await userManager.CreateAsync(user, "Admin1234**");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "admin");
                }
            }
           
        }
    }
}
