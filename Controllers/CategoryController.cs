using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MymvcApp.Data;
using MymvcApp.Models;

namespace MymvcApp.Controllers
{
         public class CategoryController : Controller
         {
                  private readonly AppDbContext _context;
                  public CategoryController(AppDbContext context)
                  {
                           _context = context;
                  }

                  //hien thi danh muc 
                  public async Task<IActionResult> Index()
                  {
                           var category =await _context.Categories.ToListAsync();
                           return View(category);
                  }

                  //hien thi trang tao danh muc san pham
                  [HttpGet]
                  public IActionResult Create()
                  {
                           return View();
                  }

                  [HttpPost]
                  public async Task<IActionResult> Create(Category c)
                  {
                           _context.Categories.Add(c);
                           await _context.SaveChangesAsync();
                           return RedirectToAction(nameof(Index));
                  }

                  //hien thi trang sua danh muc
                  [HttpGet]
                  public async Task<IActionResult> Edit(int id)
                  {
                           var category =await _context.Categories.FindAsync(id);
                           if (category == null)
                           {
                                    return NotFound();
                           }
                           return View(category);
                  }

                  [HttpPost]
                  public async Task<IActionResult> Edit(Category c)
                  {
                           _context.Categories.Update(c);
                           await _context.SaveChangesAsync();
                           return RedirectToAction(nameof(Index));
                  }

                  [HttpGet]
                  public async Task<IActionResult> Delete(int id)
                  {
                           var category =await _context.Categories.FindAsync(id);
                           if (category == null)
                           {
                                    return NotFound();
                           }
                           return View(category);
                  }
                  //xoa danh muc
                  [HttpPost]
                  [ActionName("Delete")]
                  [ValidateAntiForgeryToken]
                  public async Task<IActionResult> DeleteConfirmed(int id)
                  {
                           var category = await _context.Categories.FindAsync(id);
                           if (category == null)
                           {
                                    return NotFound();
                           }
                           _context.Categories.Remove(category);
                           await _context.SaveChangesAsync();
                           return RedirectToAction(nameof(Index));
                  }

                  //xem danh muc chi tiet
                  [HttpGet]
                  public async Task<IActionResult> Details(int id)
                  {
                           var category =await _context.Categories.FindAsync(id);
                           if (category == null)
                           {
                                    return NotFound();
                           }
                           return View(category);
                  }


         }
}