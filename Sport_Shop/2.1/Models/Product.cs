namespace SportShopV1.Models;

public class Product
{
    public int Id { get; set; }
    public string Article { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int CategoryId { get; set; }
    public int ManufacturerId { get; set; }
    public int SupplierId { get; set; }
    public decimal Price { get; set; }
    public int MeasureId { get; set; }
    public decimal Discount { get; set; }
    public int Stock { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }

    public Category Category { get; set; } = null!;
    public Manufacturer Manufacturer { get; set; } = null!;
    public Supplier Supplier { get; set; } = null!;
    public Measure Measure { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public decimal PriceDiscounted => Math.Round(Price * (1 - Discount / 100m), 2);
}
