using Inventory.Contexts;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Repositories
{
    public class InventoryRepository : Repository<string, Inventories>
    {
        public InventoryRepository(InventoryContext _context) : base(_context)
        {

        }

        public override async Task<Inventories> GetById(string Id)
        {
            var u = await context.Inventories.FirstOrDefaultAsync(u => u.Id == Id);
            return u ?? throw new Exception("No such user");
        }

        public override async Task<IEnumerable<Inventories>> GetAll()
        {
            return await context.Inventories.ToListAsync();
        }
        public override async Task<Inventories> GetByName(string s)
        {
            var c = await context.Products.FirstOrDefaultAsync(p => p.Name == s.ToUpper().Trim());
            var inv = await context.Inventories.FirstOrDefaultAsync(i => i.Id == c.InventoryId);
            return inv;
        }
    }
}