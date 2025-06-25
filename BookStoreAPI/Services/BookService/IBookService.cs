using BookStoreAPI.Models;

namespace BookStoreAPI.Services.BookService;

public interface IBookService
{
    Task<IList<Book>> GetBooksAsync();
    Task<Book?> GetBookByIdAsync(int id);
    Task<Book?> CreateBookAsync(Book? product);
    Task<Book?> UpdateBookAsync(Book? product);
    Task<bool> DeleteBookAsync(int id);
}