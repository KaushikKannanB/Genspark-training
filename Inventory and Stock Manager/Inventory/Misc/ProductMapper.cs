using Inventory.Models;
using Inventory.Models.DTOs;

namespace Inventory.Misc
{
    public class ProductMapper
    {
        public Product? MapProduct(ProductAddRequest request)
        {
            Product p = new();
            p.Name = request.Name;
            p.Description = request.Description;
            p.Price = request.price;
            p.AddedAt = DateTime.UtcNow;
            p.Status = "ACTIVE";

            return p;
        }
    }
}