using Inventory.Interfaces;
using Inventory.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

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
    }
}