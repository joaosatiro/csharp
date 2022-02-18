var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/user", () => "Sátiro!");
app.MapGet("/addHeader", (HttpResponse response) => response.Headers.Add("Teste", "Satiro"));

app.Run();
