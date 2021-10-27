﻿using System;
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
            using (DBAccess context = new DBAccess())
            {
                LanguagesTextBox.ItemsSource = Entities.Languages;

                LanguagesTextBox.SelectedIndex = context.AppSettings.Single(x => x.Id == 1).LangIndex;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            using (DBAccess context = new DBAccess())
            {
                AppSettings appSettings = context.AppSettings.Single(x => x.Id == 1);

                appSettings.LangIndex = LanguagesTextBox.SelectedIndex;
                appSettings.Code = LanguagesTextBox.SelectedValue.ToString();
                appSettings.Language = LanguagesTextBox.DisplayMemberPath.ToString();

                await context.SaveChangesAsync();
            }
        }
    }
}
