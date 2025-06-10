using Inventory.Interfaces;
using Inventory.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAutheticationService authService;


        public AuthenticationController(IAdminService a, IAutheticationService au)
        {
            authService = au;
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
        
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await authService.Logout(token);
            return Ok("Logout successful. Token invalidated.");
        }

    }
}