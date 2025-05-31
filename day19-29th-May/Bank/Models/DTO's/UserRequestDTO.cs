namespace Bank.Models.DTOs
{
    public class UserRequestDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public float InitialDeposit{ get; set; }
    }
}