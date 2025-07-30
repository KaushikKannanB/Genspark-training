using MainMigration.Context;
using MainMigration.Models;
using Microsoft.EntityFrameworkCore;

namespace MainMigration.Repositories
{
    public class ColorRepository : Repository<int, Color>
    {
        public ColorRepository(MainMigrationContext context) : base(context)
        {

        }

        public override async Task<Color> GetById(int id)
        {
            var u = await context.Colors.FirstOrDefaultAsync(u => u.ColorId==id);
            return u ?? throw new Exception("No such color");
        }
        public override async Task<IEnumerable<Color>> GetAll()
        {
            return await context.Colors.ToListAsync();
        }

    }
}