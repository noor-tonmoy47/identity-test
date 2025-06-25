using BookStoreAPI.Models;

namespace BookStoreAPI.Services.BookService;

public class BookService : IBookService
{
    private static readonly List<Book> Books =
    [
        new() { Id = 1, Title = "It Starts With Us", Author = "Colin Grover" },
        new() { Id = 2, Title = "Harry Potter and the Prisoner of Azkaban", Author = "J.K Rowling" },
        new() { Id = 3, Title = "A Song of Ice and Fire", Author = "George R.R. Martin" },
    ];

    public Task<IList<Book>> GetBooksAsync()
    {
        return Task.FromResult<IList<Book>>(Books);
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return Books.FirstOrDefault(x => x.Id == id);
    }

    public async Task<Book?> CreateBookAsync(Book? book)
    {
        if (book is null)
            return book;
        
        book.Id = Books.Max(x => x.Id) + 1;
        Books.Add(book);
        return book;
    }

    public async Task<Book?> UpdateBookAsync(Book? book)
    {
        if (book is null)
            return book;

        var pr = Books.FirstOrDefault(x => x.Id == book.Id);
        if (pr is null)
            return book;
        pr.Title = book.Title;
        pr.Author = book.Author;

        return pr;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var removeCount = Books.RemoveAll(p => p.Id == id);
        return removeCount > 0;
    }
}