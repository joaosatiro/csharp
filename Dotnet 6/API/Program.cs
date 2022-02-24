using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);
var app = builder.Build();


app.MapGet("/", () => "Hello World!");
app.MapGet("/user", () => "Sátiro!");
app.MapGet("/addHeader", (HttpResponse response) => response.Headers.Add("Teste", "Satiro"));

app.MapPost("/products", (ProductRequest request, ApplicationDbContext context) => {
    var category = context.Category.Where(x => x.Id == request.CategoryId).First();
    var product = new Product {
        Code = request.Code,
        Name = request.Name,
        Description = request.Description,
        Category = category
    };
    context.Products.Add(product);
    context.SaveChanges();
    return Results.Created("/products/" + product.Id, product.Id);
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
