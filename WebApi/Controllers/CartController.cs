using WebApi.DTOs.Requests.Cart;
using Microsoft.AspNetCore.Mvc;
using Core.Models;
using Microsoft.Extensions.Logging;
using orm;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs.Responses.Cart;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<CartController> _logger;

    public CartController(AppDbContext context, ILogger<CartController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // POST: api/cart/add
    [HttpPost("add")]
    public async Task<ActionResult> AddToCart(
        [FromBody] CartItemAddRequest request,
        [FromQuery] int userId) 
    {
        try
        {
            // Валидация
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Проверка существования товара
            var product = await _context.Products.FindAsync(request.ProductId);
            if (product == null)
                return NotFound($"Товар с ID {request.ProductId} не найден");

            // Создание записи в корзине
            var cartItem = new CartItem
            {
                ProductId = request.ProductId,
                UserId = userId,
                Quantity = request.Quantity
            };

            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                cartItem.CartItemId,
                cartItem.ProductId,
                cartItem.Quantity
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при добавлении в корзину");
            return StatusCode(500, "Произошла ошибка");
        }
    }

    // PUT: api/cart/update/5
    [HttpPut("update/{cartItemId}")]
    public async Task<ActionResult> UpdateCartItem(
        int cartItemId,
        [FromBody] UpdateCartItemRequest request)
    {
        try
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null)
                return NotFound();

            cartItem.Quantity = request.NewQuantity;
            await _context.SaveChangesAsync();

            return NoContent(); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при обновлении корзины ID: {cartItemId}");
            return StatusCode(500, "Произошла ошибка");
        }
    }
    // DELETE: api/cart/remove/5
    [HttpDelete("remove/{cartItemId}")]
    public async Task<IActionResult> RemoveFromCart(int cartItemId)
    {
        try
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem == null)
                return NotFound($"Запись в корзине с ID {cartItemId} не найдена");

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ошибка при удалении из корзины ID: {cartItemId}");
            return StatusCode(500, "Произошла ошибка");
        }
    }

    [HttpGet("{userId}")]
    [ProducesResponseType(typeof(CartResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CartResponse>> GetCart(int userId)
    {
        var cartItems = await _context.CartItems
            .Where(ci => ci.UserId == userId && ci.OrderId == null) 
            .Include(ci => ci.Product)
            .ToListAsync();

        if (!cartItems.Any())
            return NotFound("Корзина пуста");

        var response = new CartResponse(
    Items: cartItems.Select(ci => new CartItemResponse(
        Id: ci.CartItemId,          
        ProductId: ci.ProductId,
        ProductName: ci.Product.Name,
        ProductPrice: ci.Product.Price,
        Quantity: ci.Quantity,
        TotalPrice: ci.Product.Price * ci.Quantity  
    )).ToList(),
    Total: cartItems.Sum(ci => ci.Product.Price * ci.Quantity)
);

        return Ok(response);
    }
}
