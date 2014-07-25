using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Controls.DataVisualization.Charting;

namespace Amigo.ViewModels
{
    public class LoadTestStatus : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _NumberOfUsers = "No Active VUsers(Not Started)";
        private NotifiableObservableCollection<TransactionMetrics> _TransactionMetrices = new NotifiableObservableCollection<TransactionMetrics>();
        private bool _StartLoadTestButtonStatus = true;
        private bool _StopLoadTestButtonStatus = false;
        private int _ProgressPercentage = 0;
        private string _ProgressMessage = "Current Test Status: Not Started";
        public static List<KeyValuePair<int, int>> TPSSeriesValueList = new List<KeyValuePair<int, int>>();
        public static List<KeyValuePair<double, double>> throughPutSeriesValueList = new List<KeyValuePair<double, double>>();
        public static List<KeyValuePair<int, int>> errorSeriesValueList = new List<KeyValuePair<int, int>>();
        
        private string _ActiveVUsers = "Not Started";
        private string _ElaspsedTime = "Not Started";
        private string _TimeRemaining = "Not Started";
        private string _AvgRespTime = "Not Started";
        private string _LableTPS = "Not Started";
        private string _LblThroughput = "Not Started";
        private string _TotalError = "Not Started";
        private string _TotalPass = "Not Started";
        private string _PassPercentage = "Not Started";
        private string _TotalTxns = "Not Started";

        public string ActiveVUsers
        {
            get { return _ActiveVUsers; }
            set
            {
                _ActiveVUsers = value;
                OnPropertyChanged("ActiveVUsers");
            }
        }
        public string ElaspsedTime
        {
            get { return _ElaspsedTime; }
            set
            {
                _ElaspsedTime = value;
                OnPropertyChanged("ElaspsedTime");
            }
        }
        public string TimeRemaining
        {
            get { return _TimeRemaining; }
            set
            {
                _TimeRemaining = value;
                OnPropertyChanged("TimeRemaining");
            }
        }
        public string AvgRespTime
        {
            get { return _AvgRespTime; }
            set
            {
                _AvgRespTime = value;
                OnPropertyChanged("AvgRespTime");
            }
        }
        public string LableTPS
        {
            get { return _LableTPS; }
            set
            {
                _LableTPS = value;
                OnPropertyChanged("LableTPS");
            }
        }
        public string LblThroughput
        {
            get { return _LblThroughput; }
            set
            {
                _LblThroughput = value;
                OnPropertyChanged("LblThroughput");
            }
        }
        public string TotalError
        {
            get { return _TotalError; }
            set
            {
                _TotalError = value;
                OnPropertyChanged("TotalError");
            }
        }
        public string TotalPass
        {
            get { return _TotalPass; }
            set
            {
                _TotalPass = value;
                OnPropertyChanged("TotalPass");
            }
        }
        public string PassPercentage
        {
            get { return _PassPercentage; }
            set
            {
                _PassPercentage = value;
                OnPropertyChanged("PassPercentage");
            }
        }
        public string TotalTxns
        {
            get { return _TotalTxns; }
            set
            {
                _TotalTxns = value;
                OnPropertyChanged("TotalTxns");
            }
        }
        public NotifiableObservableCollection<TransactionMetrics> TransactionMetrices
        {
            get { return _TransactionMetrices; }
            set
            {
                _TransactionMetrices = value;
                OnPropertyChanged("TransactionMetrices");
            }
        }
        public bool StartLoadTestButtonStatus
        {
            get { return _StartLoadTestButtonStatus; }
            set
            {
                _StartLoadTestButtonStatus = value;
                OnPropertyChanged("StartLoadTestButtonStatus");
            }
        }
        public bool StopLoadTestButtonStatus
        {
            get { return _StopLoadTestButtonStatus; }
            set
            {
                _StopLoadTestButtonStatus = value;
                OnPropertyChanged("StopLoadTestButtonStatus");
            }
        }
        public string NumberOfUsers
        {
            get { return _NumberOfUsers; }
            set
            {
                _NumberOfUsers = value;
                OnPropertyChanged("NumberOfUsers");
            }
        }
        public int ProgressPercentage
        {
            get { return _ProgressPercentage; }
            set
            {
                _ProgressPercentage = value;
                OnPropertyChanged("ProgressPercentage");
            }
        }
        public string ProgressMessage
        {
            get { return _ProgressMessage; }
            set
            {
                _ProgressMessage = value;
                OnPropertyChanged("ProgressMessage");
            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            try
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, args);
            }
            catch { }
        }
    }
}