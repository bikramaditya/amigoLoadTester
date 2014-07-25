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
using Amigo.ViewModels;
using System.Threading;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for New_Scenario.xaml
    /// </summary>
    public partial class New_Scenario : UserControl
    {
        public New_Scenario()
        {
            InitializeComponent();
            scenario_name_text.Text = DateTime.Now.Date.ToShortDateString() + "-Scenario_100_Users_60_Min_";
            scenario_name_text.Focus();
        }
        private void scenario_save_button_Click(object sender, RoutedEventArgs e)
        {
            Scenario scenario = DBProcessingLayer.createScenario(scenario_name_text.Text,App.currentProjectID);
            if (scenario != null)
            {
                Amigo.App._project.Scenarios = DBProcessingLayer.getAllScenarios_for_Project_ID(Amigo.App.currentProjectID);
                Amigo.App._project.Scripts = new NotifiableObservableCollection<Script>();
                Window this_window = (Window)this.Parent;
                this_window.Close();
            }
        }
    }
}
