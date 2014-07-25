using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Collections;
using Amigo.ViewModels;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for open_project.xaml
    /// </summary>
    public partial class open_project : UserControl
    {
        public open_project()
        {
            InitializeComponent();
            ListView1.ItemsSource = ProjectList();
        }

        private ArrayList ProjectList()
        {
            ArrayList list = new ArrayList();
            List<Dictionary<string, string>> projects = DBProcessingLayer.getAllProjects();

            Dictionary<string, string> projectRow ;

            for (int i = 0; i < projects.Count; i++)
            {
                projectRow = projects[i];
                list.Add(new ProjectRow(projectRow["Name"], Int16.Parse(projectRow["ID"]), projectRow["Date"]));                
            }            
            return list;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void open_selected_project_Click(object sender, RoutedEventArgs e)
        {            
            // close project first            
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
            Amigo.App._project.ProjectName = ((ProjectRow)this.ListView1.SelectedItem).Name;
            Amigo.App.currentProjectID = ((ProjectRow)this.ListView1.SelectedItem).ID;
            Amigo.App._project.toExpanded = true;
            Amigo.App._project.toEnable = true;
            Amigo.App._buttons.SaveProject = true;
            retrieveRequestReferers();
            retrieveRecordingSessions();
        }

        private void retrieveRecordingSessions()
        {
            DBProcessingLayer.retrieveRecordingSessions();
        }

        private void retrieveRequestReferers()
        {
            DBProcessingLayer.retrieveRequestReferers();
        }

        private void ListView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.open_selected_project.IsEnabled = true;
        }
    }


    public class ProjectRow
    {
        public ProjectRow(string projectName, Int16 authorAge, string created_date)
        {
            this.Name = projectName;
            this.id = authorAge;
            this.date = created_date;
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private Int16 id;

        public Int16 ID
        {
            get { return id; }
            set { id = value; }
        }
        private string date;

        public string Date
        {
            get { return date; }
            set { date = value; }
        }               
    }
}
