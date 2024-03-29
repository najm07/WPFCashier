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
                var type = TypeTextBox.SelectedValue.ToString().StringtoInt();
                var amount = AmountTextBox.Text;
                var receipt = ReceiptTextBox.Text;

                if (!string.IsNullOrEmpty(client) && !string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(amount))
                {
                    var clientCredit = context.Clients.Single(x => x.Id == ClientTextBox.SelectedValue.ToString().StringtoInt()).Credit;
                    decimal newClientCredit = 0;

                    if (type.IsPayment())
                        newClientCredit = clientCredit - amount.StringtoDecimal();
                    else
                        newClientCredit = clientCredit + amount.StringtoDecimal();

                    context.Journals.Add(new Journal()
                    {
                        DealerId = ClientTextBox.SelectedValue.ToString().StringtoInt(),
                        DealerType = Entities.Client,
                        Date = date,
                        Type = type,
                        Amount = amount.StringtoDecimal(),
                        ReceiptNumber = receipt,
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
                             join t in Entities.receiptType on j.Type equals t.Index
                             where j.DealerType == Entities.Client
                             select new JournalMod { Id = j.Id, DealerId = j.DealerId, DealerName = c.Name, Date = j.Date, TypeIndex = j.Type, TypeName = t.Name, ReceiptNumber = j.ReceiptNumber, Amount = j.Amount, OldCredit = j.OldCredit, NewCredit = j.NewCredit };

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
                    var journals = context.Journals.Where(x => x.DealerId == client.Id && x.DealerType == Entities.Client).ToList();
                    foreach (Journal journal in journals)
                        DbJournals.Add(journal);
                }
                    

                var result = from j in DbJournals
                             join c in context.Clients on j.DealerId equals c.Id
                             join t in Entities.receiptType on j.Type equals t.Index
                             where j.DealerType == Entities.Client
                             select new JournalMod { Id = j.Id, DealerId = j.DealerId, DealerName = c.Name, Date = j.Date, TypeIndex = j.Type, TypeName = t.Name, ReceiptNumber = j.ReceiptNumber, Amount = j.Amount, OldCredit = j.OldCredit, NewCredit = j.NewCredit };

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
                var type = TypeTextBox.SelectedValue.ToString().StringtoInt();
                var amount = AmountTextBox.Text;
                var receipt = ReceiptTextBox.Text;

                if (!String.IsNullOrEmpty(ClientTextBox.Text) && !String.IsNullOrEmpty(date) && !String.IsNullOrEmpty(amount))
                {
                    Journal journal = context.Journals.Find(selectedJournal.Id);

                    journal.DealerId = clientId;
                    journal.Date = date;
                    journal.Type = type;
                    journal.Amount = amount.StringtoDecimal();

                    if (type.IsPayment())
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

                    if (journal.Type.IsPayment())
                        context.Clients.Single(x => x.Id == journal.DealerId).Credit = journal.NewCredit + journal.Amount;
                    else
                        context.Clients.Single(x => x.Id == journal.DealerId).Credit = journal.NewCredit - journal.Amount;

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

        public Task Print()
        {
            if (ItemList.SelectedItem != null)
            {
                Client client = new Client();
                AppSettings appsetting = new AppSettings();
                using (DatabaseContext context = new DatabaseContext())
                {

                    var clientId = ClientTextBox.SelectedValue.ToString().StringtoInt();


                    if (!String.IsNullOrEmpty(ClientTextBox.Text))
                    {

                        client = context.Clients.Find(clientId);
                    }

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

        private async void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            await Read();
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if(ItemList.SelectedItem == null)
                await Create();
            else
                await Update();

            await Read();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await Delete();
            await Read();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ItemList.SelectedItem = null;
            SupplierItemList.SelectedItem = null;
            ExpencesItemList.SelectedItem = null;
            //ItemList.Items.Clear();
        }

        private async void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text.IsEmpty())
                await Read();
            else
                await Read(SearchTextBox.Text);
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            await Create();
        }

        private async void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            await Print();
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
        private async void ExpencesDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await ExpencesDelete();
            await ExpencesRead();
        }

        private async void ExpencesUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExpencesItemList.SelectedItem == null)
                await ExpencesCreate();
            else
                await ExpencesUpdate();
            await ExpencesRead();
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
                var type = SupplierTypeTextBox.SelectedValue.ToString().StringtoInt();
                var amount = SupplierAmountTextBox.Text;
                var receipt = SupplierReceiptTextBox.Text;

                if (!string.IsNullOrEmpty(supplier) && !string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(amount))
                {
                    var supplierCredit = context.Suppliers.Single(x => x.Id == SupplierTextBox.SelectedValue.ToString().StringtoInt()).Credit;
                    decimal newSupplierCredit = 0;

                    if (type.IsPayment())
                        newSupplierCredit = supplierCredit - amount.StringtoDecimal();
                    else
                        newSupplierCredit = supplierCredit + amount.StringtoDecimal();

                    context.Journals.Add(new Journal()
                    {
                        DealerId = SupplierTextBox.SelectedValue.ToString().StringtoInt(),
                        DealerType = Entities.Supplier,
                        Date = date,
                        Type = type,
                        Amount = amount.StringtoDecimal(),
                        ReceiptNumber = receipt,
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
                             join t in Entities.receiptType on j.Type equals t.Index
                             where j.DealerType == Entities.Supplier
                             select new JournalMod { Id = j.Id, DealerId = j.DealerId, DealerName = c.Name, Date = j.Date, TypeIndex = j.Type, TypeName = t.Name, ReceiptNumber = j.ReceiptNumber, Amount = j.Amount, OldCredit = j.OldCredit, NewCredit = j.NewCredit };

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
                    var journals = context.Journals.Where(x => x.DealerId == supplier.Id && x.DealerType == Entities.Supplier).ToList();
                    foreach (Journal journal in journals)
                        DbJournals.Add(journal);
                }


                var result = from j in DbJournals
                             join c in context.Suppliers on j.DealerId equals c.Id
                             join t in Entities.receiptType on j.Type equals t.Index
                             where j.DealerType == Entities.Supplier
                             select new JournalMod { Id = j.Id, DealerId = j.DealerId, DealerName = c.Name, Date = j.Date, TypeIndex = j.Type, TypeName = t.Name, ReceiptNumber = j.ReceiptNumber, Amount = j.Amount, OldCredit = j.OldCredit, NewCredit = j.NewCredit };

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
                var type = SupplierTypeTextBox.SelectedValue.ToString().StringtoInt();
                var amount = SupplierAmountTextBox.Text;
                var receipt = SupplierReceiptTextBox.Text;

                if (!String.IsNullOrEmpty(SupplierTextBox.Text) && !String.IsNullOrEmpty(date) && !String.IsNullOrEmpty(amount))
                {
                    Journal journal = context.Journals.Find(selectedJournal.Id);

                    journal.DealerId = supplierId;
                    journal.Date = date;
                    journal.Type = type;
                    journal.Amount = amount.StringtoDecimal();

                    if (type.IsPayment())
                        journal.NewCredit = journal.OldCredit + amount.StringtoDecimal();
                    else
                        journal.NewCredit = journal.OldCredit - amount.StringtoDecimal();

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

                    if (journal.Type.IsPayment())
                        context.Suppliers.Single(x => x.Id == journal.DealerId).Credit = journal.NewCredit - journal.Amount;
                    else
                        context.Suppliers.Single(x => x.Id == journal.DealerId).Credit = journal.NewCredit + journal.Amount;

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
        {
            if (SupplierItemList.SelectedItem == null)
                await CreateSupplier();
            else
                await UpdateSupplier();

            await ReadSupplier();
        }

        private async void SupplierDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await DeleteSupplier();
            await ReadSupplier();
        }

        private void SupplierMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SupplierItemList.SelectedItem = null;
        }

        private async void SupplierSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SupplierSearchTextBox.Text.IsEmpty())
                await ReadSupplier();
            else
                await ReadSupplier(SupplierSearchTextBox.Text);
        }

        private async void SupplierCreateButton_Click(object sender, RoutedEventArgs e)
        {
            await CreateSupplier();
        }

        #endregion

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.RightToLeftLayout();
            await Read();
            await ExpencesRead();
            await ReadSupplier();
        }
    }
}
