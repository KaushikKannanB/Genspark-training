using Inventory.Contexts;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Repositories
{
    public class StockUpdateRepository : Repository<string, StockLogging>
    {
        public StockUpdateRepository(InventoryContext _context) : base(_context)
        {

        }

        public override async Task<StockLogging> GetById(string Id)
        {
            var u = await context.StockLogs.FirstOrDefaultAsync(u => u.Id == Id);
            return u ?? throw new Exception("No such user");
        }

        public override async Task<IEnumerable<StockLogging>> GetAll()
        {
            return await context.StockLogs.ToListAsync();
        }
        public override async Task<StockLogging> GetByName(string productName)
        {
            throw new NotImplementedException();
        }
    }
}