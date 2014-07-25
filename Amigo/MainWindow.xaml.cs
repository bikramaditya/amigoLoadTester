using System.Windows;
using System;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            App.ScreenWidth = (int)System.Windows.SystemParameters.PrimaryScreenWidth;
            App.ScreenHeight = (int)System.Windows.SystemParameters.PrimaryScreenHeight;
            App.Buttonsize40 = (int)(App.ScreenHeight * 0.044444);
            App.Imgsize32 = (int)(App.ScreenHeight * 0.0355555);
            
            
            InitializeComponent();
            
            this.Closing += (sender, e) =>
            {
                if (ProxyProgram.oAllSessions != null)
                ProxyProgram.DoQuit();
            };
        }
    }
}