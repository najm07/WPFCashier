﻿using System.ComponentModel.DataAnnotations;

namespace WPFCashier
{
    public class Journal
    {
        [Key]
        public int Id { get; set; }

        public int DealerId { get; set; }

        public int DealerType { get; set; }

        public int ReceiptNumber { get; set; }

        public string Date { get; set; }

        public string Type { get; set; }

        public decimal Amount { get; set; }

        public decimal OldCredit { get; set; }

        public decimal NewCredit { get; set; }
    }

    public class JournalMod
    {
        public int Id { get; set; }

        public int DealertId { get; set; }

        public string DealerName { get; set; }

        public string Date { get; set; }

        public string Type { get; set; }

        public int ReceiptNumber { get; set; }

        public decimal Amount { get; set; }

        public decimal OldCredit { get; set; }

        public decimal NewCredit { get; set; }
    }
}