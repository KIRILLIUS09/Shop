using Microsoft.EntityFrameworkCore;
using Core.Models;
using Core.Interfaces;

namespace orm
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        
        public void AddEntity<TEntity>(TEntity entity) where TEntity : class
            => base.Add(entity);

        public void RemoveEntity<TEntity>(TEntity entity) where TEntity : class
            => base.Remove(entity);

        public void UpdateEntity<TEntity>(TEntity entity) where TEntity : class
        => base.Update(entity);
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }


        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
            => new EfDbContextTransaction(await Database.BeginTransactionAsync(cancellationToken));

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Address>().HasKey(a => a.AddressId);
            modelBuilder.Entity<Order>().HasKey(o => o.OrderId);
            modelBuilder.Entity<CartItem>().HasKey(ci => ci.CartItemId);
            modelBuilder.Entity<User>().HasKey(u => u.UserId);
            modelBuilder.Entity<Product>().HasKey(p => p.ProductId);


            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity.Property(o => o.TotalAmount)
                    .HasColumnType("decimal(18,2)");

                entity.HasOne(o => o.Address)
                    .WithMany()
                    .HasForeignKey(o => o.AddressId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.User)
                    .WithMany(u => u.Orders)
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasOne(ci => ci.Order)
                    .WithMany(o => o.CartItems)
                    .HasForeignKey(ci => ci.OrderId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(ci => ci.Product)
                    .WithMany(p => p.CartItems)
                    .HasForeignKey(ci => ci.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price)
                    .HasColumnType("decimal(18,2)");


            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasOne(a => a.User)
                    .WithMany(u => u.Addresses)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Username).IsUnique();
            });
        }
        private class EfDbContextTransaction : IDbContextTransaction
        {
            private readonly Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction _transaction;

            public EfDbContextTransaction(Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction)
            {
                _transaction = transaction;
            }

            public void Commit() => _transaction.Commit();
            public void Rollback() => _transaction.Rollback();
            public void Dispose() => _transaction.Dispose();
        }
    }
}