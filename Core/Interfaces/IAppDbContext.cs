
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Address> Addresses { get; }
        DbSet<CartItem> CartItems { get; }
        DbSet<Order> Orders { get; }
        DbSet<Product> Products { get; }
        DbSet<User> Users { get; }

        void AddEntity<TEntity>(TEntity entity) where TEntity : class;
        void RemoveEntity<TEntity>(TEntity entity) where TEntity : class;
        void UpdateEntity<TEntity>(TEntity entity) where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();

        // Универсальный интерфейс транзакции
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
    }

    public interface IDbContextTransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}