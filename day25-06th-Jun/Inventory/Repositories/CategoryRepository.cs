using Inventory.Contexts;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Repositories
{
    public class CategoryRepository : Repository<string, Category>
    {
        public CategoryRepository(InventoryContext _context) : base(_context)
        {

        }

        public override async Task<Category> GetById(string Id)
        {
            var u = await context.Categories.FirstOrDefaultAsync(u => u.Id == Id);
            return u ?? throw new Exception("No such user");
        }

        public override async Task<IEnumerable<Category>> GetAll()
        {
            return await context.Categories.Include(c=>c.Products).ToListAsync();
        }
        public override async Task<Category> GetByName(string s)
        {
            var c = await context.Categories.FirstOrDefaultAsync(c => c.CategoryName == s.ToUpper());
            return c;
        }
    }
}