using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class QueryTransactionResult
    {
        public int PageCount { get; set; }
        public double Sum { get; set; }
        public double IncomeAvg { get; set; }
        public double ExpenseAvg { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction>(0);

    }
}
