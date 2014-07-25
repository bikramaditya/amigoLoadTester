using System.Windows.Controls;
using Amigo.ViewModels;
using System.Collections.Generic;
using Amigo.Interfaces;
using System.Threading;
using System.ComponentModel;
using System;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Win32;
using System.Diagnostics;
using Amigo.Utils;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using NotesFor.HtmlToOpenXml;
using System.Net;
using System.Windows.Controls.DataVisualization.Charting;
using System.Collections;
using System.Windows.Data;


namespace Amigo
{
    /// <summary>
    /// Interaction logic for Tab4.xaml
    /// </summary>
    public partial class ReportsStudio : UserControl
    {
        private static LoadTestStatus _LoadTestStatus = new LoadTestStatus();
        public static TestExecutionDetails _TestExecutionDetails = new TestExecutionDetails();        
        BackgroundWorker bw;
        private string Location = ".\\..\\..\\";
        Report _Report = new Report();
        private bool isFirstTime = true;
        private BackgroundWorker reportWorker;
        public static string copiedLocation="";
        private int selectedExecutionID;
        private  object reportPreviewContent;
        delegate void Createdocument(string documentFileName);
        List<KeyValuePair<string, double>> _avgRespTimeDSList = new List<KeyValuePair<string, double>>();
        private Window window;
        Scenario _Scenario = null;
        List<KeyValuePair<string, List<KeyValuePair<double, double>>>> responseTimeSeriesValueList = new List<KeyValuePair<string, List<KeyValuePair<double, double>>>>();
        DateTime testStartTime = new DateTime();
        List<KeyValuePair<string, double>> pieSeriesList = new List<KeyValuePair<string, double>>();
        List<KeyValuePair<string, double>> staticPieSeriesList = new List<KeyValuePair<string, double>>();
        List<KeyValuePair<string, byte[]>> graph_name_byte_list = new List<KeyValuePair<string, byte[]>>();

        public ReportsStudio()
        {            
            InitializeComponent();
            this.GotFocus += new System.Windows.RoutedEventHandler(ReportsStudio_GotFocus);
            project_name.DataContext = Amigo.App._project;
            test_exec_tree_item.DataContext = _TestExecutionDetails;
            folder_root_tree.DataContext = _TestExecutionDetails;
            reportProgressBar.DataContext = _TestExecutionDetails;
            reportProgressText.DataContext = _TestExecutionDetails;
        }

        private void refreshFileFolders()
        {            
            folder_tree_item.Items.Clear();

            DirectoryInfo dir = new DirectoryInfo(Location+"Reports");
            FileInfo[] files = dir.GetFiles();
            Array.Sort(files, (x, y) => y.CreationTime.CompareTo(x.CreationTime));
            
            //System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Location);
            int i = 0;
            foreach (System.IO.FileInfo f in files)
            {
                i++;
                //LOAD FILES 
                string file_name = f.Name;

                StackPanel _sp = new StackPanel();
                _sp.Orientation = Orientation.Horizontal;
                Image _image = new Image();
                _image.Height = 20;
                _image.Width = 20;
                _image.Margin = new System.Windows.Thickness(0, 2, 0, 2);

                if (file_name.EndsWith("docx"))
                {
                    _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/docx.png", UriKind.Relative));
                }
                else if (file_name.EndsWith("pdf") || file_name.EndsWith("PDF"))
                {
                    _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/pdf.png", UriKind.Relative));
                }
                else if (file_name.EndsWith("pptx"))
                {
                    _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/pptx.png", UriKind.Relative));
                }
                else if (file_name.EndsWith("html") || file_name.EndsWith("htm"))
                {
                    _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/web_page.png", UriKind.Relative));
                }
                else if (file_name.EndsWith("txt"))
                {
                    _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/txt.png", UriKind.Relative));
                }
                else
                {
                    _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/other_file.png", UriKind.Relative));
                }
                Label _lable = new Label();
                _lable.Content = file_name;
                _lable.Foreground = new SolidColorBrush(Colors.White);

                ContextMenu _lbl_context = new System.Windows.Controls.ContextMenu();
                MenuItem m = new MenuItem();
                m.Margin = new System.Windows.Thickness(5);
                m.Header = "Downoad This Report";
                System.Windows.Controls.Image _context_image = new System.Windows.Controls.Image
                {
                    Source = new BitmapImage(new Uri(@"/Amigo;component/icons/download.png", UriKind.Relative)),
                    Height = 20,
                    Width = 20
                };
                _context_image.Margin = new System.Windows.Thickness(-10);
                m.Icon = _context_image;
                m.Click += new System.Windows.RoutedEventHandler(m_Click);

                _lbl_context.Items.Add(m);
                _lable.ContextMenu = _lbl_context;

                _sp.Children.Add(_image);
                _sp.Children.Add(_lable);
                _sp.Margin = new Thickness(-20,2,5,2);                
                folder_tree_item.Items.Add(_sp);
            }     
        }

