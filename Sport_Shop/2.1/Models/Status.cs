namespace SportShopV1.Models;

public class Status
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
