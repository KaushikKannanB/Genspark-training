using Inventory.Models;
using Inventory.Models.DTOs;

namespace Inventory.Interfaces
{
    public interface IProductService
    {
        Task<Product> AddProduct(ProductAddRequest request);
        Task<Inventories> StockUpdate(StockUpdateDTO request);
        Task<Product> UpdateProductByPrice(UpdateProductPriceDTO request);
        Task<Product> UpdateProductByDescription(UpdateProductDescriptionDTO request);

        Task<Product> DeleteProduct(string product);

        Task<IEnumerable<Product>> GetProductsPaginated(int pagenumber);

    }
}