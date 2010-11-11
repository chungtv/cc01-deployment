using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Data;

namespace Wpf_Linq
{
    /// <summary>
    /// Interaction logic for ucNhanKhau.xaml
    /// </summary>
    public partial class ucNhanKhau : UserControl
    {
        clsHoKhauTemp hokhau;
        IQueryable<clsChiTietHoKhauTemp> nhanKhaus;
        CollectionViewSource nhanKhauViewSource;

        public ucNhanKhau(clsHoKhauTemp hokhau)
        {
            InitializeComponent();
            this.hokhau = hokhau;
            this.nhanKhaus = hokhau.ChiTiet;
            nhanKhauViewSource = ((CollectionViewSource)(this.FindResource("nhanKhauViewSource")));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            nhanKhauViewSource.Source = nhanKhaus;
            nhanKhauViewSource.View.MoveCurrentToFirst();
            lblSoHoKhau.Content = hokhau.SoHoKhau;
            lblDiaChi.Content = hokhau.SoNha + " - " + hokhau.TenDuong + " - " + hokhau.PhuongXa + " - " + hokhau.QuanHuyen + " - " + hokhau.TinhTP;
            lbxNhanKhau.Focus();
        }

        private void lbxNhanKhau_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
