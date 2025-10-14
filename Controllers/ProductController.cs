using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MymvcApp.Data;
using MymvcApp.Models;
using MymvcApp.Models.ViewModels;

namespace MymvcApp.Controllers
{
         public class ProductController : Controller
         {
                  private readonly AppDbContext _context;
                  private readonly IWebHostEnvironment _webHostEnvironment;
                  public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
                  {
                           _context = context;
                           _webHostEnvironment = webHostEnvironment;
                  }
      

                  //get : products
                  public async Task<IActionResult> Index(string searchString)
                  {
                           var products = _context.Products.Include(p => p.Category).AsQueryable();

                           //tim kiem
                           if (!string.IsNullOrEmpty(searchString))
                           {
                                    products = products.Where(p => p.Name.Contains(searchString));
                           }

                           ViewBag.SearchString = searchString;
                           return View(await products.ToListAsync());

                  }

                  //get: products/details
                  public async Task<IActionResult> Details(int? id)
                  {
                           if (id == null || _context.Products == null)
                           {
                                    return NotFound();
                           }

                           var product = await _context.Products
                                    .Include(p => p.Category)
                                    .FirstOrDefaultAsync(m => m.Id == id);
                           if (product == null)
                           {
                                    return NotFound();
                           }

                           return View(product);
                  }

                  //get:products/create
               //   [Authorize(Roles = "Admin")]
                  [HttpGet]
                  public IActionResult Create()
                  {
                           ViewBag.Categories = _context.Categories.ToList();
                           return View();
                  }
               //   [Authorize(Roles = "Admin")]
                  [HttpPost]
                  [ValidateAntiForgeryToken]
                  public async Task<IActionResult> Create(ProductCreateVm vm)
                  {
                     if(vm.ImageFile == null)
                     {
                        ModelState.AddModelError("ImageFile", "Please upload an image file.");
                     }
                     
                     //bat buoc co anh vi da required tren Vm
         if (ModelState.IsValid)
         {

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.ImageFile.FileName);
            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            if (!Directory.Exists(uploadPath))
            {
               Directory.CreateDirectory(uploadPath);
            }
            var filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
               await vm.ImageFile.CopyToAsync(stream);
            }
            var product = new Product
            {
               Name = vm.Name,
               Price = vm.Price,
               Description = vm.Description,
               CategoryId = vm.CategoryId,
               ImageUrl = "/images/" + fileName
            };

            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
         }
         else
         {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
               Console.WriteLine(error.ErrorMessage);
            }
         }
                           ViewBag.Categories = _context.Categories.ToList();
                           return View(vm);
                  }

      //get: products/edit
      //   [Authorize(Roles = "Admin")]
      [HttpGet]

      public async Task<IActionResult> Edit(int? id)
      {
         var product = await _context.Products.FindAsync(id);
         if (product == null)
         {
            return NotFound();
                  }

                  //map product -> productCreateVm
         var vm = new ProductCreateVm
         {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            CategoryId = product.CategoryId,
            ImageFile = null // ImageFile is not set here
         };
         //gui danh muc de do vao dropdown
         ViewBag.Categories = _context.Categories.ToList();
         ViewBag.ExistingImageUrl = product.ImageUrl; // Pass existing image URL to the view
         return View(vm);
      }

               //   [Authorize(Roles = "Admin")]
         [HttpPost]
                  [ValidateAntiForgeryToken]
                  public async Task<IActionResult> Edit(int id, ProductCreateVm vm)
                  {
         if (id != vm.Id)
         {
            return NotFound();
                     }
         if (ModelState.IsValid)
         {
               var product = await _context.Products.FindAsync(id);
               if (product == null)
               {
                  return NotFound();
               }
               product.Name = vm.Name;
               product.Price = vm.Price;
               product.Description = vm.Description;
               product.CategoryId = vm.CategoryId;
               
               //neu co anh moi thi upload anh moi

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
               var fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.ImageFile.FileName);
               var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");
               if (!Directory.Exists(Path.GetDirectoryName(uploadPath)))
               {
                  Directory.CreateDirectory(Path.GetDirectoryName(uploadPath));
               }
               var filePath = Path.Combine(uploadPath, fileName);
               using (var stream = new FileStream(filePath, FileMode.Create))
               {
                  await vm.ImageFile.CopyToAsync(stream);
               }
               product.ImageUrl = "/images/" + fileName;
            }
            try
            {
               _context.Update(product);
               await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
               if (!_context.Products.Any(e => e.Id == product.Id))
               {
                  return NotFound();
               }
               else
               {
                  throw;
               }
            }
            return RedirectToAction(nameof(Index));
         }
         ViewBag.Categories = _context.Categories.ToList();
         return View(vm);
      }

                  //get: products/delete
               //   [Authorize(Roles = "Admin")]
                  [HttpGet]
                  public async Task<IActionResult> Delete(int? id)
                  {
                           if (id == null || _context.Products == null)
                           {
                                    return NotFound();
                           }

                           var product = await _context.Products
                                    .Include(p => p.Category)
                                    .FirstOrDefaultAsync(m => m.Id == id);
                           if (product == null)
                           {
                                    return NotFound();
                           }

                           return View(product);
                  }

                //  [Authorize(Roles = "Admin")]
                  [HttpPost, ActionName("Delete")]
                  [ValidateAntiForgeryToken]
                  public async Task<IActionResult> DeleteConfirmed(int id)
                  {
                           if (_context.Products == null)
                           {
                                    return Problem("Entity set 'AppDbContext.Products'  is null.");
                           }
                           var product = await _context.Products.FindAsync(id);
                           if (product != null)
                           {
                                    _context.Products.Remove(product);
                           }

                           await _context.SaveChangesAsync();
                           return RedirectToAction(nameof(Index));
                  }

                  

         }
}