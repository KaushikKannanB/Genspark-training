using MainMigration.Context;
using MainMigration.Models;
using Microsoft.EntityFrameworkCore;

namespace MainMigration.Repositories
{
    public class ModelRepository : Repository<int, Model>
    {
        public ModelRepository(MainMigrationContext context) : base(context)
        {

        }

        public override async Task<Model> GetById(int id)
        {
            var u = await context.Models.FirstOrDefaultAsync(u => u.ModelId==id);
            return u ?? throw new Exception("No such Model");
        }
        public override async Task<IEnumerable<Model>> GetAll()
        {
            return await context.Models.ToListAsync();
        }

    }
}