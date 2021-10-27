using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCashier
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public decimal Credit { get; set; }
    }

    public class SupplierNames
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
