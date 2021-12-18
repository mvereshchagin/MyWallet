using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    [Serializable]
    public class Wallet : ICollection<MoneyItem>
    {
        #region private members
        private List<MoneyItem> items = new List<MoneyItem>();
        #endregion

        #region ICollection implementation
        public MoneyItem this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                items[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return ((ICollection<MoneyItem>)items).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((ICollection<MoneyItem>)items).IsReadOnly;
            }
        }

        public void Add(MoneyItem item)
        {
            ((ICollection<MoneyItem>)items).Add(item);
        }

        public void Clear()
        {
            ((ICollection<MoneyItem>)items).Clear();
        }

        public bool Contains(MoneyItem item)
        {
            return ((ICollection<MoneyItem>)items).Contains(item);
        }

        public void CopyTo(MoneyItem[] array, int arrayIndex)
        {
            ((ICollection<MoneyItem>)items).CopyTo(array, arrayIndex);
        }

        public IEnumerator<MoneyItem> GetEnumerator()
        {
            return ((ICollection<MoneyItem>)items).GetEnumerator();
        }

        public bool Remove(MoneyItem item)
        {
            return ((ICollection<MoneyItem>)items).Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<MoneyItem>)items).GetEnumerator();
        }
        #endregion

        #region public methods
        /// <summary>
        /// Counts all money in wallet in chosen currency
        /// </summary>
        /// <param name="currency">resulting currency</param>
        /// <returns>count of money</returns>
        public double CountAllMoney(Currency currency)
        {
            double sum = 0.0;
            foreach (var item in items)
                sum += item.ConvertTo(currency);

            return sum;
        }

        /// <summary>
        /// Count money stored in chosen currency
        /// </summary>
        /// <param name="currency">currency</param>
        /// <returns>amount of money in currency</returns>
        public double CountMoney(Currency currency)
        {
            if (currency == null)
                return 0.0;

            double sum = 0.0;
            foreach (var item in items)
                if (item.Currency == currency)
                    sum += item.Amount;

            return sum;
        }

        public double CountMoney(string currencyName)
        {
            Currency currency = null;
            foreach (var item in items)
                if (String.Equals(item.Currency.Name, currencyName,
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    currency = item.Currency;
                    break;
                }
            return this.CountMoney(currency);       
        }

        public IList<MoneyItem> FindItems(Currency currency)
        {
            var list = new List<MoneyItem>();
            foreach(var item in this.items)
                if(item.Currency == currency)
                    list.Add(item);

            return list;
        }

        public IList<MoneyItem> FindItems(string currencyName)
        {
            foreach (var item in this.items)
                if (String.Equals(item.Currency.Name, currencyName, 
                    StringComparison.InvariantCultureIgnoreCase))
                    return this.FindItems(item.Currency);

            return new List<MoneyItem>();
        }

        public double ComputeTotal(Currency currency)
        {
            double res = 0.0;
            foreach (var item in this.FindItems(currency))
                res += item.Amount;
            return res;
        }

        public double ComputeTotal(string currencyName)
        {
            double res = 0.0;
            foreach (var item in this.FindItems(currencyName))
                res += item.Amount;
            return res;
        }

        public double this[Currency currency] => ComputeTotal(currency);

        public double this[string currencyName] => ComputeTotal(currencyName);

        /// <summary>
        /// Filter data
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="currency"></param>
        /// <param name="minAmount"></param>
        /// <param name="maxAmount"></param>
        /// <returns></returns>
        public IList<MoneyItem> Filter(DateTime? startDate = null, DateTime? endDate = null,
            Currency currency = null, double? minAmount = null, double? maxAmount = null)
        {
            if (!startDate.HasValue)
                startDate = new DateTime(year: 1900, month: 1, day: 1);

            if (!endDate.HasValue)
                endDate = DateTime.Now;

            if (!minAmount.HasValue)
                minAmount = 0.0;

            if (!maxAmount.HasValue)
                maxAmount = Double.MaxValue;

            var list = new List<MoneyItem>();
            foreach(var item in this.items)
            {
                if (startDate.HasValue && item.TransferDate < startDate.Value)
                    continue;
                if (endDate.HasValue && item.TransferDate > endDate.Value)
                    continue;
                if (!(currency is null) && item.Currency != currency)
                    continue;
                if (minAmount.HasValue && item.Amount < minAmount.Value)
                    continue;
                if (maxAmount.HasValue && item.Amount > maxAmount.Value)
                    continue;

                list.Add(item);
            }

            return list;
        }

        public IList<MoneyItem> FilterByLastMonth(Currency currency = null,
            double? minAmount = null, double? maxAmount = null)
        {
            var now = DateTime.Now;
            var startDate = new DateTime(year: now.Year, month: now.Month, day: 1);
            return this.Filter(startDate: startDate, endDate: now, currency: currency, 
                minAmount: minAmount, maxAmount: maxAmount);
        }

        public IList<MoneyItem> FilterByLastYear(Currency currency = null,
            double? minAmount = null, double? maxAmount = null)
        {
            var now = DateTime.Now;
            var startDate = new DateTime(year: now.Year, month: 1, day: 1);
            return this.Filter(startDate: startDate, endDate: now, currency: currency,
                minAmount: minAmount, maxAmount: maxAmount);
        }
        #endregion

        #region overriden
        public override string ToString()
        {
            List<string> strList = new List<string>();
            foreach (var item in items)
                strList.Add(item.ToString());
            return String.Join("\r\n", strList);
        }
        #endregion
    }
}
