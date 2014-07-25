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
using System.ComponentModel;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for AutoCorrelationWindow.xaml
    /// </summary>
    public partial class AutoCorrelationWindow : UserControl
    {
        Parameter currentParam = new Parameter();
        NotifiableObservableCollection<Boundary> Boundaries = new NotifiableObservableCollection<Boundary>();
        double request_id = -1;
        private BackgroundWorker corrWorker;

        public AutoCorrelationWindow(ref Parameter currentParam, string global_selected_request_id)
        {
            InitializeComponent();
            this.currentParam = currentParam;
            this.request_id = double.Parse(global_selected_request_id);
            populateGrid();
        }
        void populateGrid()
        {
            corrWorker = new BackgroundWorker();
            corrWorker.WorkerReportsProgress = true;
            corrWorker.WorkerSupportsCancellation = true;
            corrWorker.DoWork += scanForCorrelations;
            corrWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(reportWorker_RunWorkerCompleted);
                        
            corrWorker.ProgressChanged += delegate(object s, ProgressChangedEventArgs args)
            {
                int progress = args.ProgressPercentage;
                corrProgressBar.Value = progress;
            };
            corrWorker.RunWorkerAsync();
        }

        private void scanForCorrelations(object sender, EventArgs e)
        {
            Boundaries.Clear();
            corrWorker.ReportProgress(1);
            string prev_response = ScriptScenarioDAO.getPreviousResponse(this.request_id);

            if (prev_response != null && prev_response.Length > 0)
            {                                            
                string ParamName = this.currentParam.ParamName;
                string param_val = this.currentParam.OriginalParamValue;
                
                if (param_val.Length == 1) return;

                if (prev_response.Contains(param_val))
                {
                    MatchCollection matches = Regex.Matches(prev_response, @"\b" + param_val + @"\b");

                    for (int i = 0; i < matches.Count; i++)
                    {
                        double progress = (double)((i + 1) * 100 / matches.Count);
                        corrWorker.ReportProgress((int)progress);
                        Thread.Sleep(50);
                        Match _match = matches[i];
                        int index = _match.Index;
                        
                        if (index < 15) continue;

                        Boundary _Boundary = new Boundary();                        
                        _Boundary.LB = prev_response.Substring(index - 15, 15);
                        _Boundary.RB = prev_response.Substring(index + param_val.Length, 15);

                        if (_Boundary.LB == "" && _Boundary.RB == "") continue;

                        if (_Boundary.LB == currentParam.LB && _Boundary.RB == currentParam.RB)
                        {
                            _Boundary.ISChecked = true;
                        }
                        else
                        {
                            _Boundary.ISChecked = false;
                        }
                        Boundaries.Add(_Boundary);
                    }
                    corrWorker.ReportProgress(100);
                }
            }
        }
        
        void reportWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            paramName.Content = "Param Name:" + this.currentParam.ParamName;
            paramValue.Content = "Recorded Param Value:" + this.currentParam.OriginalParamValue;
            autoCorrelationGrid.ItemsSource = Boundaries;
        }

        private void select_button_Click(object sender, RoutedEventArgs e)
        {
            Boundary _Boundary = (Boundary)autoCorrelationGrid.SelectedItem;            

            if (_Boundary == null)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please select a Boundary pair. Check the radio button.","Error",MessageBoxButton.OK,MessageBoxImage.Hand);
                return;
            }

            currentParam.LB = _Boundary.LB;
            currentParam.RB = _Boundary.RB;
            DBProcessingLayer.saveAll_Parameterized_Values();

            ((Window)this.Parent).Close();
        }
    }
}
