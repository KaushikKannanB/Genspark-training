using System;
using System.Text.Json.Serialization;
namespace Bank.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; } = string.Empty;
        public float Amount { get; set; }

        public DateTime Date { get; set; }
        
        [JsonIgnore]
        public User? User { get; set; }
    }
    
}