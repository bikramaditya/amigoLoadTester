using System.ComponentModel;

namespace Amigo.ViewModels
{
    public class ButtonsStatus : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        bool _new_project = true;
        bool _open_project = true;
        bool _save_project = false;
        bool _record = false;
        bool _stop = false;
        bool _replay = false;

        public bool NewProject {get { return _new_project; } set{_new_project = value;OnPropertyChanged("NewProject");}}
        public bool OpenProject { get { return _open_project; } set { _open_project = value; OnPropertyChanged("OpenProject"); } }
        public bool SaveProject { get { return _save_project; } set { _save_project = value; OnPropertyChanged("SaveProject"); } }

        public bool RecordButton { get { return _record; } set { _record = value; OnPropertyChanged("RecordButton"); } }
        public bool StopButton { get { return _stop; } set { _stop = value; OnPropertyChanged("StopButton"); } }
        public bool ReplayButton { get { return _replay; } set { _replay = value; OnPropertyChanged("ReplayButton"); } }

        protected void OnPropertyChanged(string propertyName)
        {
            //if (PropertyChanged != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, args);
            }
        }
    }
}
