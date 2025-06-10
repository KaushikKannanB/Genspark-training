using Inventory.Contexts;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Repositories
{
    public class ManagerRepository : Repository<string, Manager>
    {
        public ManagerRepository(InventoryContext _context) : base(_context)
        {

        }

        public override async Task<Manager> GetById(string id)
        {
            Console.WriteLine($"📦 GetById called with ID: {id}");
            var manager = await context.Managers.FirstOrDefaultAsync(m => m.Id == id);
            Console.WriteLine(manager == null ? "❌ Manager not found" : $"✅ Manager found: {manager.Id}");
            return manager;
        }

        public override async Task<IEnumerable<Manager>> GetAll()
        {
            return await context.Managers.ToListAsync();
        }
        public override async Task<Manager> GetByName(string s)
        {
            throw new NotImplementedException("No need");
        }
    }
}