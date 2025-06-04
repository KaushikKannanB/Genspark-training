using Bank.Interfaces;
using Bank.Models;
using Bank.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Bank.Services;
namespace Bank.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IUserService _userRepo;
        private readonly IFAQRepository faqrepo;

        private readonly ITransactionRepository trepo;
        private readonly ITransactionServices _transactionService;
        private readonly IFAQServices fAQServices;

        public BankController(IUserService userRepo, ITransactionRepository t, ITransactionServices transactionService, IFAQServices fAQServices, IFAQRepository f)
        {
            _userRepo = userRepo;
            _transactionService = transactionService;
            this.fAQServices = fAQServices;
            faqrepo = f;
            trepo = t;
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

        [HttpGet("Get-all-interactions")]
        public async Task<IActionResult> GetAllInteractions()
        {
            var all = await faqrepo.GetAllInteractions();
            return Ok(all);
        }
        [HttpGet("Get-InteractionsBy-Id")]
        public async Task<IActionResult> GetInteractionsById(string id, string password)
        {
            var user = await _userRepo.GetUserById(id);
            if (user != null)
            {
                if (user.Password == password)
                {
                    return Ok(await faqrepo.GetInteractionById(id));
                }
                else
                {
                    return BadRequest("Invalid Request-check credentials");
                }
            }
            else
            {
                return BadRequest("Invalid Request-check credentials");
            }
        }
        [HttpPost("General-Queries")]
        public async Task<IActionResult> GetGeneralQueries(UserSpecificDTO dto)
        {
            var result = await fAQServices.GetGeneralQueries(dto);
            if (result == null)
            {
                return BadRequest("Invalid Request-check credentials");
            }
            return Ok(new { Message = "Question processed and", Answer = result });
        }
        [HttpPost("UserSpecific-Queries")]
        public async Task<IActionResult> GetUserSpecificQueries(UserSpecificDTO dto)
        {
            var result = await fAQServices.GetUserSpecificQueries(dto);
            if (result == null)
            {
                return BadRequest("Invalid Request-check credentials");
            }
            return Ok(new { Message = "Question processed and", Answer = result });
        }

        [Authorize(Policy = "EliteBankUsersOnly")]
        [HttpDelete("Transaction")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var authUser = User.Identity?.Name;

            if (string.IsNullOrEmpty(authUser))
            {
                return Unauthorized("User identity not found.");
            }

            var user = await _userRepo.GetUserByMail(authUser);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            if (user.Transactions == null || !user.Transactions.Any(t => t.Id == id))
            {
                return BadRequest(user.Name);
            }

            var result = await trepo.DeleteTransaction(id);
            return Ok(result);
        }


    }
}
