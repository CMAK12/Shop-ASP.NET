using Microsoft.EntityFrameworkCore;
using MyWebApp.Models;

namespace MyWebApp
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Products> Products { get; set; }
        public DbSet<Companies> Companies { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
