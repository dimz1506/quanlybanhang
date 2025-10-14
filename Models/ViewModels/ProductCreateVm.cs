using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MymvcApp.Models.ViewModels
{
         public class ProductCreateVm
         {
                  public int Id { get; set; } 
                  [Required]
                  [StringLength(100)]
                  public string Name { get; set; } = string.Empty;

                  [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
                  public decimal Price { get; set; }

                  [StringLength(1000)]
                  public string Description { get; set; } = string.Empty;

                  [Required]
                  [Display(Name = "Category")]
                  public int CategoryId { get; set; }
                  

                  [Required]
                  [Display(Name = "Product Image")]
                  public IFormFile ImageFile { get; set; } = null!;
         }
}