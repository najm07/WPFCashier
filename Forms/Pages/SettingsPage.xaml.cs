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
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.RightToLeftLayout();
            await Read();
        }

        public Task Read()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                LanguagesTextBox.ItemsSource = Entities.Languages;

                LanguagesTextBox.SelectedIndex = context.AppSettings.Single(x => x.Id == 1).LangIndex;

                AppSettings appSettings = context.AppSettings.Single(x => x.Id == 1);

                 NameTextBox.Text = appSettings.Name;
                 LastNameTextBox.Text = appSettings.LastName;
                 CompanyNameTextBox.Text = appSettings.CompanyName;
                 CompanyAddressTextBox.Text = appSettings.CompanyAddress;
                 CompanyPhoneTextBox.Text = appSettings.CompanyPhone;
                 CompanyCommercialRegisterTextBox.Text = appSettings.CompanyCommercialRegister;
                 StatisticalNumberTextBox.Text = appSettings.CompanyStatisticalNumber;
                 TaxNumberTextBox.Text = appSettings.CompanyTaxNumber;
                 BankAccountTextBox.Text = appSettings.CompanyBankAccount;
            }

            return Task.CompletedTask;
        }

        public async void Update()
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
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Update();

            var res = MessageBox.Show(Properties.Resources.RestartMessage, Properties.Resources.RestartCaption, MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            switch(res)
            {
                case MessageBoxResult.OK:
                    Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.Cancel:
                    await Read();
                    break;
            }
        }
    }
}
