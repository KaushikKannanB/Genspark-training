using Inventory.Contexts;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Repositories
{
    public class ProductRepository : Repository<string, Product>
    {
        public ProductRepository(InventoryContext _context) : base(_context)
        {

        }

        public override async Task<Product> GetById(string Id)
        {
            var u = await context.Products.FirstOrDefaultAsync(u => u.Id == Id);
            return u ?? throw new Exception("No such user");
        }

        public override async Task<IEnumerable<Product>> GetAll()
        {
            return await context.Products.ToListAsync();
        }
        public override async Task<Product> GetByName(string s)
        {
            var c = await context.Products.FirstOrDefaultAsync(p=>p.Name==s.ToUpper());
            return c;
        }
    }
}