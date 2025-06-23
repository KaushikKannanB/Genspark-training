
using System.ComponentModel.DataAnnotations;

namespace Notify.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public byte[]? Password { get; set; }
        public User? User{ get; set; }
    }
}