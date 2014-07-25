using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for HostPortParameterization.xaml
    /// </summary>
    public partial class HostPortParameterization : UserControl
    {
        private string _request_id = "";

        public HostPortParameterization(string _lable_content_request_id)
        {
            InitializeComponent();
            this._request_id = _lable_content_request_id;
        }

        private void host_param_create_Click(object sender, RoutedEventArgs e)
        {
            string host_name_text = ""+host_name_text_box.Text;
            string port_text = ""+port_text_box.Text;
            if ("".Equals(host_name_text) || "".Equals(port_text))
            {                
                this.error_message.Content = "Values can not be empty";
                this.error_message.Foreground = new SolidColorBrush(Colors.Red);
                this.error_message.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                DBProcessingLayer.updateParameterizedHostPort(host_name_text, port_text, this._request_id);
                this.error_message.Content = "New host and port created successfully";
                this.error_message.Foreground = new SolidColorBrush(Colors.Green);
                this.error_message.Visibility = System.Windows.Visibility.Visible;
                ((Window)(this.Parent)).Close();
                Xceed.Wpf.Toolkit.MessageBox.Show("The project needs to reopen automatically. Please wait!", "Information !!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);                
                int curr_project_id = App.currentProjectID;
                Thread.Sleep(1000);
                closeProject();                

                Thread _thread = new Thread(openProject);
                _thread.Start((object)curr_project_id);
                
                //((Window)(this.Parent)).Close();
            }
        }

        private void openProject(object obj_curr_project_id)
        {
            Thread.Sleep(1000);
            int curr_project_id = int.Parse(obj_curr_project_id.ToString());
            Amigo.App._project.RecordingSessions = null;
            Amigo.App._project.ParamPanelsVisibility = System.Windows.Visibility.Hidden;
            Amigo.App._project.InfoPanelsVisibility = System.Windows.Visibility.Visible;
            Amigo.App._project.ProjectName = "";
            Amigo.App.currentProjectID = -1;
            Amigo.App._buttons.RecordButton = false;
            Amigo.App._buttons.SaveProject = false;
            Amigo.App._buttons.StopButton = false;
            Amigo.App._project.toEnable = false;
            Amigo.App._project.Scenarios = null;//new NotifiableObservableCollection<Scenario>();
            Amigo.App._project.Scripts = null;// new NotifiableObservableCollection<Script>();
            Amigo.App._project.CurrentScenario = null;
            Amigo.App._project.isEnableInitializeLoadTestButton = false;


            //then open project
            Amigo.App._project.ProjectName = ScriptScenarioDAO.getProjectName(curr_project_id);
            Amigo.App.currentProjectID = curr_project_id;
            Amigo.App._project.toExpanded = true;
            Amigo.App._project.toEnable = true;
            Amigo.App._buttons.SaveProject = true;
            DBProcessingLayer.retrieveRequestReferers();
            DBProcessingLayer.retrieveRecordingSessions();
        }

        private void closeProject()
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
    }
}
