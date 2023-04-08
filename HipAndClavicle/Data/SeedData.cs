using Color = HipAndClavicle.Models.Color;
namespace HipAndClavicle.Data;

public static class SeedData
{
    public static async Task Init(System.IServiceProvider services)
    {
        ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();
        UserManager<AppUser> userManager = services.GetRequiredService<UserManager<AppUser>>();

        if (userManager.Users.Any())
        {
            return;
        }
        await SeedUsers(userManager);
        await SeedColors(context);
        await SeedProducts(context, services);
    }
    static async Task SeedUsers(UserManager<AppUser> userManager)
    {
        AppUser michael = new()
        {
            UserName = "michael123",
            Email = "paulsonM@my.lanecc.edu",
            EmailConfirmed = true,
            FName = "Michael",
            LName = "Pauslon",
            Address = new()
            {
                Line1 = "123 Fake st",
                City = "Eugene",
                State = "OR",
                ZipCode = 97746
            }
        };

        AppUser devin = new()
        {
            UserName = "dfreem987",
            Email = "freemand@my.lanecc.edu",
            EmailConfirmed = true,
            FName = "Devin",
            LName = "Freeman",
            Address = new()
            {
                Line1 = "456 Fake st",
                City = "Eugene",
                State = "OR",
                ZipCode = 97746
            }
        };

        AppUser steven = new()
        {
            UserName = "steven123",
            Email = "bradyS@my.lanecc.edu",
            EmailConfirmed = true,
            FName = "Steven",
            LName = "Brady",
            Address = new()
            {
                Line1 = "789 Fake st",
                City = "Eugene",
                State = "OR",
                ZipCode = 97746
            }
        };

        AppUser nehemiah = new()
        {
            UserName = "nehemiah123",
            Email = "johnn@my.lanecc.edu",
            EmailConfirmed = true,
            FName = "Nehemiah",
            LName = "John",
            Address = new()
            {
                Line1 = "1011 Fake st",
                City = "Eugene",
                State = "OR",
                ZipCode = 97746
            }
        };
        await userManager.CreateAsync(devin, "!BassCase987");
        await userManager.CreateAsync(nehemiah, "@Password123");
        await userManager.CreateAsync(michael, "@Password123");
        await userManager.CreateAsync(steven, "@Password123");
    }
    static async Task SeedColors(ApplicationDbContext context)
    {
        Color blue = new()
        {
            ColorName = "blue",
            HexValue = "#0000ff",
            RGB = (0, 0, 255)
        };
        Color red = new()
        {
            ColorName = "red",
            HexValue = "#ff0000",
            RGB = (255, 0, 0)
        };
        Color green = new()
        {
            ColorName = "green",
            HexValue = "#00ff00",
            RGB = (0, 255, 0)
        };
        await context.Colors.AddRangeAsync(new List<Color>()
        {
            blue,
            red,
            green
        });
        await context.SaveChangesAsync();
    }

    static async Task SeedProducts(ApplicationDbContext context, IServiceProvider services)
    {
        if (context.Products.Any())
        {
            return;
        }
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var devin = await userManager.FindByNameAsync("dfreem987");
        var steven = await userManager.FindByNameAsync("steven123");
        var michael = await userManager.FindByNameAsync("michael123");
        var nehemiah = await userManager.FindByNameAsync("nehemiah123");
        // Create a new products
        Product butterfly = new()
        {
            Category = ProductCategory.ButterFlys,
            Name = "Butterfly Test",
            ColorOptions = new List<Color>()
            {
                context.Colors.First(c => c.ColorName == "blue"),
                context.Colors.First(c => c.ColorName == "green")
            },
            InStock = true,
            QuantityOnHand = 100,
            QuantityOrdered = 3,
            Reviews = new List<Review>()
            {
                new(){ Message = "This product is awesome"}
            }
            // TODO add a check to see if the app user has purchased the product before being able to leave a review.
        };
        Product dragon = new()
        {
            Category = ProductCategory.Dragons,
            Name = "Dragon Test",
            ColorOptions = new List<Color>()
            {
                context.Colors.First(c => c.ColorName == "red"),
                context.Colors.First(c => c.ColorName == "green")
            },
            InStock = true,
            QuantityOnHand = 100,
            QuantityOrdered = 7,
            Reviews = new List<Review>()
            {
                new(){ Message = "I never ordered this!!!" }
            }
        };
        Product dragonfly = new()
        {
            Category = ProductCategory.Dragonflys,
            Name = "Butterfly",
            ColorOptions = new List<Color>()
            {
                context.Colors.First(c => c.ColorName == "blue"),
            },
            InStock = true,
            QuantityOnHand = 100
        };

        await context.Products.AddRangeAsync(butterfly, dragon, dragonfly);
        await context.SaveChangesAsync();
    }

