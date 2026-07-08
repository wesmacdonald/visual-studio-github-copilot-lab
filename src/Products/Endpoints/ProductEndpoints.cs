using DataEntities;
using Microsoft.EntityFrameworkCore;
using Products.Data;

namespace Products.Endpoints;

public static class ProductEndpoints
{
    /// <summary>
    /// /api/Product endpoints
    /// </summary>
    /// <param name="routes"></param>
    public static void MapProductEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Product");

        group.MapGet("/", async (ProductDataContext db) =>
        {
            return await db.Product.ToListAsync();
        })
        .WithName("GetAllProducts")
        .Produces<List<Product>>(StatusCodes.Status200OK);

        group.MapGet("/{productId}", async (int productId, ProductDataContext db) =>
        {
            return await db.Product.FindAsync(productId)
                is Product model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithName("GetProductById")
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", async (Product product, ProductDataContext db) =>
        {
            if (string.IsNullOrWhiteSpace(product.Name))
                return Results.BadRequest("Product name is required");

            db.Product.Add(product);
            await db.SaveChangesAsync();
            return Results.Created($"/api/Product/{product.Id}", product);
        })
        .WithName("CreateProduct")
        .Produces<Product>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/{id}", async (int id, Product productUpdate, ProductDataContext db) =>
        {
            if (string.IsNullOrWhiteSpace(productUpdate.Name))
                return Results.BadRequest("Product name is required");

            var product = await db.Product.FindAsync(id);
            if (product is null)
                return Results.NotFound();

            product.Name = productUpdate.Name;
            product.Description = productUpdate.Description;
            product.Price = productUpdate.Price;
            product.ImageUrl = productUpdate.ImageUrl;

            await db.SaveChangesAsync();
            return Results.Ok(product);
        })
        .WithName("UpdateProduct")
        .Produces<Product>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id}", async (int id, ProductDataContext db) =>
        {
            var product = await db.Product.FindAsync(id);
            if (product is null)
                return Results.NotFound();

            db.Product.Remove(product);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithName("DeleteProduct")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}
