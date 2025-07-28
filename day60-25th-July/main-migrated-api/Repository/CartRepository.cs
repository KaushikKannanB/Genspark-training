using MainMigration.Context;
using MainMigration.Models;
using Microsoft.EntityFrameworkCore;

namespace MainMigration.Repositories
{
    public class CartRepository : Repository<int, Cart>
    {
        public CartRepository(MainMigrationContext context) : base(context)
        {

        }

        public override async Task<Cart> GetById(int id)
        {
            var u = await context.Carts.FirstOrDefaultAsync(u => u.CartId==id);
            return u ?? throw new Exception("No such Cart data");
        }
        public override async Task<IEnumerable<Cart>> GetAll()
        {
            return await context.Carts.ToListAsync();
        }

    }
}