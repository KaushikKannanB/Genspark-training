namespace Bank.Models.DTOs
{
    public class DepositRequestDTO
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public float MoneyToDeposit { get; set; }
    }
}