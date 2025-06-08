using Inventory.Interfaces;
using Inventory.Models;

namespace Inventory.Services
{
    public class EncryptService : IEncryptService
    {
        public async Task<EncryptModel> EncryptData(EncryptModel data)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(data.Data);
            data.EncryptedData = hashedPassword;

            return data;
        }
        public async Task<string> GenerateId(string starter)
        {
            string prefix = starter;
            string guidPart = Guid.NewGuid().ToString("N").Substring(0, 8); // 8 chars

            string result = $"{prefix}{guidPart}";

            return result;

        }
    }
}