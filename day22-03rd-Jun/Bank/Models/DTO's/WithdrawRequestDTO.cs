namespace Bank.Models.DTOs
{
    public class WithdrawRequestDTO
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public float MoneyToWithdraw { get; set; }
    }
}