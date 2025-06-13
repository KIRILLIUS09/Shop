
using Core.Models;


namespace Core.Contracts
{
    public interface IProductRepository
    {
        Task<Product> CreateProductAsync(Product product);
        Task<Product?> GetProductByIdAsync(int id);
        Task<List<Product>> GetProductsAsync();
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
    }
}
