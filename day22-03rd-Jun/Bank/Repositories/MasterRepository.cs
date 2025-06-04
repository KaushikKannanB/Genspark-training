using Bank.Contexts;
using Bank.Interfaces;
using Bank.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank.Repositories
{
    public class MasterRepository : IMasterRepository
    {
        private readonly BankContext context;
        public MasterRepository(BankContext context)
        {
            this.context = context;
        }
        public async Task<Master> Get(string key)
        {
            return await context.Master.SingleOrDefaultAsync(u => u.UserName == key);
        }

        public async Task<IEnumerable<Master>> GetAllMaster()
        {
            return await context.Master.ToListAsync();
        }
        public async Task<string> AddMaster(Master m)
        {
            context.Master.Add(m);
            await context.SaveChangesAsync();
            return m.UserName;
        }
            
    }
}