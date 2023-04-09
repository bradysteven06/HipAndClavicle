namespace HipAndClavicle.Data
{
    public interface IHipRepo
    {
        public Task CreateOrderAsync(Order order);
        public Task CreateProductAsync(Product product);
        public Task DeleteOrderAsync(Order order);
        public Task DeleteProductAsync(Product product);
        public Task<List<Order>> GetAdminCurrentOrdersAsync();
        public Task<List<Product>> GetAvailableProductsAsync();
        public Task<Order?> GetOrderById(int id);
        public Task<Product> GetProductById(int id);
        public Task UpdateOrderAsync(Order order);
        public Task UpdateProductAsync(Product product);
    }
}