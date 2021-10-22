using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCashier
{
    class DBAccess:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = db.db");
        }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Journal> Journals { get; set; }

        public DbSet<Expences> Expences { get; set; }
    }
}
