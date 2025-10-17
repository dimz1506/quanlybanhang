using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace MymvcApp.Models
{
         public class Order
         {
                  public int Id { get; set; }
                  public string UserId { get; set; } = string.Empty;
                  public ApplicationUser User { get; set; } = null!;
                  public string ShippingAddress { get; set; } = string.Empty;
                  public string PhoneNumber { get; set; } = string.Empty;
                  [Required]
                  public string CustomerName { get; set; } = string.Empty;
                  [Required]
                  [EmailAddress]
                  public string CustomerEmail { get; set; } = string.Empty;
                  [Required]
                  public DateTime OrderDate { get; set; } = DateTime.Now;
                  public decimal TotalAmount { get; set; }
                  public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
         }

         public class OrderItem
         {
                  public int Id { get; set; }
                  public int OrderId { get; set; }
                  public int ProductId { get; set; }
                  public int Quantity { get; set; }
                  public decimal UnitPrice { get; set; }
                  public Order Order { get; set; } = null!;
                  public Product Product { get; set; } = null!;
         }
}