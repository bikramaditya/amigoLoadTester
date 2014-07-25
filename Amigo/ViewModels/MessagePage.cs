using System;
using System.ComponentModel;

namespace Amigo.ViewModels
{
    public class MessagePage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    
        public String _message="Amigo message";

        public String Message
        {
            get { return _message; }
            set
            {                
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {         
            PropertyChangedEventArgs args = new PropertyChangedEventArgs(propertyName);
            this.PropertyChanged(this, args);         
        }
    }
}
