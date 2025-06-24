using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.EndPoints;

public static class BookEndPoints
{
    private static List<Book> Books =
    [
        new() { Id = 1, Title = "It Starts With Us", Author = "Colin Groover" },
        new() { Id = 2, Title = "Harry Potter and the Prisoner of Azkaban", Author = "J.K Rowling" },
        new() { Id = 3, Title = "A Song of Ice and Fire", Author = "George R.R. Martin" },
    ];
    public static void MapEndPoints(this IEndpointRouteBuilder app)
    {
        var bookGroup = app.MapGroup("api/v1/books").RequireAuthorization();
        
        bookGroup.MapGet("", Get).WithName("Get");
        bookGroup.MapPost("", Post).WithName("Post");
        bookGroup.MapGet("{id}", GetById).WithName("GetById");
        bookGroup.MapPut("{id}", Put).WithName("Put");
        bookGroup.MapDelete("{id}", Delete).WithName("Delete");
    }

    private static async Task<IResult> Get() => Results.Ok(Books);

    private static async Task<IResult> Post([FromBody] Book? pr)
    {
        if (pr is null)
            return Results.BadRequest();
        pr.Id = Books.Max(x => x.Id) + 1;
        Books.Add(pr);
        return Results.CreatedAtRoute("GetById", new { id = pr.Id }, pr);
    }

    private static async Task<IResult> GetById([FromRoute] int id)
    {
        var book = Books.FirstOrDefault(x => x.Id == id);
        return book == null ? Results.NotFound("Book Not Found") : Results.Ok(book);
    }

    private static async Task<IResult> Put([FromBody] Book? pr)
    {
        if (pr is null)
            return Results.BadRequest("Request body is required");
        
        var book = Books.FirstOrDefault(x => x.Id == pr.Id);
        if (book is null)
            return Results.NotFound("Book not found");
        book.Title = pr.Title;
        book.Author = pr.Author;
        
        return Results.Ok(book);
    }

    private static async Task<IResult> Delete([FromRoute] int id)
    {
        var removeCount = Books.RemoveAll(p => p.Id == id);
        return removeCount > 0 ? Results.NoContent() : Results.NotFound();
    }
}