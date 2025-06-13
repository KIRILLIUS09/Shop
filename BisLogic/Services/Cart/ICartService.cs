using Core.Models;

namespace BisLogic.Services
{
    public interface ICartService
    {
        Task<CartItem> AddToCartAsync(int userId, CartItem item);
        Task RemoveFromCartAsync(int cartItemId);
        Task<List<CartItem>> GetUserCartAsync(int userId);
        Task ClearCartAsync(int userId);
    }
}
