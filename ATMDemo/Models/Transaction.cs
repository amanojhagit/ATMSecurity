using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ATMDemo.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public string AccountNo { get; set; }
        public decimal TransactionAmount { get; set; }
        public string Purpose { get; set; }
        public DateTime Date { get; set; }
        public string TransactionType { get; set; }
    }

}
