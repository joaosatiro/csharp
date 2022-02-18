using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/user", () => "Sátiro!");
app.MapGet("/addHeader", (HttpResponse response) => response.Headers.Add("Teste", "Satiro"));
app.MapPost("/saveproduct", (Product product) => {
    ProductRepository.Add(product);
});
app.MapGet("/getproduct", ([FromQuery] string dateStart, [FromQuery] string dateEnd) => {
    return dateStart + " - " + dateEnd;
});
app.MapGet("/getproduct/{code}", ([FromRoute] string code) => {
    var product = ProductRepository.GetBy(code);
});
app.MapGet("/getproductheader", (HttpRequest request) => {
    return request.Headers["product-code"].ToString();
});
app.MapPut("/editproduct", (Product product) => {
    var productSave = ProductRepository.GetBy(product.Code);
    productSave.Name = product.Name;
});
app.MapDelete("/deleteproduct/{code}", ([FromRoute] string code) => {
    var productSave = ProductRepository.GetBy(code);
    ProductRepository.Remove(productSave);
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
