namespace SportShopV1.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public int DeliveryPointId { get; set; }
    public string ClientName { get; set; } = null!;
    public int? PickupCode { get; set; }
    public int StatusId { get; set; }

    public DeliveryPoint DeliveryPoint { get; set; } = null!;
    public Status Status { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
