namespace Notify.Models.DTO
{
    public class UserLoginRequest
    {
        public string UserEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}