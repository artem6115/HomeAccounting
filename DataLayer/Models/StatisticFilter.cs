using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Enum;

namespace DataLayer.Models
{
    public class StatisticFilter
    {
        [Range(1900, 3000)]
        public int Year { get; set; }
        [Range(1, 12)]
        public int Month { get; set; }
        public TypeTransaction TypeTransaction { get; set; }
        public TypeGroup TypeGroup { get; set; }
        public long AccountId { get; set; }
        public bool AllAccounts { get; set; }
        public bool AllTime { get; set; }

    }
}
