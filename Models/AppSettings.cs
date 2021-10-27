using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCashier
{
    class AppSettings
    {
        [Key]
        public int Id { get; set; }

        public int LangIndex { get; set; }

        public string Code { get; set; }

        public string Language { get; set; }

        public string Theme { get; set; }
    }
}
