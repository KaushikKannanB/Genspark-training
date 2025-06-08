using Inventory.Contexts;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Repositories
{
    public class AdminRepository : Repository<string, Admin>
    {
        public AdminRepository(InventoryContext _context) : base(_context)
        {

        }

        public override async Task<Admin> GetById(string Id)
        {
            var u = await context.Admins.FirstOrDefaultAsync(u => u.Id == Id);
            return u ?? throw new Exception("No such user");
        }

        public override async Task<IEnumerable<Admin>> GetAll()
        {
            return await context.Admins.ToListAsync();
        }
        public override async Task<Admin> GetByName(string s)
        {
            throw new NotImplementedException("No need");
        }
    }
}