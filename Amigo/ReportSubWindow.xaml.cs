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
using System.Threading;
using System.IO;
using Amigo.Utils;
using System.Net;
using Amigo.ViewModels;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using NotesFor.HtmlToOpenXml;
using System.Windows.Controls.DataVisualization.Charting;
using System.Collections;
using Amigo.Interfaces;
using System.ComponentModel;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for ReportSubWindow.xaml
    /// </summary>
    public partial class ReportSubWindow : UserControl
    {
        private string Location = ".\\..\\..\\";
        Report _Report = new Report();
        List<KeyValuePair<string, byte[]>> graph_name_byte_list = null;
        NotifiableObservableCollection<TransactionMetrics> TransactionMetrices = new NotifiableObservableCollection<TransactionMetrics>();
        public NotifiableObservableCollection<Script> Scripts = new NotifiableObservableCollection<Script>();
        Scenario _Scenario = null;
        int selectedExecutionID = -1;
        string[] ArrBandwidth = { "LAN (Use Max available)", "56 Kbps", "128 Kbps", "256 Kbps", "512 Kbps", "1 Mbps", "2 Mbps", "10 Mbps" };
        List<KeyValuePair<string, List<KeyValuePair<double, double>>>> responseTimeSeriesValueList = new List<KeyValuePair<string, List<KeyValuePair<double, double>>>>();
        List<KeyValuePair<string, double>> pieSeriesList = new List<KeyValuePair<string,double>>();
        List<KeyValuePair<string, double>> staticPieSeriesList = new List<KeyValuePair<string, double>>();


        public ReportSubWindow(
            int selectedExecutionID,
            List<KeyValuePair<string, byte[]>> graph_name_byte_list, 
            NotifiableObservableCollection<TransactionMetrics> TransactionMetrices,
            Report Report, 
            Scenario _Scenario, 
            List<KeyValuePair<string, List<KeyValuePair<double, double>>>> responseTimeSeriesValueList,
            List<KeyValuePair<string, double>> pieSeriesList ,
            List<KeyValuePair<string, double>> staticPieSeriesList 
            )
        {            
            InitializeComponent();
            this.responseTimeSeriesValueList = responseTimeSeriesValueList;
            this.selectedExecutionID = selectedExecutionID;
            this._Report = Report;
            this._Scenario = _Scenario;
            this.graph_name_byte_list = graph_name_byte_list;
            this.TransactionMetrices = TransactionMetrices;
            this.pieSeriesList = pieSeriesList;
            this.staticPieSeriesList = staticPieSeriesList;

            test_summary_table_grid.DataContext = TransactionMetrices;
            //throughPutSeries.DataContext = null;
            prepareAndSaveGraphs();
        }        
        private void prepareAndSaveGraphs()
        {
            prepareResponseTimeSeriesChart();
            preparePieSeries();
        }

        private void preparePieSeries()
        {
            analysisPieSeries.ItemsSource = pieSeriesList;
            staticPieSeries.ItemsSource = staticPieSeriesList;
        }

        private void prepareResponseTimeSeriesChart()
        {            
            for (int i = 0; i < responseTimeSeriesValueList.Count; i++)
            {
                KeyValuePair<string, List<KeyValuePair<double,double>>> _title_resptimeList = responseTimeSeriesValueList[i];
                LineSeries _LineSeries = new LineSeries();
                _LineSeries.Title = _title_resptimeList.Key;
                _LineSeries.DataContext = _title_resptimeList.Value;
                _LineSeries.ItemsSource = _title_resptimeList.Value;
                _LineSeries.DependentValuePath = "Key";
                _LineSeries.IndependentValuePath = "Value";
                _LineSeries.Foreground = new SolidColorBrush(Colors.Black);

                responseTimeSeriesChart.Series.Add(_LineSeries);
            }            
        }        
        
        private void generate_from_HTMLReport()
        {
            // create HTML report
            Dictionary<string, string> htmlDictionary = new Dictionary<string, string>();
            string[] lines = File.ReadAllLines(Location + "Templates\\" + "HTMLProperties.txt");
            for (int i = 0; i < lines.Length; i++)
            {
                string[] keyVal = lines[i].Split('$');
                htmlDictionary.Add(keyVal[0], keyVal[1]);
            }

            string indexHtml = "";
            indexHtml += htmlDictionary["DOCTYPE"];
            indexHtml += htmlDictionary["htmlStart"];
            indexHtml += htmlDictionary["head"];
            indexHtml += htmlDictionary["bodyStart"];
            indexHtml += htmlDictionary["firstTable"];
            indexHtml += htmlDictionary["customerImage"];
            indexHtml += htmlDictionary["yourImage"];
            indexHtml += htmlDictionary["perfTestHeader"];
            indexHtml += htmlDictionary["projectNameHeader"];
            indexHtml += htmlDictionary["version"];
            indexHtml += htmlDictionary["dateHeader"];
            indexHtml += htmlDictionary["firstTableEnd"];
            indexHtml += htmlDictionary["br1"];
            indexHtml += htmlDictionary["div1"];
            indexHtml += htmlDictionary["br2"];
            indexHtml += htmlDictionary["toc"];
            indexHtml += htmlDictionary["br3"];
            indexHtml += htmlDictionary["executiveSummary"];
            indexHtml += htmlDictionary["purpose"];
            indexHtml += htmlDictionary["objective"];
            indexHtml += htmlDictionary["overView"];
            indexHtml += htmlDictionary["br4"];
            indexHtml += htmlDictionary["testEnvironment"];
            indexHtml += htmlDictionary["targetServers"];
            indexHtml += htmlDictionary["appserver"];
            indexHtml += htmlDictionary["webServers"];
            indexHtml += htmlDictionary["dbServers"];
            indexHtml += htmlDictionary["otherservers"];
            indexHtml += htmlDictionary["deploymentDiagramText"];
            indexHtml += htmlDictionary["deploymentDiagramImg"];
            indexHtml += htmlDictionary["loadgenerators"];
            indexHtml += htmlDictionary["testResults"];
            
            indexHtml += htmlDictionary["testConditionStart"];
            htmlDictionary["testCondTarget"] = htmlDictionary["testCondTarget"].Replace("XX",_Scenario.TotalNumberOfUsers.ToString());
            indexHtml += htmlDictionary["testCondTarget"];

            htmlDictionary["testCondDuration"] = htmlDictionary["testCondDuration"].Replace("HH",(_Scenario.ExecutionTime/60).ToString());
            htmlDictionary["testCondDuration"] = htmlDictionary["testCondDuration"].Replace("MM", (_Scenario.ExecutionTime % 60).ToString());
            indexHtml += htmlDictionary["testCondDuration"];

            htmlDictionary["testCondRampUp"] = htmlDictionary["testCondRampUp"].Replace("UU",_Scenario.rampUserStep.ToString());
            htmlDictionary["testCondRampUp"] = htmlDictionary["testCondRampUp"].Replace("SS", _Scenario.rampTimeStep.ToString());
            htmlDictionary["testCondRampUp"] = htmlDictionary["testCondRampUp"].Replace("MM", _Scenario.rampUpTime.ToString());
            indexHtml += htmlDictionary["testCondRampUp"];

            htmlDictionary["testCondRampDown"] = htmlDictionary["testCondRampDown"].Replace("UU", _Scenario.rampUserStep.ToString());
            htmlDictionary["testCondRampDown"] = htmlDictionary["testCondRampDown"].Replace("SS", _Scenario.rampTimeStep.ToString());
            htmlDictionary["testCondRampDown"] = htmlDictionary["testCondRampDown"].Replace("MM", _Scenario.rampDownTime.ToString());            
            indexHtml += htmlDictionary["testCondRampDown"];

            htmlDictionary["testCondBandWidth"] = htmlDictionary["testCondBandWidth"].Replace("WW", ArrBandwidth[_Scenario.wanEmulation]);
            indexHtml += htmlDictionary["testCondBandWidth"];

            string totalTxn = ReportsDAO.getTotalTxnCount(selectedExecutionID);
            htmlDictionary["testCondTotalTxn"] = htmlDictionary["testCondTotalTxn"].Replace("TotalTxn",totalTxn);
            indexHtml += htmlDictionary["testCondTotalTxn"];

            string totalPass = ReportsDAO.getTotalPassCount(selectedExecutionID);
            htmlDictionary["testCondTotalPass"] = htmlDictionary["testCondTotalPass"].Replace("PassedTotalTxn",totalPass);
            indexHtml += htmlDictionary["testCondTotalPass"];

            int totalFail = int.Parse(totalTxn) - int.Parse(totalPass);
            htmlDictionary["testCondTotalFail"] = htmlDictionary["testCondTotalFail"].Replace("FailedTotalTxn",totalFail.ToString());
            indexHtml += htmlDictionary["testCondTotalFail"];

            double passPercent = ((double)int.Parse(totalPass)/int.Parse(totalTxn))*100;
            htmlDictionary["testCondPassPercent"] = htmlDictionary["testCondPassPercent"].Replace("PassPercent", Math.Round(passPercent,2).ToString());
            indexHtml += htmlDictionary["testCondPassPercent"];


            indexHtml += htmlDictionary["testConditionEnd"];            

            indexHtml += htmlDictionary["loadDistributionStart"];
            indexHtml += htmlDictionary["loadDistributionHead"];

            Scripts = ReportsDAO.getScripts_For_Scenario(_Scenario);            

            for (int i = 0; i < Scripts.Count; i++)
            {
                Script _Script = Scripts[i];
                string temploadDistributionBody = htmlDictionary["loadDistributionBody"];
                temploadDistributionBody = temploadDistributionBody.Replace("TransactionName", _Script.ScriptName);
                temploadDistributionBody = temploadDistributionBody.Replace("NoofUsers", _Script.NumberOfUsers.ToString());
                temploadDistributionBody = temploadDistributionBody.Replace("AvgDelayBetnItrn", ((_Script.DelayBetweenIterationMin + _Script.DelayBetweenIterationMax) / 2).ToString());
                temploadDistributionBody = temploadDistributionBody.Replace("AvgThinkTime", ((_Script.ThinkTimeMin + _Script.ThinkTimeMax) / 2).ToString());

                indexHtml += temploadDistributionBody;
            }
            
            indexHtml += htmlDictionary["loadDistributionEnd"];

            indexHtml += htmlDictionary["respTimeMetricStart"];

            htmlDictionary["respTimeMetricBody"] = "";
            for (int i = 0; i < TransactionMetrices.Count; i++)
            {
                htmlDictionary["respTimeMetricBody"] += "<tr><td>" + TransactionMetrices[i].DisplayName + "</td><td>" + TransactionMetrices[i].RespTimeMin + "</td><td>" + TransactionMetrices[i].RespTimeMax + "</td><td>" + TransactionMetrices[i].RespTimeAvg + "</td><td>" + TransactionMetrices[i].RespTime85 + "</td><td>" + TransactionMetrices[i].RespTime90 + "</td><td>" + TransactionMetrices[i].RespTime95 + "</td></tr>";
            }            

            indexHtml += htmlDictionary["respTimeMetricBody"];
            indexHtml += htmlDictionary["respTimeMetricEnd"];
                
            //indexHtml += htmlDictionary["averageRespTimeText"];
            //indexHtml += htmlDictionary["averageRespTimeImg"];
            //indexHtml += htmlDictionary["responseTimeSeriesText"];
            //indexHtml += htmlDictionary["responseTimeSeriesImg"];

            for (int k = 0; k < graph_name_byte_list.Count; k++)
            {
                KeyValuePair<string,byte[]> key_val = graph_name_byte_list[k];
                string filename = key_val.Key.Replace(" ","-")+".gif";
                indexHtml += "<br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><br/><p><font size=\"large\">3.4 " + key_val.Key + "</font><table width=\"100%\" border=\"1\"><tr>";
                indexHtml += "<td align=center><img src=\"/Images/"+filename+"\" height=\"280\" width=\"600\"/></td></tr></table></p><br/><br/>";
            }

            indexHtml += htmlDictionary["analysisPieSeriesChartText"];
            indexHtml += htmlDictionary["analysisPieSeriesChartImg"];
            indexHtml += htmlDictionary["staticPieSeriesChartText"];
            indexHtml += htmlDictionary["staticPieSeriesChartImg"];                    

            indexHtml += htmlDictionary["appendix"];

            indexHtml += htmlDictionary["bodyEnd"];
            indexHtml += htmlDictionary["htmlEnd"];
            indexHtml = indexHtml.Replace("XYZ", App._project.ProjectName.Substring(App._project.ProjectName.IndexOf("Project - ") + "Project - ".Length));
            indexHtml = indexHtml.Replace("10-Sep-2012", DateTime.Now.ToShortDateString());
            File.WriteAllText(Location + "Templates\\" + "index.html", indexHtml);


            // HTML to DOCX conversion
            MyWebServer MWS = new MyWebServer();

            Thread.Sleep(500);
            using (WebClient client = new WebClient())
            {
                client.Headers["User-Agent"] =
                "Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0) " +
                "(compatible; MSIE 6.0; Windows NT 5.1; " +
                ".NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                // Download data.
                string html = client.DownloadString("http://localhost:5050/index.html");
                // Write values.
                string filename = Location + "Reports\\" + _Report.ReportName + ".docx";
                if (File.Exists(filename)) File.Delete(filename);
                using (MemoryStream generatedDocument = new MemoryStream())
                {
                    using (WordprocessingDocument package = WordprocessingDocument.Create(generatedDocument, WordprocessingDocumentType.Document))
                    {
                        MainDocumentPart mainPart = package.MainDocumentPart;
                        if (mainPart == null)
                        {
                            mainPart = package.AddMainDocumentPart();
                            new Document(new Body()).Save(mainPart);
                        }
                        HtmlConverter converter = new HtmlConverter(mainPart);
                        converter.BaseImageUrl = new Uri("http://localhost:5050");
                        Body body = mainPart.Document.Body;

                        var paragraphs = converter.Parse(html);
                        for (int i = 0; i < paragraphs.Count; i++)
                        {
                            body.Append(paragraphs[i]);
                        }
                        mainPart.Document.Save();
                    }
                    File.WriteAllBytes(filename, generatedDocument.ToArray());
                }
                ReportsStudio.copiedLocation = filename;
                MWS.StopListen();
            }            
        }
      
        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            //saveAsImageGIF(_avgeRespTimeChart, Location + "\\Templates\\Images\\averageRespChartColumns");
            //saveAsImageGIF(responseTimeSeriesChart, Location + "\\Templates\\Images\\responseTimeSeriesChart");
            saveAsImageGIF(analysisPieSeriesChart, Location + "\\Templates\\Images\\analysisPieSeriesChart");
            saveAsImageGIF(staticPieSeriesChart, Location + "\\Templates\\Images\\staticPieSeriesChart");
            generate_from_HTMLReport();            
            ((Window)this.Parent).Close();
        }
        private void saveAsImageGIF(Chart thisChart, string filePath)
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

            using (FileStream outStream = new FileStream(filePath + ".gif", FileMode.Create))
            {
                GifBitmapEncoder encoder = new GifBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                encoder.Save(outStream);
            }
        }        
    }    
}
