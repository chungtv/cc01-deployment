using System.Windows;
using System.Collections.ObjectModel;
using Wpf_Linq;
using WPF.MDI;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Controls;

namespace Mdi_Wpf_Linq
{
    /// <summary>
    /// Interaction logic for windowMain.xaml
    /// </summary>
    public partial class windowMain : Window
    {
        public windowMain()
        {
            InitializeComponent();
        }

        private void addChildren(string title, UserControl uc)
        {
            bool found = false;
            ObservableCollection<MdiChild> charr = mdiContainer.Children;
            if (charr.Count == 0)
            {
                add(title, uc);
            }
            else
            {
                foreach (MdiChild chform in charr)
                {
                    if (chform.Title == title)
                    {
                        chform.Focus();
                        found = true;
                        break;
                    }
                    else
                        found = false;

                }
                if (found == false)
                {
                    add(title, uc);
                }
            }
        }
        private void add(string title, UserControl uc)
        {
            mdiContainer.Children.Add(new MdiChild()
            {
                Title = title,
                Content = uc,
                Width = 800,
                Height = 600,
                MinWidth = 800,
                MinHeight = 600,
                Icon = new BitmapImage(new Uri("OriginalLogo.png", UriKind.Relative))
            });
        }

        private void quanLyHoKhau_Click(object sender, RoutedEventArgs e)
        {
            addChildren("Quản lý hộ khẩu", new ucHoKhau());
        }

        private void timKiem_Click(object sender, RoutedEventArgs e)
        {
            addChildren("Tìm kiếm", new ucTemp());
        }

        private void baoCao_Click(object sender, RoutedEventArgs e)
        {
            addChildren("Báo cáo", new ucTemp());
        }
    }
}
