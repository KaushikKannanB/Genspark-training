using MainMigration.Context;
using MainMigration.Models;
using Microsoft.EntityFrameworkCore;

namespace MainMigration.Repositories
{
    public class OrderDetailRepository : Repository<int, OrderDetail>
    {
        public OrderDetailRepository(MainMigrationContext context) : base(context)
        {

        }

        public override async Task<OrderDetail> GetById(int id)
        {
            var u = await context.OrderDetails.FirstOrDefaultAsync(u => u.OrderID==id);
            return u ?? throw new Exception("No such Order");
        }
        public override async Task<IEnumerable<OrderDetail>> GetAll()
        {
            return await context.OrderDetails.ToListAsync();
        }

    }
}