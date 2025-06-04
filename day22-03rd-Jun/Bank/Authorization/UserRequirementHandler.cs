using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Bank.Interfaces;
namespace Bank.Authorization
{
    public class UserRequirementHandler : AuthorizationHandler<UserRequirement>
    {
        private readonly IUserService userService;
        public UserRequirementHandler(IUserService u)
        {
            userService = u;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirement req)
        {
            var userEmail = context.User.Identity?.Name;

            if (!string.IsNullOrEmpty(userEmail) && userEmail.Contains(req.bankEmail))
            {
                context.Succeed(req);
            }
            else
            {
                Console.WriteLine("Authorization failed: Email does not contain required domain.");
                context.Fail();  
            }

            await Task.CompletedTask; 
        }
    }
}