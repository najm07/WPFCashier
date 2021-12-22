using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Reporting.WinForms;

namespace WPFCashier
{
    /// <summary>
    /// Interaction logic for PrintReport.xaml
    /// </summary>
    public partial class PrintPreview : Window
    {
        
        public List<JournalMod> Printedjournal = new List<JournalMod>();
        public List<Client> Clientdetails = new List<Client>();
        public List<AppSettings> AppDetails = new List<AppSettings>();

        public int ReportIndex = -1;

        public PrintPreview(int reportIndex)
        {
            this.ReportIndex = reportIndex;
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
          //  this.RightToLeftLayout();
            await ShowReport(ReportIndex);
        }

        private Task ShowReport(int reportIndex)
        {
            ReportDataSource rs = new ReportDataSource();
            rs.Name = "JournalModDataSet";
            rs.Value = Printedjournal;
            reportviewer.LocalReport.DataSources.Add(rs);
            ReportDataSource cl = new ReportDataSource
            {
                Name = "ClientDataSet",
                Value = Clientdetails
            };
            reportviewer.LocalReport.DataSources.Add(cl);
            ReportDataSource app = new ReportDataSource
            {
                Name = "SettingsDataSet",
                Value = AppDetails
            };
            reportviewer.LocalReport.DataSources.Add(app);
            string Path;

            Path = Entities.reportPaths.Single(x => x.Index == reportIndex).Path;

            reportviewer.LocalReport.ReportPath = Path;

            reportviewer.ZoomMode = ZoomMode.FullPage;

            reportviewer.RefreshReport();

            return Task.CompletedTask;
        }
    }
}
