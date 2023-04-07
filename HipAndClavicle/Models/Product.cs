using Color = HipAndClavicle.Models.Color;

namespace HipAndClavicle.Models;

public class Product
{
    public int ProductId { get; set; }
    [Required]
    public string Name { get; set; } = default!;
    public ProductCategory Category { get; set; } = default!;
    public List<Color> Colors { get; set; } = new();
    public bool InStock { get; set; } = default!;
    public int Quantity { get; set; } = default!;
    public List<Review> Reviews { get; set; } = new();

}
