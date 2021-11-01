using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsForm : Window
    {
        public SettingsForm()
        {
            InitializeComponent();

            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (DatabaseContext context = new DatabaseContext())
            {

                this.RightToLeftLayout();

                LanguagesTextBox.ItemsSource = Entities.Languages;

                LanguagesTextBox.SelectedIndex = context.AppSettings.Single(x => x.Id == 1).LangIndex;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                AppSettings appSettings = context.AppSettings.Single(x => x.Id == 1);

                appSettings.LangIndex = LanguagesTextBox.SelectedIndex;
                appSettings.Code = LanguagesTextBox.SelectedValue.ToString();
                appSettings.Language = LanguagesTextBox.DisplayMemberPath.ToString();

                appSettings.Name = NameTextBox.Text;
                appSettings.LastName = LastNameTextBox.Text;
                appSettings.CompanyName = CompanyNameTextBox.Text;
                appSettings.CompanyAddress = CompanyAddressTextBox.Text;
                appSettings.CompanyPhone = CompanyPhoneTextBox.Text;
                appSettings.CompanyCommercialRegister = CompanyCommercialRegisterTextBox.Text;
                appSettings.CompanyStatisticalNumber = StatisticalNumberTextBox.Text;
                appSettings.CompanyTaxNumber = TaxNumberTextBox.Text;
                appSettings.CompanyBankAccount = BankAccountTextBox.Text;

                await context.SaveChangesAsync();
            }

            var res = MessageBox.Show(Properties.Resources.RestartMessage, Properties.Resources.RestartCaption, MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            switch(res)
            {
                case MessageBoxResult.OK:
                    Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
    }
}
