﻿
using HipAndClavicle.Models;

namespace HipAndClavicle.Repositories
{
    public class ShoppingCartRepo : IShoppingCartRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ShoppingCartRepo(ApplicationDbContext context, UserManager<AppUser> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ShoppingCart> GetShoppingCartAsync(string cartId)
        {

            
            var shoppingCart = await _context.ShoppingCarts
            .Include(cart => cart.ShoppingCartItems)
            .ThenInclude(item => item.ListingItem)
            .FirstOrDefaultAsync(cart => cart.CartId == cartId);

            /*var owner = await _userManager.FindByIdAsync(ownerId);
            shoppingCart = new ShoppingCart { CartId = cartId, Owner = owner };
            _context.ShoppingCarts.Add(shoppingCart);
            await _context.SaveChangesAsync();*/
            return shoppingCart;
                
        }

        public string GetCartIdFromDB(string ownerId)
        {
            var cartId = _context.ShoppingCarts
            .Where(cart => cart.Owner.Id == ownerId)
            .Select(cart => cart.CartId)
            .FirstOrDefault();

            return cartId;
        }

        public async void CreateShoppingCartAsync(string cartId)
        {
            var shoppingCart = new ShoppingCart { CartId = cartId };
            _context.ShoppingCarts.Add(shoppingCart);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ShoppingCartItemViewModel>> GetShoppingCartItemsAsync(IEnumerable<ShoppingCartItem> items)
        {
            var viewModels = new List<ShoppingCartItemViewModel>();

            foreach (var item in items)
            {
                var viewModel = new ShoppingCartItemViewModel(item);
                viewModels.Add(viewModel);
            }

            return viewModels;
        }

        public async Task<ShoppingCartItem> GetCartItem(int id)
        {
            return await _context.ShoppingCartItems
                .Include(item => item.ListingItem)
                .FirstOrDefaultAsync(item => item.ShoppingCartItemId == id);
        }
        public async Task AddShoppingCartItemAsync(ShoppingCartItem item)
        {
            // Check if the listing is already in the cart
            var existingItem = await _context.ShoppingCartItems
                .FirstOrDefaultAsync(i => i.ShoppingCartId == item.ShoppingCartId && i.ListingItem.ListingId == item.ListingItem.ListingId);

            if (existingItem == null)
            {
                // Add the listing to the cart
                await _context.ShoppingCartItems.AddAsync(item);
            }
            else
            {
                // Increment the quantity of the listing in the cart
                existingItem.Quantity += item.Quantity;
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(ShoppingCartItem item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(ShoppingCartItem item)
        {
            _context.ShoppingCartItems.Remove(item);
            await _context.SaveChangesAsync();            
        }

        public async Task ClearShoppingCartAsync(string cartId)
        {
            ShoppingCart shoppingCart = await GetShoppingCartAsync(cartId);

            _context.ShoppingCartItems.RemoveRange(shoppingCart.ShoppingCartItems);
            await _context.SaveChangesAsync();
        }
    }
}
