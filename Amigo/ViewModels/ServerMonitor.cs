using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Amigo.ViewModels
{
    public class ServerMonitor : INotifyPropertyChanged
    {
        private int monitor_id = 0;
        private int scenario_id = 0;
        private string monitor_name = "";
        private string host = "";
        private int port = 0;
        private int pollingFrequency = 0;

        public int MonitorId
        {
            get { return monitor_id; }
            set { monitor_id = value; NotifyPropertyChanged("MonitorId"); }
        }
        public int ScenarioId
        {
            get { return scenario_id; }
            set { scenario_id = value; NotifyPropertyChanged("ScenarioId"); }
        }
        public string MonitorName
        {
            get { return monitor_name; }
            set { monitor_name = value; NotifyPropertyChanged("MonitorName"); }
        }
        public string Host
        {
            get { return host; }
            set { host = value; NotifyPropertyChanged("Host"); }
        }
        public int Port
        {
            get { return port; }
            set { port = value; NotifyPropertyChanged("Port"); }
        }
        public int PollingFrequency
        {
            get { return pollingFrequency; }
            set { pollingFrequency = value; NotifyPropertyChanged("PollingFrequency"); }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Private Helpers

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    
    }
}
