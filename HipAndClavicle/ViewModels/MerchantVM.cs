namespace HipAndClavicle.ViewModels;

public class MerchantVM
{
    public Merchant Admin { get; set; }
    public List<Order> CurrentOrders { get; set; } = new List<Order>();
    public List<Order> ShippedOrders { get; set; } = new List<Order>();
}