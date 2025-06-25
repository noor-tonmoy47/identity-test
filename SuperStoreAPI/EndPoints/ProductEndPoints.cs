using Microsoft.AspNetCore.Mvc;
using SuperStoreAPI.Models;
using SuperStoreAPI.Services.ProductService;

namespace SuperStoreAPI.EndPoints;

public static class ProductEndPoints
{
    public static void MapEndPoints(this IEndpointRouteBuilder app)
    {
        var productGroup = app.MapGroup("api/v1/products").RequireAuthorization();
        
        productGroup.MapGet("", Get).WithName("Get");
        productGroup.MapPost("", Post).WithName("Post");
        productGroup.MapGet("{id}", GetById).WithName("GetById");
        productGroup.MapPut("{id}", Put).WithName("Put");
        productGroup.MapDelete("{id}", Delete).WithName("Delete");
    }

    private static async Task<IResult> Get(IProductService productService)
    {
        var listOfProducts = await productService.GetProductsAsync();
        return Results.Ok(listOfProducts);
    }

    private static async Task<IResult> Post(IProductService productService, [FromBody] Product? pr)
    {
        var createdProduct = await productService.CreateProductAsync(pr);
        if (createdProduct is null)
            return Results.BadRequest();
        return Results.CreatedAtRoute("GetById", new { id = pr.Id }, createdProduct);
    }

    private static async Task<IResult> GetById(IProductService productService, [FromRoute] int id)
    {
        var product = await productService.GetProductByIdAsync(id);
        return product == null ? Results.NotFound("Product Not Found") : Results.Ok(product);
    }

    private static async Task<IResult> Put(IProductService productService, [FromBody] Product? pr)
    {
       var updatedProduct = await productService.UpdateProductAsync(pr);
       return updatedProduct is null ? Results.BadRequest() : Results.Ok(updatedProduct);
    }

    private static async Task<IResult> Delete(IProductService productService, [FromRoute] int id)
    {
        var isDeleted = await productService.DeleteProductAsync(id);
        return isDeleted ? Results.NoContent() : Results.NotFound();
    }
}