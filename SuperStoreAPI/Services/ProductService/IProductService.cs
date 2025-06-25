using SuperStoreAPI.Models;

namespace SuperStoreAPI.Services.ProductService;

public interface IProductService
{
    Task<IList<Product>> GetProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<Product?> CreateProductAsync(Product? product);
    Task<Product?> UpdateProductAsync(Product? product);
    Task<bool> DeleteProductAsync(int id);
}