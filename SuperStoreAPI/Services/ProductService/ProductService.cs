using SuperStoreAPI.Models;

namespace SuperStoreAPI.Services.ProductService;

public class ProductService : IProductService
{
    private static readonly List<Product> Products =
    [
        new() { Id = 1, Name = "Apple", Price = 10m },
        new() { Id = 2, Name = "Banana", Price = 20m },
        new() { Id = 3, Name = "Orange", Price = 30m },
    ];

    public Task<IList<Product>> GetProductsAsync()
    {
        return Task.FromResult<IList<Product>>(Products);
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return Products.FirstOrDefault(x => x.Id == id);
    }

    public async Task<Product?> CreateProductAsync(Product? product)
    {
        if (product is null)
            return product;
        
        product.Id = Products.Max(x => x.Id) + 1;
        Products.Add(product);
        return product;
    }

    public async Task<Product?> UpdateProductAsync(Product? product)
    {
        if (product is null)
            return product;

        var pr = Products.FirstOrDefault(x => x.Id == product.Id);
        if (pr is null)
            return product;
        pr.Name = product.Name;
        pr.Price = product.Price;

        return pr;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var removeCount = Products.RemoveAll(p => p.Id == id);
        return removeCount > 0;
    }
}