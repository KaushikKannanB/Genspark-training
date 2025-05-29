namespace Bank.Models.DTOs
{
    public class BankTransferDTO
    {
        public int UserIdCredit { get; set; }
        public int UserIdDebit { get; set; }
        public string PasswordDebitUser { get; set; }
        public float Amount { get; set; }
    }

}