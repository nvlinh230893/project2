using Elect.Data.EF.Models;

namespace WebApp.Data.Models;

public class ProductEntity : Entity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
