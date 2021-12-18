using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace Utils
{
    [Serializable]
    public class MoneyItemsFilter
    {
        public DateTime? DateStart { get; set; }    
        public DateTime? DateEnd { get; set; }  
        public Currency? Currency { get; set; }

        public double? MinAmount { get; set; }

        public double? MaxAmount { get; set; }
    }
}
