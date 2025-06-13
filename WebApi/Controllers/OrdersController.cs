using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using orm;
using Core.Models;
using Core.Enums;
using WebApi.DTOs.Requests.Order;
using WebApi.DTOs.Responses.Order;
using WebApi.DTOs.Responses.User;

namespace WebApi.Controllers
{
    public class OrdersController : BaseController
    {
        public OrdersController(AppDbContext context) : base(context) { }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<OrderResponse>> CreateOrder(
            [FromBody] OrderCreateRequest request)
        {
            try
            {
                // 1. Проверка существования пользователя и адреса
                var user = await _context.Users.FindAsync(request.UserId);
                var address = await _context.Addresses.FindAsync(request.AddressId);

                if (user == null || address == null)
                    return BadRequest("Пользователь или адрес не найдены");

                // 2. Создание заказа
                var order = new Order
                {
                    UserId = request.UserId,
                    AddressId = request.AddressId,
                    Status = OrderStatus.New,
                    CreationDate = DateTime.UtcNow,
                    Comment = request.Comment,
                    CartItems = new List<CartItem>()
                };

                // 3. Добавление товаров и расчет суммы
                decimal totalAmount = 0;
                foreach (var item in request.Items)
                {
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product == null)
                        return BadRequest($"Товар с ID {item.ProductId} не найден");

                    order.CartItems.Add(new CartItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UserId = request.UserId
                    });

                    totalAmount += product.Price * item.Quantity;
                }
                order.TotalAmount = totalAmount;

                // 4. Сохранение
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // 5. Формирование ответа
                var response = new OrderResponse(
                    order.OrderId,
                    order.CreationDate,
                    order.Status,
                    order.TotalAmount,
                    new UserAddressResponse(
                        address.AddressId,
                        $"{address.Region}, {address.City}, {address.Street} {address.Building}",
                        address.Region,
                        address.City
                    ),
                    order.CartItems.Select(ci => new OrderItemResponse(
                        ci.ProductId,
                        ci.Product?.Name ?? "Неизвестный товар",
                        ci.Quantity,
                        ci.Product?.Price ?? 0
                    )).ToList()
                );

                return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, response);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Ошибка при создании заказа");
            }
        }

        // GET: api/orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponse>> GetOrder(int id)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.Address)
                    .Include(o => o.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(o => o.OrderId == id);

                if (order == null)
                    return NotFound();

                var response = new OrderResponse(
                    order.OrderId,
                    order.CreationDate,
                    order.Status,
                    order.TotalAmount,
                    new UserAddressResponse(
                        order.Address.AddressId,
                        $"{order.Address.Region}, {order.Address.City}, {order.Address.Street} {order.Address.Building}",
                        order.Address.Region,
                        order.Address.City
                    ),
                    order.CartItems.Select(ci => new OrderItemResponse(
                        ci.ProductId,
                        ci.Product?.Name ?? "Неизвестный товар",
                        ci.Quantity,
                        ci.Product?.Price ?? 0
                    )).ToList()
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Ошибка при получении заказа");
            }
        }
    }
}

