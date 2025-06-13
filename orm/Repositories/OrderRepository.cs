using Core.Contracts;
using Core.Enums;
using Core.Interfaces;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace orm.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IAppDbContext _context;

        public OrderRepository(IAppDbContext context) => _context = context;

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.AddEntity(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Address)
                .Include(o => o.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<List<Order>> GetUserOrdersAsync(int userId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.CartItems)
                    .ThenInclude(ci => ci.Product)
                .OrderByDescending(o => o.CreationDate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(int id, OrderStatus status)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == id);
            if (order != null)
            {
                order.Status = status;
                await _context.SaveChangesAsync();
            }
        }
    }
}
