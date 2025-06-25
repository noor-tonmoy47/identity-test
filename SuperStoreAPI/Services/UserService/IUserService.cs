using SuperStoreAPI.Models;

namespace SuperStoreAPI.Services.UserService;

public interface IUserService
{
    Task<IList<User>> GetProductsAsync();
    Task<User?> GetProductByIdAsync(int id);
    Task<User?> CreateProductAsync(User? product);
    Task<User?> UpdateProductAsync(User? product);
    Task<bool> DeleteProductAsync(int id);
}