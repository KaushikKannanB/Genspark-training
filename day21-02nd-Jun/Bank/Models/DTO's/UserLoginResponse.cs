namespace Bank.Models.DTOs
{
    public class UserLoginResponse
    {
        public string UserName { get; set; }
        public string? Token{ get; set; }
    }
}