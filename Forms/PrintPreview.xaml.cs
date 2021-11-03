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

        public PrintPreview()
        {
            InitializeComponent();
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.RightToLeftLayout();
            ReportDataSource rs = new ReportDataSource();
            rs.Name = "DataSet1";
            rs.Value = Printedjournal;
            reportviewer.LocalReport.DataSources.Add(rs);
            ReportDataSource cl = new ReportDataSource();
            cl.Name = "DataSet2";
            cl.Value = Clientdetails;
            reportviewer.LocalReport.DataSources.Add(cl);
            ReportDataSource app = new ReportDataSource();
            app.Name = "DataSet3";
            app.Value = AppDetails;
            reportviewer.LocalReport.DataSources.Add(app);
            string Path = @"Reports/JournalReport.en-US.rdlc";
            reportviewer.LocalReport.ReportPath = Path;

            reportviewer.ZoomMode = ZoomMode.FullPage;

            reportviewer.RefreshReport();
        }
    }
}
