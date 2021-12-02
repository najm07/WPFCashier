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

namespace WPFCashier.Forms
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void DealersButton_Click(object sender, RoutedEventArgs e)
        {
           /* DealersForm clientsForm = new DealersForm();
            clientsForm.Show();*/
            NavigationService.Navigate(new DealersPage());
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
           /* JournalsForm journalsForm = new JournalsForm();
            journalsForm.Show();*/
            NavigationService.Navigate(new JournalsPage());
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            //Entities.OpenNewOrRestoreWindow<SettingsForm>();
            NavigationService.Navigate(new SettingsPage());
        }   

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.RightToLeftLayout();
        }
    }
}
