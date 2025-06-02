using System.Transactions;
using Bank.Models;
using Bank.Contexts;
using Bank.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace Bank.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BankContext context;
        public TransactionRepository(BankContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<Bank.Models.Transaction>> GetAllTransactions()
        {
            return await context.Transactions.ToListAsync();
        }
        public async Task<Bank.Models.Transaction> GetTransactionById(int id)
        {
            var t = await context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
            return t;
        }
        public async Task<int> AddTransaction(Bank.Models.Transaction transaction)
        {
            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();
            return transaction.Id;
        }
    }
}