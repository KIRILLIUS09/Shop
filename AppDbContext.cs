using Microsoft.EntityFrameworkCore;

namespace orm.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ShopDB;Username=postgres;Password=081979");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Конфигурация связей
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Order)
                .WithMany(o => o.CartItems)
                .HasForeignKey(ci => ci.OrderId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}