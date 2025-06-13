using Core.Models;

namespace BisLogic.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> GetOrderByIdAsync(int id);
        Task<List<Order>> GetUserOrdersAsync(int userId);
        Task CancelOrderAsync(int orderId);
    }
}
