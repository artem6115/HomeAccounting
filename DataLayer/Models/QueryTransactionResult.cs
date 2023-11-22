using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    //Результат фильтрации
    public class QueryTransactionResult
    {
        //Количество страниц в выборке
        public int PageCount { get; set; }

        // Сумма всех транзакций в выборке
        public double Sum { get; set; }
        public double IncomeAvg { get; set; }
        public double ExpenseAvg { get; set; }

        //Сами странзакции на странице
        public List<Transaction> Transactions { get; set; } = new List<Transaction>(0);

    }
}
