using BookStoreAPI.Models;
using BookStoreAPI.Services.BookService;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.EndPoints;

public static class BookEndPoints
{
    
    public static void MapEndPoints(this IEndpointRouteBuilder app)
    {
        var bookGroup = app.MapGroup("api/v1/books").RequireAuthorization();
        
        bookGroup.MapGet("", Get).WithName("Get");
        bookGroup.MapPost("", Post).WithName("Post");
        bookGroup.MapGet("{id}", GetById).WithName("GetById");
        bookGroup.MapPut("{id}", Put).WithName("Put");
        bookGroup.MapDelete("{id}", Delete).WithName("Delete");
    }

    private static async Task<IResult> Get(IBookService bookService)
    {
        var listOfBooks = await bookService.GetBooksAsync();
        return Results.Ok(listOfBooks);
    }

    private static async Task<IResult> Post(IBookService bookService, [FromBody] Book? pr)
    {
        var createdBook = await bookService.CreateBookAsync(pr);
        if (createdBook is null)
            return Results.BadRequest();
        return Results.CreatedAtRoute("GetById", new { id = pr.Id }, createdBook);
    }

    private static async Task<IResult> GetById(IBookService bookService, [FromRoute] int id)
    {
        var book = await bookService.GetBookByIdAsync(id);
        return book == null ? Results.NotFound("Book Not Found") : Results.Ok(book);
    }

    private static async Task<IResult> Put(IBookService bookService, [FromBody] Book? pr)
    {
        var updatedBook = await bookService.UpdateBookAsync(pr);
        return updatedBook is null ? Results.BadRequest() : Results.Ok(updatedBook);
    }

    private static async Task<IResult> Delete(IBookService bookService, [FromRoute] int id)
    {
        var isDeleted = await bookService.DeleteBookAsync(id);
        return isDeleted ? Results.NoContent() : Results.NotFound();
    }
}