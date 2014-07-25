using System.Windows.Controls;
using Amigo.ViewModels;
using System.Windows;
using System.Collections.Generic;
using System;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections;
using System.Windows.Controls.DataVisualization.Charting;
using System.ComponentModel;
using System.Threading;
using Amigo.Interfaces;
using System.IO;
using System.Net;
using System.Reflection;

namespace Amigo
{
    public partial class ExecutionStudio : UserControl
    {
        string[] ArrBandwidth = { "LAN (Use Max available)", "56 Kbps", "128 Kbps", "256 Kbps", "512 Kbps", "1 Mbps", "2 Mbps", "10 Mbps" };
        private static LoadTestStatus _LoadTestStatus = null;
        public static Scenario _Scenario = null;
        private static ArrayList ScriptList = null;//new ArrayList();
        private static List<KeyValuePair<int, ArrayList>> ScriptRequestMap = null; //new List<KeyValuePair<int, ArrayList>>();
        public static List<KeyValuePair<double, string>> requestsList = null; //new List<KeyValuePair<double, string>>();
        private static List<KeyValuePair<double, ArrayList>> requestGroups = null;//new List<KeyValuePair<double, ArrayList>>();
        public static List<KeyValuePair<double, string>> txnNameList = null; // new List<KeyValuePair<double, string>>();
        private static List<KeyValuePair<double, int>> loadDistributionList = null; // new List<KeyValuePair<double, string>>();
        private static List<KeyValuePair<int, int>> requestRepeatList = null; // new List<KeyValuePair<double, string>>();
        public static List<KeyValuePair<double, List<Dictionary<string, string>>>> parametersList = null;// new List<KeyValuePair<double, string>>();
        public static List<KeyValuePair<double, List<Dictionary<string, string>>>> headersList = null;//new List<KeyValuePair<double, string>>();
        private static List<KeyValuePair<double, ArrayList>>.Enumerator enumRequestGroup;
        private Object thisLock; ////lock for thread synchronization
        private Object executionNumberLockObj;
        private static int _nextExecutionNumber = 0;
        private KeyValuePair<double, ArrayList> _key_val_Next_Lead_Req = new KeyValuePair<double, ArrayList>();
        private static bool rampDownNow;
        BackgroundWorker retrivalWorker;
        BackgroundWorker rampUpWorker;
        BackgroundWorker pollerWorker;
        private static int currentLoadedUsers;
        private static bool isMoreLoadingPossible = true;
        object rampDownLock = new object();
        private static List<KeyValuePair<decimal, decimal>> loadPreviewValueList = new List<KeyValuePair<decimal, decimal>>();
        public static int current_execution_id;
        public static ArrayList _ResponseTimePointList = null;
        public static Dictionary<int, string> responses_for_correlation = new Dictionary<int, string>();

        public ExecutionStudio()
        {
            InitializeComponent();            
            avgRespTimeRow.Content += "\n(All Reqs):";
            _ResponseTimePointList = new ArrayList();
            _LoadTestStatus = new LoadTestStatus();
            ActiveVUsers.DataContext = _LoadTestStatus;
            ElaspsedTime.DataContext = _LoadTestStatus;
            TimeRemaining.DataContext = _LoadTestStatus;
            AvgRespTime.DataContext = _LoadTestStatus;
            LableTPS.DataContext = _LoadTestStatus;
            LblThroughput.DataContext = _LoadTestStatus;
            TotalError.DataContext = _LoadTestStatus;
            TotalPass.DataContext = _LoadTestStatus;
            PassPercentage.DataContext = _LoadTestStatus;
            TotalTxns.DataContext = _LoadTestStatus;
            NumberOfUsers.DataContext = _LoadTestStatus;

            this.IsEnabled = false;
            //NoOfUsers.DataContext = _LoadTestStatus;
            testProgressBar.DataContext = _LoadTestStatus;
            testProgressText.DataContext = _LoadTestStatus;
            //modify_runtime_button.DataContext = _LoadTestStatus;
            test_summary_table_grid.DataContext = _LoadTestStatus;
            start_loadtest_now.DataContext = _LoadTestStatus;
            stop_loadtest_now.DataContext = _LoadTestStatus;
            responseTimeChart.DataContext = _LoadTestStatus;
            this.DataContextChanged += new System.Windows.DependencyPropertyChangedEventHandler(ExecutionStudio_DataContextChanged);
            thisLock = new Object(); ////lock for thread synchronization
            executionNumberLockObj = new Object();
            TPSSeries.DataContext = _LoadTestStatus;
            throughPutSeries.DataContext = _LoadTestStatus;
            //errorSeries.DataContext = _LoadTestStatus;
        }

