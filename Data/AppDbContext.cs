using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MymvcApp.Models;

namespace MymvcApp.Data
{
         public class AppDbContext : IdentityDbContext<ApplicationUser>
         {
                  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
                  {
                  }

                  public DbSet<Product> Products { get; set; }
                  public DbSet<Category> Categories { get; set; }
                  public DbSet<Order> Orders { get; set; }
                  public DbSet<OrderItem> OrderItems { get; set; }

                  protected override void OnModelCreating(ModelBuilder modelBuilder)
                  {
                           base.OnModelCreating(modelBuilder);

                           modelBuilder.Entity<Category>()
                                    .HasMany(c => c.Products)
                                    .WithOne(p => p.Category)
                                    .HasForeignKey(p => p.CategoryId)
                                    .OnDelete(DeleteBehavior.Cascade);

                           modelBuilder.Entity<Order>()
                                    .HasMany(o => o.OrderItems)
                                    .WithOne(oi => oi.Order)
                                    .HasForeignKey(oi => oi.OrderId)
                                    .OnDelete(DeleteBehavior.Cascade);

                           modelBuilder.Entity<OrderItem>()
                                    .HasOne(oi => oi.Product)
                                    .WithMany()
                                    .HasForeignKey(oi => oi.ProductId)
                                    .OnDelete(DeleteBehavior.Restrict);
                  }
         }
}