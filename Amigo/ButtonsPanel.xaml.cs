using System;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for ButtonsPanel.xaml
    /// </summary>
    public partial class ButtonsPanel : UserControl
    {
        Process proc;        
        public static Window window;
        

        public ButtonsPanel()
        {
            InitializeComponent();
            FirstSeparator.Margin = new Thickness(App.ScreenWidth * 0.24305555 - 295, 0, 0, 0);
            double left = FirstSeparator.Margin.Left;

            SecondSeparator.Margin = new Thickness(App.ScreenWidth - App.ScreenWidth * 0.20138888 - left - 300 - 140, 0, 0, 0);
            this.DataContext = Amigo.App._buttons;           
        }

        private void new_project_Button_Click(object sender, RoutedEventArgs e)
        {            
            Window window = new Window
            {
                Title = "Create a new project",
                Content = new new_project()
            };
            window.Height = 300;
            window.Width = 300;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }
        
        private void Record_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Proceed with recording a new session?", "Confirmation", MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
            {                    
                App.isRecording = true;
                this.record_button.IsEnabled = false;
                Thread.Sleep(500);
                ProcessStartInfo startInfo = new ProcessStartInfo("IExplore.exe");
                startInfo.WindowStyle = ProcessWindowStyle.Maximized;
                DBProcessingLayer.createHTMLfromURLS();
                startInfo.Arguments = Path.GetFullPath(".\\..\\..\\Interfaces\\browse.html");
                proc = Process.Start(startInfo);

                Window window = new Window
                {                   
                    Content = new TransactionName()                    
                };
                window.Height = 270;
                window.Width = 370;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.Topmost = true;
                window.Closing += new CancelEventHandler(window_Closing);
                window.Closed += new EventHandler(window_Closed);                
                window.ShowDialog();
            }
            else
            {
                return;
            }            
        }

        void window_Closed(object sender, EventArgs e)
        {
            try
            {
                if (!proc.HasExited)
                proc.Kill();
            }
            catch (Exception)
            {
               //Console.Write(procex.Message);
            }

            this.record_button.IsEnabled = true;            

            App.isRecording = false;            
            DBProcessingLayer.retrieveRecordingSessions();
            DBProcessingLayer.retrieveRequestReferers();
        }

        void window_Closing(object sender, CancelEventArgs e)
        {
            /*
            if (MessageBox.Show("Recording will be stoped !!! \n Are you sure to close?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                e.Cancel = false;
            }
            else 
            {
                e.Cancel = true;
            }*/
        }

        public static void saveAndShowScripts()
        {
            DBProcessingLayer DBPLayer = new DBProcessingLayer();
            DBPLayer.storeSessionToDB(ProxyProgram.oAllSessions);
            ProxyProgram.oAllSessions.Clear();
        }

        private void open_project_Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window
            {
                Title = "Open an existing project",
                Content = new open_project()
            };

            window.Height = 350;
            window.Width = 500;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }        
        
        private void close_button_Click(object sender, RoutedEventArgs e)
        {
            Amigo.App._project.toExpanded = false;
            Amigo.App._project.RecordingSessions = null;
            Amigo.App._project.ParamPanelsVisibility = System.Windows.Visibility.Hidden;
            Amigo.App._project.InfoPanelsVisibility = System.Windows.Visibility.Visible;
            Amigo.App._project.ProjectName = "";
            Amigo.App.currentProjectID = -1;
            Amigo.App._buttons.RecordButton = false;
            Amigo.App._buttons.SaveProject = false;
            Amigo.App._buttons.StopButton = false;
            //host_parameterization_button.IsEnabled = false;
            App._project.ReplayButtonStatus = false;
            Amigo.App._project.toEnable = false;
            Amigo.App._project.Scenarios = null;//new NotifiableObservableCollection<Scenario>();
            Amigo.App._project.Scripts = null;// new NotifiableObservableCollection<Script>();
            Amigo.App._project.CurrentScenario = null;
            Amigo.App._project.isEnableInitializeLoadTestButton = false; 
        }
        private void delete_project_button_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Project will be permanently deleted!!!\n Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
            {
                DBProcessingLayer.deleteProject(Amigo.App.currentProjectID);
                close_button_Click(sender, e);
            } 
        }        
    }
}