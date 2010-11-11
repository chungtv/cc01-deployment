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

namespace Wpf_Linq
{
    /// <summary>
    /// Interaction logic for windowNhanKhau.xaml
    /// </summary>
    public partial class windowNhanKhau : Window
    {
        public windowNhanKhau(clsHoKhauTemp hokhau)
        {
            InitializeComponent();
            this.Title = "So ho khau : " + hokhau.SoHoKhau;
            ucNhanKhau uc = new ucNhanKhau(hokhau);
            myGrid.Children.Add(uc);
        }
    }
}
