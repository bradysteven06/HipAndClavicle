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
    }
    static async Task SeedUsers(UserManager<AppUser> userManager)
    {
        Merchant michael = new()
        {
            UserName = "michael123",
            Email = "paulsonM@my.lanecc.edu",
            EmailConfirmed = true,
            FName = "Michael",
            LName = "Pauslon"
        };

        Merchant devin = new()
        {
            UserName = "dfreem987",
            Email = "freemand@my.lanecc.edu",
            EmailConfirmed = true,
            FName = "Devin",
            LName = "Freeman"
        };

        Customer steven = new()
        {
            UserName = "steven123",
            Email = "bradyS@my.lanecc.edu",
            EmailConfirmed = true,
            FName = "Steven",
            LName = "Brady",
            Address = "123 Fake st, Eugene OR, 97746"
        };
        await userManager.CreateAsync(devin, "!BassCase987");
        await userManager.CreateAsync(michael, "@Password123");
        await userManager.CreateAsync(steven, "@Password456");

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
}