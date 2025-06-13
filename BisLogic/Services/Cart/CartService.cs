using Core.Models;
using Core.Contracts;
using Microsoft.Extensions.Logging;

namespace BisLogic.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IProductRepository _productRepo;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<CartService> _logger;

        public CartService(
            ICartRepository cartRepo,
            IProductRepository productRepo,
            IUserRepository userRepo,
            ILogger<CartService> logger)
        {
            _cartRepo = cartRepo;
            _productRepo = productRepo;
            _userRepo = userRepo;
            _logger = logger;
        }

        public async Task<CartItem> AddToCartAsync(int userId, CartItem item)
        {
            // Валидация
            if (item.Quantity <= 0)
                throw new ArgumentException("Количество должно быть больше 0");

            var user = await _userRepo.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("Пользователь не найден");

            var product = await _productRepo.GetProductByIdAsync(item.ProductId);
            if (product == null)
                throw new KeyNotFoundException("Товар не найден");

            // Проверка наличия товара в корзине
            var existingItem = await _cartRepo.FindByUserAndProductAsync(userId, item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
                await _cartRepo.UpdateAsync(existingItem);
                _logger.LogInformation($"Обновлено количество товара {item.ProductId} в корзине пользователя {userId}");
                return existingItem;
            }

            // Создание новой позиции
            item.Product = product;
            var newItem = await _cartRepo.AddAsync(item);
            _logger.LogInformation($"Добавлен товар {item.ProductId} в корзину пользователя {userId}");
            
            return newItem;
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            var item = await _cartRepo.FindByIdAsync(cartItemId);
            if (item == null) return;

            await _cartRepo.RemoveAsync(cartItemId);
            _logger.LogInformation($"Удален товар {item.ProductId} из корзины пользователя {item.UserId}");
        }

        public async Task<List<CartItem>> GetUserCartAsync(int userId)
        {
            var items = await _cartRepo.GetByUserIdAsync(userId);
            return items.Select(item => new CartItem
            {
                CartItemId = item.CartItemId,
                ProductId = item.ProductId,
                Product = item.Product,
                Quantity = item.Quantity
            }).ToList();
        }

        public async Task ClearCartAsync(int userId)
        {
            await _cartRepo.ClearForUserAsync(userId);
            _logger.LogInformation($"Корзина пользователя {userId} очищена");
        }
    }
}