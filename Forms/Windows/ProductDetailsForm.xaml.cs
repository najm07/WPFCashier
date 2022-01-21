using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                var category = CategoryComboBox.Text;
                var designation = DesignationTextBox.Text;
                var buyprice = BuyPriceTextBox.Text;
                var sellprice = SellPriceTextBox.Text;
                var retailsellprice = RetailSellPriceTextBox.Text;
                var discount = DiscountTextBox.Text;
                var quantity = QuantityTextBox.Text;
                var minquantity = MinQuantityTextBox.Text;
                var unit = UnitComboBox.Text;
                var unitquantity = UnitQuantityTextBox.Text;

                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(quantity) && !string.IsNullOrEmpty(sellprice) && !string.IsNullOrEmpty(unit))
                {
                    context.Products.Add(new Product() { Name = name,Category=category,Designation=designation,BuyPrice=buyprice.StringtoInt(),
                        SellPrice = sellprice.StringtoInt(),RetailSellPrice=retailsellprice.StringtoInt(),Discount=discount.StringtoInt(),
                        Quantity = quantity.StringtoInt(),MinQuantity=minquantity.StringtoInt(), Unit = unit,UnitQuantity=unitquantity.StringtoInt() });
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task Update()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var name = NameTextBox.Text;
                var category = CategoryComboBox.Text;
                var designation = DesignationTextBox.Text;
                var buyprice = BuyPriceTextBox.Text;
                var sellprice = SellPriceTextBox.Text;
                var retailsellprice = RetailSellPriceTextBox.Text;
                var discount = DiscountTextBox.Text;
                var quantity = QuantityTextBox.Text;
                var minquantity = MinQuantityTextBox.Text;
                var unit = UnitComboBox.Text;
                var unitquantity = UnitQuantityTextBox.Text;


                if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(quantity) || !string.IsNullOrEmpty(sellprice) || !string.IsNullOrEmpty(unit))
                {
                    Product product= context.Products.Find(selectedProduct.Id);
                    product.Name = name;
                    product.Category = category;
                    product.Designation = designation;
                    product.BuyPrice = buyprice.StringtoInt();
                    product.SellPrice = sellprice.StringtoInt();
                    product.RetailSellPrice = retailsellprice.StringtoInt();
                    product.Discount = discount.StringtoInt();
                    product.Quantity = quantity.StringtoInt();
                    product.MinQuantity = minquantity.StringtoInt();
                    product.Unit = unit;
                    product.UnitQuantity = unitquantity.StringtoInt();
                    
                    await context.SaveChangesAsync();
                }
            }
        }

        public Task ReadCategories()
        {
            using (DatabaseContext context = new DatabaseContext())
            {

                CategoryComboBox.ItemsSource = context.Categories.ToList();
            }
            return Task.CompletedTask;
        }

        public Task ReadUnits()
        {
            using (DatabaseContext context = new DatabaseContext())
            {

                UnitComboBox.ItemsSource = context.Units.ToList();
            }
            return Task.CompletedTask;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (update == 0)
                await Create();
            else
                await Update();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            //  Application.Current.MainWindow = newWindow;
            // var myWindow = Window.GetWindow(this);
            // myWindow.Close();
            this.DialogResult = true;
        }

        private void DiscountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
           /* Regex regex = new Regex("[^0-9]+");
            if (regex.IsMatch(e.Text))
            {
                MessageBox.Show("Invalid Input !");
            }*/
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ReadCategories();
            await ReadUnits();
        }

        private async void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            AddForm addForm = new AddForm(Entities.Category);
            addForm.WindowParent = (Window)this;
           addForm.ShowDialog();
            if (addForm.DialogResult == true)
                await ReadCategories();
        }

        private async void AddUnitButton_Click(object sender, RoutedEventArgs e)
        {
            AddForm addForm = new AddForm(Entities.Unit);
            addForm.WindowParent = (Window)this;
            addForm.ShowDialog();
            if (addForm.DialogResult == true)
                await ReadUnits();
        }
    }
}
