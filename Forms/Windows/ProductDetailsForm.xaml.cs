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
    /// Interaction logic for ProductDetailsForm.xaml
    /// </summary>
    public partial class ProductDetailsForm : Window
    {
        public List<Product> DbProduct { get; private set; }
        public  Window WindowParent;
        private int update = 0;
        private Product selectedProduct;
        public ProductDetailsForm()
        {
            InitializeComponent();
        }
        public ProductDetailsForm(Product product)
        {
            this.selectedProduct = product;
            update = 1;
            InitializeComponent();
        }

        public async Task Create()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var name = NameTextBox.Text;
                var quantity = QuantityTextBox.Text;
                var unit = UnitTextBox.Text;
                var price = SellPriceTextBox.Text;

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(quantity) && !string.IsNullOrEmpty(price) && !string.IsNullOrEmpty(unit))
                {
                    context.Products.Add(new Product() { Name = name, Quantity = quantity.StringtoInt(), Unit = unit, SellPrice = price.StringtoDecimal() });
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task Update()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var name = NameTextBox.Text;
                var quantity = QuantityTextBox.Text;
                var unit = UnitTextBox.Text;
                var price = SellPriceTextBox.Text;

                if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(quantity) || !string.IsNullOrEmpty(price) || !string.IsNullOrEmpty(unit))
                {
                    Product product= context.Products.Find(selectedProduct.Id);
                    product.Name = name;
                    product.Quantity = quantity.StringtoInt();
                    product.Unit = unit;
                    product.SellPrice = price.StringtoDecimal();
                    await context.SaveChangesAsync();
                }
            }
        }


        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (update == 0)
                await Create();
            else
                await Update();

            this.Close();
           
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ProductsForm newWindow = new ProductsForm();
            //  Application.Current.MainWindow = newWindow;
            // var myWindow = Window.GetWindow(this);
            // myWindow.Close();
            WindowParent.Close();
            newWindow.Show();
        }
    }
}
