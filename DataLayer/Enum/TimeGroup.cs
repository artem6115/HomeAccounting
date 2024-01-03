using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enum
{
    //Как группировать транзакции и баланс
    public enum TimeGroup
    {
        Day,
        Weeks,
        Month,
        None
    }
}
