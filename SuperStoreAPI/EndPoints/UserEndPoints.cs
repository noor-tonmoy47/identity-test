using Microsoft.AspNetCore.Mvc;
using SuperStoreAPI.Models;
using SuperStoreAPI.Services.UserService;

namespace SuperStoreAPI.EndPoints;

public static class UserEndPoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup("api/v1/users").RequireAuthorization();
        
        userGroup.MapGet("", Get).WithName("Get");
        userGroup.MapPost("", Post).WithName("Post");
        userGroup.MapGet("{id}", GetById).WithName("GetById");
        userGroup.MapPut("{id}", Put).WithName("Put");
        userGroup.MapDelete("{id}", Delete).WithName("Delete");
        
    }
    
    private static async Task<IResult> Get(IUserService userService)
    {
        var listOfUsers = await userService.GetProductsAsync();
        return Results.Ok(listOfUsers);
    }

    private static async Task<IResult> Post(IUserService userService, [FromBody] User? pr)
    {
        var createdUser = await userService.CreateProductAsync(pr);
        if (createdUser is null)
            return Results.BadRequest();
       
        return Results.CreatedAtRoute("GetById", new { id = pr.Id }, createdUser);
    }

    private static async Task<IResult> GetById(IUserService userService, [FromRoute] int id)
    {
        var user = await userService.GetProductByIdAsync(id);
        return user == null ? Results.NotFound("User Not Found") : Results.Ok(user);
    }

    private static async Task<IResult> Put(IUserService userService, [FromBody] User? usr)
    {
        var updatedUser = await userService.UpdateProductAsync(usr);
        return updatedUser is null ? Results.BadRequest() : Results.Ok(updatedUser);
    }

    private static async Task<IResult> Delete(IUserService userService, [FromRoute] int id)
    {
        var isDeleted = await userService.DeleteProductAsync(id);
        return isDeleted ? Results.NoContent() : Results.NotFound();
    }
}