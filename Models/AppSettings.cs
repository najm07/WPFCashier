using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCashier
{
    public class AppSettings
    {
        [Key]
        public int Id { get; set; }

        public int LangIndex { get; set; }

        public string Code { get; set; }

        public string Language { get; set; }

        public string Theme { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string CompanyAddress { get; set; }

        public string CompanyPhone { get; set; }

        public string CompanyCommercialRegister { get; set; }

        public string CompanyTaxNumber { get; set; }

        public string CompanyStatisticalNumber { get; set; }

        public string CompanyBankAccount { get; set; }
    }
}
