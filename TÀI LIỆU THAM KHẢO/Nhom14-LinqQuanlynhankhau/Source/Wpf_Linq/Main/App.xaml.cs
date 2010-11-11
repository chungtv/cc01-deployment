using System.Windows;
using Splash;
using Wpf_Linq;
using System.Configuration;

namespace Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [System.STAThreadAttribute()]
        public static void Main()
        {
            Splasher.Splash = new Splash.SplashScreen();
            Splasher.ShowSplash();
            MessageListener.Instance.ReceiveMessage("Loading ...");

            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "main.config";
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap,ConfigurationUserLevel.None);
            string mode = config.AppSettings.Settings["mode"].Value;

            if (mode == "Tab")
            {
                System.Diagnostics.Process.Start("Wpf_Linq.exe");
            }
            else if (mode == "Mdi")
            {
                System.Diagnostics.Process.Start("Mdi_Wpf_Linq.exe");
            }
            MessageListener.Instance.ReceiveMessage("Loading .....");

            return;
        }
    }
}
