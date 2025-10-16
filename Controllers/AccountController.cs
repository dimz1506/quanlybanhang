using Microsoft.AspNetCore.Mvc;
using MymvcApp.Models;
using Microsoft.AspNetCore.Identity;

namespace MymvcApp.Controllers
{
         public class AccountController : Controller
         {
                  private readonly UserManager<ApplicationUser> _userManager;
                  private readonly SignInManager<ApplicationUser> _signInManager;
                  private readonly RoleManager<IdentityRole> _roleManager;
                  public AccountController(UserManager<ApplicationUser> userManager,
                                           SignInManager<ApplicationUser> signInManager,
                                           RoleManager<IdentityRole> roleManager)
                  {
                           _userManager = userManager;
                           _signInManager = signInManager;
                           _roleManager = roleManager;
                  }

                  //get
                  [HttpGet]
                  public IActionResult Register()
                  {
                           return View();
                  }

                  //post
                  [HttpPost]
                  public async Task<IActionResult> Register(RegisterViewModel model)
                  {
                           if (ModelState.IsValid)
                           {
                                    var user = new ApplicationUser
                                    {
                                             UserName = model.Username,
                                             Email = model.Email,
                                             FullName = model.Fullname,
                                             Address = model.Address,
                                             DateOfBirth = model.DateOfBirth,
                                             PasswordHash = model.Password

                                    };

                                    var result = await _userManager.CreateAsync(user, model.Password);
                                    if (result.Succeeded)
                                    {
                                             // Assign "User" role to the newly registered user
                                             if (!await _roleManager.RoleExistsAsync("User"))
                                             {
                                                      await _roleManager.CreateAsync(new IdentityRole("User"));
                                             }
                                             await _userManager.AddToRoleAsync(user, "User");

                                             await _signInManager.SignInAsync(user, isPersistent: false);
                                             return RedirectToAction("Index", "Home");
                                    }
                                    foreach (var error in result.Errors)
                                    {
                                             ModelState.AddModelError(string.Empty, error.Description);
                                    }
                           }
                           return View(model);
                  }

                  [HttpGet]
                  public IActionResult Login()
                  {
                           return View();
                  }
                  [HttpPost]
                  public async Task<IActionResult> Login(LoginViewModel model)
                  {
                           if (ModelState.IsValid)
                           {
                                    //tim user theo email
                                    var user1 = await _userManager.FindByEmailAsync(model.Email);
                                    if (user1 == null)
                                    {
                                             ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                                             return View(model);
                                    }
                                    else
                                    {
                                             var result = await _signInManager.PasswordSignInAsync(
                                                      user1.UserName,
                                                      model.Password,
                                                      model.RememberMe,
                                                      lockoutOnFailure: false);
                                             if (result.Succeeded)
                                             {
                                                      return RedirectToAction("Index", "Home");
                                             }
                                             ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                                    }
                           }
                           return View(model);
                  }
                  [HttpPost]
                  public async Task<IActionResult> Logout()
                  {
                           await _signInManager.SignOutAsync();
                           return RedirectToAction("Index", "Home");
                  }
         }
}