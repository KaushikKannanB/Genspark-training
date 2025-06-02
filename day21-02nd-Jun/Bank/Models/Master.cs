using System.ComponentModel.DataAnnotations;
namespace Bank.Models
{
    public class Master
    {
        [Key]
        public string UserName { get; set; }
        public string Role { get; set; }
        public byte[] Password { get; set; }
        public byte[] HashKey { get; set; }

        public User? User { get; set; }
        public Admin? Admin{ get; set; }
    }
}