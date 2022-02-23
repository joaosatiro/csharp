using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddDbContext<ApplicationDbContext>();

app.MapGet("/", () => "Hello World!");
app.MapGet("/user", () => "Sátiro!");
app.MapGet("/addHeader", (HttpResponse response) => response.Headers.Add("Teste", "Satiro"));

app.MapPost("/saveproduct", (Product product) => {
    ProductRepository.Add(product);
    return Results.Created("/products/" + product.Code, product.Code);
});
app.MapGet("/products", ([FromQuery] string dateStart, [FromQuery] string dateEnd) => {
    return dateStart + " - " + dateEnd;
});
app.MapGet("/products/{code}", ([FromRoute] string code) => {
    var product = ProductRepository.GetBy(code);
    if (product is not null)
        return Results.Ok(product);
    return Results.NotFound();
});
app.MapGet("/getproductheader", (HttpRequest request) => {
    return request.Headers["product-code"].ToString();
});
app.MapPut("/products", (Product product) => {
    var productSave = ProductRepository.GetBy(product.Code);
    productSave.Name = product.Name;
    Results.Ok();
});
app.MapDelete("/products/{code}", ([FromRoute] string code) => {
    var productSave = ProductRepository.GetBy(code);
    ProductRepository.Remove(productSave);
    Results.Ok();
});

app.Run();

public static class ProductRepository {
    public static List<Product> Products {get; set;}

    public static void Add(Product product) {
        if(product is null)
            Products = new List<Product>();
                    
        Products.Add(product);
    }

    public static Product GetBy(string code) {
        return Products.FirstOrDefault(p => p.Code == code);
    }

    public static void Remove(Product product) {
        Products.Remove(product);
    }
}

public class Category {
    public int Id { get; set; }
    public string Name { get; set; }
}

public class Tag {
    public int Id { get; set; }
    public string Name { get; set; }
    public int ProductId { get; set; }
}

public class Product {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }    
    public List<Tag> Tags { get; set; }    
}

public class ApplicationDbContext : DbContext {
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>().Property(p => p.Description).HasMaxLength(500).IsRequired(false);
        builder.Entity<Product>().Property(p => p.Name).HasMaxLength(120).IsRequired();
        builder.Entity<Product>().Property(p => p.Code).HasMaxLength(20).IsRequired();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer("Server=localhost;Database=Products;User Id=sa;Password=@Sql2019;MultipleActiveResultSets=true;Encrypt=YES;TrustServerCertificate=YES");
}
