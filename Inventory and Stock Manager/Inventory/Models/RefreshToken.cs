namespace Inventory.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsUsed { get; set; } = false;
        public bool IsRevoked { get; set; } = false;
    }
}