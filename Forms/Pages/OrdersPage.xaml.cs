using Dragablz;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        private readonly IInterTabClient _interTabClient;
        private readonly ObservableCollection<TabContent> _tabContents = new ObservableCollection<TabContent>();
        public OrdersPage()
        {
            _interTabClient = new DefaultInterTabClient();
            InitializeComponent();
        }

        public ObservableCollection<TabContent> TabContents
        {
            get { return _tabContents; }
        }

        public IInterTabClient InterTabClient
        {
            get { return _interTabClient; }
        }

        public static Func<object> NewItemFactory
        {
            get { return () => new TabContent("Introduction", new SingleOrderPage()); }
        }
    }
}
