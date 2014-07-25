using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Amigo
{
    public class Status : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        int _statusNow = 0;
        
        bool _start_button_status = true;
        bool _script_name_text_status = true;
        bool _end_button_status = false;
        bool _test_now_button_status = true;
        bool _stop_now_button_status = false;
        private bool _iteration_text_box;

        public bool test_now_button_status
        {
            get { return _test_now_button_status; }
            set
            {
                _test_now_button_status = value;
                OnPropertyChanged("test_now_button_status");
            }
        }

        public bool stop_now_button_status
        {
            get { return _stop_now_button_status; }
            set
            {
                _stop_now_button_status = value;
                OnPropertyChanged("stop_now_button_status");
            }
        }

        public bool start_button_status
        {
            get { return _start_button_status; }
            set
            {
                _start_button_status = value;
                OnPropertyChanged("start_button_status");
            }
        }

        public bool end_button_status
        {
            get { return _end_button_status; }
            set
            {
                _end_button_status = value;
                OnPropertyChanged("end_button_status");
            }
        }

        public bool script_name_text_status
        {
            get { return _script_name_text_status; }
            set
            {
                _script_name_text_status = value;
                OnPropertyChanged("script_name_text_status");
            }
        }

        public bool iteration_text_box_is_read_only
        {
            get { return _iteration_text_box; }
            set
            {
                _iteration_text_box = value;
                OnPropertyChanged("iteration_text_box");
            }
        }

        public int StatusNow
        {
            get { return _statusNow; }
            set
            {
                _statusNow = value;
                OnPropertyChanged("StatusNow");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
                this.PropertyChanged(this, args);                
            }
        }        
    }
}
