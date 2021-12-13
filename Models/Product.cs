using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCashier
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public int Name { get; set; }

        public int Designation { get; set; }

        public int Quantity { get; set; }

        public int MinQuantity { get; set; }

        public string Unit { get; set; }

        public int UnitQuantity { get; set; }

        public string AddedDate { get; set; }

        public int Category { get; set; }

        public decimal BuyPrice { get; set; }

        public decimal SellPrice { get; set; }

        public decimal RetailSellPrice { get; set; }

        public decimal Discount { get; set; }

        public string Barcode { get; set; }
    }
}
