using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Inventory.Services;
using Inventory.Interfaces;
using Inventory.Models;
using Inventory.Contexts;

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


        public ManagerController(InventoryContext co, IEncryptService en, ICurrentUserService cu, IManagerService ma, IUserService us)
        {
            currentUserService = cu;
            managerService = ma;
            userService = us;
            encryptService = en;
            context = co;
        }
        [Authorize(Roles = "MANAGER")]
        [HttpPut("Change-Password")]
        public async Task<IActionResult> ChangePassword(string NewPassword)
        {
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
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Verify whether the category already exists!");
            }
        }
    }
}