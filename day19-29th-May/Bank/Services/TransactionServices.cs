using Bank.Contexts;
using Bank.Interfaces;
using Bank.Misc;
using Bank.Models;
using Bank.Models.DTOs;

namespace Bank.Services
{
    public class TransactionService : ITransactionServices
    {
        private readonly IUserRepository userRepo;
        private readonly BankContext context;
        private readonly ITransactionRepository transactionRepo;

        public TransactionService(IUserRepository u, ITransactionRepository t, BankContext c)
        {
            userRepo = u;
            transactionRepo = t;
            context = c;
        }
        public async Task<int> AddTransaction(Transaction tr)
        {
            return await transactionRepo.AddTransaction(tr);
        }
        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            return await transactionRepo.GetAllTransactions();
        }
        public async Task<Transaction> GetTransactionById(int id)
        {
            var t = await transactionRepo.GetTransactionById(id);
            if (t == null)
            {
                return null;
            }
            return t;
        }
        public async Task<int> Deposit(DepositRequestDTO request)
        {
            try
            {
                var u = await userRepo.GetUserById(request.UserId);
                if (u == null)
                {
                    throw new Exception("No such user exists");
                }
                else
                {
                    u.Balance += request.MoneyToDeposit;
                    await context.SaveChangesAsync();
                    Transaction transaction = new();
                    transaction.UserId = u.Id;
                    transaction.Type = "Deposit";
                    transaction.Amount = request.MoneyToDeposit;

                    
                    return await transactionRepo.AddTransaction(transaction);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public async Task<int> InitialDeposit(DepositRequestDTO request)
        {
            try
            {
                var u = await userRepo.GetUserById(request.UserId);
                if (u == null)
                {
                    throw new Exception("No such user exists");
                }
                else
                {
                    await context.SaveChangesAsync();
                    Transaction transaction = new();
                    transaction.UserId = u.Id;
                    transaction.Type = "Initial Deposit";
                    transaction.Amount = request.MoneyToDeposit;

                    
                    return await transactionRepo.AddTransaction(transaction);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }
        public async Task<int> Withdraw(WithdrawRequestDTO request)
        {
            try
            {
                var u = await userRepo.GetUserById(request.UserId);
                if (u == null)
                {
                    throw new Exception("No such user exists");
                }
                else
                {
                    if (u.Balance <= request.MoneyToWithdraw)
                    {
                        throw new Exception("You broke Homie-Insufficient Funds!!");
                    }
                    u.Balance -= request.MoneyToWithdraw;
                    await context.SaveChangesAsync();

                    Transaction transaction = new();
                    transaction.UserId = u.Id;
                    transaction.Type = "Withdraw";
                    transaction.Amount = request.MoneyToWithdraw;


                    return await transactionRepo.AddTransaction(transaction);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<int> BankTransfer(BankTransferDTO request)
        {
            try
            {
                var debit_user = await userRepo.GetUserById(request.UserIdDebit);
                var credit_user = await userRepo.GetUserById(request.UserIdCredit);

                if (debit_user == null || credit_user == null)
                {
                    throw new Exception("Invalid ID's");
                }
                else
                {
                    if (debit_user.Balance < request.Amount)
                    {
                        throw new Exception("Insufficient FUNDS");

                    }
                    if (debit_user.Password != request.PasswordDebitUser)
                    {
                        throw new Exception("Wrong Password");
                    }
                    debit_user.Balance -= request.Amount;
                    credit_user.Balance += request.Amount;
                    await context.SaveChangesAsync();

                    Transaction transaction = new();
                    transaction.Type = "Bank Transfer";
                    transaction.Amount = request.Amount;
                    transaction.UserId = debit_user.Id;

                    
                    return await transactionRepo.AddTransaction(transaction);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
    }
}