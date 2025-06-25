using SuperStoreAPI.Models;

namespace SuperStoreAPI.Services.UserService;

public class UserService : IUserService
{
    private static readonly List<User> Users = 
    [
        new(){Id = 1, Name = "David", Email = "david@gmail.com"},
        new(){Id = 2, Name = "Fred", Email = "fred@gmail.com"},
        new(){Id = 3, Name = "John", Email = "john@gmail.com"}
    ];
    
    public Task<IList<User>> GetProductsAsync() => Task.FromResult<IList<User>>(Users);
    
    public Task<User?> GetProductByIdAsync(int id) => Task.FromResult(Users.FirstOrDefault(x => x.Id == id));

    public async Task<User?> CreateProductAsync(User? user)
    {
        if (user is null)
            return user;
        
        user.Id = Users.Max(x => x.Id) + 1;
        Users.Add(user);
        return user;
    }

    public async Task<User?> UpdateProductAsync(User? user)
    {
        if (user is null)
            return user;

        var pr = Users.FirstOrDefault(x => x.Id == user.Id);
        if (pr is null)
            return user;
        pr.Name = user.Name;
        pr.Email = user.Email;

        return pr;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var removeCount = Users.RemoveAll(p => p.Id == id);
        return removeCount > 0;
    }
}