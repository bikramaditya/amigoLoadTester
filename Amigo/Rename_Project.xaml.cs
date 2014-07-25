using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for Rename_Project.xaml
    /// </summary>
    public partial class Rename_Project : UserControl
    {
        public Rename_Project()
        {
            InitializeComponent();

            this.project_name_text.Text = App._project.ProjectName.Substring(App._project.ProjectName.IndexOf("Project - ") + "Project - ".Length);
        }

        private void Project_save_Button_Click(object sender, RoutedEventArgs e)
        {
            DBProcessingLayer dbproc = new DBProcessingLayer();
            string projectName = "" + this.project_name_text.Text;
            if (!projectName.Equals(""))
            {
                string message = dbproc.validateAndRenameProjectName(projectName);
                this.message_box.Foreground = new SolidColorBrush(Colors.Green);
                this.message_box.Text = message;
                if (message.Equals("success"))
                {
                    this.project_name_text.IsEnabled = false;
                    this.project_save_button.IsEnabled = false;
                    
                    Amigo.App._project.ProjectName = this.project_name_text.Text;
                    Amigo.App._project.toExpanded = true;
                    Amigo.App._project.toEnable = true;
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
