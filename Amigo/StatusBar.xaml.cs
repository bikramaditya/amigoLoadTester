using System.Windows;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for StatusBar.xaml
    /// </summary>
    public partial class StatusBar : Window
    {
        public static Status _status = new Status();
        public static int totalCount = 0;
        public static int percentage = 0;
        public StatusBar()
        {
            InitializeComponent();
            this.DataContext = StatusBar._status;
            myProgressBar.ValueChanged += new RoutedPropertyChangedEventHandler<double>(myProgressBar_ValueChanged);
        }

        void myProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (StatusBar._status.StatusNow >= 100) 
            {                
                this.Close();                
            }
        }        
    }
}