        private void refreshSummaryMetrics()
        {
            ExecutionDAO _DBResponseIO = new ExecutionDAO();
            _LoadTestStatus.TransactionMetrices = _DBResponseIO.getTransactionSummaryMetrices();
        }        
        private void averageRespSeriesColumnRefresh()
        {
            List<KeyValuePair<string, double>> _list_column_name_Val = new List<KeyValuePair<string, double>>();
            for (int i = 0; i < _LoadTestStatus.TransactionMetrices.Count; i++)
            {
                
                KeyValuePair<string, double> _column_name_Val = new KeyValuePair<string, double>(_LoadTestStatus.TransactionMetrices[i].DisplayName, double.Parse(_LoadTestStatus.TransactionMetrices[i].RespTimeAvg));
                _list_column_name_Val.Add(_column_name_Val);                
            }
            averageRespSeries.DataContext = _list_column_name_Val;
        }
        static int Compare2(KeyValuePair<string, double> a, KeyValuePair<string, double> b)
        {
            return a.Value.CompareTo(b.Value);
        }
        void ExecutionStudio_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            execution_center_stack_panel.Visibility = System.Windows.Visibility.Visible;
            if (App._project.loadTestExecutingCurrently)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("A load test is currently under progress!!!\nPlease stop the test before initialing another test", "ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.IsEnabled = true;
            _Scenario = App._project.CurrentScenario;
            if (_Scenario == null)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("To see this in action, Please validate and save the Scenario Configurations,\nThen click \"Initialize Execution Studio\"", "ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);                                
                return;
            }
            if (_Scenario.isTargetBasedOrLoadBased == "goal")
            {
                ((Label)loadtest_runtime_summary_grid.Children[1]).Content = "Achieve load of \n" + _Scenario.TPS + " TPS.";
            }
            else if (_Scenario.isTargetBasedOrLoadBased == "load")
            {
                ((Label)loadtest_runtime_summary_grid.Children[1]).Content = "Achieve Concurrent \nUser load of \n" + _Scenario.TotalNumberOfUsers + " VUsers";
            }

            ((Label)loadtest_runtime_summary_grid.Children[3]).Content = _Scenario.ExecutionTime / 60 + " hour(s) and " + _Scenario.ExecutionTime % 60 + " minute(s).";

            ((Label)loadtest_runtime_summary_grid.Children[5]).Content = _Scenario.rampUpTime / 60 + " hour(s) and " + _Scenario.rampUpTime % 60 + " minute(s).";

            ((Label)loadtest_runtime_summary_grid.Children[7]).Content = _Scenario.rampDownTime / 60 + " hour(s) and " + _Scenario.rampDownTime % 60 + " minute(s).";

            ((Label)loadtest_runtime_summary_grid.Children[9]).Content = _Scenario.rampUserStep + " VUsers in every " + _Scenario.rampTimeStep / 60 + "\nand " + _Scenario.rampTimeStep % 60 + " sec ";

            if (_Scenario.isNumberOfUsersinPercent == true)
            {
                ((Label)loadtest_runtime_summary_grid.Children[11]).Content = "Distribute in % \nof total Users";
            }
            else if (_Scenario.isNumberOfUsersinPercent == false)
            {
                ((Label)loadtest_runtime_summary_grid.Children[11]).Content = "Distribute in absolute\nnumber of total Users";
            }

            if (_Scenario.logLevel == 0) { ((Label)loadtest_runtime_summary_grid.Children[13]).Content = "Do not log anything"; }
            if (_Scenario.logLevel == 1) { ((Label)loadtest_runtime_summary_grid.Children[13]).Content = "Log only errors/exceptions"; }
            if (_Scenario.logLevel == 2) { ((Label)loadtest_runtime_summary_grid.Children[13]).Content = "Log errors, parameter\nsubstitutions and request body"; }
            if (_Scenario.logLevel == 3) { ((Label)loadtest_runtime_summary_grid.Children[13]).Content = "Log errors, parameters,\nrequest body and response\nbody(Not Recomomended)"; }

            ((Label)loadtest_runtime_summary_grid.Children[15]).Content = ArrBandwidth[_Scenario.wanEmulation];

            if (_Scenario.simulateBrowserCache == true)
            {
                ((Label)loadtest_runtime_summary_grid.Children[17]).Content = "Yes";
            }
            else
            {
                ((Label)loadtest_runtime_summary_grid.Children[17]).Content = "No";
            }

            ((Label)loadtest_runtime_summary_grid.Children[19]).Content = "Host: " + _Scenario.TargetHost + " \nand port: " + _Scenario.TargetPort;
            if (_Scenario.TargetHost == "" || _Scenario.TargetPort == 0)
            {
                ((Label)loadtest_runtime_summary_grid.Children[19]).Content = "No valid server defined.\nTest can not be run";
            }

            if (_Scenario.errorLevel == 0) { ((Label)loadtest_runtime_summary_grid.Children[21]).Content = "Ignore all errors and \nmark transactions as passed"; }
            if (_Scenario.errorLevel == 1) { ((Label)loadtest_runtime_summary_grid.Children[21]).Content = "Log errors, fail the \ntransaction and continue test"; }
            if (_Scenario.errorLevel == 2) { ((Label)loadtest_runtime_summary_grid.Children[21]).Content = "Log errors and fail the\ntest (Zero Tolerence)"; }

            ((Label)loadtest_runtime_summary_grid.Children[23]).Content = "Host :" + _Scenario.proxyHost + "\nand port :" + _Scenario.proxyPort;
            if (_Scenario.proxyHost == "" || _Scenario.proxyPort == 0)
            {
                ((Label)loadtest_runtime_summary_grid.Children[23]).Content = "No Proxy";
            }
            _LoadTestStatus.StartLoadTestButtonStatus = true;
            _LoadTestStatus.StopLoadTestButtonStatus = false;
            initializeCharts();
            retrieveServerResourceMonitors(_Scenario);
        }

        private void retrieveServerResourceMonitors(Scenario _Scenario)
        {
            ArrayList monitors = DBProcessingLayer.retrieveServerResourceMonitors(_Scenario.ScenarioID);
            int treeCount = monitors_tree.Items.Count;            

            for (int j = 1; j < treeCount; j++)
            {
                monitors_tree.Items.RemoveAt(1);
            }
            
            server_resource_monitors_stackPanel.Children.Clear();

            for (int i = 0; i < monitors.Count; i++)
            {
                ////create tree view and attach

                ServerMonitor monitor = (ServerMonitor)monitors[i];
                TreeViewItem _TreeViewItem = new TreeViewItem();
                _TreeViewItem.Margin = new Thickness(20,10,10,0);

                StackPanel _StackPanel = new StackPanel();
                _StackPanel.Orientation = Orientation.Horizontal;
                
                Label _Label = new Label();
                _Label.FontSize = 12;
                _Label.Foreground = new SolidColorBrush(Colors.White);
                _Label.Content = monitor.MonitorName;

                Image _image = new Image();
                _image.Height = 25;
                _image.Width = 25;
                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/server.png", UriKind.Relative));

                _StackPanel.Children.Add(_image);
                _StackPanel.Children.Add(_Label);

                _TreeViewItem.Header = _StackPanel;                
                
                //// create context menu and attach

                ContextMenu Menu = new ContextMenu();
                MenuItem mt4 = new MenuItem();
                mt4.Header = "Modify Monitor";
                mt4.Icon = new System.Windows.Controls.Image
                {
                    Source = new BitmapImage(new Uri(@"/Amigo;component/icons/rename.png", UriKind.Relative)),
                    Height = 20,
                    Width = 20
                };
                //mt4.Click += new RoutedEventHandler(delete_request_Click);

                MenuItem mt5 = new MenuItem();
                mt5.Header = "Delete Monitor";
                mt5.Icon = new System.Windows.Controls.Image
                {
                    Source = new BitmapImage(new Uri(@"/Amigo;component/icons/delete.png", UriKind.Relative)),
                    Height = 20,
                    Width = 20
                };

                Menu.Items.Add(mt4);
                Menu.Items.Add(mt5);

                _TreeViewItem.ContextMenu = Menu;
                ////now add the item to tree
                monitors_tree.Items.Add(_TreeViewItem);
                
                //// create graphs and attach               

                System.Windows.Controls.DataVisualization.Charting.Chart _chart = new System.Windows.Controls.DataVisualization.Charting.Chart();
                _chart.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                _chart.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                _chart.Height = 250;
                _chart.Foreground = new SolidColorBrush(Colors.Black);
                _chart.BorderBrush = new SolidColorBrush(Colors.White);
                _chart.BorderThickness = new Thickness(0.1);
                _chart.Title = monitor.MonitorName;
                _chart.FontSize = 10;
                LinearAxis xaxis = new LinearAxis();
                xaxis.Orientation = AxisOrientation.X;
                xaxis.Title = "Elapsed Time";
                xaxis.Maximum = _Scenario.ExecutionTime;
                xaxis.Minimum = 0;

                LinearAxis yaxis = new LinearAxis();
                yaxis.Orientation = AxisOrientation.Y;
                yaxis.Title = "Usage in %";
                yaxis.Maximum = 100;
                yaxis.Minimum = 0;
                yaxis.ShowGridLines = true;

                _chart.Axes.Add(xaxis);
                _chart.Axes.Add(yaxis);
                
                server_resource_monitors_stackPanel.Children.Add(_chart);
            }            
        }        

        private void Runtime_Settings_Button_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window
            {
                Title = "Scenario Runtime Settings",
                Content = new ScenarioRuntimeSettings(_Scenario),
            };
            window.Closed += new System.EventHandler(window_Closed);
            window.Height = 500;
            window.Width = 730;
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            window.ShowDialog();            
        }

