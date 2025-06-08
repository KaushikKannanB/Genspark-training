using Inventory.Contexts;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Repositories
{
    public class ProductUpdateRepository : Repository<string, ProductUpdateLog>
    {
        public ProductUpdateRepository(InventoryContext _context) : base(_context)
        {

        }

        public override async Task<ProductUpdateLog> GetById(string Id)
        {
            var u = await context.ProductUpdateLogs.FirstOrDefaultAsync(u => u.Id == Id);
            return u ?? throw new Exception("No such user");
        }

        public override async Task<IEnumerable<ProductUpdateLog>> GetAll()
        {
            return await context.ProductUpdateLogs.ToListAsync();
        }
        public override async Task<ProductUpdateLog> GetByName(string productName)
        {
            throw new NotImplementedException();
        }
    }
}