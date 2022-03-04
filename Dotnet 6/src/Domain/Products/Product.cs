namespace IWantApp.Domain.Products;

public class Product : Entity
{
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    public string Description { get; set; }
    public bool Stock { get; set; }
    public bool Active { get; set; } = true;
}
