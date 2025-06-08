using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Inventory.Interfaces;
namespace Inventory.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string Email { get; }

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            var user = contextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                Email = user.FindFirst(ClaimTypes.Email)?.Value;
            }
        }
    }
}