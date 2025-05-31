namespace Bank.Models.DTOs
{
    public class BankTransferDTO
    {
        public string UserIdCredit { get; set; }
        public string UserIdDebit { get; set; }
        public string PasswordDebitUser { get; set; }
        public float Amount { get; set; }
    }

}