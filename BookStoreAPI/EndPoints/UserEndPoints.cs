using BookStoreAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreAPI.EndPoints;

public static class UserEndPoints
{
    private static List<User> Users = 
    [
        new(){Id = 1, Name = "David", Email = "david@gmail.com"},
        new(){Id = 2, Name = "Fred", Email = "fred@gmail.com"},
        new(){Id = 3, Name = "John", Email = "john@gmail.com"}
    ];

    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup("api/v1/users").RequireAuthorization();
        
        userGroup.MapGet("", Get).WithName("Get");
        userGroup.MapPost("", Post).WithName("Post");
        userGroup.MapGet("{id}", GetById).WithName("GetById");
        userGroup.MapPut("{id}", Put).WithName("Put");
        userGroup.MapDelete("{id}", Delete).WithName("Delete");
        
    }
    
    private static async Task<IResult> Get() => Results.Ok(Users);

    private static async Task<IResult> Post([FromBody] User? pr)
    {
        if (pr is null)
            return Results.BadRequest();
        pr.Id = Users.Max(x => x.Id) + 1;
        Users.Add(pr);
        return Results.CreatedAtRoute("GetById", new { id = pr.Id }, pr);
    }

    private static async Task<IResult> GetById([FromRoute] int id)
    {
        var user = Users.FirstOrDefault(x => x.Id == id);
        return user == null ? Results.NotFound("User Not Found") : Results.Ok(user);
    }

    private static async Task<IResult> Put([FromBody] User? usr)
    {
        if (usr is null)
            return Results.BadRequest("Request body is required");
        
        var user = Users.FirstOrDefault(x => x.Id == usr.Id);
        if (user is null)
            return Results.NotFound("User not found");
        user.Name = usr.Name;
        user.Email = usr.Email;
        
        return Results.Ok(user);
    }

    private static async Task<IResult> Delete([FromRoute] int id)
    {
        var removeCount = Users.RemoveAll(p => p.Id == id);
        return removeCount > 0 ? Results.NoContent() : Results.NotFound();
    }
}