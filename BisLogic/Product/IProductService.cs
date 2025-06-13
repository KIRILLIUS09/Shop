using Core.Models;

namespace BisLogic.Services
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> GetProductsAsync();
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
