using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Inventory.Services;
using Inventory.Interfaces;
using Inventory.Models;
using Inventory.Contexts;
using Inventory.Misc;
using Microsoft.AspNetCore.SignalR;
using Inventory.Hubs;

namespace Inventory.Controllers
{
    [ApiController]
    [Route("api/manager")]
    public class ManagerController : ControllerBase
    {
        private readonly InventoryContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IUserService userService;
        private readonly IEncryptService encryptService;
        private readonly IManagerService managerService;
        private readonly IHubContext<NotificationHub> hubContext;

        PasswordValidation passval;


        public ManagerController(IHubContext<NotificationHub> hub, InventoryContext co, IEncryptService en, ICurrentUserService cu, IManagerService ma, IUserService us)
        {
            currentUserService = cu;
            managerService = ma;
            userService = us;
            encryptService = en;
            context = co;
            passval = new();
            hubContext = hub;
        }
        [Authorize(Roles = "MANAGER")]
        [HttpPut("Change-Password-Manager")]
        public async Task<IActionResult> ChangePassword(string NewPassword)
        {
            if (!passval.IsValid(NewPassword))
            {
                return BadRequest("Invalid Password, a password must contain an Uppercase letter, a digit, a special character, and more than 8 characters");
            }
            try
            {
                var cur_user_manager = await managerService.GetByMail(currentUserService.Email);
                var cur_user_user = await userService.GetByMail(currentUserService.Email);
                var encryptedData = await encryptService.EncryptData(new EncryptModel { Data = NewPassword });
                cur_user_manager.Password = encryptedData.EncryptedData;
                cur_user_user.Password = encryptedData.EncryptedData;

                await context.SaveChangesAsync();
                return Ok("Changed Password!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
                
        }

        [Authorize(Roles = "MANAGER")]
        [HttpPost("Category-Add-Requested")]
        public async Task<IActionResult> RaiseAddCategoryRequest(string category)
        {
            var result = await managerService.RaiseAddCategoryRequest(category);
            var cur_user_manager = await managerService.GetByMail(currentUserService.Email);
            
            if (result != null)
            {
                await hubContext.Clients.All.SendAsync("ReceiveNotification", $"Category: {category.ToUpper()} request made by ${cur_user_manager.Id}: ${cur_user_manager.Name} --> notified at {DateTime.UtcNow}");
                return Ok(result);
            }
            else
            {
                return BadRequest("Verify whether the category already exists!");
            }
        }
    }
}