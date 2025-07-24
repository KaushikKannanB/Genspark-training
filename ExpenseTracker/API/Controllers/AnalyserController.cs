using ExpenseTrackerAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
namespace ExpenseTrackerAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "Analyser")]
    public class AnalyserController : ControllerBase
    {
        private readonly IExpenseRepository expenseRepository;
        private readonly ICategoryRepository categRepository;

        private readonly IUserRepository userRepository;

        public AnalyserController(IExpenseRepository exp, IUserRepository us, ICategoryRepository cat)
        {
            expenseRepository = exp;
            userRepository = us;
            categRepository = cat;
        }

        [HttpGet("Get-all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var allusers = await userRepository.GetAllAsync();
            var onlyusers = allusers.Where(u => u.Role == Enums.Roles.User);

            return Ok(onlyusers);
        }
        [HttpGet("Get-Expenses-By-User-Id")]
        public async Task<IActionResult> GetAllExpensesUser(Guid userid)
        {
            var allexpenses = await expenseRepository.GetAllByUserIdAsync(userid);
            return Ok(allexpenses);
        }

        [HttpGet("Get-top5-Days-expenses-User")]
        public async Task<IActionResult> GetTop5ExpensesDayUser(Guid userid)
        {
            var allexpenses = await expenseRepository.GetAllByUserIdAsync(userid);

            var grouped = allexpenses
                .GroupBy(e => e.ExpenseDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .OrderByDescending(g => g.TotalAmount)
                .Take(5)
                .ToList();

            return Ok(grouped);
        }

        [HttpGet("Get-top5-Individual-expenses-User")]
        public async Task<IActionResult> GetTop5ExpenseIndividualUser(Guid userid)
        {
            var allexpenses = await expenseRepository.GetAllByUserIdAsync(userid);

            var grouped = allexpenses
                .Select(g=>new
                {
                    Amount = g.Amount,
                    Description = g.Description
                })
                .OrderByDescending(x=>x.Amount)
                .Take(5)
                .ToList();

            return Ok(grouped);
        }

        [HttpGet("Get-latest5-expenses-User")]
        public async Task<IActionResult> GetLatest5ExpenseUser(Guid userid)
        {
            var allexpenses = await expenseRepository.GetAllByUserIdAsync(userid);

            var grouped = allexpenses
                .Select(g=>new
                {
                    Amount = g.Amount,
                    Description = g.Description,
                    Date = g.ExpenseDate
                })
                .OrderByDescending(x=>x.Date.Date)
                .Take(5)
                .ToList();

            return Ok(grouped);
        }
        [HttpGet("Get-top5-categories-User-by-amount")]
        public async Task<IActionResult> GetTopCategoriesUser(Guid userid)
        {
            var expenses = await expenseRepository.GetAllByUserIdAsync(userid);

            var grouped = expenses
                .GroupBy(e => e.CategoryId)
                .Select(g => new
                {
                    CategoryId = g.Key,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .OrderByDescending(g => g.TotalAmount)
                .Take(5)
                .ToList();

            var result = new List<object>();

            foreach (var item in grouped)
            {
                var cat = await categRepository.GetByIdAsync(item.CategoryId);
                result.Add(new
                {
                    CategoryName = cat?.Name ?? "Unknown",
                    TotalAmount = item.TotalAmount

                });
            }

            return Ok(result);
        }

        [HttpGet("Get-top5-categories-User-by-frequency")]
        public async Task<IActionResult> GetTopCategoriesUserFrequency(Guid userid)
        {
            var expenses = await expenseRepository.GetAllByUserIdAsync(userid);

            var grouped = expenses
                .GroupBy(e => e.CategoryId)
                .Select(g => new
                {
                    CategoryId = g.Key,
                    Frequency = g.Count()
                })
                .OrderByDescending(g => g.Frequency)
                .Take(5)
                .ToList();

            var result = new List<object>();

            foreach (var item in grouped)
            {
                var cat = await categRepository.GetByIdAsync(item.CategoryId);
                result.Add(new
                {
                    CategoryName = cat?.Name ?? "Unknown",
                    Frequency = item.Frequency

                });
            }

            return Ok(result);
        }


        [HttpGet("Get-top5-categories")]
        public async Task<IActionResult> GetTopCategories()
        {
            var expenses = await expenseRepository.GetAll();

            var grouped = expenses
                .GroupBy(e => e.CategoryId)
                .Select(g => new
                {
                    CategoryId = g.Key,
                    TotalAmount = g.Sum(x => x.Amount)
                })
                .OrderByDescending(g => g.TotalAmount)
                .Take(5)
                .ToList();

            var result = new List<object>();

            foreach (var item in grouped)
            {
                var cat = await categRepository.GetByIdAsync(item.CategoryId);
                result.Add(new
                {
                    CategoryName = cat?.Name ?? "Unknown",
                    TotalAmount = item.TotalAmount

                });
            }

            return Ok(result);
        }

        [HttpGet("Get-top-users")]
        public async Task<IActionResult> GetTopUsers()
        {
            var allexpenses = await expenseRepository.GetAll();

            var grouped = allexpenses
                .GroupBy(u => u.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    TotalExpenses = g.Sum(x => x.Amount)
                })
                .OrderByDescending(g => g.TotalExpenses)
                .Take(5)
                .ToList();

            var result = new List<object>();

            foreach (var i in grouped)
            {
                var user = await userRepository.GetByIdAsync(i.UserId.Value);

                result.Add(new
                {
                    UserName = user?.UserName ?? "Unknown",
                    TotalExpenses = i.TotalExpenses
                });
            }

            return Ok(result);
        }


        [HttpPost("send-suggestion-mail")]
        public async Task<IActionResult> SendSuggestion(string email, string content)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse("kaushikkannan02@gmail.com"));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = "Expense Tracker - Suggestion from our Analyser";

                var builder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <div style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 30px;'>
                            <div style='max-width: 600px; margin: auto; background-color: #ffffff; border-radius: 8px; padding: 20px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
                                <h2 style='color: #4CAF50; margin-bottom: 20px;'>💡 Suggestion from Analyser</h2>
                                <p style='font-size: 16px; color: #333;'>{content}</p>
                                <hr style='margin: 30px 0; border: none; border-top: 1px solid #ddd;'/>
                                <p style='font-size: 14px; color: #888;'>If you have any questions, feel free to reply to this email.</p>
                                <p style='margin-top: 30px; font-size: 16px; color: #444;'>Best Regards,<br/><strong style='color: #4CAF50;'>Expense Tracker Team</strong></p>
                            </div>
                        </div>"
                };

                message.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("kaushikkannan02@gmail.com", "nbepnpvbimmimtoa"); // App password
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

                return Ok(new { message = "Email sent successfully" });
            }
            catch (SmtpCommandException ex) when (ex.ErrorCode == SmtpErrorCode.RecipientNotAccepted)
            {
                return BadRequest("Email address does not exist or was rejected by the server.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Failed to send email: " + ex.Message);
            }
        }
    }
}