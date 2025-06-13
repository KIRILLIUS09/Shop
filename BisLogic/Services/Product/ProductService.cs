using Core.Models;
using Core.Contracts;
using Microsoft.Extensions.Logging;

namespace BisLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IProductRepository productRepository,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
                // Валидация продукта
                ValidateProduct(product);

                var createdProduct = await _productRepository.CreateProductAsync(product);
                _logger.LogInformation($"Создан новый продукт ID: {createdProduct.ProductId}");
                return createdProduct;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при создании продукта");
                throw;
            }
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Продукт с ID {id} не найден");
                }
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении продукта ID: {id}");
                throw;
            }
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            try
            {
                var products = await _productRepository.GetProductsAsync();
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка продуктов");
                throw;
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            try
            {
                // Валидация продукта
                ValidateProduct(product);

                await _productRepository.UpdateProductAsync(product);
                _logger.LogInformation($"Обновлен продукт ID: {product.ProductId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при обновлении продукта ID: {product.ProductId}");
                throw;
            }
        }

        public async Task DeleteProductAsync(int id)
        {
            try
            {
                await _productRepository.DeleteProductAsync(id);
                _logger.LogInformation($"Удален продукт ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении продукта ID: {id}");
                throw;
            }
        }

        private void ValidateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                throw new ArgumentException("Название продукта не может быть пустым");

            if (product.Price <= 0)
                throw new ArgumentException("Цена продукта должна быть больше 0");

            if (string.IsNullOrWhiteSpace(product.Description))
                throw new ArgumentException("Описание продукта не может быть пустым");

            if (string.IsNullOrWhiteSpace(product.Color))
                throw new ArgumentException("Цвет продукта не может быть пустым");

            if (string.IsNullOrWhiteSpace(product.Size))
                throw new ArgumentException("Размер продукта не может быть пустым");
        }
    }
}
