using Inventory.Interfaces;
using Inventory.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Inventory.Models;

namespace Inventory.Controllers
{
    [ApiController]
    [Route("api/authentication")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAutheticationService authService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;


        private readonly IRefreshTokenRepository _refreshTokenRepo;


        public AuthenticationController(ITokenService _t, IUserService _u, IRefreshTokenRepository re, IAdminService a, IAutheticationService au)
        {
            authService = au;
            // userrepo = u;
            _refreshTokenRepo = re;
            _userService = _u;
            _tokenService = _t;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
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

        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> Refresh(RefreshRequestDto request)
        {
            var storedToken = await _refreshTokenRepo.GetTokenAsync(request.RefreshToken);
            if (storedToken == null || storedToken.IsUsed || storedToken.IsRevoked || storedToken.ExpiryDate < DateTime.UtcNow)
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            storedToken.IsUsed = true;
            await _refreshTokenRepo.InvalidateTokenAsync(storedToken);

            var user = await _userService.GetByMail(storedToken.Email);
            var newAccessToken = await _tokenService.TokenGenerator(user);
            var newRefreshToken = await _tokenService.GenerateRefreshToken();

            await _refreshTokenRepo.SaveRefreshTokenAsync(new RefreshToken
            {
                Token = newRefreshToken,
                Email = user.Email,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new UserLoginResponse
            {
                Email = user.Email,
                Token = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        // [Authorize]
        [HttpGet("Get-User-By-Email")]
        public async Task<IActionResult> GetUserByMail(string email)
        {
            var user = await _userService.GetByMail(email);
            return Ok(user);
        }


    }
}