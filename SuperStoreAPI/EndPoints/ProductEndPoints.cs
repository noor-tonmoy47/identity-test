using Microsoft.AspNetCore.Mvc;
using SuperStoreAPI.Models;

namespace SuperStoreAPI.EndPoints;

public static class ProductEndPoints
{
    private static List<Product> Products =
    [
        new() { Id = 1, Name = "Apple", Price = 10m },
        new() { Id = 2, Name = "Banana", Price = 20m },
        new() { Id = 3, Name = "Orange", Price = 30m },
    ];
    public static void MapEndPoints(this IEndpointRouteBuilder app)
    {
        var productGroup = app.MapGroup("api/v1/products").RequireAuthorization();
        
        productGroup.MapGet("", Get).WithName("Get");
        productGroup.MapPost("", Post).WithName("Post");
        productGroup.MapGet("{id}", GetById).WithName("GetById");
        productGroup.MapPut("{id}", Put).WithName("Put");
        productGroup.MapDelete("{id}", Delete).WithName("Delete");
    }

    private static async Task<IResult> Get() => Results.Ok(Products);

    private static async Task<IResult> Post([FromBody] Product? pr)
    {
        if (pr is null)
            return Results.BadRequest();
        pr.Id = Products.Max(x => x.Id) + 1;
        Products.Add(pr);
        return Results.CreatedAtRoute("GetById", new { id = pr.Id }, pr);
    }

    private static async Task<IResult> GetById([FromRoute] int id)
    {
        var product = Products.FirstOrDefault(x => x.Id == id);
        return product == null ? Results.NotFound("Product Not Found") : Results.Ok(product);
    }

    private static async Task<IResult> Put([FromBody] Product? pr)
    {
        if (pr is null)
            return Results.BadRequest("Request body is required");
        
        var product = Products.FirstOrDefault(x => x.Id == pr.Id);
        if (product is null)
            return Results.NotFound("Product not found");
        product.Name = pr.Name;
        product.Price = pr.Price;
        
        return Results.Ok(product);
    }

    private static async Task<IResult> Delete([FromRoute] int id)
    {
        var removeCount = Products.RemoveAll(p => p.Id == id);
        return removeCount > 0 ? Results.NoContent() : Results.NotFound();
    }
}