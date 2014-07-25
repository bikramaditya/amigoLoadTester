using System.ComponentModel;
using Amigo.ViewModels;

namespace Amigo
{
    public class Project : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string project_name = "Project - ";
        bool _isExpanded = false;
        bool _toEnable = false;
        string _selected_request = "";
        System.Windows.Visibility param_panels_visibility = System.Windows.Visibility.Collapsed;
        System.Windows.Visibility info_panels_visibility = System.Windows.Visibility.Visible;

        NotifiableObservableCollection<RequestReferer> _request_referers = new NotifiableObservableCollection<RequestReferer>();
        NotifiableObservableCollection<RecordingSession> _recording_sessions = new NotifiableObservableCollection<RecordingSession>();
        public NotifiableObservableCollection<Parameter> _parameters = new NotifiableObservableCollection<Parameter>();
        public NotifiableObservableCollection<SingleHeader> _Request_Headers = new NotifiableObservableCollection<SingleHeader>();
        private NotifiableObservableCollection<Scenario> _scenarios = new NotifiableObservableCollection<Scenario>();
        private NotifiableObservableCollection<Script> _scripts = new NotifiableObservableCollection<Script>();

        private string _SelectedRequestPart2;
        private string _SelectedRequestPart1;
        private Scenario _CurrentScenario = null;
        private bool _isEnableInitializeLoadTestButton = false;
        private bool _ReplayButtonStatus = false;

        public Project(string p, bool isEx)
        {
            this.project_name = "Project - "+p;
            this._isExpanded = isEx;
        }

        public bool ReplayButtonStatus
        {
            get { return _ReplayButtonStatus; }
            set
            {
                _ReplayButtonStatus = value;
                OnPropertyChanged("ReplayButtonStatus");
            }
        }

        public System.Windows.Visibility ParamPanelsVisibility
        {
            get { return param_panels_visibility; }
            set
            {
                param_panels_visibility = value;
                OnPropertyChanged("ParamPanelsVisibility");
            }
        }

        public System.Windows.Visibility InfoPanelsVisibility
        {
            get { return info_panels_visibility; }
            set
            {
                info_panels_visibility = value;
                OnPropertyChanged("InfoPanelsVisibility");
            }
        }

        public Scenario CurrentScenario
        {
            get { return _CurrentScenario; }
            set
            {
                _CurrentScenario = value;
                OnPropertyChanged("CurrentScenario");
            }
        }

        public NotifiableObservableCollection<Scenario> Scenarios
        {
            get { return _scenarios; }
            set
            {
                _scenarios = value;
                OnPropertyChanged("Scenarios");
            }
        }

        public NotifiableObservableCollection<Script> Scripts
        {
            get { return _scripts; }
            set
            {
                _scripts = value;
                OnPropertyChanged("Scripts");
            }
        }

        public NotifiableObservableCollection<SingleHeader> RequestHeaders
        {
            get { return _Request_Headers; }
            set
            {
                _Request_Headers = value;
                OnPropertyChanged("RequestHeaders");
            }
        }

        public string SelectedRequestPart1
        {
            get { return _SelectedRequestPart1; }
            set
            {
                _SelectedRequestPart1 = value;
                SelectedRequest = "Selected Request: "+SelectedRequestPart1 +SelectedRequestPart2;
            }
        }
        

        public string SelectedRequestPart2
        {
            get { return _SelectedRequestPart2; }
            set
            {
                _SelectedRequestPart2 = value;
                SelectedRequest = "Selected Request: " + SelectedRequestPart1 + SelectedRequestPart2;
            }
        }

        public string SelectedRequest
        {
            get { return _selected_request; }
            set
            {
                _selected_request = value;
                OnPropertyChanged("SelectedRequest");
            }
        }

        public NotifiableObservableCollection<Parameter> Parameters
        {
            get { return _parameters; }
            set
            {
                _parameters = value;
                OnPropertyChanged("Parameters");
            }
        }

        public NotifiableObservableCollection<RequestReferer> RequestReferers
        {
            get { return _request_referers; }
            set
            {
                _request_referers = value ;
                OnPropertyChanged("RequestReferers");
            }
        }


        public NotifiableObservableCollection<RecordingSession> RecordingSessions
        {
            get { return _recording_sessions; }
            set
            {
                _recording_sessions = value;
                OnPropertyChanged("RecordingSessions");
            }
        }

        public string ProjectName
        {
            get { return project_name; }
            set
            {
                project_name = "Project - " + value;
                Amigo.App._buttons.RecordButton = true;
                OnPropertyChanged("ProjectName");
            }
        }

        public bool toExpanded
        {
            get { return _isExpanded; }
            set
            {
                _isExpanded = value;
                OnPropertyChanged("toExpanded");
            }
        }


        public bool toEnable
        {
            get { return _toEnable; }
            set
            {
                _toEnable = value;
                OnPropertyChanged("toEnable");
            }
        }

        public bool isEnableInitializeLoadTestButton
        {
            get { return _isEnableInitializeLoadTestButton; }
            set
            {
                _isEnableInitializeLoadTestButton = value;
                OnPropertyChanged("isEnableInitializeLoadTestButton");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {            
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);                
                this.PropertyChanged(this, args);
            }
        }

        public bool loadTestExecutingCurrently { get; set; }
    }
}
