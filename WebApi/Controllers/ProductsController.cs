using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using orm;
using Core.Models;
using WebApi.DTOs.Requests.Product;
using WebApi.DTOs.Responses.Product;

namespace WebApi.Controllers
{
    public class ProductsController : BaseController
    {
        public ProductsController(AppDbContext context) : base(context) { }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponse>>> GetProducts(
            [FromQuery] string? color,
            [FromQuery] string? size,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            try
            {
                var query = _context.Products.AsQueryable();

                if (!string.IsNullOrEmpty(color))
                    query = query.Where(p => p.Color == color);

                if (!string.IsNullOrEmpty(size))
                    query = query.Where(p => p.Size == size);

                if (minPrice.HasValue)
                    query = query.Where(p => p.Price >= minPrice.Value);

                if (maxPrice.HasValue)
                    query = query.Where(p => p.Price <= maxPrice.Value);

                var products = await query.ToListAsync();
                var response = products.Select(p => new ProductResponse(
                    p.ProductId,
                    p.Name,
                    p.Price,
                    p.Description,
                    p.Color,
                    p.Size
                ));

                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Ошибка при получении товаров");
            }
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductResponse>> CreateProduct(
            [FromBody] ProductCreateRequest request)
        {
            try
            {
                var product = new Product
                {
                    Name = request.Name,
                    Price = request.Price,
                    Description = request.Description,
                    Color = request.Color,
                    Size = request.Size
                };

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var response = new ProductResponse(
                    product.ProductId,
                    product.Name,
                    product.Price,
                    product.Description,
                    product.Color,
                    product.Size
                );

                return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, response);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Ошибка при создании товара");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductResponse>> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                var response = new ProductResponse(
                    product.ProductId,
                    product.Name,
                    product.Price,
                    product.Description,
                    product.Color,
                    product.Size
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Ошибка при получении товара");
            }
        }
    }
}
