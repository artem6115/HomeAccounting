using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Enum;

namespace DataLayer.Models
{
    //Фильтр для статистики
    public class StatisticFilter
    {
        [Range(1900, 3000)]
        public int Year { get; set; } = DateTime.Now.Year;
        [Range(1, 12)]
        public int Month { get; set; } = DateTime.Now.Month;
        public TypeTransaction TypeTransaction { get; set; }
        public TypeGroup TypeGroup { get; set; }
        public long AccountId { get; set; }

        // Использовать все счета
        public bool AllAccounts { get; set; } = true;

        //За весть промежуток времени
        public bool AllTime { get; set; }

    }
}
