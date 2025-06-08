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

        public override async Task<Manager> GetById(string Id)
        {
            var u = await context.Managers.FirstOrDefaultAsync(u => u.Id == Id);
            return u ?? throw new Exception("No such Manager");
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