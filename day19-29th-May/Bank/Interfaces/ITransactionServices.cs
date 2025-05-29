using Bank.Models;
using Bank.Models.DTOs;
namespace Bank.Interfaces
{
    public interface ITransactionServices
    {
        public Task<IEnumerable<Transaction>> GetAllTransactions();
        public Task<Transaction> GetTransactionById(int id);
        public Task<int> AddTransaction(Transaction transaction);
        public Task<int> Deposit(DepositRequestDTO depositRequest);
        public Task<int> Withdraw(WithdrawRequestDTO withdrawRequest);
        public Task<int> BankTransfer(BankTransferDTO bankTransferRequest);
    }
}