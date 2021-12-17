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
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class ProductsForm : Window
    {
        public ProductsForm()
        {
            InitializeComponent();
        }

        public Task Read()
        {
            using (DatabaseContext context = new DatabaseContext())
            {

                ItemList.ItemsSource = context.Products.ToList();
            }
            return Task.CompletedTask;
        }

        public async Task Delete()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                Product selectedProduct = ItemList.SelectedItem as Product;

                if (selectedProduct != null)
                {
                    Product product = context.Products.Single(x => x.Id == selectedProduct.Id);

                    context.Remove(product);

                    await context.SaveChangesAsync();
                }
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Read();
        }

        private async void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            ProductDetailsForm productDetailsForm = new ProductDetailsForm();
            productDetailsForm.WindowParent = (Window)this;
            productDetailsForm.ShowDialog();
            if (productDetailsForm.DialogResult == true)
                await Read();
        }

        private void ItemList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Product selectedProduct = ItemList.SelectedItem as Product;
            ProductDetailsForm productDetailsForm = new ProductDetailsForm(selectedProduct);
            productDetailsForm.WindowParent = (Window)this;
            productDetailsForm.Show();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            await Delete();
            await Read();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Product selectedProduct = ItemList.SelectedItem as Product;
            ProductDetailsForm productDetailsForm = new ProductDetailsForm(selectedProduct);
            productDetailsForm.WindowParent = (Window)this;
            productDetailsForm.Show();
        }
    }
}
