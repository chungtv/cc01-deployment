using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
//using Splash;

namespace Wpf_Linq
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// My Main()
        /// </summary>
        [System.STAThreadAttribute()]
        public static void Main()
        {
            //Splasher.Splash = new Splash.SplashScreen();
            //Splasher.ShowSplash();
            //MessageListener.Instance.ReceiveMessage("Loading ...");

            App app = new App();
            //app.InitializeComponent();
            app.Run();
        }
    }
}
