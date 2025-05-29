using Bank.Models;
namespace Bank.Misc
{
    public class UserMapper
    {
        public User? MapUser(User user)
        {
            User u = new();
            u.Id = user.Id;
            u.Name = user.Name;
            u.Email = user.Email;
            u.Password = user.Password;
            u.Balance = user.Balance;

            return u;

        }
    }
}