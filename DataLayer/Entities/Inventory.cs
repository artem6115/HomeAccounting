using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities
{
    public class Inventory
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public virtual Account Account { get; set; } = null!;

    }
}
