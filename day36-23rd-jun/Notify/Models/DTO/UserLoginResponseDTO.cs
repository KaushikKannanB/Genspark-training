namespace Notify.Models.DTO
{
    public class UserLoginResponse
    {
        public string UserEmail { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}