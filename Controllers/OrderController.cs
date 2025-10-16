using Microsoft.EntityFrameworkCore;
using MymvcApp.Data;
using MymvcApp.Models;
using MymvcApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using MymvcApp.Helpers;
using Microsoft.AspNetCore.Authorization;


namespace MymvcApp.Controllers
{
         public class OrderController : Controller
         {
                  private readonly AppDbContext _context;
                  private readonly UserManager<ApplicationUser> _userManager;
                  public OrderController(AppDbContext context, UserManager<ApplicationUser> userManager)
                  {
                           _context = context;
                           _userManager = userManager;
                  }

        //get: orders
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                     .Include(o => o.OrderItems)
                     .ThenInclude(oi => oi.Product)
                     .ToListAsync();
            return View(orders); //=> List<Order>
        }

        //get: orders/details/5
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (orders == null)
            {
                return NotFound();
            }
            return View(orders);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            //lay gio hang tu session
            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Cart");
            }
            var vm = new OrderCreateVm
            {
                CartItems = cart,
                TotalAmount = cart.Sum(c => c.UnitPrice * c.Quantity)
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(OrderCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                //xem loi de debug
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage);
                    }
                }
                return View(vm);
            }


            // var user = await _userManager.GetUserAsync(User);
            // if (user == null)
            // {
            //          return RedirectToAction("Login", "Account");
            // }

            var cart = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Cart");
            if (cart == null || !cart.Any())
            {
                ModelState.AddModelError("", "Your cart is empty.");
                return View(vm);
            }
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine("Validation error: " + error.ErrorMessage);
                }
            }

            //map tu viewmodel sang entity order
            var order = new Order
            {
                //  UserId = user.Id,
                CustomerName = vm.CustomerName,
                CustomerEmail = vm.CustomerEmail,
                PhoneNumber = vm.PhoneNumber,
                ShippingAddress = vm.ShippingAddress,
                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(c => c.UnitPrice * c.Quantity),
                OrderItems = cart.Select(oi => new OrderItem
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            //xoa gio hang trong session
            HttpContext.Session.Remove("Cart");

            return RedirectToAction(nameof(Index));


        }
         }
}