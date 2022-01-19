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
        bool ContextChanged = true;

        private App()
        {
            Task.Run(() => Initialize()).Wait();

            using (DatabaseContext context = new DatabaseContext())
            {
                string code = context.AppSettings.Single(x => x.Id == 1).Code;
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(code);
            }
        }

        private async Task Initialize()
        {
            if (ContextChanged)
            {
                DatabaseFacade tempfacade = new DatabaseFacade(new TempDatabaseContext());
                if (await tempfacade.EnsureCreatedAsync())
                {
                    using (TempDatabaseContext tempContext = new TempDatabaseContext())
                    {
                        using (DatabaseContext context = new DatabaseContext())
                        {
                            tempContext.CopyDataBase(context);
                            await tempContext.SaveChangesAsync();
                            context.Dispose();
                        }

                        using (DatabaseContext context = new DatabaseContext())
                            await context.Database.EnsureDeletedAsync();

                        DatabaseFacade facade = new DatabaseFacade(new DatabaseContext());
                        if (await facade.EnsureCreatedAsync())
                        {
                            using (DatabaseContext context = new DatabaseContext())
                            {
                                context.CopyDataBase(tempContext);
                                if(context.Users.Count() == 0)
                                    context.Users.Add(new User() { Username = "Admin", Password = "1234", Role = 0 });
                                await context.SaveChangesAsync();
                                await tempContext.Database.EnsureDeletedAsync();
                            }
                        }
                    }
                }
            }
            else
            {
                DatabaseFacade facade = new DatabaseFacade(new DatabaseContext());
                if (await facade.EnsureCreatedAsync())
                {
                    using (DatabaseContext context = new DatabaseContext())
                    {
                        context.AppSettings.Add(new AppSettings() { Code = "en-US", Language = "English" });
                        context.Users.Add(new User() { Username = "Admin", Password = "1234", Role = 0 });
                        context.SaveChanges();
                    }
                }
            }

            using (DatabaseContext context = new DatabaseContext())
            {
                var code = context.AppSettings.Single(x => x.Id == 1).Code;
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(code);
            }
        }
    }
}
