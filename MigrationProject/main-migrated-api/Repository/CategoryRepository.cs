using MainMigration.Context;
using MainMigration.Models;
using Microsoft.EntityFrameworkCore;

namespace MainMigration.Repositories
{
    public class CategoryRepository : Repository<int, Category>
    {
        public CategoryRepository(MainMigrationContext context) : base(context)
        {

        }

        public override async Task<Category> GetById(int id)
        {
            var u = await context.Categories.FirstOrDefaultAsync(u => u.CategoryId==id);
            return u ?? throw new Exception("No such Category");
        }
        public override async Task<IEnumerable<Category>> GetAll()
        {
            return await context.Categories.ToListAsync();
        }

    }
}