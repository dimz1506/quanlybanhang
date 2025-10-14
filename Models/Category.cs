using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MymvcApp.Models
{
         public class Category
         {
                  public int Id { get; set; }
                  [Required]
                  [StringLength(100)]
                  public string Name { get; set; } = string.Empty;
                  [StringLength(255)]
                  public string Description { get; set; } = string.Empty;
                  public DateTime CreatedAt { get; set; } = DateTime.Now;
                  public ICollection<Product> Products { get; set; } = new List<Product>();
         }
}