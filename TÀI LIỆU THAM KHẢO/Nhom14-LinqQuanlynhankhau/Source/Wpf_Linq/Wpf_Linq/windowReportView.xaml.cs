using System.Windows;
using CrystalDecisions.Windows.Forms;

namespace Wpf_Linq
{
    /// <summary>
    /// Interaction logic for windowReportView.xaml
    /// </summary>
    public partial class windowReportView : Window
    {
        public CrystalReportViewer rptViewer = new CrystalReportViewer();

        public windowReportView()
        {
            InitializeComponent();

            host.Child = rptViewer;
        }
    }
}
