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
    /// Interaction logic for AddForm.xaml
    /// </summary>
    public partial class AddForm : Window
    {
        public Window WindowParent;

        public int Type=-1;

        public AddForm(int type)
        {
            Type = type;
            InitializeComponent();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            await Create();
        }

        public async Task Create()
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                var name = NameTextBox.Text;
               

                if (!string.IsNullOrEmpty(name))
                {
                    if (Type == Entities.Category)
                    {
                        context.Categories.Add(new Category() { Name = name });
                    }
                    else if(Type == Entities.Unit)
                    {
                        context.Units.Add(new Unit() { Name = name });
                    }
                    
                    await context.SaveChangesAsync();
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
