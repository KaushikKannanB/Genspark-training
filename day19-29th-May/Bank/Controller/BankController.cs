using Bank.Interfaces;
using Bank.Models;
using Bank.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Bank.Services;
namespace Bank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IUserService _userRepo;
        // private readonly ITransactionRepository _userRepo;

        private readonly ITransactionServices _transactionService;

        public BankController(IUserService userRepo, ITransactionServices transactionService)
        {
            _userRepo = userRepo;
            _transactionService = transactionService;
        }

        // User Endpoints

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(UserRequestDTO user)
        {
            var id = await _userRepo.AddUser(user);
            return Ok(new { Message = "User created", UserId = id });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userRepo.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userRepo.GetUserById(id);
            if (user == null)
                return NotFound("User not found");
            return Ok(user);
        }

        // Transaction Endpoints

        [HttpGet("transactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _transactionService.GetAllTransactions();
            return Ok(transactions);
        }

        [HttpGet("transactions/{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var t = await _transactionService.GetTransactionById(id);
            if (t == null)
                return NotFound("Transaction not found");
            return Ok(t);
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(DepositRequestDTO dto)
        {
            var transactionId = await _transactionService.Deposit(dto);
            return Ok(new { Message = "Deposit successful", TransactionId = transactionId });
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(WithdrawRequestDTO dto)
        {
            var transactionId = await _transactionService.Withdraw(dto);
            return Ok(new { Message = "Withdrawal successful", TransactionId = transactionId });
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> BankTransfer(BankTransferDTO dto)
        {
            var transactionId = await _transactionService.BankTransfer(dto);
            return Ok(new { Message = "Transfer successful", TransactionId = transactionId });
        }
    }
}
