using System;
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
        public List<Journal> DbJournals { get; private set; }


        public DetailsForm(Client client)
        {
            this.client = client;
            InitializeComponent();
        }
        public DetailsForm(Supplier supplier)
        {
            this.supplier = supplier;
            InitializeComponent();
        }

        public Task ShowClient()
        {
            DbJournals = new List<Journal>();
            using (DatabaseContext context = new DatabaseContext())
            {
                var journals = context.Journals.Where(x => x.DealerId == client.Id && x.DealerType == 0).ToList();
                foreach (Journal journal in journals)
                   DbJournals.Add(journal);



            var result = from j in DbJournals
                         join c in context.Clients on j.DealerId equals c.Id
                         join t in Entities.receiptType on j.Type equals t.Index
                         where j.DealerType == 0
                         select new JournalMod { Id = j.Id, DealerId = j.DealerId, DealerName = c.Name, Date = j.Date, TypeIndex = j.Type, TypeName = t.Name, ReceiptNumber = j.ReceiptNumber, Amount = j.Amount, OldCredit = j.OldCredit, NewCredit = j.NewCredit };

            ItemList.ItemsSource = result.ToList();

            }
            return Task.CompletedTask;
        }

        public Task ShowSupplier()
        {
            DbJournals = new List<Journal>();
            using (DatabaseContext context = new DatabaseContext())
            {
                var journals = context.Journals.Where(x => x.DealerId == supplier.Id && x.DealerType == 1).ToList();
                foreach (Journal journal in journals)
                    DbJournals.Add(journal);



                var result = from j in DbJournals
                             join c in context.Suppliers on j.DealerId equals c.Id
                             join t in Entities.receiptType on j.Type equals t.Index
                             where j.DealerType == 1
                             select new JournalMod { Id = j.Id, DealerId = j.DealerId, DealerName = c.Name, Date = j.Date, TypeIndex = j.Type, TypeName = t.Name, ReceiptNumber = j.ReceiptNumber, Amount = j.Amount, OldCredit = j.OldCredit, NewCredit = j.NewCredit };

                ItemList.ItemsSource = result.ToList();

            }
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
                }
                PrintPreview printReport = new PrintPreview(0);
                var q = ItemList.SelectedItem as JournalMod;
                printReport.Printedjournal.Add(q);
                printReport.Clientdetails.Add(client);
                printReport.AppDetails.Add(appsetting);
                printReport.Show();
            }

            return Task.CompletedTask;
        }
        public async void SelectClientorSupplier()
        {
            if (client != null)
            {

                LabelName.Content = client.Name;
                LabelAddress.Content = client.Address;
                LabelPhone.Content = client.Phone;
                await ShowClient();
            }
            else
            {

                LabelName.Content = supplier.Name;
                LabelAddress.Content = supplier.Address;
                LabelPhone.Content = supplier.Phone;
                await ShowSupplier();
            }
           
        }
    }
}
