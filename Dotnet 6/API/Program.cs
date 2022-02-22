using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

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

public class Product {
    public string Code { get; set; }
    public string Name { get; set; }
}
