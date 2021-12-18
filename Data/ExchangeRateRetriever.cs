using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public abstract class ExchangeRateRetriever
    {
        public abstract double Retrieve(string currencyName);

        public override string ToString()
        {
            return "Retieve data avount Exchange Rate";
        }
    }
}
