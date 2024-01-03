using DataLayer.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Settings
    {
        //Inventoty
        public TimeGroup AutoInvetory { get; set; } = TimeGroup.None;

        //Transaction
        public bool CreateBalancingTransaction { get; set; } = true;

        //Telegram

        public bool AddNewCategoryIfNotExist { get; set; } = false;
        public long? AccountId { get; set; } = null;

    }
}
