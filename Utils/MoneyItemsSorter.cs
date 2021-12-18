using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    [Serializable]
    public class MoneyItemsSorter
    {
        public SortDirection Direction { get; set; }
        public string Name { get; set; }
    }
}
