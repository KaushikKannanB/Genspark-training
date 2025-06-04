using Microsoft.AspNetCore.Authorization;
namespace Bank.Authorization
{
    public class UserRequirement : IAuthorizationRequirement
    {
        public string bankEmail { get; }

        public UserRequirement(string s)
        {
            bankEmail = s;
        }
    }
}