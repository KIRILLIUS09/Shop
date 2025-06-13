using Core.Contracts;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace orm.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IAppDbContext _context;

        public ProductRepository(IAppDbContext context) => _context = context;

        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.AddEntity(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.CartItems)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.UpdateEntity(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == id);
            if (product != null)
            {
                _context.RemoveEntity(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}