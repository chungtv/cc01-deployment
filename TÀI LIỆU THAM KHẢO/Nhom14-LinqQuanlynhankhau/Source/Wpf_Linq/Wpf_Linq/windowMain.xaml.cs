using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;

namespace Wpf_Linq
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class windowMain : Window
    {
        public static string connectionString;
        public windowMain()
        {
            InitializeComponent();

            Encrypt.Encrypt encrypt = new Encrypt.Encrypt();
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "main.config";
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            connectionString = encrypt.ToInsecureString(encrypt.DecryptString(config.AppSettings.Settings["connectstring"].Value));
        }
        //private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        //{
        //    Button button = sender as Button;
        //    button.ContextMenu.IsOpen = false;
        //    e.Handled = true;
        //}

        //private void Button_ContextMenuOpening(object sender, RoutedEventArgs e)
        //{
        //    // Get the button and check for nulls
        //    Button button = sender as Button;
        //    if (button == null || button.ContextMenu == null)
        //        return;
        //    // Set the placement target of the ContextMenu to the button
        //    button.ContextMenu.PlacementTarget = button;
        //    // Open the ContextMenu
        //    button.ContextMenu.IsOpen = true;
        //    e.Handled = true;
        //}

        private bool check(string header)
        {
            if (ucMain.tc.Items.Count == 0)
            {
                return true;
            }
            else
            {
                foreach (TabItem chform in ucMain.tc.Items)
                {
                    if (chform.Header.ToString() == header)
                    {
                        chform.Focus();
                        return false;
                    }
                }
            }
            return true;
        }
        private void addChildren(string header, UserControl uc)
        {
            TabItem myTabItem = new TabItem();
            myTabItem.Header = header;
            myTabItem.Content = uc;
            ucMain.tc.Items.Add(myTabItem);
            ucMain.tc.SelectedItem = myTabItem;
        }
        private void btnQuanLy_Click(object sender, RoutedEventArgs e)
        {
            if (check("Quản lý hộ khẩu"))
                addChildren("Quản lý hộ khẩu", new ucHoKhau());
        }

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            if (check("Tìm kiếm hộ khẩu"))
                addChildren("Tìm kiếm hộ khẩu", new ucTimKiem());
        }

        private void btnBaoCao_Click(object sender, RoutedEventArgs e)
        {
            if (check("Báo cáo hộ khẩu"))
                addChildren("Báo cáo hộ khẩu", new ucBaoCaoHoKhau());
        }

        private void btnHuongDan_Click(object sender, RoutedEventArgs e)
        {
            if (check("Hướng dẫn"))
                addChildren("Hướng dẫn", new ucTemp());
        }

        private void btnCauHinh_Click(object sender, RoutedEventArgs e)
        {
            if (check("Cấu hình"))
                addChildren("Cấu hình", new ucCauHinh());
        }

        private void miChuyenHoKhau_Click(object sender, RoutedEventArgs e)
        {
            if (check("Chuyển hộ khẩu"))
                addChildren("Chuyển hộ khẩu", new ucChuyenHoKhau());
        }

        private void miCatHoKhau_Click(object sender, RoutedEventArgs e)
        {
            if (check("Cắt hộ khẩu"))
                addChildren("Cắt hộ khẩu", new ucCatHoKhau());
        }

        private void miTimKiemNhanKhau_Click(object sender, RoutedEventArgs e)
        {
            if (check("Tìm kiếm nhân khẩu"))
                addChildren("Tìm kiếm nhân khẩu", new ucTimKiemNhanKhau());
        }

        private void miBaocaoNhanKhau_Click(object sender, RoutedEventArgs e)
        {
            if (check("Báo cáo nhân khẩu"))
                addChildren("Báo cáo nhân khẩu", new ucBaoCaoNhanKhau());
        }


    }
}
