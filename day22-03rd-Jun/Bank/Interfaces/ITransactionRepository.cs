using Bank.Models;
namespace Bank.Interfaces
{
    public interface ITransactionRepository
    {
        public Task<IEnumerable<Transaction>> GetAllTransactions();
        public Task<Transaction> GetTransactionById(int id);
        public Task<int> AddTransaction(Transaction transaction);
        public Task<string> DeleteTransaction(int id);
    }
}