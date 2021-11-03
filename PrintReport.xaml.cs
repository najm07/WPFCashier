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
    public partial class PrintReport : Window
    {
        public List<JournalMod> Printedjournal=new List<JournalMod>();
        public List<Client> Clientdetails = new List<Client>();
        public List<AppSettings> AppDetails = new List<AppSettings>();
        public PrintReport()
        {
            InitializeComponent();
           
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            
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
            string Path = @"C:\Users\Mohamed\Source\Repos\WPFCashier\Report1.rdlc";
            reportviewer.LocalReport.ReportPath = Path;
           

              reportviewer.Dock = DockStyle.Fill;
             reportviewer.SetDisplayMode(DisplayMode.Normal);

            reportviewer.RefreshReport();
            // reportviewer.Refresh();
        }
    }
}
