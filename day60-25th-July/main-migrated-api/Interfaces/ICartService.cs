using MainMigration.Models;

namespace MainMigration.Interfaces
{
    public interface ICartService
    {
        Task<Cart> AddtoCart(int productid);
        Task<IEnumerable<Cart>> GetMyCart();
        Task<Order> BuyAllCart();
        Task<Order> BuySpecificItemFromCart(int productid);
        Task<Cart> RemoveItemFromCart(int productid);
    }
}