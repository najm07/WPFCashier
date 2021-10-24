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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClientsButton_Click(object sender, RoutedEventArgs e)
        {
            ClientsForm clientsForm = new ClientsForm();
            clientsForm.Show();
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            JournalsForm journalsForm = new JournalsForm();
            journalsForm.Show();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsForm settings = new SettingsForm();
            settings.Show();
        }
    }
}
