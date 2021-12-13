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
    /// Interaction logic for LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        Task Read()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                UsernameBox.ItemsSource = context.Users.ToList();
            }
            return Task.CompletedTask;
        }

        Task Auth()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                if(context.Users.Count(x => x.Username == UsernameBox.Text && x.Password == PassBox.Password) >= 1)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
            }

            return Task.CompletedTask;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Read();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            await Auth();
        }
    }
}
