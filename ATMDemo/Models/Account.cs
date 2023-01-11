namespace ATMDemo.Models
{
    public class Account
    {
        public Guid ID { get; set; }
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public decimal Amount { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Passport { get; set; }
        public string DateOfOpened { get; set; }
        public string AccountType { get; set; }
        public string FingerprintID { get; set; }
        public string SMS { get; set; }
        public string SMSport { get; set; }
    }

}