        void window_Closed(object sender, System.EventArgs e)
        {
            this.DataContext = _LoadTestStatus;
        }
        private static decimal graphTime = 0;
        private void initializeCharts()
        {
            RespTimeXaxis.Maximum = _Scenario.ExecutionTime;
            TPSSeriesXAxis.Maximum = _Scenario.ExecutionTime;
            throughPutSeriesXaxis.Maximum = _Scenario.ExecutionTime;
            loadPatternAxis.Maximum = _Scenario.ExecutionTime;
            //averageRespSeriesXAxis.Maximum = _Scenario.ExecutionTime;
 
            loadPatternPreviewChart.FontSize = 12;
            List<KeyValuePair<int, int>> otherValueList = new List<KeyValuePair<int, int>>();
            List<KeyValuePair<int, int>> loadProgressSeriesValueList = new List<KeyValuePair<int, int>>();
            loadPreviewValueList = new List<KeyValuePair<decimal, decimal>>();
            int rampupTime = _Scenario.rampUpTime*60;
            int userStep = _Scenario.rampUserStep;
            int maxUsers = _Scenario.TotalNumberOfUsers;
            int rampDownTime = _Scenario.rampDownTime*60;
            int rampTimeStep = _Scenario.rampTimeStep;

            decimal XTime = 0;
            int YUser = 0;

            //initialize y axis for all graphs            

            for (int i = 0; i < (int)(rampupTime/rampTimeStep) && YUser<maxUsers; i++)
            {
                XTime += rampTimeStep;
                YUser += userStep;
                if (i % 5 == 0)
                {
                    graphTime = XTime/60;
                    graphTime = Math.Round(graphTime, 1);
                    loadPreviewValueList.Add(new KeyValuePair<decimal, decimal>(graphTime, YUser));
                    otherValueList.Add(new KeyValuePair<int, int>((int)graphTime, 0));
                }
            }

            int stableTime = _Scenario.ExecutionTime - rampupTime/60 - rampDownTime/60;

            for (int i = 0; i < stableTime / 5; i++)
            {
                graphTime += 5;
                YUser = maxUsers;
                graphTime = Math.Round(graphTime, 1);
                loadPreviewValueList.Add(new KeyValuePair<decimal, decimal>(graphTime, YUser));
                otherValueList.Add(new KeyValuePair<int, int>((int)graphTime, 0));
            }

            XTime = graphTime * 60;

            for (int i = 0; i < (int)(rampDownTime / rampTimeStep) && YUser > 0; i++)
            {
                XTime += rampTimeStep;
                YUser -= userStep;
                if (i % 5 == 0)
                {
                    graphTime = XTime / 60;
                    graphTime = Math.Round(graphTime,1);
                    loadPreviewValueList.Add(new KeyValuePair<decimal, decimal>(graphTime, YUser));
                    otherValueList.Add(new KeyValuePair<int, int>((int)graphTime, 0));
                }
            }
            loadPreviewValueList.Add(new KeyValuePair<decimal, decimal>(_Scenario.ExecutionTime, 0));

            loadPatternPreviewSeries.DataContext = loadPreviewValueList;
            loadPatternPreviewSeries.Title = "X: Time in Min\nY: No. of Users";
            loadPatternPreviewSeries.Refresh();
            throughPutSeries.DataContext = otherValueList;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl _TabControl = (TabControl)sender;
            if (_TabControl.SelectedIndex == 0)
            {
                server_resource_stackPanel.Visibility = Visibility.Collapsed;
                current_test_status_stackPanel.Visibility = Visibility.Visible;                
            }
            else if (_TabControl.SelectedIndex == 1)
            {
                current_test_status_stackPanel.Visibility = Visibility.Collapsed;
                server_resource_stackPanel.Visibility = Visibility.Visible;
            }
        }

