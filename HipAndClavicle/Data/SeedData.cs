using Color = HipAndClavicle.Models.Color;
namespace HipAndClavicle.Data;

public static class SeedData
{
    static ApplicationDbContext? _context;
    static UserManager<AppUser>? _userManager;

    public static async Task Init(IServiceProvider services, ApplicationDbContext context)
    {
        _context = context;
        _userManager = services.GetRequiredService<UserManager<AppUser>>();
        await Task.CompletedTask;
    }

    public static async Task SeedUsers()
    {

        if (_userManager!.Users.Any())
        {
            return;
        }
        AppUser michael = new()
        {
            UserName = "michael123",
            Email = "paulsonM@my.lanecc.edu",
            EmailConfirmed = true,
            FName = "Michael",
            LName = "Pauslon",
            Address = "123 fake st. Eugene, OR 97448"
        };

        AppUser devin = new()
        {
            UserName = "dfreem987",
            Email = "freemand@my.lanecc.edu",
            EmailConfirmed = true,
            FName = "Devin",
            LName = "Freeman",
            Address = "123 fake st. Eugene, OR 97448"
        };

        AppUser steven = new()
        {
            UserName = "steven123",
            Email = "bradyS@my.lanecc.edu",
            EmailConfirmed = true,
            FName = "Steven",
            LName = "Brady",
            Address = "123 fake st. Eugene, OR 97448"
        };

        AppUser nehemiah = new()
        {
            UserName = "nehemiah123",
            Email = "johnn@my.lanecc.edu",
            EmailConfirmed = true,
            FName = "Nehemiah",
            LName = "John",
            Address = "123 fake st. Eugene, OR 97448"
        };
        _ = await _userManager!.CreateAsync(devin, "!BassCase987");
        _ = await _userManager!.CreateAsync(nehemiah, "@Password123");
        _ = await _userManager!.CreateAsync(michael, "@Password123");
        _ = await _userManager!.CreateAsync(steven, "@Password123");
    }
    public static async Task SeedColors()
    {
        if (_context!.NamedColors.Any())
        {
            return;
        }
        Color blue = new()
        {
            ColorName = "blue",
            HexValue = "#0000ff",
            RGB = (0, 0, 255)
        };
        _ = await _context!.NamedColors.AddAsync(blue);
        Color red = new()
        {
            ColorName = "red",
            HexValue = "#ff0000",
            RGB = (255, 0, 0)
        };
        _ = await _context.NamedColors.AddAsync(red);
        Color green = new()
        {
            ColorName = "green",
            HexValue = "#00ff00",
            RGB = (0, 255, 0)
        };
        _ = await _context.NamedColors.AddAsync(green);
        _ = await _context.SaveChangesAsync();
    }

    public static async Task SeedProducts()
    {
        if (_context!.Products.Any())
        {
            return;
        }
        var devin = await _userManager!.FindByNameAsync("dfreem987");
        var steven = await _userManager!.FindByNameAsync("steven123");
        var michael = await _userManager!.FindByNameAsync("michael123");
        var nehemiah = await _userManager!.FindByNameAsync("nehemiah123");
        // Create a new products
        Product butterfly = new()
        {
            Category = ProductCategory.ButterFlys,
            Name = "Butterfly Test",
            InStock = true,
            QuantityOnHand = 100,
            QuantityOrdered = 3,
            Reviews = new List<Review>()
            {
                new(){ Message = "This product is awesome"}
            }
            // TODO add a check to see if the app user has purchased the product before being able to leave a review.
        };
        _context.Products.Add(butterfly);

        Product dragon = new()
        {
            Category = ProductCategory.Dragons,
            Name = "Dragon Test",
            InStock = true,
            QuantityOnHand = 100,
            QuantityOrdered = 7,
            Reviews = new List<Review>()
            {
                new(){ Message = "I never ordered this!!!" }
            }
        };
        _ = await _context.Products.AddAsync(dragon);
        Product dragonfly = new()
        {
            Category = ProductCategory.Dragonflys,
            Name = "Butterfly",
            InStock = true,
            QuantityOnHand = 100
        };
        _ = await _context.Products.AddAsync(dragonfly);
        _ = await _context.SaveChangesAsync();
    }

