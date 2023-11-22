using DataLayer.Entities;
using DataLayer.Models;



namespace PresentationLayer.Models
{
    public class TransactionViewModel
    {
        public List<Account> Accounts { get; set; }
        public List<Category> Categories { get; set; }
        public Filter Filter { get; set; }
        public List<Transaction> Transactions { get; set; }
        public int NumberOfLastPage { get; set; }


        //Адресс сервера, схема + адрес
        public string Url { get; set; }

        //Сумма всех транзакций
        public string Sum { get; set; }
        
    }
}
