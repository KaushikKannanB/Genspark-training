using MainMigration.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MainMigration.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public string Name { get; }

        public CurrentUserService(IHttpContextAccessor contextAccessor)
        {
            var user = contextAccessor.HttpContext?.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                Name = user.FindFirst(ClaimTypes.Name)?.Value;
            }
        }
    }
}