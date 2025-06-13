using Core.Models;

namespace Core.Contracts
{
    public interface ICartRepository
    {
        Task<CartItem?> FindByIdAsync(int cartItemId);
        Task<CartItem> AddAsync(CartItem item);
        Task RemoveAsync(int cartItemId);
        Task<List<CartItem>> GetByUserIdAsync(int userId);
        Task ClearForUserAsync(int userId);
        Task<CartItem?> FindByUserAndProductAsync(int userId, int productId);
        Task UpdateAsync(CartItem item);
    }
}