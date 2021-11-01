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
using System.Data;

namespace WPFCashier
{
    /// <summary>
    /// Interaction logic for JournalsForm.xaml
    /// </summary>
    public partial class JournalsForm : Window
    {
        public List<Journal> DbJournals { get; private set; }
        public List<JournalMod> DbJournalsMOD { get; private set; }
        public List<Client> DbClient { get; private set; }
        public List<ClientNames> ClientListName;
        public List<Supplier> DbSupplier { get; private set; }
        public List<SupplierNames> SupplierListName;

        public JournalsForm()
        {
            InitializeComponent();
        }

        #region Client_History

        public async Task Create()
        {
            //Console.WriteLine("create function");
            using (DatabaseContext context = new DatabaseContext())
            {
                var client = ClientTextBox.Text;
                var date = DateTextBox.Text;
                var type = TypeTextBox.Text;
                var amount = AmountTextBox.Text;
                var receipt = ReceiptTextBox.Text;

                if (!string.IsNullOrEmpty(client) && !string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(amount) && !string.IsNullOrEmpty(receipt))
                {
                    var clientCredit = context.Clients.Single(x => x.Id == ClientTextBox.SelectedValue.ToString().StringtoInt()).Credit;
                    decimal newClientCredit = 0;

                    if (type.Equals("payment"))
                        newClientCredit = clientCredit - amount.StringtoDecimal();
                    else
                        newClientCredit = clientCredit + amount.StringtoDecimal();

                    context.Journals.Add(new Journal()
                    {
                        DealerId = ClientTextBox.SelectedValue.ToString().StringtoInt(),
                        DealerType = 0,
                        Date = date,
                        Type = type,
                        Amount = amount.StringtoDecimal(),
                        ReceiptNumber = receipt.StringtoInt(),
                        OldCredit = clientCredit,
                        NewCredit = newClientCredit
                    });

                    context.Clients.Single(x => x.Id == ClientTextBox.SelectedValue.ToString().StringtoInt()).Credit = newClientCredit;

                    await context.SaveChangesAsync();
                }
            }
        }

        public Task Read()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                //DbJournals = context.Journals.ToList();
                //ModJournal();
                TypeTextBox.ItemsSource = Entities.receiptType;
                ReadClientNames();

                var result = from j in context.Journals
                             join c in context.Clients on j.DealerId equals c.Id
                             where j.DealerType == 0
                             select new { Id = j.Id, ClientId = j.DealerId, ClientName = c.Name, Date = j.Date, Type = j.Type, ReceiptNumber = j.ReceiptNumber, Amount = j.Amount, Old = j.OldCredit, New = j.NewCredit};

