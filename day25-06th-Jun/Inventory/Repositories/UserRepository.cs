using Inventory.Contexts;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Repositories
{
    public class UserRepository : Repository<string, User>
    {
        public UserRepository(InventoryContext _context) : base(_context)
        {

        }

        public override async Task<User> GetById(string Id)
        {
            var u = await context.Users.FirstOrDefaultAsync(u => u.Id == Id);
            return u ?? throw new Exception("No such user");
        }

        public override async Task<IEnumerable<User>> GetAll()
        {
            return await context.Users.ToListAsync();
        }
        public override async Task<User> GetByName(string s)
        {
            throw new NotImplementedException("No need");
        }
    }
}