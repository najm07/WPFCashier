using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCashier
{
    public class Expences
    {
        [Key]
        public int Id { get; set; }

        public string Date { get; set; }

        public string Type { get; set; }

        public decimal Amount { get; set; }
    }
}
