using Notify.Models;
using Notify.Interfaces;
using Microsoft.EntityFrameworkCore;
using Notify.Context;

namespace Notify.Repositories
{
    public class MemberRepository : Repository<int, Member>
    {
        public MemberRepository(NotifyContext context) : base(context)
        {

        }
        public override async Task<Member> GetById(string id)
        {
            var t = await context.Members.FirstOrDefaultAsync(m => m.Email == id);
            return t;
        }

        public override async Task<IEnumerable<Member>> GetAll()
        {
            return await context.Members.ToListAsync();
        }
    }
}