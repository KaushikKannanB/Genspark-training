using Inventory.Models;

namespace Inventory.Interfaces
{
    public interface IFAQService
    {
        Task<double> TotalSales();
        Task<IEnumerable<Product>> TopProductsUser();
        Task<int> ProductsAdded();
        Task<IEnumerable<Product>> TopProductsGeneral();
        Task<IEnumerable<Category>> TopCategoryGeneral();
        Task<IEnumerable<Product>> LowStockProducts();
        Task<string> GetSpecificQueries(string question);
        
    }
}