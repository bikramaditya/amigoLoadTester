using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Amigo.ViewModels;

namespace Amigo.ViewModels
{
    public class TestExecutionDetails : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        NotifiableObservableCollection<KeyValuePair<int, string>> _test_executions = new NotifiableObservableCollection<KeyValuePair<int, string>>();
        NotifiableObservableCollection<FileFolder> report_files = new NotifiableObservableCollection<FileFolder>();
        private int _ProgressPercentage = 0;

        public NotifiableObservableCollection<KeyValuePair<int, string>> TestExecutions
        {
            get { return _test_executions; }
            set
            {
                _test_executions = value;
                OnPropertyChanged("TestExecutions");
            }
        }
        public NotifiableObservableCollection<FileFolder> ReportFiles
        {
            get { return report_files; }
            set
            {
                report_files = value;
                OnPropertyChanged("ReportFiles");
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
        protected void OnPropertyChanged(string propertyName)
        {
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, args);
            }
        }
    }
}
