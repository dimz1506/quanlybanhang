using Microsoft.AspNetCore.Identity;
using MymvcApp.Models;
using System.Threading.Tasks;


namespace MymvcApp.Data
{
         public class DbSeeder
         {
                  public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
                  {
                           //tao role neu chua co 
                           if (!await roleManager.RoleExistsAsync("Admin"))
                           {
                                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                           }
                           if (!await roleManager.RoleExistsAsync("User"))
                           {
                                    await roleManager.CreateAsync(new IdentityRole("User"));
                           }

                           //tao tai khoan admin mac dinh
                           var adminEmail = "admin@gmail.com";
                           var adminUser = await userManager.FindByEmailAsync(adminEmail);

                           if (adminUser == null)
                           {
                                    adminUser = new ApplicationUser
                                    {
                                             UserName = "admin",
                                             Email = adminEmail,
                                             FullName = "Administrator",
                                             EmailConfirmed = true
                                    };
                                    var result = await userManager.CreateAsync(adminUser, "Admin@123");
                                    if (result.Succeeded)
                                    {
                                             await userManager.AddToRoleAsync(adminUser, "Admin");
                                    }
                           }
                  }
         }
}