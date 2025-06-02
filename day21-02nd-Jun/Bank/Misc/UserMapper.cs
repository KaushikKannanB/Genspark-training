using Bank.Models;
using Bank.Models.DTOs;
namespace Bank.Misc
{
    public class UserMapper
    {
        public User? MapUser(UserRequestDTO user)
        {
            User u = new();
            u.Name = user.Name;
            u.Email = user.Email;
            u.Password = user.Password;
            u.Balance = user.InitialDeposit;

            return u;
        }
        
    }
}