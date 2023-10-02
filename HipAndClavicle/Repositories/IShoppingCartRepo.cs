
namespace HipAndClavicle.Repositories
{
    public interface IShoppingCartRepo
    {
        Task<ShoppingCart> GetShoppingCartAsync(string cartId);
        void CreateShoppingCartAsync(string cartId);
        string GetCartIdFromDB(string ownerId);
        Task<List<ShoppingCartItemViewModel>> GetShoppingCartItemsAsync(IEnumerable<ShoppingCartItem> items);
        Task <ShoppingCartItem> GetCartItem(int id);
        Task AddShoppingCartItemAsync(ShoppingCartItem item);
        Task UpdateItemAsync(ShoppingCartItem item);
        Task RemoveItemAsync(ShoppingCartItem item);
        Task ClearShoppingCartAsync(string cartId);
    }
}
