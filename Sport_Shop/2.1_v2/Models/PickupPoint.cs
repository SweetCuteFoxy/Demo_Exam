namespace SportShopV2.Models;

public class PickupPoint
{
    public int Id { get; set; }
    public string Address { get; set; } = null!;
    public string? Phone { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
