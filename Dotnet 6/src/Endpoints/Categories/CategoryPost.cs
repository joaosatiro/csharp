using IWantApp.Domain.Products;
using IWantApp.Infra.Data;

namespace IWantApp.Endpoints.Categories;

public class CategoryPost
{
    public static string Template => "/categories";
    public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
    public static Delegate Handle => Action;

    public static async Task<IResult> Action(CategoryRequest categoryRequest, HttpContext http, ApplicationDbContext context)
    {
        var category = new Category
        {
            Name = categoryRequest.Name,
        };

        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        return Results.Created($"/categories/{category.Id}", category.Id);
    }
}
