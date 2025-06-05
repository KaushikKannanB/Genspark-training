using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notify.Context;
using Notify.Hubs;
using Notify.Interfaces;
using Notify.Models;
using Notify.Models.DTO;
using Notify.Repositories;
using Microsoft.AspNetCore.SignalR;
// using Microsoft.AspNetCore.Mvc;

namespace Notify.Controllers
{

    [ApiController]
    [Route("api/notify")]

    public class NotifyController : ControllerBase
    {
        // private readonly NotifyContext context;
        private readonly IRepository<int, Admin> adminRepo;
        private readonly IHubContext<NotificationHub> _hubContext;

        private readonly IRepository<int, Member> memberRepo;
        private readonly IRepository<string, User> userRepo;
        private readonly ITokenService tokenService;
        // private readonly ILogger<AuthenticationController> _logger;
        private readonly IAutheticationService authService;
        private readonly IEncryptService encryptService;


        public NotifyController(IHubContext<NotificationHub> hubContext, IAutheticationService auth, IEncryptService e, ITokenService t, IRepository<int, Admin> a, IRepository<int, Member> m, IRepository<string, User> u)
        {
            adminRepo = a;
            memberRepo = m;
            userRepo = u;
            tokenService = t;
            encryptService = e;
            authService = auth;

            _hubContext = hubContext;
        }

        [HttpPost("addAdmin")]
        public async Task<IActionResult> AddAdmin(AdminMemberAddRequest request)
        {
            var ad = await adminRepo.GetById(request.Email);
            if (ad == null)
            {
                EncryptModel em = new();
                em.Data = request.Password;
                var encryptedData = await encryptService.EncryptData(em);
                User? u = new();
                u.UserEmail = request.Email;
                u.Password = encryptedData.EncryptedData;
                u.HashKey = encryptedData.HashKey;
                u.Role = "Admin";

                var user = await userRepo.Add(u);


                Admin? a = new();
                a.Name = request.Name;
                a.Email = request.Email;
                a.Password = encryptedData.EncryptedData;

                var admin = await adminRepo.Add(a);

                return Ok(admin);
            }
            else
                return BadRequest("such User exists");
        }
        [HttpPost("addMember")]
        public async Task<IActionResult> AddMember(AdminMemberAddRequest request)
        {
            var mem = await memberRepo.GetById(request.Email);
            if (mem == null)
            {
                EncryptModel em = new();
                em.Data = request.Password;
                var encryptedData = await encryptService.EncryptData(em);
                User? u = new();
                u.UserEmail = request.Email;
                u.Password = encryptedData.EncryptedData;
                u.HashKey = encryptedData.HashKey;
                u.Role = "Member";

                var user = await userRepo.Add(u);

                Member? m = new();
                m.Name = request.Name;
                m.Email = request.Email;
                m.Password = encryptedData.EncryptedData;

                var member = await memberRepo.Add(m);

                return Ok(member);
            }
            return BadRequest("such User already exixts");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            try
            {
                var response = await authService.Login(request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest("Unsuccessful Login!" + e.Message);
            }

        }

        [Authorize(Roles ="Admin")]
        [HttpPost("upload")]

        public async Task<IActionResult> UploadFile(IFormFile file)

        {

            if (file == null || file.Length == 0)

                return BadRequest("No file uploaded.");
 
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
 
            if (!Directory.Exists(uploadsFolder))

                Directory.CreateDirectory(uploadsFolder);
 
            var filePath = Path.Combine(uploadsFolder, file.FileName);
 
            using (var stream = new FileStream(filePath, FileMode.Create))

            {

                await file.CopyToAsync(stream);

            }
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", $" New file uploaded: {file.FileName}");

            return Ok(new { file.FileName, file.Length });

        }
        [Authorize(Roles ="Member")]
        [HttpGet("download/{fileName}")]

        public IActionResult DownloadFile(string fileName)

        {

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

            var filePath = Path.Combine(uploadsFolder, fileName);
 
            if (!System.IO.File.Exists(filePath))

                return NotFound("File not found.");
 
            var contentType = "application/octet-stream"; // Or use FileExtensionContentTypeProvider for better MIME types

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
 
            return File(fileBytes, contentType, fileName);

        }
 
    }
}