using MainMigration.Models;
using MainMigration.Models.DTOs;

namespace MainMigration.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetByproductname(string name);
        Task<Product> AddProduct(AddProductDTO request);
        Task<IEnumerable<Product>> GetFilteredProducts(string cat, string color, string model, string prodname);
    }
}