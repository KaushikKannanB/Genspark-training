using ExpenseTrackerAPI.Interfaces;
using ExpenseTrackerAPI.Models;
using ExpenseTrackerAPI.Enums;
using ExpenseTrackerAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace ExpenseTrackerAPI.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository userRepository;


        public UserController(IUserService userService, IUserRepository us)
        {
            _userService = userService;
            userRepository = us;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(kvp => kvp.Value?.Errors?.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
            }

            if (await _userService.IsEmailTakenAsync(request.Email))
            {
                return Conflict(ApiResponse<object>.ErrorResponse("Email is already registered.", new
                {
                    Email = new[] { "Email is already taken." }
                }));
            }
            
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Role = Roles.User,
                IsActive = true,
                IsDeleted = false
            };

            var result = await _userService.RegisterUserAsync(user, request.Password);

            if (!result)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Failed to register user.", new
                {
                    General = new[] { "An internal error occurred." }
                }));
            }

            return Ok(ApiResponse<object>.SuccessResponse((object?)null, "User registered successfully"));
        }

        [HttpPost("register-admin")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(kvp => kvp.Value?.Errors?.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
            }

            if (await _userService.IsEmailTakenAsync(request.Email))
            {
                return Conflict(ApiResponse<object>.ErrorResponse("Email is already registered.", new
                {
                    Email = new[] { "Email is already taken." }
                }));
            }

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Role = Roles.Admin,
                IsActive = true,
                IsDeleted = false
            };

            var result = await _userService.RegisterUserAsync(user, request.Password);

            if (!result)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Failed to register admin.", new
                {
                    General = new[] { "An internal error occurred." }
                }));
            }

            return Ok(ApiResponse<object>.SuccessResponse((object?)null, "Admin registered successfully"));
        }


        [HttpPost("create-analyser")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAnalyser([FromBody] RegisterUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(kvp => kvp.Value?.Errors?.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return BadRequest(ApiResponse<object>.ErrorResponse("Validation failed", errors));
            }

            if (await _userService.IsEmailTakenAsync(request.Email))
            {
                return Conflict(ApiResponse<object>.ErrorResponse("Email is already registered.", new
                {
                    Email = new[] { "Email is already taken." }
                }));
            }

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                Role = Roles.Analyser,
                IsActive = true,
                IsDeleted = false
            };

            var result = await _userService.RegisterUserAsync(user, request.Password);

            if (!result)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Failed to register user.", new
                {
                    General = new[] { "An internal error occurred." }
                }));
            }

            return Ok(ApiResponse<object>.SuccessResponse((object?)null, "User registered successfully"));
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("User not found", new
                {
                    Id = new[] { "No user found with the given ID." }
                }));
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role.ToString()
            };

            return Ok(ApiResponse<UserDto>.SuccessResponse(userDto, "User fetched successfully"));
        }

        [HttpGet("email/{email}")]
        [Authorize]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("User not found", new
                {
                    Email = new[] { "No user found with the given email." }
                }));
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role.ToString()
            };

            return Ok(ApiResponse<UserDto>.SuccessResponse(userDto, "User fetched successfully"));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Get-all-analysers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var allusers = await userRepository.GetAllAsync();
            var onlyanalysers = allusers.Where(u => u.Role == Enums.Roles.Analyser);

            return Ok(onlyanalysers);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromForm] EmailFileRequest request)
        {
            try
            {
                var userid = GetUserId();
                var user = await _userService.GetUserByIdAsync(userid);

                using var stream = new MemoryStream();
                await request.File.CopyToAsync(stream);
                stream.Position = 0;

                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse("kaushikkannan02@gmail.com"));
                message.To.Add(MailboxAddress.Parse(request.Email));
                message.Subject = $"Expense Report from {user.UserName}";

                var builder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <div style='font-family: Arial, sans-serif; color: #333;'>
                            <h2 style='color: #007BFF;'>Expense Report Received</h2>
                            <p>Hello,</p>
                            <p><strong>{user.UserName}</strong> has shared their latest expense report with you.</p>
                            <p>Please find the attached document below.</p>
                            <p style='margin-top: 20px;'>If you have any questions, feel free to reach out to them via the Expense Tracker platform.</p>
                            <p style='margin-top: 30px;'>Best regards,<br/><strong>Expense Tracker Team</strong></p>
                        </div>"
                };

                // Attach the uploaded file
                builder.Attachments.Add(request.File.FileName, stream.ToArray(), ContentType.Parse(request.File.ContentType));
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


        [HttpPost("send-analyer-credentials")]
        public async Task<IActionResult> SendCredentialsEmail(string email, string content)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse("kaushikkannan02@gmail.com"));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = "Analyser Credentials - Expense Tracker";

                var builder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <div style='font-family: Arial, sans-serif; color: #333;'>
                            <h2 style='color: #007BFF;'>Welcome to Expense Tracker!</h2>
                            <p>Dear Analyser,</p>
                            <p>Your account has been successfully created. Please use the credentials below to log in:</p>
                            <table style='border-collapse: collapse;'>
                                <tr>
                                    <td style='padding: 8px; font-weight: bold;'>Email:</td>
                                    <td style='padding: 8px;'>{email}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 8px; font-weight: bold;'>Password:</td>
                                    <td style='padding: 8px; color: #D9534F;'>{content}</td>
                                </tr>
                            </table>
                            <p style='margin-top: 20px;'>You can now <a href='http://localhost:4200/auth/login'>log in here</a> and start using the system.</p>
                            <p style='margin-top: 30px;'>Best Regards,<br/><strong>Expense Tracker Team</strong></p>
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


        [HttpPost("send-welcome-mail")]
        public async Task<IActionResult> SendWelcomeMail(string email, string username)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(MailboxAddress.Parse("kaushikkannan02@gmail.com"));
                message.To.Add(MailboxAddress.Parse(email));
                message.Subject = "Expense Tracker - Welcome Aboard";

                var builder = new BodyBuilder
                {
                    HtmlBody = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; background-color: #f9fafb; padding: 30px; border-radius: 10px; box-shadow: 0 4px 8px rgba(0,0,0,0.1);'>
                        <h2 style='color: #2d3748;'>👋 Welcome, {username}!</h2>
                        <p style='color: #4a5568; font-size: 16px;'>
                        We're thrilled to have you on board at <strong>Expense Tracker</strong>! 🎉
                        </p>

                        <p style='color: #4a5568; font-size: 16px;'>
                        With your new account, you can now:
                        </p>
                        <ul style='color: #4a5568; font-size: 16px; padding-left: 20px;'>
                        <li>💰 Track your daily expenses</li>
                        <li>📊 Analyze spending patterns</li>
                        <li>🔒 Keep your data secure and private</li>
                        </ul>

                        <p style='color: #4a5568; font-size: 16px;'>
                        You can get started by logging in and exploring your dashboard.
                        </p>

                        <div style='text-align: center; margin: 30px 0;'>
                        <a href='http://localhost:4200/auth/login' style='background-color: #38b2ac; color: white; padding: 12px 24px; text-decoration: none; border-radius: 6px; font-weight: bold; display: inline-block;'>Login Now</a>
                        </div>

                        <p style='color: #a0aec0; font-size: 14px; text-align: center;'>
                        If you have any questions, feel free to reply to this email.<br />
                        Welcome aboard! 🚀
                        </p>
                    </div>
                    "

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
        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value!);
        }
    }
}
