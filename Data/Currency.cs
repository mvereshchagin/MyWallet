using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    /// <summary>
    /// Currency class
    /// </summary>
    [Serializable]
    public class Currency : IEquatable<Currency>, IComparable<Currency>, IComparable
    {
        #region Properties

        /// <summary>
        /// Short name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Full Name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Exchange Rate
        /// </summary>
        public double ExchangeRate { get; set; }

        #endregion

        #region public methods
        public string SaveToString(char separator = ',')
        {
            return SaveToString(this, separator);
        }
        #endregion

        #region operators
        public static bool operator == (Currency first, Currency second)
        {
            return Equals(first, second);     
        }

        public static bool operator !=(Currency first, Currency second)
        {
            return !(first == second);
        }
        #endregion

        #region static methods

        public static bool Equals(Currency first, Currency second)
        {
            if (Object.ReferenceEquals(first, null))
                return false;

            if (Object.ReferenceEquals(second, null))
                return false;

            return String.Equals(first.Name, second.Name,
                StringComparison.InvariantCultureIgnoreCase);
        }

        public static string SaveToString(Currency currency, char separator = ',')
        {
            if (currency is null)
                throw new ArgumentNullException("currency");

            string[] args = new string[] { currency.Name, currency.FullName, currency.ExchangeRate.ToString() };
            return String.Join(separator: separator.ToString(), args);

            //StringBuilder sb = new StringBuilder();
            //sb.Append(currency.Name).Append(currency.FullName).Append(currency.ExchangeRate);
            //return sb.ToString();
        }

        public static Currency LoadFromString(string data, char separator = ',')
        {
            if (String.IsNullOrEmpty(data))
                return null;

            var items = data.Split(separator);
            if (items.Length < 3)
                return null;

            Currency currency = new Currency();
            currency.Name = items[0];
            currency.FullName = items[1];
            try
            {
                currency.ExchangeRate = Convert.ToDouble(items[2]);
            }
            catch
            { }

            return currency;
        }

        #endregion

        #region overriden

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return false;
            if (obj.GetType() != this.GetType())
                return false;

            return Equals(this, obj as Currency);
        }

        public override string ToString()
        {
            return this.Name;
        }

        #endregion

        #region IEquatables
        public bool Equals(Currency other)
        {
            return Equals(this, other);
        }

        #endregion

        #region IComparable
        public int CompareTo(Currency other)
        {
            if (other == null || other.ExchangeRate == 0)
                return 1;

            return ExchangeRate.CompareTo(other.ExchangeRate);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj.GetType() == this.GetType())
                return 1;

            return CompareTo(obj as Currency);
        }


        #endregion

    }
}
