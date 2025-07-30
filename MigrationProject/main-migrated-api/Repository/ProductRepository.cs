using MainMigration.Context;
using MainMigration.Models;
using Microsoft.EntityFrameworkCore;

namespace MainMigration.Repositories
{
    public class ProductRepository : Repository<int, Product>
    {
        public ProductRepository(MainMigrationContext context) : base(context)
        {

        }

        public override async Task<Product> GetById(int id)
        {
            var u = await context.Products.FirstOrDefaultAsync(p=>p.ProductId== id);
            return u ?? throw new Exception("No such Product");
        }
        public override async Task<IEnumerable<Product>> GetAll()
        {
            return await context.Products.ToListAsync();
        }

    }
}