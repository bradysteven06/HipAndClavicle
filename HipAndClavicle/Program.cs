using HipAndClavicle.UtilityClasses;

var builder = WebApplication.CreateBuilder(args);
string? connectionString;
// Add services to the container.

connectionString = builder.Configuration.GetConnectionString("MYSQL_CONNECTION");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, MySqlServerVersion.Parse("mysql-8.0")));

#region Repositories
builder.Services.AddTransient<IAdminRepo, AdminRepo>();
builder.Services.AddTransient<ICustRepo, CustRepo>();
builder.Services.AddTransient<IShippingRepo, ShippingRepo>();
builder.Services.AddTransient<IShoppingCartRepo, ShoppingCartRepo>();
builder.Services.AddTransient<IProductRepo, ProductRepo>();
builder.Services.AddTransient<IOrderRepo, OrderRepo>();
builder.Services.AddTransient<IAccountRepo, AccountRepo>();
#endregion

builder.Services.AddHttpContextAccessor();

#region Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

#endregion

#region Toast Notifications

builder.Services.AddNotyf(configure =>
{
    configure.DurationInSeconds = 4;
    configure.HasRippleEffect = true;
    configure.IsDismissable = true;
    configure.Position = NotyfPosition.TopRight;
});

#endregion

// Load configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");
// Register the CookieUtility class with the DI container
builder.Services.AddScoped<CookieUtility>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseNotyf();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

#region Seed Data
using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    await SeedUsers.Seed(services);
    await SeedRoles.SeedAdminRole(services);
    await SeedRoles.KeepMessagesWorking(services);
    await SeedData.Seed(services, context);
    await SeedListings.Seed(services, context);
    await SeedShoppingCart.Seed(context, services);

}
#endregion
app.Run();

