namespace MainMigration.Models.DTOs
{
    public class UserLoginResponse
    {
        public string UserName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}