        private void New_Monitor_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window
            {
                Title = "Create a new monitor",
                Content = new NewMonitor(),
            };            
            window.Height = 450;
            window.Width = 450;
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            window.Closed += new EventHandler(new_monitor_window_closed);
            window.ShowDialog();
        }
        void new_monitor_window_closed(object sender, System.EventArgs e)
        {
            retrieveServerResourceMonitors(_Scenario);
        }
        void _LineSeries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LineSeries _lineSeries = (LineSeries)sender;
            Brush back = new SolidColorBrush(Colors.White);
            string series_name = _lineSeries.Name;
            
            Chart _Chart = (Chart)_lineSeries.SeriesHost;
            for (int i = 0; i < _Chart.Series.Count; i++)
            {
                ((LineSeries)_Chart.Series[i]).PolylineStyle = (Style)this.Resources["DashedPolyLine"];
            }
            _lineSeries.PolylineStyle = (Style)this.Resources["ThickPolyLine"];

            for (int i = 0; i < test_summary_table_grid.Items.Count; i++)
            {
                TransactionMetrics _TransactionMetrics = (TransactionMetrics)test_summary_table_grid.Items[i];
                if (series_name == _TransactionMetrics.TransactionName)
                {
                    test_summary_table_grid.SelectedIndex = i;
                    break;
                }
            }    
        }
        private void stop_loadtest_now_Click(object sender, RoutedEventArgs e)
        {
            _LoadTestStatus.StopLoadTestButtonStatus = false;
            rampDownNow = true;
        }
        private void initializeAll_if_restarting_Load()
        {
            isMoreLoadingPossible = true;
            rampDownNow = false;
            currentLoadedUsers = 0;
            _nextExecutionNumber = 0;
        }
        private void start_loadtest_now_Click(object sender, RoutedEventArgs e)
        {
            ScriptRequestMap = new List<KeyValuePair<int, ArrayList>>();
            requestsList = new List<KeyValuePair<double, string>>();
            requestGroups = new List<KeyValuePair<double, ArrayList>>();
            txnNameList = new List<KeyValuePair<double, string>>();
            _nextExecutionNumber = 0;
            currentLoadedUsers = 0;
            isMoreLoadingPossible = true;
            current_execution_id = 0;            

            ExecutionDAO _DBResponseWriter = new ExecutionDAO();
            current_execution_id = _DBResponseWriter.addTestExecution(ExecutionStudio._Scenario.ProjectID, ExecutionStudio._Scenario.ScenarioID);

            initializeAll_if_restarting_Load();
            test_summary_table_grid.DataContext = _LoadTestStatus;
            _LoadTestStatus.StartLoadTestButtonStatus = false;
            _LoadTestStatus.StopLoadTestButtonStatus = true;            
            App._project.loadTestExecutingCurrently = true;
            
            startPoller();

            Thread startRampDownTimerThread = new Thread(startRampDownTimer);
            startRampDownTimerThread.Start();
            
            retrivalWorker = new BackgroundWorker();
            retrivalWorker.WorkerReportsProgress = true;
            retrivalWorker.WorkerSupportsCancellation = true;
            retrivalWorker.DoWork += retrieveAll;
            retrivalWorker.ProgressChanged += delegate(object s, ProgressChangedEventArgs args)
            {
                int progress = args.ProgressPercentage;
                if (progress == 30)
                {
                    _LoadTestStatus.ProgressPercentage = 30;
                    _LoadTestStatus.ProgressMessage = "Retrieving Requests...";
                }
                else if (progress == 60)
                {
                    _LoadTestStatus.ProgressPercentage = 60;
                    _LoadTestStatus.ProgressMessage = "Retrieving Parameters...";
                }
                else if (progress == 90)
                {
                    _LoadTestStatus.ProgressPercentage = 90;
                    _LoadTestStatus.ProgressMessage = "Retrieving Headers...";
                }
                else if (progress == 100)
                {
                    _LoadTestStatus.ProgressPercentage = 100;
                    _LoadTestStatus.ProgressMessage = "Retrieval Completed.";
                }
            };
            retrivalWorker.RunWorkerAsync();
            retrivalWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(retrivalWorker_RunWorkerCompleted);
        }

        private void startRampDownTimer()
        {
            double rampDownTimePosition = _Scenario.rampDownTime;
            bool isTestStarted = false;
            DateTime start_time = new DateTime();
            //int time_remaining = 0;
            while (true)
            {
                if (start_time == new DateTime())
                {
                    ExecutionDAO _DBResponseIO = new ExecutionDAO();
                    start_time = _DBResponseIO.getTestStartTime();
                }
                else
                {
                    isTestStarted = true;
                }
                if (isTestStarted)
                {
                    TimeSpan elapsedTime = DateTime.Now - start_time;
                    double timeRemaining = _Scenario.ExecutionTime - elapsedTime.TotalMinutes;
                    if (timeRemaining <= rampDownTimePosition)
                    {
                        rampDownNow = true;
                        _LoadTestStatus.StopLoadTestButtonStatus = false;                        
                        break;
                    }
                }
                Thread.Sleep(30);
            }
        }
        private void retrieveAll(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            retrivalWorker.ReportProgress(30);
            Thread.Sleep(1000);
            retrieveSessions_and_RequestIDs();
            retrivalWorker.ReportProgress(60);
            Thread.Sleep(1000);
            retrieveParameters();
            retrivalWorker.ReportProgress(90);
            Thread.Sleep(1000);
            retrieveHeaders();
            retrivalWorker.ReportProgress(100);
            Thread.Sleep(500);
        }
        void retrivalWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            decideMaxLimit_for_Each_Txn_Group();
            rampUpWorker = new BackgroundWorker();
            rampUpWorker.WorkerReportsProgress = true;
            rampUpWorker.WorkerSupportsCancellation = true;
            rampUpWorker.DoWork += processRampUp;
            rampUpWorker.ProgressChanged += delegate(object s, ProgressChangedEventArgs args)
            {
                int progress = args.ProgressPercentage;
                if (progress == 1001)
                {
                    ExecutionDAO _DBResponseWriter = new ExecutionDAO();
                    _DBResponseWriter.addTestEvent(current_execution_id, _Scenario.ProjectID, _Scenario.ScenarioID, "STABLE_STATE_ACHIEVED");

                    _LoadTestStatus.ProgressMessage = "Stable State achieved !!!";
                }
                else if (progress == 1002)
                {
                    _LoadTestStatus.ProgressMessage = "Ramping Down <<<";
                    ExecutionDAO _DBResponseWriter = new ExecutionDAO();
                    _DBResponseWriter.addTestEvent(current_execution_id, _Scenario.ProjectID, _Scenario.ScenarioID, "PREMATURE_RAMPDOWN_STARTED");
                }
                else
                {
                    _LoadTestStatus.ProgressPercentage = progress;
                    _LoadTestStatus.ProgressMessage = "Ramping Up >>>";
                }                
                _LoadTestStatus.NumberOfUsers = currentLoadedUsers.ToString()+" Vusers";
            };
            rampUpWorker.RunWorkerAsync();            
        }                
        private void processRampUp(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            ExecutionDAO _DBResponseWriter = new ExecutionDAO();
            _DBResponseWriter.addTestEvent(current_execution_id, _Scenario.ProjectID, _Scenario.ScenarioID, "RAMPUP_STARTED");

            enumRequestGroup = requestGroups.GetEnumerator();//// initialize the enum for iterations
            rampUpWorker.ReportProgress(-1);
            Thread.Sleep(1000);
            int maxUsers = 100;//_Scenario.TotalNumberOfUsers;
            int timeStep = _Scenario.rampTimeStep;
            int userStep = _Scenario.rampUserStep;
            int rampupTime = _Scenario.rampUpTime * 60;

            while (isMoreLoadingPossible && (!rampDownNow) && currentLoadedUsers < maxUsers)
            {
                Thread[] array = new Thread[userStep];
                for (int i = 0; i < array.Length; i++)
                {                    
                    array[i] = new Thread(new ThreadStart(Start));
                    array[i].Start();

                    if (currentLoadedUsers + i + 1 == maxUsers)
                    {
                        userStep = maxUsers - currentLoadedUsers;
                        break; 
                    }
                }
                
                currentLoadedUsers += userStep;
                double progress = (currentLoadedUsers * 100) / maxUsers;
                rampUpWorker.ReportProgress((int)progress);
                Thread.Sleep(timeStep*1000);
            }
            if ((currentLoadedUsers >= maxUsers) && (!rampDownNow))
            {
                rampUpWorker.ReportProgress(1001);
            }
            else if (rampDownNow)
            {
                rampUpWorker.ReportProgress(1002);
            }
        }
        private void decideMaxLimit_for_Each_Txn_Group()
        {
            HashSet<KeyValuePair<double, string>> HashTxnNames = new HashSet<KeyValuePair<double, string>>(txnNameList);
            List<KeyValuePair<double, string>> HashTxnNamesList = new List<KeyValuePair<double, string>>(HashTxnNames);

            requestRepeatList = new List<KeyValuePair<int, int>>();
            loadDistributionList = new List<KeyValuePair<double, int>>();
            object obj = requestGroups;
            for (int i = 0; i < ScriptRequestMap.Count; i++)//find if any script contains more than onepage
            {
                KeyValuePair<int, ArrayList> _key_val = ScriptRequestMap[i];
                ArrayList request_ids = _key_val.Value;
                int presenceCount = 0;

                for (int reqCount = 0; reqCount < HashTxnNamesList.Count; reqCount++)
                {
                    double request_id = HashTxnNamesList[reqCount].Key;
                    if (request_ids.Contains(request_id))
                    {
                        presenceCount++;
                    }
                }
                requestRepeatList.Add(new KeyValuePair<int, int>(_key_val.Key, presenceCount));
            }
            for (int reqCount = 0; reqCount < ScriptList.Count; reqCount++)
            {
                Script _script = (Script)ScriptList[reqCount];
                KeyValuePair<int, ArrayList> _key_val_script_req = ScriptRequestMap.Find(x => x.Key == _script.ScriptID);
                KeyValuePair<int, int> _key_val_script_repeat = requestRepeatList.Find(x => x.Key == _script.ScriptID);
                for (int i = 0; i < HashTxnNamesList.Count; i++)
                {
                    KeyValuePair<double,string> txn_name_key_val = HashTxnNamesList[i];
                    if (_key_val_script_req.Value.Contains(txn_name_key_val.Key))
                    {
                        loadDistributionList.Add(new KeyValuePair<double, int>(txn_name_key_val.Key, (int)Math.Ceiling((decimal)_script.NumberOfUsers / _key_val_script_repeat.Value)));
                    }
                }                
            }
        }
        private bool imageSaved = false;
        private void startPoller()
        {
            RespTimeXaxis.Maximum = _Scenario.ExecutionTime;
            pollerWorker = new BackgroundWorker();
            pollerWorker.WorkerReportsProgress = true;
            pollerWorker.WorkerSupportsCancellation = true;
            pollerWorker.DoWork += pollNow;
            pollerWorker.ProgressChanged += delegate(object s, ProgressChangedEventArgs args)
            {
                if (rampDownNow && !imageSaved)
                {
                    saveGraphsToDB();
                    imageSaved = true;
                }
                updateServerMonitorGraphs();

                TPSSeries.ItemsSource = LoadTestStatus.TPSSeriesValueList;
                TPSSeries.Refresh();

                try
                {
                    throughPutSeries.ItemsSource = LoadTestStatus.throughPutSeriesValueList;
                }
                catch { }
                throughPutSeries.Refresh();
                
                Thread summaryThread = new Thread(refreshSummaryMetrics);
                summaryThread.SetApartmentState(ApartmentState.STA);
                summaryThread.Start();

                List<KeyValuePair<decimal, decimal>> loadProgressValueList = new List<KeyValuePair<decimal, decimal>>();
                int time = args.ProgressPercentage;
                loadProgressValueList.Add(new KeyValuePair<decimal, decimal>((decimal)(time/60), 0));
                loadProgressValueList.Add(new KeyValuePair<decimal, decimal>((decimal)(time/60), _Scenario.TotalNumberOfUsers));

                Thread refreshTestStatusTextsThread = new Thread(refreshTestStatusTexts);
                refreshTestStatusTextsThread.SetApartmentState(ApartmentState.STA);
                refreshTestStatusTextsThread.Start();                

                responseTimeChart.Series.Clear();
                for (int k = 0; k < _ResponseTimePointList.Count; k++)
                {
                    System.Windows.Controls.DataVisualization.Charting.LineSeries _LineSeries = new System.Windows.Controls.DataVisualization.Charting.LineSeries();
                    _LineSeries.DependentValuePath = "Value";
                    _LineSeries.IndependentValuePath = "Key";
                    _LineSeries.Foreground = new SolidColorBrush(Colors.Black);
                    List<KeyValuePair<double, double>> myList = (List<KeyValuePair<double, double>>)_ResponseTimePointList[k];

                    _LineSeries.ItemsSource = myList;
                    _LineSeries.DataContext = myList;
                    _LineSeries.Name = "Series_" + loadDistributionList[k].Key.ToString();
                    _LineSeries.IsSelectionEnabled = true;                    
                    _LineSeries.SelectionChanged += new SelectionChangedEventHandler(_LineSeries_SelectionChanged);

                    responseTimeChart.Series.Add(_LineSeries);
                }

                averageRespSeriesColumnRefresh();                
            };
            pollerWorker.RunWorkerAsync();
        }

        private void updateServerMonitorGraphs()
        {
            //server_resource_monitors_stackPanel.Children
            string[] strMeasures = {"CPU","RAM","DISK"};
            ArrayList monitors = DBProcessingLayer.retrieveServerResourceMonitors(_Scenario.ScenarioID);

            for (int i = 0; i < monitors.Count; i++)
            {
                ServerMonitor monitor = (ServerMonitor)monitors[i];

                Chart _chart = null;
                for (int l = 0; l < server_resource_monitors_stackPanel.Children.Count; l++)
                {
                    _chart = (Chart)server_resource_monitors_stackPanel.Children[i];
                    if ((string)_chart.Title == (string)monitor.MonitorName)
                    {
                        break;
                    }
                }

                if (_chart != null)
                {
                    _chart.Series.Clear();

                    for (int k = 0; k < strMeasures.Length; k++)
                    {
                        List<KeyValuePair<double, double>> monitor_value_list = ExecutionDAO.getMonitorValueList_from_DB(monitor.MonitorId, current_execution_id, strMeasures[k]);                    
                    
                        System.Windows.Controls.DataVisualization.Charting.LineSeries _LineSeries = new System.Windows.Controls.DataVisualization.Charting.LineSeries();                        
                        _LineSeries.Title = strMeasures[k];
                        _LineSeries.Title += " Usage in %";                        
                        _LineSeries.DependentValuePath = "Value";
                        _LineSeries.IndependentValuePath = "Key";
                        _LineSeries.IsSelectionEnabled = true;
                        _LineSeries.Foreground = new SolidColorBrush(Colors.Black);
                        _LineSeries.FontSize = 10;

                        _LineSeries.ItemsSource = monitor_value_list;

                        _chart.Series.Add(_LineSeries);
                    }
                }
            }
        }

        private void refreshTestStatusTexts()
        {
            while (_LoadTestStatus.StopLoadTestButtonStatus)
            {
                Thread.Sleep(5000);
                _LoadTestStatus.ActiveVUsers = "" + currentLoadedUsers;
                
                ExecutionDAO _DBResponseIO = new ExecutionDAO();
                TimeSpan _time_span = DateTime.Now - _DBResponseIO.getTestStartTime();
                _LoadTestStatus.ElaspsedTime = _time_span.Hours + " Hrs " + _time_span.Minutes + " Min " + _time_span.Seconds + " Sec ";

                TimeSpan t1 = _time_span.Subtract(new TimeSpan(0, _Scenario.ExecutionTime, 0));
                _LoadTestStatus.TimeRemaining = t1.Hours + " Hrs " + t1.Minutes + " Min " + t1.Seconds + " Sec ";

                _LoadTestStatus.AvgRespTime = _DBResponseIO.getAverageRespTime_For_All_Txn() + " sec";
                KeyValuePair<int, int> _TPS_key_Val_pair = _DBResponseIO.getLatestTPS();
                _LoadTestStatus.LableTPS = _TPS_key_Val_pair.Value.ToString()+" per sec ";
                _LoadTestStatus.LblThroughput = (_DBResponseIO.getThroughPut(5)).Value.ToString() + " KBps";
                _LoadTestStatus.TotalError = _DBResponseIO.getErrorCount();
                _LoadTestStatus.TotalTxns = _DBResponseIO.getTotalTxnCount();
                _LoadTestStatus.TotalPass = (Int32.Parse(_LoadTestStatus.TotalTxns) - Int32.Parse(_LoadTestStatus.TotalError)).ToString();

                if (Int32.Parse(_LoadTestStatus.TotalTxns) > 0)
                {
                    _LoadTestStatus.PassPercentage = ((Int32.Parse(_LoadTestStatus.TotalPass) * 100) / Int32.Parse(_LoadTestStatus.TotalTxns)).ToString() + " % ";
                }
                else
                {
                    _LoadTestStatus.PassPercentage = "N/A";
                }
            }
        }
        void pollNow(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            int graph_time = 0;
            while (_LoadTestStatus.StopLoadTestButtonStatus || !imageSaved)
            {
                refreshResponseTimeChart();
                refreshTPSGraph();
                refreshTHPUTGraph();
                refreshErrorGraph();
                fetchMonitorStats();
                graph_time += 15;
                pollerWorker.ReportProgress(graph_time);
                Thread.Sleep(15000);
            }
        }

        private void fetchMonitorStats()
        {
            ArrayList monitors = DBProcessingLayer.retrieveServerResourceMonitors(_Scenario.ScenarioID);
            for (int i = 0; i < monitors.Count; i++)
            {                
                Thread _monitorThread = new Thread(fetchMonitorStatsThread);
                _monitorThread.Start(monitors[i]);                
            }
        }
        object obj = new object();
        private void fetchMonitorStatsThread(Object obj)
        {           
            ServerMonitor monitor = (ServerMonitor)obj;
            string host = ((ServerMonitor)monitor).Host;
            int port = ((ServerMonitor)monitor).Port;

            Uri URL = new Uri("http://" + host + ":" + port+"/cpu");
            fetchMonitorStat_For_CPU(monitor,URL);

            URL = new Uri("http://" + host + ":" + port + "/ram");
            fetchMonitorStat_For_RAM(monitor, URL);

            URL = new Uri("http://" + host + ":" + port + "/disk");
            fetchMonitorStat_For_DISK(monitor, URL);
        }

        private void fetchMonitorStat_For_DISK(ServerMonitor monitor, Uri URL)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(URL);
            webrequest.KeepAlive = false;
            webrequest.Method = "GET";
            webrequest.ContentType = "text/html";
            webrequest.AllowAutoRedirect = true;
            webrequest.ProtocolVersion = HttpVersion.Version10;
            webrequest.ServicePoint.Expect100Continue = false;

            HttpWebResponse webresponse = null;
            try
            {
                webresponse = (HttpWebResponse)webrequest.GetResponse();
                StreamReader reader = new StreamReader(webresponse.GetResponseStream());
                string navigation_string = reader.ReadToEnd();

                reader.Close();
                webresponse.Close();

                int startPos = navigation_string.IndexOf("perfStart");
                startPos += 9;

                int endPos = navigation_string.IndexOf("perfEnd");
                int len = endPos - startPos;
                string perf = navigation_string.Substring(startPos, len);
                perf = perf.Remove(perf.IndexOf("Exiting"));

                string[] splits = null;
                if (perf != null && perf != "")
                {
                    splits = perf.Split(',');
                }
                string measure = "DISK";
                string value = splits[splits.Length - 1].ToString();
                value = value.Replace("\"", "");
                value = value.Replace("\n", "");
                value = (Math.Round(double.Parse(value), 2)).ToString();

                ExecutionDAO _DBResponseIO = new ExecutionDAO();
                double time = 0.0;
                try
                {
                    time = ((TimeSpan)(DateTime.Now - _DBResponseIO.getTestStartTime())).TotalMinutes;
                }
                catch { }
                saveStatToDB(monitor, current_execution_id, measure, value, time);
            }
            catch (Exception) { }
        }

        private void fetchMonitorStat_For_RAM(ServerMonitor monitor, Uri URL)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(URL);
            webrequest.KeepAlive = false;
            webrequest.Method = "GET";
            webrequest.ContentType = "text/html";
            webrequest.AllowAutoRedirect = true;
            webrequest.ProtocolVersion = HttpVersion.Version10;
            webrequest.ServicePoint.Expect100Continue = false;

            HttpWebResponse webresponse = null;
            try
            {
                webresponse = (HttpWebResponse)webrequest.GetResponse();
                StreamReader reader = new StreamReader(webresponse.GetResponseStream());
                string navigation_string = reader.ReadToEnd();

                reader.Close();
                webresponse.Close();

                int startPos = navigation_string.IndexOf("perfStart");
                startPos += 9;

                int endPos = navigation_string.IndexOf("perfEnd");
                int len = endPos - startPos;
                string perf = navigation_string.Substring(startPos, len);
                //perf = perf.Remove(perf.IndexOf("Exiting"));

                string[] splits = null;
                if (perf != null && perf != "")
                {
                    splits = perf.Split('\n');
                }
                string measure = "RAM";


                string RAMTotal = (splits[0].Replace("KiB","")).Trim();
                RAMTotal = (splits[0].Replace("RAM total:", "")).Trim();
                RAMTotal = (RAMTotal.Replace("KiB","")).Trim();
                

                string RAMUsed = (splits[2].Replace("KiB", "")).Trim();
                RAMUsed = (splits[2].Replace("RAM used:", "")).Trim();
                RAMUsed = (RAMUsed.Replace("KiB", "")).Trim();

                string value = Math.Round((double.Parse(RAMUsed) * 100) / double.Parse(RAMTotal),2).ToString();

                ExecutionDAO _DBResponseIO = new ExecutionDAO();
                double time = 0.0;
                try
                {
                    time = ((TimeSpan)(DateTime.Now - _DBResponseIO.getTestStartTime())).TotalMinutes;
                }
                catch { }
                saveStatToDB(monitor, current_execution_id, measure, value, time);
            }
            catch (Exception) { }
        }

        private void fetchMonitorStat_For_CPU(ServerMonitor monitor, Uri URL)
        {
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(URL);
            webrequest.KeepAlive = false;
            webrequest.Method = "GET";
            webrequest.ContentType = "text/html";
            webrequest.AllowAutoRedirect = true;
            webrequest.ProtocolVersion = HttpVersion.Version10;
            webrequest.ServicePoint.Expect100Continue = false;

            HttpWebResponse webresponse = null;
            try
            {
                webresponse = (HttpWebResponse)webrequest.GetResponse();
                StreamReader reader = new StreamReader(webresponse.GetResponseStream());
                string navigation_string = reader.ReadToEnd();

                reader.Close();
                webresponse.Close();

                int startPos = navigation_string.IndexOf("perfStart");
                startPos += 9;

                int endPos = navigation_string.IndexOf("perfEnd");
                int len = endPos - startPos;
                string perf = navigation_string.Substring(startPos, len);
                perf = perf.Remove(perf.IndexOf("Exiting"));

                string[] splits = null;
                if (perf != null && perf != "")
                {
                    splits = perf.Split(',');
                }
                string measure = "CPU";
                string value = splits[splits.Length-1].ToString();
                value = value.Replace("\"", "");
                value = value.Replace("\n", "");
                value = (Math.Round(double.Parse(value), 2)).ToString();

                ExecutionDAO _DBResponseIO = new ExecutionDAO();
                double time = 0.0;
                try
                {
                    time = ((TimeSpan)(DateTime.Now - _DBResponseIO.getTestStartTime())).TotalMinutes;
                }catch{}
                saveStatToDB(monitor,current_execution_id ,measure, value, time);
            }
            catch (Exception) { }
        }

        private void saveStatToDB(ServerMonitor monitor, int current_execution_id, string measure, string value, double p)
        {
            int monitor_id = monitor.MonitorId;
            string monitor_name = monitor.MonitorName;
            int execution_id = current_execution_id;
            double time = Math.Round(p,2);
            ExecutionDAO _executionDAO = new ExecutionDAO();
            _executionDAO.saveStatsToDB(monitor_id,execution_id,measure,value,time,monitor_name);
        }
        private void refreshTHPUTGraph()
        {
            ExecutionDAO _DBResponseIO = new ExecutionDAO();
            KeyValuePair<double, double> _THPUT_key_Val_pair = _DBResponseIO.getLatestTHPUT();            
            TimeSpan _time_span = DateTime.Now - _DBResponseIO.getTestStartTime();
            _THPUT_key_Val_pair = new KeyValuePair<double, double>(_time_span.TotalMinutes, _THPUT_key_Val_pair.Value);

            LoadTestStatus.throughPutSeriesValueList.Add(_THPUT_key_Val_pair);
        }
        private void refreshTPSGraph()
        {
            ExecutionDAO _DBResponseIO = new ExecutionDAO();

            KeyValuePair<int, int> _TPS_key_Val_pair = _DBResponseIO.getLatestTPS();

            TimeSpan _time_span = DateTime.Now - _DBResponseIO.getTestStartTime();
            _TPS_key_Val_pair = new KeyValuePair<int, int>((int)_time_span.TotalMinutes, _TPS_key_Val_pair.Value);

            LoadTestStatus.TPSSeriesValueList.Add(_TPS_key_Val_pair);
        }
        private void refreshErrorGraph()
        {
            ExecutionDAO _DBResponseIO = new ExecutionDAO();

            KeyValuePair<int, int> _Error_key_Val_pair = _DBResponseIO.getLatestError(30);

            TimeSpan _time_span = DateTime.Now - _DBResponseIO.getTestStartTime();
            _Error_key_Val_pair = new KeyValuePair<int, int>((int)_time_span.TotalMinutes, _Error_key_Val_pair.Value);

            LoadTestStatus.errorSeriesValueList.Add(_Error_key_Val_pair);
        }
        private void refreshResponseTimeChart()
        {
            ExecutionDAO _DBResponseIO = new ExecutionDAO();
            _ResponseTimePointList = new ArrayList();
            for (int i = 0; loadDistributionList != null && i < loadDistributionList.Count; i++)
            {
                KeyValuePair<double, int> _request_key_val = loadDistributionList[i];
                List<KeyValuePair<double, double>> _response_time_list = _DBResponseIO.getResponseTimeFor_request(_request_key_val.Key);
                List<KeyValuePair<double, double>> myList = new List<KeyValuePair<double, double>>();

                for (int j = 0; j < _response_time_list.Count; j++)
                {
                    KeyValuePair<double, double> _resp_time_pair = _response_time_list[j];
                    myList.Add(new KeyValuePair<double, double>(_resp_time_pair.Key, _resp_time_pair.Value));
                }

                _ResponseTimePointList.Add(myList);
            }
        }
        void Start()
        {
            int count = 0;
            KeyValuePair<double, ArrayList> _key_val = getNextLeadRequest(ref count);
            startRequestSetExecution(_key_val);
        }
        object testingLock = new object();        
        private void startRequestSetExecution(KeyValuePair<double, ArrayList> _key_val)
        {
            while(!rampDownNow)
            {
                KeyValuePair<int, ArrayList> script_req_key_val = new KeyValuePair<int,ArrayList>();
                execute_And_Note_RequestSet(_key_val);
                for (int i = 0; i < ScriptRequestMap.Count; i++)
                {
                    script_req_key_val = ScriptRequestMap[i];
                    if (script_req_key_val.Value.Contains(_key_val.Key))
                    {
                        break;
                    }
                }

                int script_id = script_req_key_val.Key;
                Script script_here = null;
                for (int i = 0; i < ScriptList.Count; i++)
                {
                    if (((Script)ScriptList[i]).ScriptID == script_id)
                    {
                        script_here = (Script)ScriptList[i];
                    }
                }
                
                int thinkTimeMin = 15000;
                int thinkTimeMax = 15000;
                if (script_here != null)
                {
                    thinkTimeMin = script_here.ThinkTimeMin*1000;
                    thinkTimeMax = script_here.ThinkTimeMax*1000;
                }
                Random _r = new Random();
                int thinkTime = _r.Next(thinkTimeMin, thinkTimeMax);
                Thread.Sleep(thinkTime);//Think Time
            }
            lock (rampDownLock)
            {
                if (currentLoadedUsers == _Scenario.TotalNumberOfUsers)
                {
                    ExecutionDAO _DBResponseWriter = new ExecutionDAO();
                    _DBResponseWriter.addTestEvent(current_execution_id, _Scenario.ProjectID, _Scenario.ScenarioID, "RAMPDOWN_STARTED");
                }
                currentLoadedUsers--;
                _LoadTestStatus.NumberOfUsers = currentLoadedUsers.ToString() + " Vusers";
                _LoadTestStatus.ProgressPercentage = (currentLoadedUsers*100) / _Scenario.TotalNumberOfUsers;
                _LoadTestStatus.ProgressMessage = "Ramping Down <<<";
                int waitTime = 500;
                try
                {
                    waitTime = _Scenario.rampDownTime * 60 / _Scenario.TotalNumberOfUsers;
                }
                catch { }

                Thread.Sleep(waitTime);
            }
            if (currentLoadedUsers == 0)
            {
                ExecutionDAO _DBResponseWriter = new ExecutionDAO();
                _DBResponseWriter.addTestEvent(current_execution_id, _Scenario.ProjectID, _Scenario.ScenarioID, "RAMPDOWN_COMPLETED");                
                _DBResponseWriter.updateTestExecution(current_execution_id,_Scenario.ProjectID, _Scenario.ScenarioID);                                

                _LoadTestStatus.ProgressMessage = "Ramping Down completed, test over";
                
                App._project.loadTestExecutingCurrently = false;
                _LoadTestStatus.StartLoadTestButtonStatus = true;
                _LoadTestStatus.StopLoadTestButtonStatus = false;
            }
        }

        private void saveGraphsToDB()
        {
            try
            {
                saveAsImageToDB(averageRespChart, "Average Response Time(Top 10)");
            }
            catch { }
            try
            {
                saveAsImageToDB(responseTimeChart, "Response Time vs. Time");
            }
            catch { }
            try
            {
                saveAsImageToDB(TPSSeriesChart, "Transaction per second(TPS) vs Time");
            }
            catch { }
            try
            {
                saveAsImageToDB(throughPutSeriesChart, "ThroughPut vs Time");
            }
            catch { }

            for (int i = 0; i < server_resource_monitors_stackPanel.Children.Count; i++)
            {
                try
                {
                    Chart _chart = (Chart)server_resource_monitors_stackPanel.Children[i];
                    if (_chart != null)
                    {
                        saveAsImageToDB(_chart, (_chart.Title).ToString());
                    }
                }
                catch{}
            }
        }
        private void saveAsImageToDB(Chart thisChart, string graphName)
        {
            Rect bounds = VisualTreeHelper.GetDescendantBounds(thisChart);

            RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
              (int)(thisChart.ActualWidth * 2),
              (int)(thisChart.ActualHeight * 2),
              192d,
              192d,
              PixelFormats.Pbgra32);

            DrawingVisual dv = new DrawingVisual();

            using (DrawingContext ctx = dv.RenderOpen())
            {
                VisualBrush vb = new VisualBrush(thisChart);
                ctx.DrawRectangle(vb, null, new Rect(new Point(), bounds.Size));
            }

            renderBitmap.Render(dv);

            //using (FileStream outStream = new FileStream("c:\\imgs\\"+graphName + ".gif", FileMode.Create))
            //{
            //    GifBitmapEncoder encoder = new GifBitmapEncoder();
            //    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
            //    encoder.Save(outStream);
            //}

            using (MemoryStream outStream = new MemoryStream())
            {
                GifBitmapEncoder encoder = new GifBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(outStream);
                byte[] bytes = outStream.ToArray();
                ExecutionDAO.saveExecutionGraph(current_execution_id, bytes, graphName);
            }
        }
        private void execute_And_Note_RequestSet(KeyValuePair<double, ArrayList> _key_val)
        {
            //for lead request
            ThreadParameters ThreadParams = new ThreadParameters();
            int nextExecutionNumber = getNextExecutionNumber();
            int thread_id = Thread.CurrentThread.ManagedThreadId;
            ThreadParams.parent_request = _key_val.Key;
            ThreadParams.child = -1;
            ThreadParams.itrNo = nextExecutionNumber;
            ThreadParams.threadID = thread_id;
            startRequestExecution(ThreadParams);
            // lead request ends

            int child_requests = 0;
            if (_key_val.Value != null)
            {
                child_requests = _key_val.Value.Count;
            }
            
            int ThreadMaxNumber = 6;
            ThreadPool.SetMaxThreads(ThreadMaxNumber,25);
            int total_children = child_requests;
            
            for (int s = 0; s < total_children; s++)
            {
                ThreadParams = new ThreadParameters();
                ThreadParams.parent_request = _key_val.Key;
                ThreadParams.child = (double)_key_val.Value[s];
                ThreadParams.itrNo = nextExecutionNumber;
                ThreadParams.threadID = thread_id;

                ThreadPool.QueueUserWorkItem(new WaitCallback(startRequestExecution), (object)ThreadParams);
            }
        }
        private int getNextExecutionNumber()
        {
            lock (executionNumberLockObj)
            {
                _nextExecutionNumber++;
            }
            return _nextExecutionNumber;
        }        
        private void startRequestExecution(object obj)
        {
            ThreadParameters ThreadParam = (ThreadParameters)obj;
            RequestExecutor _RequestExecutor = new RequestExecutor(ThreadParam);
            _RequestExecutor.executeRequest();
        }
        private KeyValuePair<double, ArrayList> getNextLeadRequest(ref int count)
        {
            count++;            
            lock (thisLock)
            {
                if (enumRequestGroup.MoveNext())
                {
                    _key_val_Next_Lead_Req = enumRequestGroup.Current;
                }
                else
                {
                    enumRequestGroup.Dispose();
                    enumRequestGroup = requestGroups.GetEnumerator();//// initialize the enum for iterations
                }
                if (!isWithinLimit(_key_val_Next_Lead_Req.Key))
                {
                    if (count < requestGroups.Count)
                    {
                        getNextLeadRequest(ref count); //// recurssion used to find next valid request.
                    }
                    else
                    {
                        isMoreLoadingPossible = false;
                        _key_val_Next_Lead_Req = enumRequestGroup.Current;
                    }
                }
            }
            return _key_val_Next_Lead_Req;
        }
        private bool isWithinLimit(double request_id)
        {
            for (int i = 0; i < loadDistributionList.Count; i++)
            {
                KeyValuePair<double, int> _key_val = loadDistributionList[i];
                if (_key_val.Key == request_id)
                {
                    if (_key_val.Value > 0)
                    {
                        int reduced_val = _key_val.Value;
                        reduced_val--;
                        KeyValuePair<double, int> _new_key_val = new KeyValuePair<double, int>(request_id, reduced_val);
                        loadDistributionList.RemoveAt(i);
                        loadDistributionList.Insert(i, _new_key_val);
                        return true;
                    }                    
                }
            }
            return false;
        }
        private void retrieveHeaders()
        {
            headersList = new List<KeyValuePair<double, List<Dictionary<string, string>>>>();
            for (int reqCounter = 0; reqCounter < requestsList.Count; reqCounter++)
            {
                KeyValuePair<double, string> _KeyValuePair = requestsList[reqCounter];
                double request_id = _KeyValuePair.Key;
                List<Dictionary<string, string>> header_list = ScriptScenarioDAO.getAllHeaders_for_request_ID(request_id.ToString());
                headersList.Add(new KeyValuePair<double, List<Dictionary<string, string>>>(request_id,header_list));
            }            
        }

        private void retrieveParameters()
        {
            parametersList = new List<KeyValuePair<double, List<Dictionary<string, string>>>>();            
            for (int reqCounter = 0; reqCounter < requestsList.Count; reqCounter++)
            {
                KeyValuePair<double, string> _KeyValuePair = requestsList[reqCounter];
                double request_id = _KeyValuePair.Key;
                DBProcessingLayer.getAllParams_for_request_ID(_KeyValuePair.Key.ToString(), "ScriptReplay");
                NotifiableObservableCollection<Parameter> _params = Amigo.App.Parameters_For_Replay;                

                //param_dictionary
                List<Dictionary<string, string>> _parameter_list = new List<Dictionary<string, string>>();

                for (int j = 0; j < _params.Count; j++)
                {
                    Dictionary<string, string> param_dict = new Dictionary<string, string>();
                    param_dict.Add("ParamName", _params[j].ParamName.ToString());
                    param_dict.Add("ParamValue", _params[j].SubstututedParamValue);                    
                    param_dict.Add("SelectedCSVColumnName", _params[j].SelectedCSVColumnName);
                    param_dict.Add("ParameterizationSource", _params[j].ParameterizationSource.ToString());
                    param_dict.Add("LB", _params[j].LB);
                    param_dict.Add("RB", _params[j].RB);

                    _parameter_list.Add(param_dict);
                }
                parametersList.Add(new KeyValuePair<double, List<Dictionary<string, string>>>(request_id, _parameter_list));
            }
        }

        private void retrieveSessions_and_RequestIDs()
        {
            ScriptList = DBProcessingLayer.retriveScripts_for_Scenario(_Scenario.ScenarioID);
            for (int scriptCounter = 0; scriptCounter < ScriptList.Count; scriptCounter++)
            {
                double group_head_request = -1;
                Script _Script = (Script)ScriptList[scriptCounter];
                ArrayList child_requests = new ArrayList();
                ArrayList request_ids = DBProcessingLayer.getAllRequestIDs_for_Sessions(_Script.ScriptID);
                ScriptRequestMap.Add(new KeyValuePair<int,ArrayList> (_Script.ScriptID,request_ids));

                for (int reqCounter = 0; reqCounter < request_ids.Count; reqCounter++)
                {
                    double requestID = double.Parse(request_ids[reqCounter].ToString());
                    string context_path = "";
                    ArrayList paths = DBProcessingLayer.getAllPaths_for_Request_ID(requestID);
                    for (int j = 0; j < paths.Count; j++)
                    {
                        context_path += "/" + paths[j];
                    }
                    context_path = context_path.Replace("//", "/");
                    if (paths.Count > 0)
                    {                        
                        string[] _host_port = DBProcessingLayer.getReferer_for_RequestID(requestID.ToString());
                        string fullUrl = _host_port[0] + ":" + _host_port[1] + context_path;
                        requestsList.Add(new KeyValuePair<double, string>(requestID, fullUrl));
                        String title = "" + DBProcessingLayer.getTitle_for_request_ID(requestID.ToString());
                        if (title.Length > 0)
                        {
                            if (group_head_request == -1) ////first time
                            {
                                group_head_request = requestID;
                                child_requests = new ArrayList();
                                txnNameList.Add(new KeyValuePair<double, string>(requestID, title));
                            }
                            else //// nest time onwards
                            {
                                requestGroups.Add(new KeyValuePair<double, ArrayList>(group_head_request, child_requests));
                                group_head_request = requestID;
                                child_requests = new ArrayList();
                                txnNameList.Add(new KeyValuePair<double, string>(requestID, title));
                            }                            
                        }
                        else
                        {
                            child_requests.Add(requestID);
                        }
                    }                 
                }
                requestGroups.Add(new KeyValuePair<double, ArrayList>(group_head_request, child_requests)); //// for the last request group
            }
        }
    }
}