namespace Bank.Models
{
    public class Admin
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public Master? Master{ get; set; }
    }
}