using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.ConnectionUI;
using DataLayer;
using System.Configuration;
using Encrypt;

namespace Wpf_Linq
{
    /// <summary>
    /// Interaction logic for ucCauHinh.xaml
    /// </summary>
    public partial class ucCauHinh : UserControl
    {
        private string strConnect;
        private string mode;
        private ExeConfigurationFileMap fileMap;
        private Configuration config;
        Encrypt.Encrypt encrypt = new Encrypt.Encrypt();

        public ucCauHinh()
        {
            InitializeComponent();
        }

        private void btn3Cham_Click(object sender, RoutedEventArgs e)
        {
            DataConnectionDialog dcd = new DataConnectionDialog();
            DataConnectionConfiguration dcs = new DataConnectionConfiguration();
            dcs.LoadConfiguration(dcd);

            if (DataConnectionDialog.Show(dcd) == System.Windows.Forms.DialogResult.OK)//DialogResult.OK
            {
                //check
                //HokhauDataContext hk = new HokhauDataContext(dcd.ConnectionString);
                strConnect = dcd.ConnectionString;
                txtChuoiKetNoi.Text = strConnect;
            }
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            string newmode = cboCheDoMo.SelectedIndex.ToString() == "0" ? "Tab" : "Mdi";

            if (mode != newmode)
            {
                mode = newmode;
                config.AppSettings.Settings["mode"].Value = mode;
                if (MessageBox.Show("Chế độ mở đã thay đổi. Bạn có muốn khởi động lại chương trình ?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start("Main.exe");
                    config.AppSettings.Settings["connectstring"].Value = strConnect;
                    config.Save(ConfigurationSaveMode.Modified);
                    Application.Current.Shutdown();
                    return;//???
                }
            }
            config.AppSettings.Settings["connectstring"].Value = encrypt.EncryptString(encrypt.ToSecureString(strConnect));
            windowMain.connectionString = strConnect;

            config.Save(ConfigurationSaveMode.Modified);
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e)
        {
            cboCheDoMo.SelectedIndex = mode == "Tab" ? 0 : 1;
            txtChuoiKetNoi.Text = strConnect;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "main.config";
            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            strConnect = encrypt.ToInsecureString(encrypt.DecryptString(config.AppSettings.Settings["connectstring"].Value));
            mode = config.AppSettings.Settings["mode"].Value;

            cboCheDoMo.SelectedIndex = mode == "Tab" ? 0 : 1;
            txtChuoiKetNoi.Text = strConnect;
        }
    }
}
