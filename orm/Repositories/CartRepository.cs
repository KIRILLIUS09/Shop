using Core.Contracts;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace orm.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly IAppDbContext _context;

        public CartRepository(IAppDbContext context) => _context = context;

        public async Task<CartItem?> FindByIdAsync(int cartItemId)
        {
            return await _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
        }

        public async Task<CartItem> AddAsync(CartItem item)
        {
            _context.AddEntity(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task RemoveAsync(int cartItemId)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
            if (item != null)
            {
                _context.RemoveEntity(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<CartItem>> GetByUserIdAsync(int userId)
        {
            return await _context.CartItems
                .Where(ci => ci.UserId == userId && ci.OrderId == null)
                .Include(ci => ci.Product)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task ClearForUserAsync(int userId)
        {
            var items = await _context.CartItems
                .Where(ci => ci.UserId == userId && ci.OrderId == null)
                .ToListAsync();

            foreach (var item in items)
            {
                _context.RemoveEntity(item);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<CartItem?> FindByUserAndProductAsync(int userId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci =>
                    ci.UserId == userId &&
                    ci.ProductId == productId &&
                    ci.OrderId == null);
        }

        public async Task UpdateAsync(CartItem item)
        {
            _context.UpdateEntity(item);
            await _context.SaveChangesAsync();
        }
    }
}