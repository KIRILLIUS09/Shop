using Core.Models;
using Core.Enums;

namespace Core.Contracts
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Order order); 
        Task<Order?> GetOrderByIdAsync(int id);
        Task<List<Order>> GetUserOrdersAsync(int userId);
        Task UpdateOrderStatusAsync(int id, OrderStatus status);
    }
}
