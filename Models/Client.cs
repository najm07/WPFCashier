using System.ComponentModel.DataAnnotations;

namespace WPFCashier
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public decimal Credit { get; set; }
    }

    public class ClientNames
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}