                ItemList.ItemsSource = result.ToList();
                ClientTextBox.ItemsSource = ClientListName;
            }
            return Task.CompletedTask;
        }

        public Task Read(string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                DbClient = context.Clients.Where(x => x.Name.ToLower().Contains(name)).ToList();
                DbJournals = new List<Journal>();
                
                foreach (Client client in DbClient)
                {
                    var journals = context.Journals.Where(x => x.DealerId == client.Id && x.DealerType == 0).ToList();
                    foreach (Journal journal in journals)
                        DbJournals.Add(journal);
                }
                    

                var result = from j in DbJournals
                             join c in context.Clients on j.DealerId equals c.Id
                             where j.DealerId == c.Id && j.DealerType == 0
                             select new { Id = j.Id, ClientId = j.DealerId, ClientName = c.Name, Date = j.Date, Type = j.Type, ReceiptNumber = j.ReceiptNumber, Amount = j.Amount, Old = j.OldCredit, New = j.NewCredit };

                ItemList.ItemsSource = result.ToList();
            }
            return Task.CompletedTask;
        }

        public async Task Update()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var selectedJournal = ItemList.SelectedItem as JournalMod;
                var clientId = ClientTextBox.SelectedValue.ToString().StringtoInt();
                var date = DateTextBox.Text;
                var type = TypeTextBox.Text;
                var amount = AmountTextBox.Text;
                var receipt = ReceiptTextBox.Text;

                if (!String.IsNullOrEmpty(ClientTextBox.Text) && !String.IsNullOrEmpty(date) && !String.IsNullOrEmpty(type) && !String.IsNullOrEmpty(amount) && !String.IsNullOrEmpty(receipt))
                {
                    Journal journal = context.Journals.Find(selectedJournal.Id);

                    journal.DealerId = clientId;
                    journal.Date = date;
                    journal.Type = type;
                    journal.Amount = amount.StringtoDecimal();

                    if (type.Equals("payment"))
                        journal.NewCredit = journal.OldCredit - amount.StringtoDecimal();
                    else
                        journal.NewCredit = journal.OldCredit + amount.StringtoDecimal();

                    context.Clients.Single(x => x.Id == clientId).Credit = journal.NewCredit;

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task Delete()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                JournalMod selectedJournal = ItemList.SelectedItem as JournalMod;

                if (selectedJournal != null)
                {
                    Journal journal = context.Journals.Single(x => x.Id == selectedJournal.Id);

                    context.Remove(journal);

                    await context.SaveChangesAsync();
                }
            }
        }

        public Task ReadClientNames()
        {
            ClientListName = new List<ClientNames>();
            using (DatabaseContext context = new DatabaseContext())
            {
                foreach (Client client in context.Clients)
                    ClientListName.Add(new ClientNames() { Id = client.Id, Name = client.Name });
            }

            return Task.CompletedTask;
        }

        public Task<string> GetSingleClientName(int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var name = context.Clients.Single(x => x.Id == id).Name;
                return Task.FromResult(name);
            }
        }

        private async void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            await Read();
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            await Update();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await Delete();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ItemList.Items.Clear();
        }

        private async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text.Length == 0)
                await Read();
            else
                await Read(SearchTextBox.Text);
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            await Create();
        }

        #endregion

        #region Expences

        public async Task ExpencesCreate()
        {
            //Console.WriteLine("create function");
            using (DatabaseContext context = new DatabaseContext())
            {
                var date = ExpencesDateTextBox.Text;
                var type = ExpencesTypeTextBox.Text;
                var amount = ExpencesAmountTextBox.Text;

                if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(amount))
                {
                    context.Expences.Add(new Expences()
                    {
                        Date = date,
                        Type = type,
                        Amount = amount.StringtoDecimal(),
                    });

                    await context.SaveChangesAsync();
                }
            }
        }

        public Task ExpencesRead()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                ExpencesItemList.ItemsSource = context.Expences.ToList();
            }
            return Task.CompletedTask;
        }

        public Task ExpencesRead(string from, string to)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                ItemList.ItemsSource = context.Expences.Where(x => DateTime.Parse(x.Date) > DateTime.Parse(from) && DateTime.Parse(x.Date) < DateTime.Parse(to)).ToList();
            }
            return Task.CompletedTask;
        }

        public async Task ExpencesUpdate()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var selectedExpence = ItemList.SelectedItem as Expences;
                var date = ExpencesDateTextBox.Text;
                var type = ExpencesTypeTextBox.Text;
                var amount = ExpencesAmountTextBox.Text;

                if (!String.IsNullOrEmpty(date) && !String.IsNullOrEmpty(type) && !String.IsNullOrEmpty(amount))
                {
                    Expences expence = context.Expences.Find(selectedExpence.Id);

                    expence.Date = date;
                    expence.Type = type;
                    expence.Amount = amount.StringtoDecimal();

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task ExpencesDelete()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                Expences selectedExpence = ExpencesItemList.SelectedItem as Expences;

                if (selectedExpence != null)
                {
                    Expences expence = context.Expences.Single(x => x.Id == selectedExpence.Id);

                    context.Remove(expence);

                    await context.SaveChangesAsync();
                }
            }
        }

        #endregion

        #region Supplier_History

        public async Task CreateSupplier()
        {
            //Console.WriteLine("create function");
            using (DatabaseContext context = new DatabaseContext())
            {
                var supplier = SupplierTextBox.Text;
                var date = SupplierDateTextBox.Text;
                var type = SupplierTypeTextBox.Text;
                var amount = SupplierAmountTextBox.Text;
                var receipt = SupplierReceiptTextBox.Text;

                if (!string.IsNullOrEmpty(supplier) && !string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(amount) && !string.IsNullOrEmpty(receipt))
                {
                    var supplierCredit = context.Suppliers.Single(x => x.Id == SupplierTextBox.SelectedValue.ToString().StringtoInt()).Credit;
                    decimal newSupplierCredit = 0;

                    if (type.Equals("payment"))
                        newSupplierCredit = supplierCredit - amount.StringtoDecimal();
                    else
                        newSupplierCredit = supplierCredit + amount.StringtoDecimal();

                    context.Journals.Add(new Journal()
                    {
                        DealerId = SupplierTextBox.SelectedValue.ToString().StringtoInt(),
                        DealerType = 1,
                        Date = date,
                        Type = type,
                        Amount = amount.StringtoDecimal(),
                        ReceiptNumber = receipt.StringtoInt(),
                        OldCredit = supplierCredit,
                        NewCredit = newSupplierCredit
                    });

                    context.Suppliers.Single(x => x.Id == SupplierTextBox.SelectedValue.ToString().StringtoInt()).Credit = newSupplierCredit;

                    await context.SaveChangesAsync();
                }
            }
        }

        public Task ReadSupplier()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                //DbJournals = context.Journals.ToList();
                //ModJournal();
                SupplierTypeTextBox.ItemsSource = Entities.receiptType;
                ReadSupplierNames();

                var result = from j in context.Journals
                             join c in context.Suppliers on j.DealerId equals c.Id
                             where j.DealerType == 1
                             select new { Id = j.Id, SupplierId = j.DealerId, SupplierName = c.Name, Date = j.Date, Type = j.Type, ReceiptNumber = j.ReceiptNumber, Amount = j.Amount, Old = j.OldCredit, New = j.NewCredit };

                SupplierItemList.ItemsSource = result.ToList();
                SupplierTextBox.ItemsSource = SupplierListName;
            }
            return Task.CompletedTask;
        }

        public Task ReadSupplier(string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                DbSupplier = context.Suppliers.Where(x => x.Name.ToLower().Contains(name)).ToList();
                DbJournals = new List<Journal>();

                foreach (Supplier supplier in DbSupplier)
                {
                    var journals = context.Journals.Where(x => x.DealerId == supplier.Id && x.DealerType == 1).ToList();
                    foreach (Journal journal in journals)
                        DbJournals.Add(journal);
                }


                var result = from j in DbJournals
                             join c in context.Suppliers on j.DealerId equals c.Id
                             where j.DealerId == c.Id && j.DealerType == 1
                             select new { Id = j.Id, SupplierId = j.DealerId, SupplierName = c.Name, Date = j.Date, Type = j.Type, ReceiptNumber = j.ReceiptNumber, Amount = j.Amount, Old = j.OldCredit, New = j.NewCredit };

                SupplierItemList.ItemsSource = result.ToList();
            }
            return Task.CompletedTask;
        }

        public Task ReadSupplierNames()
        {
            SupplierListName = new List<SupplierNames>();
            using (DatabaseContext context = new DatabaseContext())
            {
                foreach (Supplier supplier in context.Suppliers)
                    SupplierListName.Add(new SupplierNames() { Id = supplier.Id, Name = supplier.Name });
            }

            return Task.CompletedTask;
        }

        public async Task UpdateSupplier()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var selectedJournal = SupplierItemList.SelectedItem as JournalMod;
                var supplierId = SupplierTextBox.SelectedValue.ToString().StringtoInt();
                var date = SupplierDateTextBox.Text;
                var type = SupplierTypeTextBox.Text;
                var amount = SupplierAmountTextBox.Text;
                var receipt = SupplierReceiptTextBox.Text;

                if (!String.IsNullOrEmpty(SupplierTextBox.Text) && !String.IsNullOrEmpty(date) && !String.IsNullOrEmpty(type) && !String.IsNullOrEmpty(amount) && !String.IsNullOrEmpty(receipt))
                {
                    Journal journal = context.Journals.Find(selectedJournal.Id);

                    journal.DealerId = supplierId;
                    journal.Date = date;
                    journal.Type = type;
                    journal.Amount = amount.StringtoDecimal();

                    if (type.Equals("payment"))
                        journal.NewCredit = journal.OldCredit - amount.StringtoDecimal();
                    else
                        journal.NewCredit = journal.OldCredit + amount.StringtoDecimal();

                    context.Suppliers.Single(x => x.Id == supplierId).Credit = journal.NewCredit;

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteSupplier()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                JournalMod selectedJournal = SupplierItemList.SelectedItem as JournalMod;

                if (selectedJournal != null)
                {
                    Journal journal = context.Journals.Single(x => x.Id == selectedJournal.Id);

                    context.Remove(journal);

                    await context.SaveChangesAsync();

                }
            }
        }

        private async void SupplierReadButton_Click(object sender, RoutedEventArgs e)
        {
            await ReadSupplier();
        }

        private async void SupplierUpdateButton_Click(object sender, RoutedEventArgs e)
        {   if (SupplierEnableCheckBox.IsChecked == false)
                SupplierEnableCheckBox.IsChecked=true;
            await UpdateSupplier();
        }

        private async void SupplierDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await DeleteSupplier();
        }

        private void SupplierMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SupplierItemList.Items.Clear();
        }

        private async void SupplierSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SupplierSearchTextBox.Text.Length == 0)
                await ReadSupplier();
            else
                await ReadSupplier(SupplierSearchTextBox.Text);
        }

        private async void SupplierCreateButton_Click(object sender, RoutedEventArgs e)
        {
            await CreateSupplier();
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.RightToLeftLayout();
            Read();
            ReadSupplier();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var result = from j in context.Journals
                             select j ;
                StiReport report = new StiReport();
                report.Load(@"C:\Users\Mohamed\source\repos\WPFCashier\bin\Debug\Report.mrt");
                report.Compile();
               StiPage page = report.Pages[0];
                report.RegData(result);
                report.Dictionary.Synchronize();
                report.Render();
                report.Show();
            }
        }
    }
}
