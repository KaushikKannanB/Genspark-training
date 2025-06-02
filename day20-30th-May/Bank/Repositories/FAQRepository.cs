using Bank.Contexts;
using Bank.Interfaces;
using Bank.Models;
using Microsoft.EntityFrameworkCore;
namespace Bank.Repositories
{
    public class FAQRepository : IFAQRepository
    {
        private readonly BankContext context;

        public FAQRepository(BankContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<FAQ>> GetAllInteractions()
        {
            return await context.Interactions.ToListAsync();
        }
        public async Task<int> AddInteraction(FAQ faq)
        {
            await context.Interactions.AddAsync(faq);
            await context.SaveChangesAsync();
            return faq.Id;
        }
        public async Task<IEnumerable<FAQ>> GetInteractionById(string id)
        {
            var f = await context.Interactions.Where(i => i.UserId == id).ToListAsync();
            return f;
        }
    }
    
}