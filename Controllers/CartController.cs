using Microsoft.AspNetCore.Mvc;
using MymvcApp.Helpers;
using MymvcApp.Models;
using Microsoft.EntityFrameworkCore;
using MymvcApp.Data;
using Microsoft.VisualBasic;

namespace MymvcApp.Controllers
{
         public class CartController : Controller
         {
                  private readonly AppDbContext _context;
                  public CartController(AppDbContext context)
                  {
                           _context = context;
                  }
                  //hien thi gio hang
                  [HttpGet]
                  public IActionResult Index()
                  {
                           var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
                           if (cart == null)
                           {
                                    cart = new List<CartItem>();
                           }
                           return View(cart);
                  }

                  //them san pham vao gio hang
                  [HttpPost]
                  public async Task<IActionResult> AddToCart(int productId)
                  {
                           var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
                           if (product == null)
                           {
                                    return NotFound();
                           }

                           var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
                           if (cart == null)
                           {
                                    cart = new List<CartItem>();
                           }

                           var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
                           if (cartItem != null)
                           {
                                    cartItem.Quantity++;
                           }
                           else
                           {
                                    cart.Add(new CartItem
                                    {
                                             ProductId = product.Id,
                                             ProductName = product.Name,
                                             UnitPrice = product.Price,
                                             Quantity = 1,
                                             ImageUrl = product.ImageUrl

                                    });
                           }

                           HttpContext.Session.SetObjectAsJson("Cart", cart);
                           return RedirectToAction("Index");
                  }

                  //xoa san pham khoi gio
                  [HttpPost]
                  public IActionResult RemoveFromCart(int productId)
                  {
                           var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
                           if (cart == null)
                           {
                                    return RedirectToAction("Index");
                           }

                           var cartItem = cart.FirstOrDefault(c => c.ProductId == productId);
                           if (cartItem != null)
                           {
                                    cart.Remove(cartItem);
                                    HttpContext.Session.SetObjectAsJson("Cart", cart);
                           }

                           return RedirectToAction("Index");
                  }
         }
}