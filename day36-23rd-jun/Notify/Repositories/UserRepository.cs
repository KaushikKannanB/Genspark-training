using Notify.Models;
using Notify.Interfaces;
using Microsoft.EntityFrameworkCore;
using Notify.Context;

namespace Notify.Repositories
{
    public class UserRepository : Repository<string, User>
    {
        public UserRepository(NotifyContext context) : base(context)
        {

        }
        public override async Task<User > GetById(string mail)
        {
            var t = await context.Users.FirstOrDefaultAsync(m => m.UserEmail == mail);
            return t ?? throw new Exception("No member");
        }

        public override async Task<IEnumerable<User >> GetAll()
        {
            return await context.Users.ToListAsync();
        }
    }
}