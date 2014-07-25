using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Threading;
using System.Windows.Media;
using System.ComponentModel;
using System.Collections;
using System.Text.RegularExpressions;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for TransactionName.xaml
    /// </summary>
    public partial class TransactionName : UserControl
    {
        public static Status _status = new Status();        
        
        public TransactionName()
        {
            InitializeComponent();
            end_button.IsEnabled = false;
            start_button.IsEnabled = true;
            this.DataContext = _status;
        }

        private void start_button_Click(object sender, RoutedEventArgs e)
        {
            myProgressBar.Visibility = System.Windows.Visibility.Hidden;
            string script_name = ""+this.script_name_text.Text;
            bool isSuccess = true;

            if (script_name.Equals(""))
            {
                this.error_message.Background = new SolidColorBrush(Colors.White);
                this.error_message.Foreground = new SolidColorBrush(Colors.Red);
                this.error_message.Text = "Please enter a transaction name";
                isSuccess = false;
            }
            else
            {
                List<Dictionary<string, string>> _sessions = ScriptScenarioDAO.retrieveRecordingSessions();
                String existing_session_name = "";

                for (int i = 0; i < _sessions.Count; i++)
                {
                    Dictionary<string, string> _session = _sessions[i];
                    existing_session_name = _session["Name"];
                    if (script_name.Equals(existing_session_name))
                    {
                        this.error_message.Background = new SolidColorBrush(Colors.White);
                        this.error_message.Foreground = new SolidColorBrush(Colors.Red);
                        this.error_message.Text = "Transaction name tobe unique for a project";
                        isSuccess = false;
                        break;
                    }                    
                }
            }
            if (isSuccess)
            {
                this.error_message.Background = new SolidColorBrush(Colors.White);
                this.error_message.Foreground = new SolidColorBrush(Colors.Green);
                this.error_message.Text = "Recording started !!! Browser activities will be recorded.";
                this.script_name_text.IsEnabled = false;
                Amigo.App.recordingSessionName = script_name;
                App.isRecording = true;                
                ProxyStart();
                TransactionName._status.start_button_status = false;
                TransactionName._status.end_button_status = true;
                TransactionName._status.script_name_text_status = false;
            }
        }

        private void ProxyStart()
        {
            ProxyProgram.ProxyStart();
        }
        
        private void ProxyStop()
        {
            ButtonsPanel.saveAndShowScripts();
            ProxyProgram.DoQuit();
        }

        private void end_button_Click(object sender, RoutedEventArgs e)
        {
            App.isRecording = false;

            myProgressBar.Visibility = System.Windows.Visibility.Visible;
            this.error_message.Text = "Recording paused ... ";
            this.script_name_text.IsEnabled = true;
            
            end_button.IsEnabled = false;            

            Thread _thread = new Thread(ProxyStop);
            
            _thread.Start();
        }

        private void quit_button_Click(object sender, RoutedEventArgs e)
        {
            Window win = (Window)this.TxnNameUC.Parent;
            win.Close();
        }        
    }
}
