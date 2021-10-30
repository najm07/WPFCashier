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

namespace WPFCashier
{
    /// <summary>
    /// Interaction logic for ClientsForm.xaml
    /// </summary>
    public partial class DealersForm : Window
    {
        public List<Client> DbClient { get; private set; }
        public List<Supplier> DbSupplier { get; private set; }

        public DealersForm()
        {
            InitializeComponent();
        }

        #region Client

        public async Task Create()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var name = NameTextBox.Text;
                var address = AddressTextBox.Text;
                var phone = PhoneTextBox.Text;
                var credit = CreditTextBox.Text;

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(credit))
                {
                    context.Clients.Add(new Client() { Name = name, Address = address, Phone = phone, Credit = credit.StringtoDecimal() });
                    await context.SaveChangesAsync();
                }
            }
        }

        public Task Read()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                DbClient = context.Clients.ToList();
                ItemList.ItemsSource = DbClient;
            }
            return Task.CompletedTask;
        }

        public Task Read(string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                DbClient = context.Clients.Where(x => x.Name.ToLower().Contains(name)).ToList();
                ItemList.ItemsSource = DbClient;
            }
            return Task.CompletedTask;
        }

        public async Task Update()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                Client selectedClient = ItemList.SelectedItem as Client;
                var name = NameTextBox.Text;
                var address = AddressTextBox.Text;
                var phone = PhoneTextBox.Text;
                var credit = CreditTextBox.Text;

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(address) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(credit))
                {
                    Client client = context.Clients.Find(selectedClient.Id);

                    client.Name = name;
                    client.Address = address;
                    client.Phone = phone;
                    client.Credit = credit.StringtoDecimal();

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task Delete()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                Client selectedClient = ItemList.SelectedItem as Client;

                if (selectedClient != null)
                {
                    Client client = context.Clients.Single(x => x.Id == selectedClient.Id);

                    context.Remove(client);

                    await context.SaveChangesAsync();
                }
            }
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            await Create();
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
            ItemList.SelectedItem = null; //.Items.Clear();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text.Length == 0)
                Read();
            else
                Read(SearchTextBox.Text);
        }

        private void JournalButton_Click(object sender, RoutedEventArgs e)
        {
            JournalsForm journalsForm = new JournalsForm();
            journalsForm.Show();
        }

        #endregion

        #region Supplier

        public async Task CreateSuppliers()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var name = SuppliersNameTextBox.Text;
                var address = SuppliersAddressTextBox.Text;
                var phone = SuppliersPhoneTextBox.Text;
                var credit = SuppliersCreditTextBox.Text;

                if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(address) && !String.IsNullOrEmpty(phone) && !String.IsNullOrEmpty(credit))
                {
                    context.Suppliers.Add(new Supplier() { Name = name, Address = address, Phone = phone, Credit = credit.StringtoDecimal() });
                    await context.SaveChangesAsync();
                }
            }
        }

        public Task ReadSuppliers()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                DbSupplier = context.Suppliers.ToList();
                SuppliersItemList.ItemsSource = DbSupplier;
            }
            return Task.CompletedTask;
        }

        public Task ReadSuppliers(string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                DbSupplier = context.Suppliers.Where(x => x.Name.ToLower().Contains(name)).ToList();
                SuppliersItemList.ItemsSource = DbSupplier;
            }
            return Task.CompletedTask;
        }

        public async Task UpdateSuppliers()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                Supplier selectedSupplier = SuppliersItemList.SelectedItem as Supplier;
                var name = SuppliersNameTextBox.Text;
                var address = SuppliersAddressTextBox.Text;
                var phone = SuppliersPhoneTextBox.Text;
                var credit = SuppliersCreditTextBox.Text;

                if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(address) && !String.IsNullOrEmpty(phone) && !String.IsNullOrEmpty(credit))
                {
                    Supplier Supplier = context.Suppliers.Find(selectedSupplier.Id);

                    Supplier.Name = name;
                    Supplier.Address = address;
                    Supplier.Phone = phone;
                    Supplier.Credit = credit.StringtoDecimal();

                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteSuppliers()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                Supplier selectedSupplier = SuppliersItemList.SelectedItem as Supplier;

                if (selectedSupplier != null)
                {
                    Supplier supplier = context.Suppliers.Single(x => x.Id == selectedSupplier.Id);

                    context.Remove(supplier);

                    await context.SaveChangesAsync();
                }
            }
        }

        private async void SuppliersReadButton_Click(object sender, RoutedEventArgs e)
        {
            await ReadSuppliers();
        }

        private async void SuppliersCreateButton_Click(object sender, RoutedEventArgs e)
        {
            await CreateSuppliers();
        }

        private async void SuppliersUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            await UpdateSuppliers();
        }

        private async void SuppliersDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await DeleteSuppliers();
        }

        private void SuppliersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SuppliersItemList.SelectedItem = null; //.Items.Clear();
        }

        private async void SuppliersSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SuppliersSearchTextBox.Text.Length == 0)
                await ReadSuppliers();
            else
                await ReadSuppliers(SuppliersSearchTextBox.Text);
        }

        #endregion

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.RightToLeftLayout();
            await Read();
            await ReadSuppliers();
        }
    }
}
