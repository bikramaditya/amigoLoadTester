using System.ComponentModel;

namespace Amigo.ViewModels
{
    public class SingleHeader
    {
        private string _key;
        private string _value;

        public string HeaderKey
        {
            get { return _key; }
            set { _key = value; NotifyPropertyChanged("HeaderKey"); }
        }

        public string HeaderValue
        {
            get { return _value; }
            set { _value = value; NotifyPropertyChanged("HeaderValue"); }
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
