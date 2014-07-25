using System.ComponentModel;

namespace Amigo.ViewModels
{
    public enum ParamSources
    {
        KeepOriginal,
        AutoCorrelation,
        CSV        
    };

    public enum IterationType
    {        
        None,
        Unique,
        Sequential,
        Random
    };

    public class Parameter : INotifyPropertyChanged
    {        
        private string _paramID;
        private string _paramName;
        private string _originalParamValue;
        public ParamSources _parameterizationSource;
        private string _substututedParamValue;
        public string _selected_csv_columnName;
        public IterationType _selectedIterationType;
        private NotifiableObservableCollection<string> _csv_columns;

        public string LB {get;set;}
        public string RB { get; set; }

        public NotifiableObservableCollection<string> CSVColumns
        {
            get { return _csv_columns; }
            set
            {
                _csv_columns = value;
                NotifyPropertyChanged("CSVColumns");
            }
        }

        public string ParamID
        {
            get { return _paramID; }
            set { _paramID = value; NotifyPropertyChanged("ParamID"); }
        }

        public string ParamName
        {
            get { return _paramName; }
            set { _paramName = value; NotifyPropertyChanged("ParamName"); }
        }

        public string OriginalParamValue
        {
            get { return _originalParamValue; }
            set {_originalParamValue = value;NotifyPropertyChanged("OriginalParamValue");}
        }

        public ParamSources ParameterizationSource
        {
            get { return _parameterizationSource; }
            set { _parameterizationSource = value; NotifyPropertyChanged("ParameterizationSource"); }
        }

        public string SubstututedParamValue
        {
            get { return _substututedParamValue; }
            set {_substututedParamValue = value;NotifyPropertyChanged("SubstututedParamValue");}
        }

        public string SelectedCSVColumnName 
        {
            get { return _selected_csv_columnName; }
            set { _selected_csv_columnName = value; NotifyPropertyChanged("SelectedCSVColumnName"); }
        }

        public IterationType SelectedIterationType  
        {
            get { return _selectedIterationType; }
            set { _selectedIterationType = value; NotifyPropertyChanged("SelectedIterationType"); }
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
