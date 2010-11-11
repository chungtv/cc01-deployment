using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DataLayer;

namespace Wpf_Linq
{
    /// <summary>
    /// Interaction logic for windowCapNhatQHVoiChuHo.xaml
    /// </summary>
    public partial class windowCapNhatQHVoiChuHo : Window
    {
        List<clsChiTietHoKhauTemp> ChiTiet;
        public windowCapNhatQHVoiChuHo(List<clsChiTietHoKhauTemp> ChiTiet)
        {
            InitializeComponent();
            this.ChiTiet = ChiTiet;
            for (int i = 0; i < ChiTiet.Count; ++i)
                ChiTiet[i].QHVoiChuHo = string.Empty;
            dataGridCapNhat.ItemsSource = ChiTiet;
        }

        private void btnDongY_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
    public class QHList : List<string>
    {
        public QHList()
        {
            HokhauDataContext hKDataContext = new HokhauDataContext(windowMain.connectionString);
            var query = from qh in hKDataContext.QUANHEs
                        where qh.MAQH != 0
                        select qh.QUANHE1;
            List<string> temp = query.ToList();
            for (int i = 0; i < temp.Count(); ++i)
            {
                this.Add(temp[i]);
            }
        }
    }
}
