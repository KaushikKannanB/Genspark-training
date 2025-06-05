using Notify.Models;
using Notify.Interfaces;
using Microsoft.EntityFrameworkCore;
using Notify.Context;

namespace Notify.Repositories
{
    public class AdminRepository : Repository<int, Admin>
    {
        public AdminRepository(NotifyContext context) : base(context)
        {

        }
        public override async Task<Admin > GetById(string id)
        {
            var t = await context.Admins.FirstOrDefaultAsync(m => m.Email == id);
            return t;
        }

        public override async Task<IEnumerable<Admin >> GetAll()
        {
            return await context.Admins.ToListAsync();
        }
    }
}