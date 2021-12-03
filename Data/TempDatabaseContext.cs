using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCashier
{
    public class TempDatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = tempdb.db");
        }

        public DbSet<Client> Clients { get; set; }
        
        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Journal> Journals { get; set; }

        public DbSet<Expences> Expences { get; set; }
        
        public DbSet<AppSettings> AppSettings { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}
