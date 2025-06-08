using Inventory.Contexts;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Repositories
{
    public class CategoryAddRequestRepository : Repository<string, CategoryAddRequest>
    {
        public CategoryAddRequestRepository(InventoryContext _context) : base(_context)
        {

        }

        public override async Task<CategoryAddRequest> GetById(string Id)
        {
            var u = await context.CategoryAddRequests.FirstOrDefaultAsync(u => u.Id == Id);
            return u ?? throw new Exception("No such requests");
        }

        public override async Task<IEnumerable<CategoryAddRequest>> GetAll()
        {
            return await context.CategoryAddRequests.ToListAsync();
        }
        public override async Task<CategoryAddRequest> GetByName(string s)
        {
            var c = await context.CategoryAddRequests.FirstOrDefaultAsync(c => c.CategoryName == s.ToUpper());
            return c;
        }
    }
}