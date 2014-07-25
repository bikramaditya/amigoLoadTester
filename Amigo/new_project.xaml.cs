using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for new_project.xaml
    /// </summary>
    public partial class new_project : UserControl
    {                
        public new_project()
        {
            InitializeComponent();
            this.project_name_text.Text = "Enter a Unique Name";
            this.project_name_text.GotFocus += new RoutedEventHandler(project_name_text_GotFocus); 
        }
        void project_name_text_GotFocus(object sender, RoutedEventArgs e)
        {
            this.project_name_text.Text = "";
        }
        private void Project_save_Button_Click(object sender, RoutedEventArgs e)
        {
            DBProcessingLayer dbproc = new DBProcessingLayer();
            string projectName = ""+this.project_name_text.Text;
            if (!projectName.Equals(""))
            {
                string message = dbproc.validateAndSaveProjectName(projectName);
                this.message_box.Foreground = new SolidColorBrush(Colors.Green);
                this.message_box.Text = message;
                if (message.Equals("success"))
                {
                    this.project_name_text.IsEnabled = false;
                    this.project_save_button.IsEnabled = false;                    
                    Amigo.App._project.ProjectName = this.project_name_text.Text;
                    Amigo.App._project.toExpanded = true;
                    Amigo.App._project.toEnable = true;
                    DBProcessingLayer.retrieveRecordingSessions();
                    DBProcessingLayer.retrieveRequestReferers();
                    Amigo.App._buttons.SaveProject = true;
                    Window this_window = (Window)this.Parent;
                    this_window.Close();
                }
                else
                {
                    this.message_box.Foreground = new SolidColorBrush(Colors.Red);
                    this.message_box.Text = "The project name must be unique";
                }
            }
            else 
            {
                this.message_box.Foreground = new SolidColorBrush(Colors.Red);
                this.message_box.Text = "The project name can not be empty";
            }
        }
    }
}
