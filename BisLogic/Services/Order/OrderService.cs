using Core.Models;
using Core.Contracts;
using Core.Enums;
using Microsoft.Extensions.Logging;

namespace BisLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartService _cartService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IUserRepository userRepository,
            IProductRepository productRepository,
            ICartService cartService,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _cartService = cartService;
            _logger = logger;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            try
            {
                // 1. Валидация пользователя
                var user = await _userRepository.GetUserByIdAsync(order.UserId);
                if (user == null)
                {
                    throw new ArgumentException("Пользователь не найден");
                }

                // 2. Проверка товаров в корзине
                foreach (var cartItem in order.CartItems)
                {
                    var product = await _productRepository.GetProductByIdAsync(cartItem.ProductId);
                    if (product == null)
                    {
                        throw new ArgumentException($"Товар с ID {cartItem.ProductId} не найден");
                    }

                    // Устанавливаем ссылку на продукт
                    cartItem.Product = product;
                }

                // 3. Расчет общей суммы
                order.TotalAmount = order.CartItems.Sum(item => item.Product.Price * item.Quantity);
                order.CreationDate = DateTime.UtcNow;
                order.Status = OrderStatus.New;

                // 4. Сохранение заказа
                var createdOrder = await _orderRepository.CreateOrderAsync(order);

                // 5. Очистка корзины
                await _cartService.ClearCartAsync(order.UserId);

                _logger.LogInformation($"Создан новый заказ ID: {createdOrder.OrderId}");
                return createdOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании заказа");
                throw;
            }
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(id);
                if (order == null)
                {
                    throw new KeyNotFoundException("Заказ не найден");
                }
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении заказа ID: {id}");
                throw;
            }
        }

        public async Task<List<Order>> GetUserOrdersAsync(int userId)
        {
            try
            {
                var orders = await _orderRepository.GetUserOrdersAsync(userId);
                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении заказов пользователя ID: {userId}");
                throw;
            }
        }

        public async Task CancelOrderAsync(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    throw new KeyNotFoundException("Заказ не найден");
                }

                if (order.Status != OrderStatus.New)
                {
                    throw new InvalidOperationException("Можно отменять только новые заказы");
                }

                await _orderRepository.UpdateOrderStatusAsync(orderId, OrderStatus.Cancelled);
                _logger.LogInformation($"Заказ ID: {orderId} отменен");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при отмене заказа ID: {orderId}");
                throw;
            }
        }
    }
}