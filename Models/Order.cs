
namespace HipAndClavicle.Models;

public class Order
{
    public int OrderId { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    public int? PurchacerId { get; set; }
    public bool IsPaid { get; set; } = false;
    public bool IsShipped { get; set; } = false;
    public DateTime DateOrdered { get; set; } = DateTime.Now;
    public int? ShipmentId { get; set; } = default!;
    public double TotalPrice { get; set; } = 0D;
    public string ShippingAddress { get; set; } = default!;
}
