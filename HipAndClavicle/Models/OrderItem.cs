namespace HipAndClavicle.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        [Required]
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Order Order { get; set; } = default!;
        public int ItemColor { get; set; }
    }
}
