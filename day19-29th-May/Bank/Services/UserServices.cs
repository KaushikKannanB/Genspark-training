using Bank.Interfaces;
using Bank.Misc;
using Bank.Models;

namespace Bank.Services
{
    public class UserService : IUserRepository
    {
        private readonly IUserRepository userRepo;
        public UserService(IUserRepository userRepository)
        {
            userRepo = userRepository;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await userRepo.GetAllUsers();
        }
        public async Task<User> GetUserById(int id)
        {
            var u = await userRepo.GetUserById(id);
            if (u != null)
            {
                return u;
            }
            return null;
        }
        public async Task<int> AddUser(User user)
        {
            var us = await userRepo.GetUserByMail(user.Email);
            if (us == null)
            {
                User u = new();
                u.Name = user.Name;
                u.Email = user.Email;
                u.Password = user.Password;
                u.Balance = user.Balance;

                return await userRepo.AddUser(u);
            }
            return -1;
        }
        public async Task<User> GetUserByMail(string email)
        {
            return await userRepo.GetUserByMail(email);
        }
    }
}