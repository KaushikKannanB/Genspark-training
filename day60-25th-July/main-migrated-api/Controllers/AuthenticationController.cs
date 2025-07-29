using MainMigration.Models.DTOs;
using MainMigration.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MainMigration.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService authService;
        public AuthenticationController(IAuthService a)
        {
            authService = a;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(UserSignUpRequest req)
        {
            var user = await authService.SignUp(req);
            if (user == null)
            {
                return BadRequest("Invalid sign up request");
            }
            else
            {
                return Ok("Signed Up!");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest req)
        {
            var response = await authService.Login(req);
            if (response == null)
            {
                return BadRequest("Invalid login request");
            }
            else
            {
                return Ok(response);
            }
        }
    }
}