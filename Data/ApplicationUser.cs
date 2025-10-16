using Microsoft.AspNetCore.Identity;
namespace MymvcApp.Models
{
         public class ApplicationUser : IdentityUser
         {
                  // Additional properties can be added here
                  public string FullName { get; set; } = string.Empty;
                  public string Address { get; set; } = string.Empty;
                  public DateTime DateOfBirth { get; set; }
         }
}