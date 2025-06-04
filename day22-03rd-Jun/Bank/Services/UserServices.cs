using Bank.Contexts;
using Bank.Interfaces;
using Bank.Misc;
using Bank.Models;
using Bank.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Bank.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepo;
        private readonly IMasterRepository masterRepo;
        private readonly ITransactionRepository transactionRepository;
        private readonly UserMapper userMapper;
        private readonly ITransactionServices transactionServices;
        private readonly IEncryptService _encryptionService;
        private readonly BankContext context;

        public UserService(IUserRepository userRepository, ITransactionServices ts, BankContext c, IEncryptService e, IMasterRepository m, ITransactionRepository tr)
        {
            userRepo = userRepository;
            userMapper = new UserMapper();
            transactionServices = ts;
            context = c;
            _encryptionService = e;
            masterRepo = m;
            transactionRepository = tr;
        }
        public async Task<IEnumerable<User>> GetAllUsers()
        {

            return await userRepo.GetAllUsers();
        }
        public async Task<User> GetUserById(string id)
        {
            var u = await userRepo.GetUserById(id);
            if (u != null)
            {
                return u;
            }
            return null;
        }
        public async Task<User> GetUserByMail(string id)
        {
            var u = await context.Users.Include(u=>u.Transactions).FirstOrDefaultAsync(u => u.Name == id);
            if (u != null)
            {
                return u;
            }
            return null;
        }
        public async Task<string> AddUser(UserRequestDTO user)
        {
            var us = await userRepo.GetUserByMail(user.Email);
            if (us == null)
            {
                User? u = userMapper.MapUser(user);
                Master m = new();
                m.UserName = user.Name;
                var encryptedData = await _encryptionService.EncryptData(new EncryptModel
                {
                    Data = user.Password
                });
                m.Password = encryptedData.EncryptedData;
                m.HashKey = encryptedData.HashKey;
                m.Role = "User";
                string id = await masterRepo.AddMaster(m);
                u.Id = GenerateGuidBasedId();
                string str = await userRepo.AddUser(u);
                DepositRequestDTO dto = new();
                dto.UserId = u.Id;
                dto.Password = u.Password;
                dto.MoneyToDeposit = u.Balance;
                int dep = await transactionServices.InitialDeposit(dto);
                return str;

            }
            return "Already Exists";
        }
        public static string GenerateGuidBasedId()
        {

            string base64 = Convert.ToBase64String(Guid.NewGuid().ToByteArray());


            string cleaned = new string(base64.Where(char.IsLetterOrDigit).ToArray());

            return cleaned.Substring(0, Math.Min(10, cleaned.Length));
        }
        // public async Task<User> GetUserByMail(string email)
        // {
        //     return await userRepo.GetUserByMail(email);
        // }
    }
}