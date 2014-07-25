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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Amigo.Interfaces;
using System.Text.RegularExpressions;
using Amigo.ViewModels;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for NewMonitor.xaml
    /// </summary>
    public partial class NewMonitor : UserControl
    {
        public NewMonitor()
        {
            InitializeComponent();
        }

        private void monitor_save_button_Click(object sender, RoutedEventArgs e)
        {
            string monitor_name = (monitor_name_text.Text).Trim();

            if (string.IsNullOrEmpty(monitor_name) || !IsAlphaNum(monitor_name) || ExecutionDAO.checkUniqueMonitor(monitor_name, ExecutionStudio._Scenario.ScenarioID) > 0)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please enter a valid and unique Monitor Name!!!\nA Monitor name is Unique for a Scenario\nA Monitor name can not have special characters", "Monitor Name ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string host_name = host_name_text.Text;

            if (!IsAlphaNum(host_name) || host_name == "")
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please enter a valid host Name/IP!!!\nA Host name can not have special characters or spaces", "Host Name ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string sPort = port_text.Text;
            int port = 0;
            try 
            {
                port = int.Parse(sPort);
            }
            catch 
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please enter a valid Integer for Port!!!\nIf you are not sure leave it default as 19797.", "Port ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            
            string polling_interval = polling_text.Text;
            int interval = 5;
            try 
            {
                interval = int.Parse(polling_interval);
            }
            catch 
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please enter a valid Integer for interval!!!\nPlease dont give less than 5 sec.\nBest practice is 30 sec.", "Interval ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (interval <= 5)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please enter an Integer for interval more than 5 sec!!!\nBest practice is 30 sec.", "Interval ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ServerMonitor monitor = new ServerMonitor();
            monitor.Host = host_name;
            monitor.MonitorName = monitor_name;
            monitor.PollingFrequency = interval;
            monitor.Port = port;
            monitor.ScenarioId = ExecutionStudio._Scenario.ScenarioID;

            ExecutionDAO.saveMonitor(monitor);
            ((Window)this.Parent).Close();
        }
        public static bool IsAlphaNum(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            for (int i = 0; i < str.Length; i++)
            {
                if (!(char.IsLetter(str[i])) && (!(char.IsNumber(str[i]))) && !'.'.Equals(str[i]) && !'-'.Equals(str[i]))
                    return false;
            }

            return true;
        }
    }
}
