using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WPFCashier
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            
        }

        App()
        {
            DatabaseFacade facade = new DatabaseFacade(new DatabaseContext());
            if (facade.EnsureCreated())
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    context.AppSettings.Add(new AppSettings() { Code = "en-US", Language = "English" });
                    context.SaveChanges();
                    //var code = context.AppSettings.Single(x => x.Id == 1).Code;
                    //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(code);
                }
            }

            using (DatabaseContext context = new DatabaseContext())
            {
                var code = context.AppSettings.Single(x => x.Id == 1).Code;
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(code);
            }
            
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en"); 
        }
    }
}
