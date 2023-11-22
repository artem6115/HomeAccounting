using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enum
{
    //Тип транзакций доход, расход или оба варианты
    public enum TypeTransaction
    {
        Expense=-1,
        IncomeAndExpense,
        Income
    }
}
