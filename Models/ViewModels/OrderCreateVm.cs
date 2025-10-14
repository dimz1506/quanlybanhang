namespace MymvcApp.Models.ViewModels
{
         public class OrderCreateVm
         {
                  public string CustomerName { get; set; } = string.Empty;
                  public string CustomerEmail { get; set; } = string.Empty;
                  public string ShippingAddress { get; set; } = string.Empty;
                  public string PhoneNumber { get; set; } = string.Empty;

                  public decimal TotalAmount { get; set; }
                  public List<CartItem> CartItems { get; set; } = new();
         }
}
