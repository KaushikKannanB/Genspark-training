namespace Bank.Models.DTOs
{
    public class DepositRequestDTO
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public float MoneyToDeposit { get; set; }
    }
}