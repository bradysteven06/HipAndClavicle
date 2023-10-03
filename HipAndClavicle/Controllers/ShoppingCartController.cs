using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using STJ = System.Text.Json;

namespace HipAndClavicle.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepo _shoppingCartRepo;
        private readonly ICustRepo _custRepo;
        private readonly IHttpContextAccessor _contextAccessor;
        public const string shoppingCartCookieName = "HnPCartId";

        public ShoppingCartController(IShoppingCartRepo shoppingCartRepository, ICustRepo custRepo, IHttpContextAccessor httpContextAccessor)
        {
            _shoppingCartRepo = shoppingCartRepository;
            _custRepo = custRepo;
            _contextAccessor = httpContextAccessor;
        }

        // Gets cookie with cartId in it
        public string GetCookie(string cookieName)
        {
            return _contextAccessor.HttpContext.Request.Cookies[cookieName];
        }

        // Sets cartId to cookie
        public void SetCookie(string cookieName, string cookieValue)
        {
            _contextAccessor.HttpContext.Response.Cookies.Append(cookieName, cookieValue, new CookieOptions());
        }

        public void DeleteCookie(string cookieName)
        {
            Response.Cookies.Delete(cookieName);
        }

        public void DeleteShoppingCartCookie(string cookieName)
        {
            DeleteCookie(cookieName);
        }

        public void RefreshShoppingCartCookie(string cookieName)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            string ownerId = GetOwnerId();
            string cartId = !(User.Identity.IsAuthenticated) ? GetCookie(shoppingCartCookieName) : _shoppingCartRepo.GetCartIdFromDB(ownerId);
            
            bool needsCart = false;
            if ( cartId == null) 
            {
                cartId = GenerateCartId();
                SetCookie(shoppingCartCookieName, cartId);
                needsCart = true;
            }
            else
            {
                // Sets cartId to cookie in the instance that a user logs in and gets cartId from already existing cart.
                SetCookie(shoppingCartCookieName, cartId);
            }

            
            ShoppingCartViewModel viewModel;

            

            if (needsCart)
            {
                await _shoppingCartRepo.CreateShoppingCartAsync(cartId);
            }
            ShoppingCart shoppingCart = await _shoppingCartRepo.GetShoppingCartAsync(cartId);

            viewModel = new ShoppingCartViewModel
            {
                CartId = shoppingCart.CartId,
                ShoppingCartItems = await _shoppingCartRepo.GetShoppingCartItemsAsync(shoppingCart.ShoppingCartItems),
            };

            return View(viewModel);
        }

        // TODO: For testing adds 1 item to cart. Quantity needs to be set on listing page.
        [HttpPost]
        public async Task<IActionResult> AddToCart(int listingId, int quantity = 1)
        {
            // Get the cart ID
            var cartId = GetCookie(shoppingCartCookieName);
            string ownerId = GetOwnerId();

            // Get the shopping cart using the cart ID
            var shoppingCart = await _shoppingCartRepo.GetShoppingCartAsync(cartId);

            // Find the listing with the given listingId
            var listing = await _custRepo.GetListingByIdAsync(listingId);

            if (listing == null)
            {
                return NotFound();
            }

            // Create a new ShoppingCartItem with the shoppingCartId, listing, and quantity
            var shoppingCartItem = new ShoppingCartItem
            {
                ShoppingCartId = shoppingCart.Id,
                ListingItem = listing,
                Quantity = quantity
            };

            await _shoppingCartRepo.AddShoppingCartItemAsync(shoppingCartItem);


            return RedirectToAction("Index", "ShoppingCart");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart(int itemId, int quantity)
        {

            // Handle logged-in users
            var item = await _shoppingCartRepo.GetCartItem(itemId);
            if (item == null)
            {
                return NotFound();
            }

            item.Quantity = quantity;
            await _shoppingCartRepo.UpdateItemAsync(item);


            return RedirectToAction("Index", "ShoppingCart");
        }

        // Removes single item from cart
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int itemId)
        {
            
            var item = await _shoppingCartRepo.GetCartItem(itemId);
            if (item == null)
            {
                return NotFound();
            }

            await _shoppingCartRepo.RemoveItemAsync(item);
            
            return RedirectToAction("Index", "ShoppingCart");
        }

        // Removes all items from cart
        [HttpPost]
        public async Task<IActionResult> ClearCart(string cartId)
        {

            string ownerId = GetOwnerId();
            await _shoppingCartRepo.ClearShoppingCartAsync(cartId);

            return RedirectToAction("Index", "ShoppingCart");
        }

        // Helper method to get the cartId for the current user
        private string GenerateCartId()
        {
            return Guid.NewGuid().ToString();
        }

        // Helper method to get the ownerId
        private string GetOwnerId()
        {
            var httpContext = _contextAccessor.HttpContext;
            string ownerId = "";
            if (User.Identity.IsAuthenticated)
            {
                ownerId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                
            }
            else
            {
                ownerId = "default";
            }

            return ownerId;
        }
    }

}

