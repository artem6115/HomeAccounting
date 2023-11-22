using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    //Объект для передачи на клиент
    public class StatisticData
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Description { get; set; }
    }
}
