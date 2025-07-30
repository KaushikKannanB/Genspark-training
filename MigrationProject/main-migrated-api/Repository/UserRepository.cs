using MainMigration.Context;
using MainMigration.Models;
using Microsoft.EntityFrameworkCore;

namespace MainMigration.Repositories
{
    public class UserRepository : Repository<int, User>
    {
        public UserRepository(MainMigrationContext context) : base(context)
        {

        }

        public override async Task<User> GetById(int id)
        {
            var u = await context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            return u ?? throw new Exception("No such User");
        }
        public override async Task<IEnumerable<User>> GetAll()
        {
            return await context.Users.ToListAsync();
        }

    }
}