using System;
namespace Bank.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public float Balance { get; set; }
        
        public ICollection<Transaction> Transactions { get; set; }
        
    }
}