    public static async Task SeedItems()
    {
        if (_context!.OrderItems.Any())
        {
            return;
        }
        
        // Create Orders from the products
        var _item1 = await _context.Products.FirstOrDefaultAsync(p => p.Category.Equals(ProductCategory.ButterFlys));
        var _item2 = await _context.Products.FirstOrDefaultAsync(p => p.Category.Equals(ProductCategory.Dragons));
        var _item3 = await _context.Products
            .FirstOrDefaultAsync(p => p.Category
                .Equals(ProductCategory.Dragonflys));

        OrderItem item1 = new()
        {
            Item = _item1!,
            ItemType = _item1!.Category
        };
        _ = await _context.OrderItems.AddAsync(item1);
        _context.Products.Update(_item1);
        OrderItem item2 = new()
        {
            Item = _item2!,
            //ItemColor = _item2!.ColorOptions[0],
            ItemType = _item2!.Category
        };
        _ = await _context.OrderItems.AddAsync(item2);
        _context.Products.Update(_item2);
        OrderItem item3 = new()
        {
            Item = _item3!,
            ItemType = _item3!.Category
        };
        _ = await _context.OrderItems.AddAsync(item3);
        _context.Products.Update(_item3);
    }

    public static async Task SeedOrders()
    {
        if (await _context!.Orders.AnyAsync()) { return; }

        var item1 = await _context.OrderItems.Include(o => o.Item).Include(o => o.ItemColor).FirstOrDefaultAsync(o => o.Item.Category.Equals(ProductCategory.ButterFlys));
        var item2 = await _context.OrderItems.Include(o => o.Item).Include(o => o.ItemColor).FirstOrDefaultAsync(o => o.Item.Category.Equals(ProductCategory.Dragons));
        var item3 = await _context.OrderItems.Include(o => o.Item).Include(o => o.ItemColor).FirstOrDefaultAsync(o => o.Item.Category.Equals(ProductCategory.Dragonflys));
        
        var devin = await _userManager!.FindByNameAsync("dfreem987");
        var steven = await _userManager!.FindByNameAsync("steven123");
        var michael = await _userManager!.FindByNameAsync("michael123");
        var nehemiah = await _userManager!.FindByNameAsync("nehemiah123");
        
        Order order1 = new()
        {
            Items = new() { item1!, item2!, item3! },
            DateOrdered = DateTime.Now,
            PurchaserId = devin!.Id,
            ShippingAddress = devin!.Address!,
            TotalPrice = 25.00d,

        };
        _ = await _context.Orders.AddAsync(order1);
        Order order2 = new()
        {
            Items = new() { item1!, item2!, item3! },
            DateOrdered = DateTime.Now,
            PurchaserId = michael!.Id,
            ShippingAddress = michael!.Address!,
            TotalPrice = 125.00d,
        };
        _ = await _context.Orders.AddAsync(order2);
        Order order3 = new()
        {
            Items = new() { item1!, item2!, item3! },
            DateOrdered = DateTime.Now,
            PurchaserId = steven!.Id,
            ShippingAddress = steven!.Address!,
            TotalPrice = 25.00d,

        };
        _ = await _context.Orders.AddAsync(order3);
        Order order4 = new()
        {
            Items = new() { item1!, item2!, item3! },
            DateOrdered = DateTime.Now,
            PurchaserId = nehemiah!.Id,
            ShippingAddress = nehemiah!.Address!,
            TotalPrice = 125.00d
        };
        _ = await _context.Orders.FindAsync(order4);
        _ = await _context.SaveChangesAsync();

        _ = await _userManager!.UpdateAsync(steven);
        _ = await _userManager!.UpdateAsync(devin);
        _ = await _userManager!.UpdateAsync(michael);
        _ = await _userManager!.UpdateAsync(nehemiah);

    }
}