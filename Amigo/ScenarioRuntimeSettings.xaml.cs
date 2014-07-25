using System;
using System.Windows;
using System.Windows.Controls;
using Amigo.ViewModels;
using System.Diagnostics;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for ScenarioRuntimeSettings.xaml
    /// </summary>
    public partial class ScenarioRuntimeSettings : UserControl
    {
        Scenario _Scenario = null;
        public ScenarioRuntimeSettings(Scenario _Scenario)
        {
            InitializeComponent();
            this._Scenario = _Scenario;
            setValuesToScreenFrom_Object(_Scenario);
        }
        private void setValuesToScreenFrom_Object(Scenario _Scenario)
        {
            if (_Scenario.isTargetBasedOrLoadBased == "goal")
            {
                goalBased.IsChecked = true;
                tps_value.Text = _Scenario.TPS.ToString();
                noof_user_value.IsEnabled = false;
            }
            else if (_Scenario.isTargetBasedOrLoadBased == "load" || _Scenario.isTargetBasedOrLoadBased == null)
            {
                loadBased.IsChecked = true;
                noof_user_value.Text = _Scenario.TotalNumberOfUsers.ToString();
                tps_value.IsEnabled = false;
            }

            int hr = _Scenario.ExecutionTime / 60;
            int min = _Scenario.ExecutionTime % 60;

            duration_hr_value.Text = hr.ToString();
            duration_min_value.Text = min.ToString();

            hr = _Scenario.rampUpTime / 60;
            min = _Scenario.rampUpTime % 60;

            ramp_up_duration_hr_value.Text = hr.ToString();
            ramp_up_duration_min_value.Text = min.ToString();

            hr = _Scenario.rampDownTime / 60;
            min = _Scenario.rampDownTime % 60;

            ramp_Down_duration_hr_value.Text = hr.ToString();
            ramp_Down_duration_min_value.Text = min.ToString();

            ramp_step_user_value.Text = _Scenario.rampUserStep.ToString();

            min = _Scenario.rampTimeStep / 60;
            int sec = _Scenario.rampTimeStep % 60;

            ramp_step_duration_min_value.Text = min.ToString();
            ramp_Down_duration_sec_value.Text = sec.ToString();

            if (_Scenario.isNumberOfUsersinPercent == true) { percentage.IsChecked = true; }
            else {absolute.IsChecked = true;}

            if (_Scenario.logLevel == 0) { logLevel0.IsChecked = true; }
            if (_Scenario.logLevel == 1) { logLevel1.IsChecked = true; }
            if (_Scenario.logLevel == 2) { logLevel2.IsChecked = true; }
            if (_Scenario.logLevel == 3) { logLevel3.IsChecked = true; }

            host_name_text.Text = _Scenario.TargetHost;
            port_number_text.Text = _Scenario.TargetPort.ToString();

            if (_Scenario.errorLevel == 0) { errorLevel0.IsChecked = true; }
            if (_Scenario.errorLevel == 1) { errorLevel1.IsChecked = true; }
            if (_Scenario.errorLevel == 2) { errorLevel2.IsChecked = true; }

            band_width_combo.SelectedIndex = _Scenario.wanEmulation;

            if (_Scenario.simulateBrowserCache == true) { cacheEnabled.IsChecked = true; }
            else { cacheDisabled.IsChecked = true; }

            proxy_host_name_text.Text = _Scenario.proxyHost;
            proxy_port_number_text.Text = _Scenario.proxyPort.ToString();            
        }
        private void Review_save_TabItem_GotFocus(object sender, RoutedEventArgs e)
        {
            if (goalBased.IsChecked==true)
            {
                _Scenario.isTargetBasedOrLoadBased="goal";
                _Scenario.TPS = Int32.Parse(tps_value.Text);
                ((Label)runtime_summary_grid.Children[1]).Content = "Achieve load of " + tps_value.Text + " TPS.";
            }
            else if (loadBased.IsChecked == true)
            { 
                _Scenario.isTargetBasedOrLoadBased = "load";
                _Scenario.TotalNumberOfUsers = Int32.Parse(noof_user_value.Text);
                ((Label)runtime_summary_grid.Children[1]).Content = "Achieve Concurrent User load of " + noof_user_value.Text+ " VUsers";
            }

            _Scenario.ExecutionTime = Int32.Parse(duration_hr_value.Text) * 60 + Int32.Parse(duration_min_value.Text);
            ((Label)runtime_summary_grid.Children[3]).Content = duration_hr_value.Text + " hour(s) and " + duration_min_value.Text + " minute(s).";

            _Scenario.rampUpTime = Int32.Parse(ramp_up_duration_hr_value.Text) * 60 + Int32.Parse(ramp_up_duration_min_value.Text);
            ((Label)runtime_summary_grid.Children[5]).Content = ramp_up_duration_hr_value.Text + " hour(s) and " + ramp_up_duration_min_value.Text + " minute(s).";

            _Scenario.rampDownTime = Int32.Parse(ramp_Down_duration_hr_value.Text) * 60 + Int32.Parse(ramp_Down_duration_min_value.Text);
            ((Label)runtime_summary_grid.Children[7]).Content = ramp_Down_duration_hr_value.Text + " hour(s) and " + ramp_Down_duration_min_value.Text + " minute(s).";
            
            _Scenario.rampUserStep = Int32.Parse(ramp_step_user_value.Text);            
            _Scenario.rampTimeStep = Int32.Parse(ramp_step_duration_min_value.Text) * 60 + Int32.Parse(ramp_Down_duration_sec_value.Text);
            ((Label)runtime_summary_grid.Children[9]).Content = ramp_step_user_value.Text + " VUsers in every " + ramp_step_duration_min_value.Text + " and " + ramp_Down_duration_sec_value.Text + " sec ";

            if (percentage.IsChecked == true) 
            { 
                _Scenario.isNumberOfUsersinPercent = true;
                ((Label)runtime_summary_grid.Children[11]).Content = "Distribute in % of total Users";
            }
            else if (absolute.IsChecked == true) 
            { 
                _Scenario.isNumberOfUsersinPercent = false;
                ((Label)runtime_summary_grid.Children[11]).Content = "Distribute in absolute number of total Users";
            }

            if (logLevel0.IsChecked == true) { _Scenario.logLevel = 0; ((Label)runtime_summary_grid.Children[13]).Content = "Do not log anything"; }
            if (logLevel1.IsChecked == true) { _Scenario.logLevel = 1; ((Label)runtime_summary_grid.Children[13]).Content = "Log only errors/exceptions"; }
            if (logLevel2.IsChecked == true) { _Scenario.logLevel = 2; ((Label)runtime_summary_grid.Children[13]).Content = "Log errors, parameter substitutions and request body"; }
            if (logLevel3.IsChecked == true) { _Scenario.logLevel = 3; ((Label)runtime_summary_grid.Children[13]).Content = "Log errors, parameters, request body and response body(Not Recomomended)"; }                                    
            
            _Scenario.wanEmulation = band_width_combo.SelectedIndex;
            ((Label)runtime_summary_grid.Children[15]).Content = ((ComboBoxItem)band_width_combo.SelectedItem).Content;

            if (cacheEnabled.IsChecked == true) 
            {
                _Scenario.simulateBrowserCache = true;
                ((Label)runtime_summary_grid.Children[17]).Content = "Yes";
            }
            else 
            { 
                _Scenario.simulateBrowserCache = false;
                ((Label)runtime_summary_grid.Children[17]).Content = "No";
            }

            _Scenario.TargetHost = host_name_text.Text;
            _Scenario.TargetPort = Int32.Parse(port_number_text.Text);
            ((Label)runtime_summary_grid.Children[19]).Content = "Host: "+host_name_text.Text + " and port: " + port_number_text.Text;
            if (host_name_text.Text == "" || port_number_text.Text == "0")
            {
                ((Label)runtime_summary_grid.Children[19]).Content = "No valid server defined. Test can not be run";
            }

            if (errorLevel0.IsChecked == true) { _Scenario.errorLevel = 0; ((Label)runtime_summary_grid.Children[21]).Content = "Ignore all errors and mark transactions as passed"; }
            if (errorLevel1.IsChecked == true) { _Scenario.errorLevel = 1; ((Label)runtime_summary_grid.Children[21]).Content = "Log errors, fail the transaction and continue test"; }
            if (errorLevel2.IsChecked == true) { _Scenario.errorLevel = 2; ((Label)runtime_summary_grid.Children[21]).Content = "Log errors and fail the test (Zero Tolerence)"; }

            _Scenario.proxyHost = proxy_host_name_text.Text;
            _Scenario.proxyPort = Int32.Parse(proxy_port_number_text.Text);
            ((Label)runtime_summary_grid.Children[23]).Content = "Host :" + proxy_host_name_text.Text + " and port :" + proxy_port_number_text.Text;
            if (proxy_host_name_text.Text == "" || proxy_port_number_text.Text == "0")
            {
                ((Label)runtime_summary_grid.Children[23]).Content = "No Proxy";
            }
        }
        private void goalBased_Checked(object sender, RoutedEventArgs e)
        {
            goalBased.IsChecked = false;
            loadBased.IsChecked = true;
            Xceed.Wpf.Toolkit.MessageBox.Show("This functionality is currently not available. Is planned in subsequent versions.", "Message !!!", MessageBoxButton.OK, MessageBoxImage.Error);            
        }
        private void loadBased_Checked(object sender, RoutedEventArgs e)
        {
            tps_value.IsEnabled = false;
            noof_user_value.IsEnabled = true;
        }
        private void save_scenario_runtime_Click(object sender, RoutedEventArgs e)
        {
            DBProcessingLayer.saveScenario(_Scenario);
            Window win = (Window)this.Parent;
            App._project.CurrentScenario = _Scenario;
            App._project.isEnableInitializeLoadTestButton = true;
            win.Close();
        }
        private void text_field_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox _TextBox = (TextBox)e.Source;
            if (_TextBox.Text == null || _TextBox.Text == "")
            {
                _TextBox.Text = "" + 0;
            }
        }

        private void execution_time_TabItem_LostFocus(object sender, RoutedEventArgs e)
        {            
            int execution_time = Int32.Parse(duration_hr_value.Text) * 60 + Int32.Parse(duration_min_value.Text);
            int ramp_up_time = Int32.Parse(ramp_up_duration_hr_value.Text) * 60 + Int32.Parse(ramp_up_duration_min_value.Text);
            int ramp_down_time = Int32.Parse(ramp_Down_duration_hr_value.Text) * 60 + Int32.Parse(ramp_Down_duration_min_value.Text);
            
            if ((ramp_up_time + ramp_down_time) > execution_time)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Total execution time can't be less than Ramp Up + Ramp Down Time", "ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            /*
            int userStep = Int32.Parse(ramp_step_user_value.Text);
            int rampStep = Int32.Parse(ramp_step_duration_min_value.Text) * 60 + Int32.Parse(ramp_Down_duration_sec_value.Text);
            int maxUsers = Int16.Parse(noof_user_value.Text);
            int ramp_up_time_in_sec = ramp_up_time * 60;
            int achievableUser = userStep * (ramp_up_time_in_sec / rampStep);
            if (achievableUser > maxUsers)
            {
                if ((achievableUser - maxUsers) / maxUsers > 0.1)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Ramp Load shooting beyond MAX " + achievableUser + " users in " + ramp_up_time_in_sec + " sec.", "ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            if (achievableUser < maxUsers)
            {
                if ((maxUsers - achievableUser) / achievableUser > 0.1)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("Ramp load unable to reach " + maxUsers + " users in " + ramp_up_time_in_sec + " sec.", "ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
             * */
        }
        private void validate_rampup_Click(object sender, RoutedEventArgs e)
        {
            int user_step = int.Parse(ramp_step_user_value.Text);
            int time_step = (int.Parse(ramp_step_duration_min_value.Text)) * 60 + int.Parse(ramp_Down_duration_sec_value.Text);
            int maxUsers = int.Parse(noof_user_value.Text);
            int ramp_up_time = ((maxUsers/user_step)*time_step)/60;
            ramp_up_duration_hr_value.Text = (ramp_up_time / 60).ToString();
            ramp_up_duration_min_value.Text = (ramp_up_time % 60).ToString();
            ramp_Down_duration_hr_value.Text = (ramp_up_time / 60).ToString();
            ramp_Down_duration_min_value.Text = (ramp_up_time % 60).ToString();
        }

        private void percentage_Checked(object sender, RoutedEventArgs e)
        {
            absolute.IsChecked = true;
            percentage.IsChecked = false;
            Xceed.Wpf.Toolkit.MessageBox.Show("Please calculate the % and assign them in scripts table.\nAuto distribution causes confusion", "Message !!!", MessageBoxButton.OK, MessageBoxImage.Error);            
        }
    }
}
