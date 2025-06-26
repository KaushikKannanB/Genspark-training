using Inventory.Contexts;
using Inventory.Hubs;
using Inventory.Interfaces;
using Inventory.Misc;
using Inventory.Models;
using Inventory.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Inventory.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly InventoryContext context;
        private readonly IAdminService adminService;
        private readonly IEncryptService encryptService;

        private readonly ICurrentUserService currentUserService;

        private readonly IUserService userService;
        private readonly IHubContext<NotificationHub> hubContext;


        private readonly IRepository<string, Admin> adminrepo;
        private readonly IRepository<string, Manager> managerrepo;
        private readonly IRepository<string, CategoryAddRequest> categaddrepo;

        PasswordValidation passval;


        public AdminController(IHubContext<NotificationHub> hu, InventoryContext co, IUserService u, IRepository<string, CategoryAddRequest> cat, IEncryptService en, ICurrentUserService cu, IAdminService a, IRepository<string, Admin> ad, IRepository<string, Manager> ma)
        {
            adminService = a;
            adminrepo = ad;
            currentUserService = cu;
            managerrepo = ma;
            encryptService = en;
            categaddrepo = cat;
            passval = new();
            userService = u;
            context = co;
            hubContext = hu;

        }

        [HttpPost("Add-Admin")]
        public async Task<IActionResult> AddAdmin(AdminManagerAddRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var admin = await adminService.AddAdmin(request);
            if (admin != null)
            {
                return Ok(admin);
            }
            else
            {
                return BadRequest("Admin with such mail already exists");
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("Add-Manager")]
        public async Task<IActionResult> AddManager(AdminManagerAddRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var manager = await adminService.AddManager(request);
            if (manager != null)
            {
                return Ok(manager);
            }
            else
            {
                return BadRequest("Manager with such mail already exists");
            }
        }
        [Authorize(Roles = "ADMIN")]
        [HttpPost("Add-Category")]
        public async Task<IActionResult> AddCategory(string Category)
        {

            var newcat = await adminService.AddCategory(Category);
            if (newcat == null)
            {
                return BadRequest("Already exixts!");
            }
            else
            {
                // var cur_user = await adminService.GetByMail(currentUserService.Email);
                await hubContext.Clients.All.SendAsync("ReceiveNotification", $"Category: {Category.ToUpper()} added--> notified at {DateTime.UtcNow}");
                
                return Ok(newcat);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("Get-All-Admins")]
        public async Task<IActionResult> GetAll()
        {
            var admins = await adminrepo.GetAll();
            return Ok(admins);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("Get-All-Managers")]
        public async Task<IActionResult> GetAllManagers()
        {
            var mans = await managerrepo.GetAll();
            return Ok(mans);
        }
        [Authorize(Roles = "ADMIN")]
        [HttpGet("Get-Managers-Created-By-ME")]
        public async Task<IActionResult> GetAdminsCreatedByMe()
        {
            var managers = await managerrepo.GetAll();
            var cur_user = await adminService.GetByMail(currentUserService.Email);

            var by_me = managers.Where(m => m.CreatedBy == cur_user.Id);
            return Ok(by_me);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("Get-All-Category-Requests")]
        public async Task<IActionResult> GetAllCategoryAddRequests()
        {
            var allreqs = await categaddrepo.GetAll();
            return Ok(allreqs);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("Delete-Manager")]
        public async Task<IActionResult> DeleteManager(string managerId)
        {
            var result = await adminService.DeleteManager(managerId);
            if (result == null)
            {
                return BadRequest("Not Deleted!");
            }
            else
            {
                return Ok(result);
            }
        }

        [Authorize]
        [HttpGet("Get-Manager-Report")]
        public async Task<IActionResult> GetManagerReport(string id)
        {
            var result = await adminService.CheckManagerActivity(id);
            return Ok(result);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("Get-Admin-Report")]
        public async Task<IActionResult> GetAdminReport(string id)
        {
            var result = await adminService.AdminActivity(id);
            return Ok(result);

        }


        [Authorize(Roles = "ADMIN")]
        [HttpPut("Change-Password-Admin")]
        public async Task<IActionResult> ChangePassword(string NewPassword)
        {
            if (!passval.IsValid(NewPassword))
            {
                return BadRequest("Invalid Password, a password must contain an Uppercase letter, a digit, a special character, and more than 8 characters");
            }
            try
                {
                    var cur_user_admin = await adminService.GetByMail(currentUserService.Email);
                    var cur_user_user = await userService.GetByMail(currentUserService.Email);
                    var encryptedData = await encryptService.EncryptData(new EncryptModel { Data = NewPassword });
                    cur_user_admin.Password = encryptedData.EncryptedData;
                    cur_user_user.Password = encryptedData.EncryptedData;

                    await context.SaveChangesAsync();
                    return Ok("Changed Password!");
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }

        }
    }
}