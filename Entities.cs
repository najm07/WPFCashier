using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCashier
{
    public static class Entities
    {
        public static List<string> receiptType = new List<string>() { "payment", "loan" };
        public static List<string> Languages = new List<string>() { "English", "Arabic" };

        public static decimal StringtoDecimal(this string text)
        {
            decimal number;
            Decimal.TryParse(text, out number);
            return number;
        }

        public static int StringtoInt(this string text)
        {
            int number;
            Int32.TryParse(text, out number);
            return number;
        }
    }
}
