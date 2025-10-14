using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MymvcApp.Models
{
         public class Product
         {
                  public int Id { get; set; }
                  public string Name { get; set; } = string.Empty;
                  public decimal Price { get; set; }
                  public string Description { get; set; } = string.Empty;
                  [Required]
                  [DisplayName("Category")]
                  public int CategoryId { get; set; }
                  [StringLength(255)]
                  public string ImageUrl { get; set; } = string.Empty;
                  public DateTime CreatedAt { get; set; } = DateTime.Now;
                  public Category Category { get; set; } = null!;

                  [NotMapped] //thuoc tinh nay se khong map vao database
                  public IFormFile ImageFile { get; set; } = null!;
         }
}