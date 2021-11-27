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

        /// <summary>
        /// list of payment types
        /// </summary>
        public static List<PaymentType> receiptType = new List<PaymentType>() { new PaymentType() { Index = 0, Name = Properties.Resources.Payment }, new PaymentType() { Index = 1, Name = Properties.Resources.Loan } };

        public static List<ReportPath> reportPaths = new List<ReportPath>() { new ReportPath() { Index = 0, Path = Properties.Resources.JournalReport } };

        public static int Client = 0;

        public static int Supplier = 1;

        public static int Payment = 0;

        public static int Loan = 1;

        /// <summary>
        /// A list that contains the languages
        /// </summary>
        public static List<Lang> Languages = new List<Lang>() { new Lang() { Index = 0, Code = "en-US", Language = Properties.Resources.English }, new Lang() { Index = 1, Code = "ar-DZ", Language = Properties.Resources.Arabic }};

        /// <summary>
        /// A function that parse a string into
        /// an decimal
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static decimal StringtoDecimal(this string text)
        {
            decimal number;
            Decimal.TryParse(text, out number);
            return number;
        }

        /// <summary>
        /// A function that parse a string into
        /// an integer
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int StringtoInt(this string text)
        {
            int number;
            Int32.TryParse(text, out number);
            return number;
        }

        /// <summary>
        /// This function changes the form 
        /// layout to "Right to Left"
        /// </summary>
        /// <param name="window"></param>
        public static void RightToLeftLayout(this Window window)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                if (context.AppSettings.Single(x => x.Id == 1).LangIndex == 1)
                    window.FlowDirection = FlowDirection.RightToLeft;
            }
        }

        public static bool IsArabic(this Window window)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                if (context.AppSettings.Single(x => x.Id == 1).LangIndex == 1)
                    return true;

                return false;
            }
        }

        public static bool IsClient(this int dealertype)
        {
            if (dealertype == 0)
                return true;

            return false;
        } 
        
        public static bool IsPayment(this int type)
        {
            if (type == 0)
                return true;

            return false;
        }

        public static bool IsEmpty(this string text)
        {
            if (text.Length == 0)
                return true;

            return false;
        }
    }

    public class Lang
    {
        public int Index { get; set; }
        public string Code { get; set; }
        public string Language { get; set; }
    }

    public class PaymentType
    {
        public int Index { get; set; }
        public string Name { get; set; }
    }

    public class ReportPath
    {
        public int Index { get; set; }

        public string Path { get; set; }
    }
}
