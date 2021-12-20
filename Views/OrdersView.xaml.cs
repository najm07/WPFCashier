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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFCashier
{
    /// <summary>
    /// Interaction logic for OrdersView.xaml
    /// </summary>
    public partial class OrdersView : Page
    {
        public int OrderType;
        public OrdersView(int orderType)
        {
            InitializeComponent();
            OrderType = orderType;
        }

        private void NewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            OrdersTabControl.Items.Add("New Order");
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var result = from o in context.Orders
                             join c in context.Clients on o.DealerId equals c.Id
                             where o.Type == OrderType
                             select new { Id = o.Id, DealerId = o.DealerId, DealerName = c.Name, Date = o.Date, OrderNumber = o.Number, Total = o.Total, Payed = o.Payed, Rest = o.Rest };

                OrdersList.ItemsSource = result.ToList();
            }
        }
    }
}
