using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFCashier
{
    public static class Entities
    {
        public static List<string> receiptType = new List<string>() { "payment", "loan" };
        public static List<Lang> Languages = new List<Lang>() { new Lang() { Index = 0, Code = "en-US", Language = "English" }, new Lang() { Index = 1, Code = "ar-DZ", Language = "Arabic" }};

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

        public static void RightToLeftLayout(this Window window)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                if (context.AppSettings.Single(x => x.Id == 1).LangIndex == 1)
                {
                    window.FlowDirection = FlowDirection.RightToLeft;
                }
            }
        }
    }

    public class Lang
    {
        public int Index { get; set; }
        public string Code { get; set; }
        public string Language { get; set; }
    }
}
