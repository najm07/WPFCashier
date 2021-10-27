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

                if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(address) && !String.IsNullOrEmpty(phone) && !String.IsNullOrEmpty(credit))
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

                if (!String.IsNullOrEmpty(name) && !String.IsNullOrEmpty(address) && !String.IsNullOrEmpty(phone) && !String.IsNullOrEmpty(credit))
                {
                    Client client = context.Clients.Find(selectedClient.Id);

                    client.Name = name;
                    client.Address = name;
                    client.Phone = name;
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

        private async void SuppliersReadButton_Click(object sender, RoutedEventArgs e)
        {
            await ReadSuppliers();
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
