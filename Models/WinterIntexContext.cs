using Microsoft.EntityFrameworkCore;

namespace WinterIntex.Models
{
    public class WinterIntexContext : DbContext
    {
        public WinterIntexContext(DbContextOptions<WinterIntexContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
    }
}
