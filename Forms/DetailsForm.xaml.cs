﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFCashier.Forms
{
    /// <summary>
    /// Interaction logic for DetailsForm.xaml
    /// </summary>
    public partial class DetailsForm : Window
    {
        public Client client { get; private set; }
        public Supplier supplier { get; private set; }

        private int dealerType;
        private int dealerId;


        public DetailsForm(Client client)
        {
            this.client = client;
            this.dealerType = 0;
            this.dealerId = client.Id;
            InitializeComponent();
        }
        public DetailsForm(Supplier supplier)
        {
            this.supplier = supplier;
            this.dealerType = 1;
            this.dealerId = supplier.Id;
            InitializeComponent();
        }

        public Task ShowDealerDetails(string name, string address, string phone, decimal credit)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var journals = context.Journals.Where(x => x.DealerId == dealerId && x.DealerType == dealerType).ToList();
                if (dealerType == 0)
                {
                    var result = from j in journals
                             join c in context.Clients on j.DealerId equals c.Id
                             join t in Entities.receiptType on j.Type equals t.Index
                             where j.DealerType == dealerType
                             select new JournalMod { Id = j.Id, DealerId = j.DealerId, DealerName = c.Name, Date = j.Date, TypeIndex = j.Type, TypeName = t.Name, ReceiptNumber = j.ReceiptNumber, Amount = j.Amount, OldCredit = j.OldCredit, NewCredit = j.NewCredit };
                    ItemList.ItemsSource = result.ToList();
                }
                else
                {
                    var result = from j in journals
                                 join c in context.Suppliers on j.DealerId equals c.Id
                                 join t in Entities.receiptType on j.Type equals t.Index
                                 where j.DealerType == dealerType
                                 select new JournalMod { Id = j.Id, DealerId = j.DealerId, DealerName = c.Name, Date = j.Date, TypeIndex = j.Type, TypeName = t.Name, ReceiptNumber = j.ReceiptNumber, Amount = j.Amount, OldCredit = j.OldCredit, NewCredit = j.NewCredit };
                    ItemList.ItemsSource = result.ToList();
                }
            }

            LabelName.Content = name;
            LabelAddress.Content = address;
            LabelPhone.Content = phone;
            LabelCredit.Content = credit;

            return Task.CompletedTask;
        }

        private  void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.RightToLeftLayout();
            SelectClientorSupplier();

        }

        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            await Print();
        }

        public Task Print()
        {
            if (ItemList.SelectedItem != null)
            {
               // Client client = new Client();
                AppSettings appsetting = new AppSettings();
                using (DatabaseContext context = new DatabaseContext())
                {
                    appsetting = context.AppSettings.Single(x => x.Id == 1);
                    PrintPreview printReport = new PrintPreview(0);
                    var q = ItemList.SelectedItem as JournalMod;
                    printReport.Printedjournal.Add(q);
                    printReport.Clientdetails.Add(client);
                    printReport.AppDetails.Add(appsetting);
                    printReport.Show();
                }
            }

            return Task.CompletedTask;
        }
        public async void SelectClientorSupplier()
        {
            if (client != null)
                await ShowDealerDetails(client.Name, client.Address, client.Phone, client.Credit);
            else
                await ShowDealerDetails(supplier.Name, supplier.Address, supplier.Phone, supplier.Credit);
           
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if(DateFrom.SelectedDate.HasValue && DateTo.SelectedDate.HasValue)
            {
                using (DatabaseContext context = new DatabaseContext())
                {
                    if (dealerType.IsClient())
                    {
                        var journals = context.Journals.Where(x => x.DealerId == dealerId && x.DealerType == dealerType).ToList();
                        var list = from j in journals
                                   join c in context.Clients on j.DealerId equals c.Id
                                   join t in Entities.receiptType on j.Type equals t.Index
                                   where j.DealerType == dealerType
                                   select new JournalMod { Id = j.Id, DealerId = j.DealerId, DealerName = c.Name, Date = j.Date, TypeIndex = j.Type, TypeName = t.Name, ReceiptNumber = j.ReceiptNumber, Amount = j.Amount, OldCredit = j.OldCredit, NewCredit = j.NewCredit };

                        var res = list.Where(x => DateTime.Parse(x.Date) >= DateFrom.SelectedDate && DateTime.Parse(x.Date) <= DateTo.SelectedDate);
                        ItemList.ItemsSource = res;
                    }
                    else
                    {
                        var journals = context.Journals.Where(x => x.DealerId == dealerId && x.DealerType == dealerType).ToList();
                        var list = from j in journals
                                     join c in context.Suppliers on j.DealerId equals c.Id
                                     join t in Entities.receiptType on j.Type equals t.Index
                                     where j.DealerType == dealerType
                                     select new JournalMod { Id = j.Id, DealerId = j.DealerId, DealerName = c.Name, Date = j.Date, TypeIndex = j.Type, TypeName = t.Name, ReceiptNumber = j.ReceiptNumber, Amount = j.Amount, OldCredit = j.OldCredit, NewCredit = j.NewCredit };

                        var res = list.Where(x => DateTime.Parse(x.Date) >= DateFrom.SelectedDate && DateTime.Parse(x.Date) <= DateTo.SelectedDate);
                        ItemList.ItemsSource = res;
                    }
                }
            }
        }
    }
}
