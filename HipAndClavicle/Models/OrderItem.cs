<<<<<<< HEAD
﻿namespace HipAndClavicle.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public Color? ItemColor { get; set; } = default!;
        public bool IsPulled { get; set; }
        public bool IsCut { get; set; }
        public bool IsFolded { get; set; }
        public bool IsStickered { get; set; }
        public Product Item { get; set; } = default!;
        public ProductCategory ItemType { get; set; } = default!; 
    }
}
=======
﻿namespace HipAndClavicle.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public Color? ItemColor { get; set; } = default!;
        public bool IsPulled { get; set; }
        public bool IsCut { get; set; }
        public bool IsFolded { get; set; }
        public bool IsStickered { get; set; }
        public Product? Item { get; set; } = default!;
        public ProductCategory ItemType { get; set; } = default!; 
    }
}
>>>>>>> f6b757a49d1eddb176cd701cc052bdbe39ddf702