        void m_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ContextMenu ctx = (ContextMenu)((MenuItem)e.Source).Parent;
            Label _lbl = (Label)ctx.PlacementTarget;
            string file_name = _lbl.Content.ToString();

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = file_name; // Default file name
            dlg.DefaultExt = file_name.Substring(file_name.LastIndexOf(".")+1); // Default file extension
            

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results 
            if (result == true)
            {
                string sourcefile = Location + "\\Reports\\" + file_name;
                string destFile = dlg.InitialDirectory + dlg.FileName;
                try
                {
                    File.Copy(sourcefile, destFile);
                }
                catch (FileNotFoundException ex)
                {
                    if (ex.Message.Contains("Could not find file"))
                    {
                        Xceed.Wpf.Toolkit.MessageBox.Show("File Not found in source folder.\nPlease reload the reports tab to refresh");
                    }
                    else
                        Xceed.Wpf.Toolkit.MessageBox.Show("Some Error Occured. Could not copy the file");
                }
                catch (System.IO.IOException ex)
                {
                    if (ex.Message.Contains("already exists"))
                    {                        
                        File.Copy(sourcefile, destFile,true);                        
                    }
                }
            }
        }

        void ReportsStudio_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            refreshFileFolders();

            if (App.currentProjectID != -1)
            {
                loadtest_runtime_summary_grid.Visibility = System.Windows.Visibility.Visible;
                retrieveReportValues();
                int execution_count = ReportsDAO.getExecutionCount(App.currentProjectID);

                if (_TestExecutionDetails.TestExecutions.Count != execution_count)
                {
                    bw = new BackgroundWorker();
                    bw.WorkerReportsProgress = true;
                    bw.WorkerSupportsCancellation = true;
                    bw.DoWork += retrieveTestExecutions;
                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);
                    retrivalCounter.Visibility = System.Windows.Visibility.Visible;
                    bw.ProgressChanged += delegate(object s, ProgressChangedEventArgs args)
                    {
                        int count = args.ProgressPercentage;
                        retrivalCounter.Text = "Retrieving...    Count: " + count;
                    };
                    bw.RunWorkerAsync();
                }
            }
        }

        private void retrieveReportValues()
        {
            try
            {
                if (isFirstTime)
                {
                    isFirstTime = false;
                    _Report = ReportsDAO.retrieveReport(App.currentProjectID);

                    if (_Report.ReportID == -1) return;

                    projectnameTextBox.Text = _Report.ReportName;
                    yourLogo.Text = _Report.YourLogoText;
                    customerLogo.Text = _Report.CustomerLogoText;
                    testDescription.Text = _Report.TestDescription;
                    //chkStablePeriod.IsEnabled = _Report.TimeFrame;
                    deploymentDiagramTextBox.Text = _Report.DeploymentDiagramText;
                    webServersTextBox.Text = _Report.WebServersText;
                    appServersTextBox.Text = _Report.AppServersText;
                    dbServersTextBox.Text = _Report.DbServersText;
                    otherServersTextBox.Text = _Report.OtherServersText;

                    string sections = _Report.SelectedSections;

                    string[] arrSections = sections.Split(',');

                    for (int i = 1; i <= arrSections.Length; i++)
                    {
                        string name = arrSections[i - 1];
                        System.Windows.Controls.CheckBox _CheckBox = (System.Windows.Controls.CheckBox)LayoutRoot.FindName(name);
                        _CheckBox.IsChecked = true;
                    }

                    if (_Report.ReportFormat == "RadioPDF") RadioPDF.IsChecked = true;
                    if (_Report.ReportFormat == "RadioDOCX") RadioDOCX.IsChecked = true;
                    if (_Report.ReportFormat == "RadioHTML") RadioHTML.IsChecked = true;
                    //if (_Report.ReportFormat == "RadioPPTX") RadioPPTX.IsChecked = true;

                    yourLogo.Text = _Report.YourLogoText;
                    customerLogo.Text = _Report.CustomerLogoText;
                    deploymentDiagramTextBox.Text = _Report.DeploymentDiagramText;
                }
            }
            catch { }
        }

        void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            retrivalCounter.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void retrieveTestExecutions(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            ReportsDAO.retrieveExecutions(App.currentProjectID , ref bw);
        }

        private void projectnameTextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (projectnameTextBox.Text.Trim() == "")
            {
                projectnameTextBox.Text = "Please enter a logicaliy identifiable name";
            }
        }

        private void projectnameTextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (projectnameTextBox.Text == "Please enter a logicaliy identifiable name")
            {
                projectnameTextBox.Text = "";
            }
        }

        private void yourLogo_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                yourLogo.Text = dlg.FileName;                
            }
        }

        private void customerLogo_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                customerLogo.Text = dlg.FileName;             
            }
        }
        private void deploymentDiagramTextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Images (*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                deploymentDiagramTextBox.Text = dlg.FileName;
            }
        }

        private void save_button_Click(object sender, RoutedEventArgs e)
        {
            _avgRespTimeDSList.Clear();

            _Report.ProjectID = App.currentProjectID;
            _Report.ReportName = projectnameTextBox.Text;
            _Report.YourLogoText = yourLogo.Text;
            _Report.CustomerLogoText = customerLogo.Text;
            _Report.TestDescription = testDescription.Text;
            //_Report.TimeFrame = chkStablePeriod.IsEnabled;
            _Report.DeploymentDiagramText = deploymentDiagramTextBox.Text;
            _Report.WebServersText = webServersTextBox.Text;
            _Report.AppServersText = appServersTextBox.Text;
            _Report.DbServersText = dbServersTextBox.Text;
            _Report.OtherServersText = otherServersTextBox.Text;

            string sections = "";
            for (int i = 1; i <= 10; i++)
            {
                string name = "sec"+i;
                System.Windows.Controls.CheckBox _CheckBox = (System.Windows.Controls.CheckBox)LayoutRoot.FindName(name);
                if (_CheckBox!=null && _CheckBox.IsChecked == true)
                {
                    sections += name+ ",";
                }
            }
            if (sections.Length>0)
            sections = sections.Remove(sections.Length-1);

            _Report.SelectedSections = sections;

            if (RadioPDF.IsChecked==true)
            {
                _Report.ReportFormat = "RadioPDF";
            }
            else if (RadioDOCX.IsChecked == true)
            {
                _Report.ReportFormat = "RadioDOCX";
            }
            else if (RadioHTML.IsChecked == true)
            {
                _Report.ReportFormat = "RadioHTML";
            }
            //else if (RadioPPTX.IsChecked == true)
            //{
            //    _Report.ReportFormat = "RadioPPTX";
            //}


            if ("Choose Your Logo" != _Report.YourLogoText) _Report.YourLogoImage = getLogoImage(_Report.YourLogoText);
            if ("Choose Customer's Logo" != _Report.CustomerLogoText) _Report.CustomerLogoImage = getLogoImage(_Report.CustomerLogoText);
            if ("Click here to choose the image" != _Report.DeploymentDiagramText) _Report.DeploymentDiagramImage = getLogoImage(_Report.DeploymentDiagramText);

            ReportsDAO.saveReport(_Report);

            generate_button.IsEnabled = true;
        }

        private byte[] getLogoImage(string _FileName)
        {
            byte[] _Buffer = null;
            try
            {
                System.IO.FileStream _FileStream = new System.IO.FileStream(_FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.IO.BinaryReader _BinaryReader = new System.IO.BinaryReader(_FileStream);
                long _TotalBytes = new System.IO.FileInfo(_FileName).Length;
                _Buffer = _BinaryReader.ReadBytes((Int32)_TotalBytes);
                _FileStream.Close();
                _FileStream.Dispose();
                _BinaryReader.Close();
            }
            catch (Exception _Exception)
            {             
                Console.WriteLine("Exception caught in process: {0}", _Exception.ToString());
            }
            return _Buffer;
        }

        private void generate_button_Click(object sender, RoutedEventArgs e)
        {
            KeyValuePair<int, string> selected_execution = new KeyValuePair<int,string>(-1,"");
            try
            {
                selected_execution = (KeyValuePair<int, string>)execution_root_tree.SelectedItem;
            }
            catch(Exception) { }

            if (selected_execution.Key == -1)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please select the execution from LEFT TREE for which you want to generate the report", "ERROR !!!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                selectedExecutionID = selected_execution.Key;
            }

            
            DirectoryInfo dir = new DirectoryInfo(Location + "Reports");
            FileInfo[] files = dir.GetFiles();
            FileInfo _file_info = Array.Find(files, x => x.Name.Equals(_Report.ReportName + ".docx"));

            if (_file_info == null)
            {
                generate_button.IsEnabled = false;

                reportWorker = new BackgroundWorker();
                reportWorker.WorkerReportsProgress = true;
                reportWorker.WorkerSupportsCancellation = true;
                reportWorker.DoWork += retrieveGraphData;
                reportWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(reportWorker_RunWorkerCompleted);
                
                reportWorker.ProgressChanged += delegate(object s, ProgressChangedEventArgs args)
                {
                    int progress = args.ProgressPercentage;
                    _TestExecutionDetails.ProgressPercentage = progress;
                };
                reportWorker.RunWorkerAsync();
            }
            else
            {
                generate_button.IsEnabled = false;
                Xceed.Wpf.Toolkit.MessageBox.Show("A File by this name already exists.\nPlease change the report name. You may give a version no. too","ERROR !!!",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        void reportWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            prepareReport();
        }
        private void retrieveGraphData(object sender, EventArgs e)
        {            
            testStartTime = ReportsDAO.getTestStartTime(selectedExecutionID);
            _Scenario = ReportsDAO.retrieveScenario_for_Execution_ID(selectedExecutionID);
            if (_Scenario == null) _Scenario = new Scenario();

            preparePieSeriesData();

            reportWorker.ReportProgress(1);
            
            retrieveTransactionSummaryMetric();
            
            reportWorker.ReportProgress(33);

            retrieveDataForAllGraphs();////done

            reportWorker.ReportProgress(66);

            retrieveResponseTimeData();

            reportWorker.ReportProgress(99);

            

            writeImagesFromBytes();
        }
        private void preparePieSeriesData()
        {
            ArrayList contentTypeList = ReportsDAO.getUniqueContentTypes(selectedExecutionID);
            double staticSize = 0.0, dynamicSize = 0.0;

            for (int i = 0; i < contentTypeList.Count; i++)
            {
                string contentType = contentTypeList[i].ToString();
                double totalSize = ReportsDAO.getTotalRespSize_for_contentType(contentType, selectedExecutionID);

                if (totalSize == 0) continue;                

                if (contentType.Contains("text"))
                {
                    dynamicSize += totalSize;
                }
                else
                {
                    staticSize += totalSize;

                    string subContentType = "";
                    if (contentType.Contains(';'))
                    {
                        subContentType = contentType.Remove(contentType.IndexOf(';'));
                    }
                    else
                    {
                        subContentType = contentType;
                    }

                    subContentType = subContentType +" - " +(Math.Round(totalSize, 2)).ToString()+" KB";

                    staticPieSeriesList.Add(new KeyValuePair<string, double>(subContentType, Math.Round(totalSize, 2)));
                }
            }
            pieSeriesList.Add(new KeyValuePair<string, double>("Dynamic - " + Math.Round(dynamicSize, 2)+" KB - "+ Math.Round((100 * (Math.Round(dynamicSize, 2)) / (Math.Round(dynamicSize, 2) + Math.Round(staticSize, 2))),2) +" %", Math.Round(dynamicSize, 2)));
            pieSeriesList.Add(new KeyValuePair<string, double>("Static - " + Math.Round(staticSize, 2) + " KB - " + Math.Round((100 * (Math.Round(staticSize, 2)) / (Math.Round(dynamicSize, 2) + Math.Round(staticSize, 2))), 2) + " %", Math.Round(staticSize, 2)));
        }
        private void retrieveResponseTimeData()
        {
            responseTimeSeriesValueList = new List< KeyValuePair<string, List<KeyValuePair<double, double>>>>();

            ArrayList parentTxnList = ReportsDAO.getPrinaryRequests_for_Execution(selectedExecutionID);
            for (int i = 0; parentTxnList != null && i < parentTxnList.Count; i++)
            {
                double request_id = (double)parentTxnList[i];
                String title = "" + DBProcessingLayer.getTitle_for_request_ID(request_id.ToString());
                ArrayList iterations = ReportsDAO.getIterations_for_RequestID(request_id, selectedExecutionID);

                List<KeyValuePair<double, double>> responseTimes = new List<KeyValuePair<double, double>>();

                for (int itr = 0; itr < iterations.Count; itr++)
                {
                    DateTime iteration_start_time = ReportsDAO.getIterationTime((int)iterations[itr],selectedExecutionID);

                    double graph_time = (iteration_start_time - testStartTime).TotalMinutes;
                    
                    responseTimes.Add(new KeyValuePair<double,double>(Math.Round(ReportsDAO.getResponseTime_for_Iteration((int)iterations[itr], selectedExecutionID)/1000,2), Math.Round(graph_time,2)));
                }
                responseTimeSeriesValueList.Add(new KeyValuePair<string, List<KeyValuePair<double,double>>>(title, responseTimes));
                
                double progress = 66 + ((double)(99 - 66) / parentTxnList.Count) * i;
                reportWorker.ReportProgress((int)progress);
            }
        }

        private void retrieveTransactionSummaryMetric()
        {
            _LoadTestStatus.TransactionMetrices.Clear();

            ArrayList parentTxnList = ReportsDAO.getPrinaryRequests_for_Execution(selectedExecutionID);
            
            for (int i = 0; parentTxnList != null && i < parentTxnList.Count; i++)
            {
                try
                {
                    double request_id = (double)parentTxnList[i];
                    String title = "" + DBProcessingLayer.getTitle_for_request_ID(request_id.ToString());
                    ArrayList iterations = ReportsDAO.getIterations_for_RequestID(request_id, selectedExecutionID);

                    List<double> responseTimes = new List<double>();

                    for (int itr = 0; itr < iterations.Count; itr++)
                    {
                        responseTimes.Add(ReportsDAO.getResponseTime_for_Iteration((int)iterations[itr], selectedExecutionID));
                    }

                    responseTimes.Sort();

                    TransactionMetrics _TransactionMetrics = new TransactionMetrics();
                    _TransactionMetrics.DisplayName = title;
                    _TransactionMetrics.RespTimeMin = ((double)Math.Round((responseTimes.Min()), 2) / 1000).ToString();
                    _TransactionMetrics.RespTimeMax = ((double)Math.Round((responseTimes.Max()), 2) / 1000).ToString();
                    _TransactionMetrics.RespTimeAvg = ((double)Math.Round((responseTimes.Average()), 2) / 1000).ToString();

                    int pos85 = (int)Math.Floor((0.85) * responseTimes.Count + 0.5) - 1;
                    int pos90 = (int)Math.Floor((0.90) * responseTimes.Count + 0.5) - 1;
                    int pos95 = (int)Math.Floor((0.95) * responseTimes.Count + 0.5) - 1;

                    _TransactionMetrics.RespTime85 = (Math.Round(responseTimes[pos85], 2) / 1000).ToString();
                    _TransactionMetrics.RespTime90 = (Math.Round(responseTimes[pos90], 2) / 1000).ToString();
                    _TransactionMetrics.RespTime95 = (Math.Round(responseTimes[pos95], 2) / 1000).ToString();

                    _LoadTestStatus.TransactionMetrices.Add(_TransactionMetrics);
                    double progress = 1 + ((double)(33 - 1) / parentTxnList.Count) * i;
                    reportWorker.ReportProgress((int)progress);
                }
                catch { }
            }
        }

        private void writeImagesFromBytes()
        {
            try
            {
                File.WriteAllBytes(Location + "\\Templates\\Images\\customerLogo.jpg", _Report.CustomerLogoImage);
            }catch(Exception){}
            try
            {
                File.WriteAllBytes(Location + "\\Templates\\Images\\yourLogo.jpg", _Report.YourLogoImage);
            }
            catch (Exception) { }
            try
            {
                File.WriteAllBytes(Location + "\\Templates\\Images\\deploymentDiagram.jpg", _Report.DeploymentDiagramImage);
            }
            catch (Exception) { }
        }

        private void retrieveDataForAllGraphs()
        {
            string Location = ".\\..\\..\\Templates\\Images\\";
            graph_name_byte_list = ReportsDAO.getAllStoredGraphs_for_execution(selectedExecutionID);
            for (int i = 0; i < graph_name_byte_list.Count; i++)
            {
                reportWorker.ReportProgress((int)(33 + ((graph_name_byte_list.Count - 1) / graph_name_byte_list.Count) * 33));

                KeyValuePair<string, byte[]> key_val = graph_name_byte_list[i];
                string name = key_val.Key;
                name = name.Replace(" ","-");
                byte[] data = key_val.Value;
                File.WriteAllBytes(Location+name+".gif",data);
            }
        }

        private void prepareReport()
        {
            window = new Window
            {
                Title = "Report Preview Window",
                Content = new ReportSubWindow(
                    selectedExecutionID,
                    graph_name_byte_list, 
                    _LoadTestStatus.TransactionMetrices, 
                    _Report, 
                    _Scenario, 
                    responseTimeSeriesValueList,
                    pieSeriesList,
                    staticPieSeriesList
                    ),
            };
            window.Height = 600;
            window.Width = 900;
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            _TestExecutionDetails.ProgressPercentage = 100;

            Thread.Sleep(500);
            window.ShowDialog();
            refreshFileFolders();
            if (copiedLocation == "")
            {
                return;
            }
            copiedLocation = Path.GetFullPath(copiedLocation);

            if (Xceed.Wpf.Toolkit.MessageBox.Show("File saved at\n" + copiedLocation + "\nDo you want to open the file?", "Copy Status !!!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                MyWebServer.isListeningEnabled = false;

                ProcessStartInfo startInfo = new ProcessStartInfo(copiedLocation);
                startInfo.WindowStyle = ProcessWindowStyle.Maximized;
                Process.Start(startInfo);                
            }
            using (WebClient client = new WebClient())
            {
                client.Headers["User-Agent"] =
                "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) " +
                "(compatible; MSIE 6.0; Windows NT 5.1; " +
                ".NET CLR 1.1.4322; .NET CLR 2.0.50727)";
                string html = client.DownloadString("http://localhost:5050/index.html");
            }
        }
        
        private void execution_root_tree_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }        
    }
}