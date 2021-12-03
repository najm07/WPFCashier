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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Read();
        }
    }
}
