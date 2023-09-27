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
        private readonly string _shoppingCartCookieName = "HnPCartId";

        public ShoppingCartController(IShoppingCartRepo shoppingCartRepository, ICustRepo custRepo, IHttpContextAccessor httpContextAccessor)
        {
            _shoppingCartRepo = shoppingCartRepository;
            _custRepo = custRepo;
            _contextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var httpContext = _contextAccessor.HttpContext;
            string cartId = GetCartId();
            string ownerId = GetOwnerId();
            ShoppingCartViewModel viewModel;

            // Determine if the user is logged in and retrieve the shopping cart accordingly
            /*if (User.Identity.IsAuthenticated)
            {
                string ownerId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                ShoppingCart shoppingCart = await _shoppingCartRepo.GetOrCreateShoppingCartAsync(cartId, ownerId);

                viewModel = new ShoppingCartViewModel
                {
                    CartId = shoppingCart.CartId,
                    ShoppingCartItems = await _shoppingCartRepo.GetShoppingCartItemsAsync(shoppingCart.ShoppingCartItems),
                };
            }
            else
            {
                SimpleShoppingCart simpleShoppingCart = GetShoppingCartFromCookie();

                viewModel = new ShoppingCartViewModel
                {
                    CartId = cartId,
                    ShoppingCartItems = simpleShoppingCart.Items.Select(item => new ShoppingCartItemViewModel(item)).ToList(),
                };
            }*/

            ShoppingCart shoppingCart = await _shoppingCartRepo.GetOrCreateShoppingCartAsync(cartId, ownerId);

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
            var cartId = GetCartId();
            string ownerId = GetOwnerId();

            // Get the shopping cart using the cart ID
            var shoppingCart = await _shoppingCartRepo.GetOrCreateShoppingCartAsync(cartId, ownerId);

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
            await _shoppingCartRepo.ClearShoppingCartAsync(cartId, ownerId);
            
            return RedirectToAction("Index", "ShoppingCart");
        }

        // Helper method to get the cartId for the current user
        private string GetCartId()
        {
            /*var httpContext = _contextAccessor.HttpContext;
            if (httpContext?.User?.Identity?.IsAuthenticated ?? false)
            {
                return httpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            }
            else
            {
                return null;
            }*/
            

            if (User.Identity.IsAuthenticated)
            {
                return Guid.NewGuid().ToString();
            }
            else
            {
                return GetCartIdFromCookie();
            }

        }

        // Helper method to get the cartId from cookie
        private string GetCartIdFromCookie()
        {
            var cartCookie = _contextAccessor.HttpContext.Request.Cookies[_shoppingCartCookieName];
            if (cartCookie == null)
            {
                // If the cart cookie doesn't exist, create new id for cart
                string cartId = Guid.NewGuid().ToString();
                SetCartIdToCookie(cartId);
                return cartId;
            }
            else
            {
                // return the cart id cookie
                return cartCookie;
            }
        }

        // Helper method to save the cartId to a cookie
        private void SetCartIdToCookie(string cartId)
        {
            // Save cartId to a cookie
            _contextAccessor.HttpContext.Response.Cookies.Append(_shoppingCartCookieName, cartId, new CookieOptions()); // Cookie will expire once browser is closed
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

