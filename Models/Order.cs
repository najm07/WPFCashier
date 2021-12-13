using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCashier
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int Number { get; set; }

        public string DealerId { get; set; }

        public int Type { get; set; }

        public decimal Total { get; set; }

        public decimal Payed { get; set; }

        public decimal Rest { get; set; }
    }
}