    static async Task SeedItems(ApplicationDbContext context, IServiceProvider services)
    { // Create Orders from the products
        var _item1 = await context.Products.Include(p => p.ColorOptions).FirstOrDefaultAsync(p => p.Category.Equals(ProductCategory.ButterFlys));
        var _item2 = await context.Products.Include(p => p.ColorOptions).FirstOrDefaultAsync(p => p.Category.Equals(ProductCategory.Dragons));
        var _item3 = await context.Products.Include(p => p.ColorOptions).FirstOrDefaultAsync(p => p.Category.Equals(ProductCategory.Dragonflys));

        OrderItem item1 = new()
        {
            Item = _item1!,
            ItemColor = _item1.ColorOptions[0],
            ItemType = _item1.Category
        };

        OrderItem item2 = new()
        {
            Item = _item2!,
            ItemColor = _item2.ColorOptions[0],
            ItemType = _item2.Category
        };

        OrderItem item3 = new()
        {
            Item = _item3!,
            ItemColor = _item3.ColorOptions[0],
            ItemType = _item3.Category
        };
        await context.OrderItems.AddRangeAsync(item1, item2, item3);
        await context.SaveChangesAsync();
    }

    static async Task SeedOrders(ApplicationDbContext context, IServiceProvider services)
    {
        var item1 = await context.OrderItems.Include(o => o.Item).Include(o => o.ItemColor).FirstOrDefaultAsync(o => o.Item.Category.Equals(ProductCategory.ButterFlys));
        var item2 = await context.OrderItems.Include(o => o.Item).Include(o => o.ItemColor).FirstOrDefaultAsync(o => o.Item.Category.Equals(ProductCategory.Dragons));
        var item3 = await context.OrderItems.Include(o => o.Item).Include(o => o.ItemColor).FirstOrDefaultAsync(o => o.Item.Category.Equals(ProductCategory.Dragonflys));
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var devin = await userManager.FindByNameAsync("dfreem987");
        var steven = await userManager.FindByNameAsync("steven123");
        var michael = await userManager.FindByNameAsync("michael123");
        var nehemiah = await userManager.FindByNameAsync("nehemiah123");
        Order order1 = new()
        {
            Items = new() { item1!, item2!, item3! },
            DateOrdered = DateTime.Now,
            PurchaserId = devin!.Id,
            ShippingAddress = devin!.Address!,
            TotalPrice = 25.00d,

        };
        Order order2 = new()
        {
            Items = new() { item1!, item2!, item3! },
            DateOrdered = DateTime.Now,
            PurchaserId = michael!.Id,
            ShippingAddress = michael!.Address!,
            TotalPrice = 125.00d,
        };
        Order order3 = new()
        {
            Items = new() { item1!, item2!, item3! },
            DateOrdered = DateTime.Now,
            PurchaserId = steven!.Id,
            ShippingAddress = steven!.Address!,
            TotalPrice = 25.00d,

        };
        Order order4 = new()
        {
            Items = new() { item1!, item2!, item3! },
            DateOrdered = DateTime.Now,
            PurchaserId = nehemiah!.Id,
            ShippingAddress = nehemiah!.Address!,
            TotalPrice = 125.00d
        };
        await context.Orders.AddRangeAsync(order1, order2, order3, order4);
        await context.SaveChangesAsync();
  
        await userManager.UpdateAsync(steven);
        await userManager.UpdateAsync(devin);
        await userManager.UpdateAsync(michael);
        await userManager.UpdateAsync(nehemiah);

    }
}