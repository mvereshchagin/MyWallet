using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class SimpleExchangeRateRetiever : ExchangeRateRetriever
    {
        public override double Retrieve(string currencyName)
        {
            switch(currencyName)
            {
                case "RUR":
                    return 1.0;
                case "USD":
                    return 75.0;
                case "EUR":
                    return 85.0;
            }

            return 0.0;
        }
    }
}
