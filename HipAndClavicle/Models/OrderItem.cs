namespace HipAndClavicle.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        [Required]
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Color ItemColor { get; set; }
        public bool IsPulled { get; set; }
        public bool IsCut { get; set; }
        public bool IsFolded { get; set; }
        public bool IsStickered { get; set; }
        public Product Item { get; set; } = default!;
        public ProductCategory ItemType { get; set; } = default!; 
    }
}
