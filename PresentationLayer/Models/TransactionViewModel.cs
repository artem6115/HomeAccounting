using DataLayer.Entities;
using DataLayer;


namespace PresentationLayer.Models
{
    public class TransactionViewModel
    {
        public List<Account> Accounts { get; set; }
        public List<Category> Categories { get; set; }
        public Filter Filter { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int NumberOfPage { get; set; } 
        public int NumberOfLastPage { get; set; }
    }
}
