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
using System.Collections;
using System.Diagnostics;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for ScenarioStudio.xaml
    /// </summary>
    public partial class ScenarioStudio : UserControl
    {
        public ScenarioStudio()
        {
            InitializeComponent();
            LeftBorder.Width = App.ScreenWidth * 0.24305555;
            RightBorder.Width = App.ScreenWidth * 0.20138888;
            this.DataContext = Amigo.App._project;
                        
            ContextMenu _ContextMenu = new ContextMenu();
            MenuItem mt1 = new MenuItem();
            mt1.Header = "Delete this row";
            mt1.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(@"/Amigo;component/icons/delete.png", UriKind.Relative)),
                Height = 20,
                Width = 20
            };
            mt1.Click += new RoutedEventHandler(delete_row_Click);
            _ContextMenu.Items.Add(mt1);
            scenario_data_grid.ContextMenu = _ContextMenu;

            ContextMenu _ContextMenu1 = new ContextMenu();
            MenuItem _MenuItem = new MenuItem();
            _MenuItem.Header = "Delete this Scenario";
            _MenuItem.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(@"/Amigo;component/icons/delete.png", UriKind.Relative)),
                Height = 20,
                Width = 20
            };
            _MenuItem.Click += new RoutedEventHandler(delete_scenario_Click);
            _ContextMenu1.Items.Add(_MenuItem);
            scenario_root_tree.ContextMenu = _ContextMenu1;
            scenario_root_tree.ContextMenuOpening += new ContextMenuEventHandler(scenario_root_tree_ContextMenuOpening);

            scenario_data_grid.Columns[1].Header = scenario_data_grid.Columns[1].Header + "\n(Execution Sequence)";
            scenario_data_grid.Columns[2].Header = scenario_data_grid.Columns[2].Header + "\n(Ref. Runtime Setting)";
            scenario_data_grid.Columns[3].Header = scenario_data_grid.Columns[3].Header + "\n(For iteration based test)";
            scenario_data_grid.Columns[3].Visibility = System.Windows.Visibility.Collapsed;
            scenario_data_grid.Columns[4].Header = scenario_data_grid.Columns[4].Header + "\n(Unit - Sec)";
            scenario_data_grid.Columns[5].Header = scenario_data_grid.Columns[5].Header + "\n(Unit - Sec)";
            scenario_data_grid.Columns[6].Header = scenario_data_grid.Columns[6].Header + "\n(Unit - Sec)";
            scenario_data_grid.Columns[7].Header = scenario_data_grid.Columns[7].Header + "\n(Unit - Sec)";
        }
        
        void scenario_root_tree_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (!Amigo.App._project.toEnable)
            {
                e.Handled = true;
                return;
            }
                //_toEnable
        }
        private void delete_row_Click(object sender, RoutedEventArgs e)
        {            
            Script _Script = (Script)scenario_data_grid.SelectedItem;
            
            if (_Script != null)
            {
                DBProcessingLayer.deleteScript_Runtime(_Script.SequenceID);
                App._project.Scripts.Remove(_Script);
            }
        }
        private void delete_scenario_Click(object sender, RoutedEventArgs e)
        {
            Scenario _Scenario = null;
            try
            {
                _Scenario = (Scenario)scenario_root_tree.SelectedItem;
            }
            catch { }

            if (_Scenario != null)
            {
                DBProcessingLayer.delete_Scenario_Runtime(_Scenario.ScenarioID);
                App._project.Scenarios.Remove(_Scenario);
                Amigo.App._project.Scripts = new NotifiableObservableCollection<Script>();
            }
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please select a scenario from the list !!","Error",MessageBoxButton.OK,MessageBoxImage.Stop);
            }
        }
        private void scenario_tree_Expanded(object sender, RoutedEventArgs e)
        {
            if (Amigo.App.currentProjectID != -1)
            {
                Amigo.App._project.Scenarios = DBProcessingLayer.getAllScenarios_for_Project_ID(Amigo.App.currentProjectID);
            }
        }
        
        private void New_Scenario_Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window
            {
                Title = "Create new host and port",
                Content = new New_Scenario(),
            };
            window.Height = 200;
            window.Width = 500;
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            window.ShowDialog();            
        }

        private void button_right_arrow_Click(object sender, RoutedEventArgs e)
        {
            if (parent_check_box.IsChecked == true)
            {
                for (int i = 0; i < recording_sessions.Items.Count; i++)
                {
                    RecordingSession _RecordingSession = (RecordingSession)recording_sessions.Items[i];
                    Scenario scenario = null;
                    try
                    {
                        scenario = (Scenario)scenario_root_tree.SelectedItem;
                    }
                    catch { }
                    if (scenario == null || _RecordingSession == null)
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("Please select the script from LEFT and a scenario from RIGHT");
                        return;
                    }
                    ArrayList ArrScript = DBProcessingLayer.createScriptFromSession(_RecordingSession.SessionID, scenario.ScenarioID);
                    NotifiableObservableCollection<Script> Scripts = new NotifiableObservableCollection<Script>();
                    for (int j = 0; j < ArrScript.Count; j++)
                    {
                        Scripts.Add((Script)ArrScript[j]);
                    }
                    App._project.Scripts = Scripts;
                }
            }
            else 
            {
                RecordingSession _RecordingSession = (RecordingSession)script_root_tree.SelectedItem;
                Scenario scenario = null;
                try
                {
                    scenario = (Scenario)scenario_root_tree.SelectedItem;
                }
                catch { }
                if (scenario == null || _RecordingSession==null)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Please select the script from LEFT and a scenario from RIGHT","Please select !!!",MessageBoxButton.OK,MessageBoxImage.Hand);
                    return;
                }
                ArrayList ArrScript = DBProcessingLayer.createScriptFromSession(_RecordingSession.SessionID, scenario.ScenarioID);
                NotifiableObservableCollection<Script> Scripts = new NotifiableObservableCollection<Script>();

                for (int j = 0; j < ArrScript.Count; j++)
                {
                    Scripts.Add((Script)ArrScript[j]);
                }
                App._project.Scripts = Scripts;
            }
        }
        private void scenario_tree_Selected(object sender, RoutedEventArgs e)
        {
            if (scenario_root_tree.SelectedItem.ToString().Contains("Scenario"))
            {
                Scenario scn = (Scenario)scenario_root_tree.SelectedItem;
                ArrayList arrScript = DBProcessingLayer.retriveScripts_for_Scenario(scn.ScenarioID);
                NotifiableObservableCollection<Script> Scripts = new NotifiableObservableCollection<Script>();
                for (int j = 0; j < arrScript.Count; j++)
                {
                    Scripts.Add((Script)arrScript[j]);
                }
                App._project.Scripts = Scripts;
                ((Button)scenario_stack_panel.Children[1]).IsEnabled = true;
                ((Button)scenario_stack_panel.Children[2]).IsEnabled = true;
                App._project.isEnableInitializeLoadTestButton = false;
                App._project.CurrentScenario = null;
            }
        }

        private void scenario_data_grid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            Script srcipt = (Script)e.Row.Item;
            string fieldName = e.Column.SortMemberPath.ToString();
            if (fieldName == "DelayBetweenIterationMax")
            {
                TextBox EditingElement = (TextBox)e.EditingElement;
                try
                {
                    srcipt.DelayBetweenIterationMax = Int32.Parse(EditingElement.Text);
                }
                catch 
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Only Numeric values accepted", "ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                }
            }
            else if (fieldName == "DelayBetweenIterationMin")
            {
                TextBox EditingElement = (TextBox)e.EditingElement;
                try
                {
                    srcipt.DelayBetweenIterationMin = Int32.Parse(EditingElement.Text);
                }
                catch
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Only Numeric values accepted", "ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                }
            }
            else if (fieldName == "NumberOfIterations")
            {
                TextBox EditingElement = (TextBox)e.EditingElement;
                try
                {
                    srcipt.NumberOfIterations = Int32.Parse(EditingElement.Text);
                }
                catch
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Only Numeric values accepted", "ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                }
            }
            else if (fieldName == "NumberOfUsers")
            {
                TextBox EditingElement = (TextBox)e.EditingElement;
                try
                {
                    srcipt.NumberOfUsers = Int32.Parse(EditingElement.Text);
                }
                catch
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Only Numeric values accepted", "ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                }
            }
            else if (fieldName == "StartAfter")
            {
                TextBox EditingElement = (TextBox)e.EditingElement;
                try
                {
                    srcipt.StartAfter = Int32.Parse(EditingElement.Text);
                }
                catch
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Only Numeric values accepted", "ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                }
            }
            else if (fieldName == "ThinkTimeMin")
            {
                TextBox EditingElement = (TextBox)e.EditingElement;
                try
                {
                    srcipt.ThinkTimeMin = Int32.Parse(EditingElement.Text);
                }
                catch
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Only Numeric values accepted", "ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                }
            }
            else if (fieldName == "ThinkTimeMax")
            {
                TextBox EditingElement = (TextBox)e.EditingElement;
                try
                {
                    srcipt.ThinkTimeMax = Int32.Parse(EditingElement.Text);
                }
                catch
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Only Numeric values accepted", "ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                    e.Cancel = true;
                }
            }
            if (!e.Cancel)
            {
                DBProcessingLayer.updateScript(srcipt);
            }
        }

        private void Runtime_Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            Scenario _Scenario = null;
            try
            {
                _Scenario = (Scenario)scenario_root_tree.SelectedItem;
            }
            catch { }

            if (_Scenario == null)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please select a scenario from RIGHT", "ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            _Scenario.TotalNumberOfUsers = getTotalUsers_from_scripts();
            Window window = new Window
            {
                Title = "Scenario Runtime Settings",
                Content = new ScenarioRuntimeSettings(_Scenario),
            };
            window.Closed += new EventHandler(window_Closed);
            window.Height = 470;
            window.Width = 730;
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            window.ShowDialog();                        
        }

        void window_Closed(object sender, EventArgs e)
        {
            Scenario _Scenario = App._project.CurrentScenario;
            if (_Scenario == null) return;

            int scn_total_users = _Scenario.TotalNumberOfUsers;

            int script_total_users = 0;
            for (int i = 0; i < scenario_data_grid.Items.Count; i++)
            {
                script_total_users+= ((Script)scenario_data_grid.Items[i]).NumberOfUsers;
            }

            int pseudo_total_users = 0;
            
            if (scn_total_users != script_total_users)
            {
                NotifiableObservableCollection<Script> Scripts = App._project.Scripts;
                for (int i = 0; i < Scripts.Count; i++)
                {
                    int sub_users = scn_total_users/scenario_data_grid.Items.Count;
                    Scripts[i].NumberOfUsers = sub_users;
                    pseudo_total_users += sub_users;
                }
                Scripts[0].NumberOfUsers += (scn_total_users - pseudo_total_users);
                App._project.Scripts = new NotifiableObservableCollection<Script>();
                App._project.Scripts = Scripts;
            }
            for (int i = 0; i < App._project.Scripts.Count; i++)
            {
                DBProcessingLayer.updateScript(App._project.Scripts[i]);
            }            
        }

        private int getTotalUsers_from_scripts()
        {
            int total_users = 0;
            for (int i = 0; i < scenario_data_grid.Items.Count; i++)
            {
                Script _script = (Script)scenario_data_grid.Items[i];
                total_users += _script.NumberOfUsers; 
            }
            return total_users;
        }

        private void start_load_test_Button_Click(object sender, RoutedEventArgs e)
        {
            if (scenario_data_grid.Items.Count > 0)
            {}
            else
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("No Scripts added. Please add scripts from left", "Warning !!!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (App._project.loadTestExecutingCurrently)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("A load test is currently under progress!!!\nPlease stop the test before initialing another test", "ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            TabItem _TabItem = (TabItem)this.Parent;
            TabControl _tabControl = (TabControl)_TabItem.Parent;
            DockPanel _DockPanel = (DockPanel)_tabControl.Parent;
            ((TabItem)_tabControl.Items[2]).IsSelected = true;
            ((ExecutionStudio)((TabItem)_tabControl.Items[2]).Content).DataContext = new object();//App._project.CurrentScenario;
        }
    }
}