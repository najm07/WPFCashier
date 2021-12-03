using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFCashier
{
    /// <summary>
    /// Interaction logic for DetailsPage.xaml
    /// </summary>
    public partial class DetailsPage : Page
    {
        public Client Client { get; private set; }
        public Supplier Supplier { get; private set; }
        public ObservableCollection<JournalMod> SomeListViewList { get; set; }

        private readonly int dealerType;
        private readonly int dealerId;


        public DetailsPage(Client client)
        {
            Client = client;
            dealerType = Entities.Client;
            dealerId = client.Id;
            InitializeComponent();
        }

        public DetailsPage(Supplier supplier)
        {
            Supplier = supplier;
            dealerType = Entities.Supplier;
            dealerId = supplier.Id;
            InitializeComponent();
        }

        public Task ShowDealerDetails(string name, string address, string phone, decimal credit)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var journals = context.Journals.Where(x => x.DealerId == dealerId && x.DealerType == dealerType).ToList();
                if (dealerType.IsClient())
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
                List<JournalMod> sortlist = new List<JournalMod>();

                using (DatabaseContext context = new DatabaseContext())
                {
                    appsetting = context.AppSettings.Single(x => x.Id == 1);
                    PrintPreview printReport = new PrintPreview(0);
                    foreach (var item in ItemList.SelectedItems)
                    {
                        var s = item as JournalMod;
                        sortlist.Add(s);
                    }

                    var ascendingOrder = sortlist.OrderBy(i => i.Id);
                    printReport.Printedjournal.AddRange(ascendingOrder);
                    printReport.Clientdetails.Add(Client);
                    printReport.AppDetails.Add(appsetting);
                    printReport.Show();
                }
            }

            return Task.CompletedTask;
        }
        public async void SelectClientorSupplier()
        {
            if (Client != null)
                await ShowDealerDetails(Client.Name, Client.Address, Client.Phone, Client.Credit);
            else
                await ShowDealerDetails(Supplier.Name, Supplier.Address, Supplier.Phone, Supplier.Credit);
        }

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if (DateFrom.SelectedDate.HasValue && DateTo.SelectedDate.HasValue)
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

            SelectAllItems.IsChecked = false;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ItemList.SelectedItem = null;
            SelectAllItems.IsChecked = false;
        }

        private void SelectAllItems_Checked(object sender, RoutedEventArgs e)
        {
            ItemList.SelectAll();
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /* HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
             if (r.VisualHit.GetType() != typeof(ListViewItem))
             {
                 ItemList.SelectedItems.Clear();
                 SelectAllItems.IsChecked = false;
             }
                */
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.RightToLeftLayout();
            SelectClientorSupplier();
        }
    }
}

