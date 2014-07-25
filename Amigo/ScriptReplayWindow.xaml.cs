using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Collections;
using System.Net;
using Amigo.ViewModels;
using System.Threading;
using System.Reflection;
using System.ComponentModel;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using Amigo.Utils;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for ScriptReplayWindow.xaml
    /// </summary>
    public partial class ScriptReplayWindow : UserControl
    {
        ArrayList request_ids;
        List <Dictionary<string, StackPanel>> _list_status_code = new List<Dictionary<string, StackPanel>>();
        private static string navigation_string = "";
        Dictionary<double, string> _request_contextPath_pairs = new Dictionary<double, string>();
        ArrayList global_context_paths = new ArrayList();
        ArrayList valid_request_ids = new ArrayList();
        List<Dictionary<string, HttpWebRequest>> _HttpWebRequestList = null;
        Dictionary<string, List<Dictionary<string, string>>> List_param_dictionary_for_all_req = new Dictionary<string, List<Dictionary<string, string>>>();
        Dictionary<string, string> responses = null;
        static int private_recording_session_id;
        private DateTime downTime;
        private object downSender;
        public static int total_iterations = 1;
        BackgroundWorker worker;
        ProgressDialog pd;
        public static Status _status = new Status();
        public static bool yummyCookiesEnabled = true;
        public static bool preview_browser_IsVisible = true;
        public ScriptReplayWindow(int _recording_session_id)
        {            
            InitializeComponent();
            this.DataContext = _status;
            private_recording_session_id = _recording_session_id;
            recording_session_name.Content = Amigo.App.recordingSessionName;
            populateScriptTree(_recording_session_id);
            replay_window.SizeChanged += new SizeChangedEventHandler(replay_window_SizeChanged);             
            preview_browser.Navigated += new NavigatedEventHandler(preview_browser_Navigated);            
        }        
        void preview_browser_Navigated(object sender, NavigationEventArgs e)
        {
            SuppressScriptErrors(preview_browser, true);
            Thread.Sleep(100);
        }
        void replay_window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            preview_browser.Height = this.ActualHeight - 0.4 * (this.ActualHeight);
            log_text_box.Height = preview_browser.Height * 0.45;
        }
        private void populateScriptTree(int _recording_session_id)
        {
            try
            {
                TreeViewItem _item = verify_script_tree;
                TreeViewItem _static_tree_item = new TreeViewItem();

                if (true)
                {
                    int _session_id = _recording_session_id;
                    int itemCount = _item.Items.Count;
                    for (int rem = 0; rem < itemCount; rem++)
                    {
                        _item.Items.RemoveAt(0);
                    }

                    request_ids = DBProcessingLayer.getAllRequestIDs_for_Sessions(_session_id);

                    TreeViewItem titleItem = null;

                    for (int i = 0; i < request_ids.Count; i++)
                    {
                        string context_path = "";
                        double req = (double)request_ids[i];
                        ArrayList paths = DBProcessingLayer.getAllPaths_for_Request_ID(req);
                        for (int j = 0; j < paths.Count; j++)
                        {
                            context_path += "/" + paths[j];
                        }

                        context_path = context_path.Replace("//", "/");

                        if (paths.Count > 0)
                        {
                            Image _image = new Image();
                            Image _http_status_image = new Image();

                            Label _hidden_id = new Label();
                            _hidden_id.Visibility = System.Windows.Visibility.Hidden;
                            _hidden_id.Content = "" + req;

                            TextBlock _text_httpStatusCode = new TextBlock();
                            _text_httpStatusCode.Visibility = System.Windows.Visibility.Hidden;                            

                            _image.Height = 25;
                            _image.Width = 25;

                            _http_status_image.Height = 20;
                            _http_status_image.Width = 20;
                            

                            bool isStatic = false;

                            _request_contextPath_pairs.Add(double.Parse(req.ToString()), context_path);

                            if (context_path.EndsWith(".png"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/png.png", UriKind.Relative));
                                isStatic = true;
                            }
                            else if (context_path.EndsWith(".jpg") || context_path.EndsWith(".jpeg"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/jpeg.png", UriKind.Relative));
                                isStatic = true;
                            }
                            else if (context_path.EndsWith(".ico"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/ico.png", UriKind.Relative));
                                isStatic = true;
                            }
                            else if (context_path.EndsWith(".bmp"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/bmp.png", UriKind.Relative));
                                isStatic = true;
                            }
                            else if (context_path.EndsWith(".gif"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/gif.png", UriKind.Relative));
                                isStatic = true;
                            }
                            else if (context_path.Equals("/"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/browser.png", UriKind.Relative));
                                isStatic = false;
                            }
                            else if (context_path.EndsWith(".js"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/js.png", UriKind.Relative));
                                isStatic = true;
                            }
                            else if (context_path.EndsWith(".css"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/css.png", UriKind.Relative));
                                isStatic = true;
                            }
                            else
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/browser.png", UriKind.Relative));
                                isStatic = false;
                            }

                            if (!isStatic)
                            {
                                _image.Height = 16;
                                _image.Width = 16;
                            }

                            Label _label = new Label();
                            _label.Foreground = new SolidColorBrush(Colors.White);

                            string[] _host_dtls = DBProcessingLayer.getReferer_for_RequestID("" + req);
                            string _context_path_lbl = _host_dtls[0] + ":" + _host_dtls[1] + context_path;

                            _label.Content = _context_path_lbl;

                            StackPanel _stackPanel = new StackPanel();
                            _stackPanel.Orientation = Orientation.Horizontal;

                            _http_status_image.Name = "ststus_image_" + req.ToString().Replace('.','_');
                            _http_status_image.ToolTip = isStatic.ToString();
                            _text_httpStatusCode.Name = "ststus_lable_" + req.ToString().Replace('.', '_');
                            
                            _stackPanel.Children.Add(_http_status_image);
                            _stackPanel.Children.Add(_text_httpStatusCode);
                            _stackPanel.Children.Add(_image);
                            _stackPanel.Children.Add(_label);
                            _stackPanel.Children.Add(_hidden_id);

                            TreeViewItem _new_item = new TreeViewItem();
                            _new_item.Foreground = new SolidColorBrush(Colors.White);

                            String title = "" + DBProcessingLayer.getTitle_for_request_ID(request_ids[i].ToString());

                            if (title.Length > 0)
                            {
                                titleItem = new TreeViewItem();
                                StackPanel titleStackPanel = new StackPanel();
                                titleStackPanel.Orientation = Orientation.Horizontal;
                                titleStackPanel.Height = 25;
                                Image _title_image = new Image();
                                _title_image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/web_page.png", UriKind.Relative));
                                _title_image.Height = 20;
                                _title_image.Width = 20;
                                EditableTextBlock _title_lable = new EditableTextBlock();
                                _title_lable.Foreground = new SolidColorBrush(Colors.White);
                                _title_lable.FontSize = 12;
                                _title_lable.Text = title;

                                titleStackPanel.Children.Add(_title_image);
                                titleStackPanel.Children.Add(_title_lable);

                                titleItem.Header = titleStackPanel;
                                titleItem.ToolTip = "Tripple click to rename";
                            }

                            if (titleItem != null)
                            {
                                _new_item.Header = _stackPanel;
                                titleItem.Items.Add(_new_item);
                                titleItem.IsExpanded = true;
                                if (!_item.Items.Contains(titleItem))
                                {
                                    _item.Items.Add(titleItem);
                                }
                            }

                            valid_request_ids.Add(req.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }
        double double_current_request_id = 0;
        private void test_now_button_Click(object sender, RoutedEventArgs e)
        {
            total_iterations = Int32.Parse(iteration_text_box.Text);

            _status.iteration_text_box_is_read_only = true;
            _status.test_now_button_status = false;
            _status.stop_now_button_status = true;

            Amigo.App.http_status_code = "";
            Amigo.App.http_response = "";
            Amigo.App.http_status_message = "";

            clearPreviousResults();
            prepareScripts();
            int currItr = 0;
            
            pd = new ProgressDialog();
            pd.Cancel += CancelProcess;

            System.Windows.Threading.Dispatcher pdDispatcher = pd.Dispatcher;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;

            worker.DoWork += executeScript;
            worker.ProgressChanged += delegate(object s, ProgressChangedEventArgs args)
            {
                string current_request_id = args.ProgressPercentage.ToString();
                if (current_request_id.Equals("-1"))
                {
                    currItr++;
                    ItrProgressBar.Value = (1 - ((Double)total_iterations - (Double)currItr) / (Double)total_iterations) * 100;
                    itrNumber.Content = "Processing Iteration: " + currItr.ToString();
                }
                else
                {
                    string http_status_code = Amigo.App.http_status_code;
                    TextBlock foundTextBlock = null;
                    Image foundImage = null;
                    
                    log_text_box.Text = log_text_box.Text + "httpStatusCode=" + Amigo.App.http_status_code + "\n message =" + Amigo.App.http_status_message + "\n\n";
                    log_text_box.ScrollToEnd();
                    try
                    {
                        foundTextBlock = FindChild<TextBlock>(script_root_tree1, "ststus_lable_" + current_request_id.Replace('.', '_'));
                        if (foundTextBlock != null)
                        {
                            foundTextBlock.Visibility = System.Windows.Visibility.Collapsed;
                            foundTextBlock.Text = "httpStatusCode=" + Amigo.App.http_status_code + "\n message =" + Amigo.App.http_status_message + "\n\n";
                        }

                        foundImage = FindChild<Image>(script_root_tree1, "ststus_image_" + current_request_id.Replace('.','_'));

                        bool isStatic = false;

                        if (foundImage != null)
                        {
                            foundImage.MouseDown += new MouseButtonEventHandler(foundImage_MouseDown);
                            foundImage.MouseUp += new MouseButtonEventHandler(foundImage_MouseUp);
                            isStatic = Boolean.Parse(foundImage.ToolTip.ToString());

                            if (Amigo.App.http_response != null && !("".Equals(Amigo.App.http_response)) && !isStatic)
                            {
                                preview_browser.NavigateToString(Amigo.App.http_response);
                            }

                            StackPanel _select_stack_panel = (StackPanel)foundImage.Parent;
                            TreeViewItem _selectTree_Item = (TreeViewItem)_select_stack_panel.Parent;

                            _selectTree_Item.IsSelected = true;
                            _selectTree_Item.Selected += new RoutedEventHandler(_selectTree_Item_Selected);

                            if (http_status_code.Equals("200") || http_status_code.Equals("304") || http_status_code.Equals("0"))
                            {
                                foundImage.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/200.png", UriKind.Relative));
                            }
                            else if (http_status_code.Equals("204"))
                            {
                                foundImage.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/200.png", UriKind.Relative));
                            }
                            else
                            {
                                foundImage.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/http_error.png", UriKind.Relative));
                            }
                        }
                    }
                    catch (Exception) { }
                }
            };
            worker.RunWorkerAsync();
        }
        void _selectTree_Item_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                TreeViewItem _selected_item = (TreeViewItem)e.Source;                
                StackPanel sp = (StackPanel)_selected_item.Header;
                Label ll = (Label)sp.Children[4];
                Label file_name = (Label)sp.Children[3];
                string extn_name = file_name.Content.ToString();
                string req = ll.Content.ToString();
                extn_name = extn_name.Substring(extn_name.LastIndexOf('.')+1);
                if (responses.ContainsKey(req) && responses[req] != null && responses[req].Length > 0) preview_browser.NavigateToString(responses[req]);
            }
            catch (Exception exc) { Console.WriteLine(exc.Message); }
        }
        void CancelProcess(object sender, EventArgs e)
        {
            worker.CancelAsync();
        }
        public bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        } 
        private void executeScript(object sender, DoWorkEventArgs doWorkEventArgs)
        {

            //WebProxy proxyForPost = WebProxy.GetDefaultProxy();            
            //ProxyProgram.ProxyStart();
                            
        
            for (int itr = 0; itr < total_iterations; itr++)
            {
                if (worker.CancellationPending)
                {
                    doWorkEventArgs.Cancel = true;
                    break;
                }
                worker.ReportProgress(-1);
                string request_id = "";
                string message = "";
                responses = new Dictionary<string, string>();
                string previous_response = "";
                CookieContainer cookieJar = new CookieContainer();
                List<string> redirect_url_list = new List<string>();

                for (int requestCounter = 0; requestCounter < _HttpWebRequestList.Count && preview_browser.IsVisible; requestCounter++)
                {
                    try
                    {
                        ////check if cancell called - start
                        if (worker.CancellationPending)
                        {
                            doWorkEventArgs.Cancel = true;
                            break;
                        }
                        Thread.Sleep(Amigo.App.speed);
                        ////check if cancell called - end

                        ////get a request id from the list
                        request_id = (string)valid_request_ids[requestCounter];

                        for (int counter = 0; counter < _HttpWebRequestList.Count; counter++)
                        {
                            HttpWebRequest _HttpWebRequest = null;
                            bool usingNTLM = false;
                            Dictionary<string, HttpWebRequest> _dictionary = _HttpWebRequestList[counter];
                            try
                            {                                
                                ////----------find the HttpWebRequest for request id-- end
                                if (_dictionary.ContainsKey(request_id))
                                {
                                    _HttpWebRequest = (HttpWebRequest)_dictionary[request_id];
                                    counter = _HttpWebRequestList.Count;
                                }
                                else { continue; }
                                ////----------find the HttpWebRequest for request id-- end

                                if (_HttpWebRequest != null)
                                {
                                    List<Dictionary<string, string>> parameters = List_param_dictionary_for_all_req[request_id];
                                    Dictionary<string, string>[] param_dicts = (Dictionary<string, string>[])parameters.ToArray();
                                    string keyValues = "";

                                    for (int i = 0; i < param_dicts.Length; i++)
                                    {
                                        if (param_dicts[i]["ParamName"] == "NTLMuserName" || param_dicts[i]["ParamName"] == "NTLMpassword" || param_dicts[i]["ParamName"] == "NTLMDomain")
                                        {
                                            continue;
                                        }
                                        if (("" + param_dicts[i]["SelectedCSVColumnName"]).Length > 0)
                                        {                                            
                                            string CSVfilePath = param_dicts[i]["ParamValue"];
                                            string CSVColumn = param_dicts[i]["SelectedCSVColumnName"];
                                            string paramValue = getCSVCellValue(CSVfilePath, CSVColumn, itr);
                                            keyValues = keyValues + param_dicts[i]["ParamName"] + "=" + paramValue.Trim();
                                        }
                                        else if (("" + param_dicts[i]["ParameterizationSource"]) == ParamSources.AutoCorrelation.ToString())
                                        {
                                            string LB = param_dicts[i]["LB"];
                                            string RB = param_dicts[i]["RB"];
                                            string capture = "";

                                            Regex r = new Regex(Regex.Escape(LB) + "(.*?)" + Regex.Escape(RB));
                                            MatchCollection matches = r.Matches(previous_response);
                                            for (int ii = 0; ii < matches.Count; ii++)
                                            {
                                                if (capture.Length == 0)
                                                {
                                                    capture = matches[ii].Groups[1].Value;
                                                }
                                                else
                                                {
                                                    if (matches[ii].Value.Length < capture.Length)
                                                    {
                                                        capture = matches[ii].Groups[1].Value;
                                                    }
                                                }
                                            }
                                            keyValues = keyValues + param_dicts[i]["ParamName"] + "=" + capture;
                                        }
                                        else
                                        {
                                            keyValues = keyValues + param_dicts[i]["ParamName"] + "=" + param_dicts[i]["ParamValue"];
                                        }
                                        
                                        if (i != param_dicts.Length - 1)
                                        { keyValues = keyValues + "&"; }
                                    }

                                    if (keyValues.Length > 0) { keyValues = "?" + keyValues; }

                                    List<Dictionary<string, string>> header_list = ScriptScenarioDAO.getAllHeaders_for_request_ID(request_id);

                                    for (int i = 0; i < header_list.Count; i++)
                                    {
                                        Dictionary<string, string> _dict = header_list[i];
                                        string key = _dict["HeaderKey"];

                                        try
                                        {
                                            if ("Accept".Equals(key)) _HttpWebRequest.Accept = _dict["HeaderValue"];
                                            else if ("Referer".Equals(key)) _HttpWebRequest.Referer = _dict["HeaderValue"];
                                            else if ("User-Agent".Equals(key)) _HttpWebRequest.UserAgent = _dict["HeaderValue"];
                                            else if ("Content-Type".Equals(key)) _HttpWebRequest.ContentType = _dict["HeaderValue"];
                                            else if ("Connection".Equals(key)) continue;
                                            else if ("Accept-Language".Equals(key)) _HttpWebRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, _dict["HeaderValue"]);
                                            else if ("Accept-Encoding".Equals(key)) _HttpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, _dict["HeaderValue"]);
                                            else if ("Host".Equals(key)){}// _HttpWebRequest.Host = _dict["HeaderValue"];
                                            else if ("Content-Length".Equals(key)) _HttpWebRequest.ContentLength = Int32.Parse(_dict["HeaderValue"]);
                                            else if ("Cookie".Equals(key))
                                            {
                                                string cookieString = _dict["HeaderValue"];
                                                cookieJar.SetCookies(_HttpWebRequest.RequestUri, cookieString);
                                                string[] arrcookies = cookieString.Split(';');
                                                for (int co = 0; co < arrcookies.Length; co++)
                                                {
                                                    try
                                                    {
                                                        int indexOfEqual = arrcookies[co].IndexOf('=');
                                                        string name = arrcookies[co].Substring(0, indexOfEqual).Trim();
                                                        string value = arrcookies[co].Substring(indexOfEqual + 1).Trim();
                                                        Cookie cookie = new Cookie(name, value);
                                                        cookie.Domain = _HttpWebRequest.RequestUri.Host;
                                                        if (yummyCookiesEnabled)
                                                        {
                                                            cookieJar.Add(cookie);
                                                        }
                                                    }
                                                    catch (CookieException) { }
                                                }
                                            }
                                            else if ("If-Modified-Since".Equals(key)) { }
                                            else if ("Proxy-Connection".Equals(key)) { }
                                            else if ("Authorization".Equals(key)) 
                                            {
                                                if (_dict["HeaderValue"].Contains("NTLM"))
                                                {
                                                    usingNTLM = true;
                                                    _HttpWebRequest.Headers.Add(_dict["HeaderKey"], _dict["HeaderValue"]);
                                                }
                                            }
                                            else _HttpWebRequest.Headers.Add(_dict["HeaderKey"], _dict["HeaderValue"]);
                                        }
                                        catch (Exception) { }
                                    }

                                    message = "Server=" + _HttpWebRequest.RequestUri + "\nParameters=\n" + keyValues + "\n\nRequest Headers=" +
                                                         _HttpWebRequest.Headers.ToString() + "\n\nActual Response=";

                                    _HttpWebRequest.AllowAutoRedirect = false;
                                    _HttpWebRequest.CookieContainer = cookieJar;
                                    _HttpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
                                    _HttpWebRequest.Expect = null;
                                    if (usingNTLM)
                                    {
                                        string userName = "";
                                        string password = "";
                                        string domain = "";
                                        
                                        for(int jj = 0 ; jj < param_dicts.Length ; jj++)
                                        {
                                            if(param_dicts[jj]["ParamName"] == "NTLMuserName") userName = param_dicts[jj]["ParamValue"];
                                            if (param_dicts[jj]["ParamName"] == "NTLMpassword") password = param_dicts[jj]["ParamValue"];
                                            if (param_dicts[jj]["ParamName"] == "NTLMDomain") domain = param_dicts[jj]["ParamValue"];
                                        }                                                                                
                                        _HttpWebRequest.Credentials = new NetworkCredential(userName,password,domain);
                                    }
                                    else
                                    {
                                        _HttpWebRequest.UseDefaultCredentials = true;
                                    }
                                    CookieCollection _CookieCollection = cookieJar.GetCookies(_HttpWebRequest.RequestUri);

                                    for (int coo = 0; coo < _CookieCollection.Count; coo++)
                                    {
                                        Cookie _cookie = _CookieCollection[coo];
                                        if (keyValues.Contains(_cookie.Name + "="))
                                        {
                                            int cookiePos = keyValues.IndexOf(_cookie.Name + "=");
                                            int posStart = cookiePos;
                                            int posStart2 = keyValues.IndexOf('=', posStart + 1);
                                            int lastPos = keyValues.IndexOf('&', posStart);

                                            if (posStart2 < lastPos) posStart = posStart2;

                                            string subCookie = keyValues.Substring(posStart + 1, lastPos - posStart - 1);
                                            if (subCookie.Length > 0 && _cookie.Value.Length > 0)
                                            {
                                                keyValues = keyValues.Replace(subCookie, _cookie.Value);
                                            }
                                        }
                                    }

                                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                                    
                                    try
                                    {
                                        HttpWebResponse webresponse = null;
                                        int http_status_code = 0;
                                        if (_HttpWebRequest.Method == "POST" || _HttpWebRequest.Method == "post")
                                        {
                                            if (keyValues.Length > 0)
                                            keyValues = keyValues.Remove(0, 1);

                                            if (keyValues.StartsWith("SOAPBody="))
                                            {
                                                keyValues = keyValues.Replace("SOAPBody=", "");
                                                if (keyValues.EndsWith("&"))
                                                keyValues = keyValues.Remove(keyValues.Length-1,1);
                                            }

                                            _HttpWebRequest.CookieContainer = cookieJar;

                                            bool isRedirectExists = false;

                                            for (int red = 0; red < redirect_url_list.Count; red++)
                                            {
                                                if (redirect_url_list[red].Contains(_HttpWebRequest.RequestUri.LocalPath))
                                                {
                                                    isRedirectExists = true;
                                                    redirect_url_list[red] = "";
                                                    break;
                                                }
                                            }

                                            if (isRedirectExists)
                                            {
                                                isRedirectExists = false;
                                                goto endOFLoop;
                                            }

                                            _HttpWebRequest.ContentLength = keyValues.Length;
                                            StreamWriter newStream = new StreamWriter(_HttpWebRequest.GetRequestStream());
                                            newStream.Write(keyValues, 0, keyValues.Length);
                                            newStream.Close();
                                            webresponse = (HttpWebResponse)_HttpWebRequest.GetResponse();
                                            cookieJar = FixCookies(cookieJar, webresponse.Cookies, _HttpWebRequest.RequestUri);

                                            if (webresponse.StatusCode == HttpStatusCode.Redirect || webresponse.StatusCode == HttpStatusCode.MovedPermanently)
                                            {
                                            CheckRedirect:

                                                string newUrl = webresponse.Headers["Location"];

                                                redirect_url_list.Add(newUrl);

                                                _HttpWebRequest = (HttpWebRequest)WebRequest.Create(newUrl);
                                                _HttpWebRequest.CookieContainer = cookieJar;
                                                webresponse = (HttpWebResponse)_HttpWebRequest.GetResponse();

                                                cookieJar = FixCookies(cookieJar, webresponse.Cookies, _HttpWebRequest.RequestUri);

                                                if (webresponse.StatusCode == HttpStatusCode.Redirect || webresponse.StatusCode == HttpStatusCode.MovedPermanently)
                                                {
                                                    goto CheckRedirect;
                                                }
                                            }
                                            http_status_code = (int)webresponse.StatusCode;
                                            StreamReader reader = new StreamReader(webresponse.GetResponseStream());
                                            navigation_string = reader.ReadToEnd();

                                            if (webresponse.ContentType.Contains("json"))
                                            {
                                                navigation_string = "<html><body><h3><I><font color='GREEN'>JSON Objects returned from server. This is success but cant display properly in this browser</font></I></h3></body></html>" + navigation_string;
                                            }

                                            reader.Close();
                                            newStream.Close();
                                            webresponse.Close();
                                        }
                                        else if (_HttpWebRequest.Method == "GET" || _HttpWebRequest.Method == "get")
                                        {
                                            _HttpWebRequest = (HttpWebRequest)WebRequest.Create(_HttpWebRequest.RequestUri + keyValues);
                                            _HttpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;

                                            for (int i = 0; i < header_list.Count; i++)
                                            {
                                                Dictionary<string, string> _dict = header_list[i];
                                                string key = _dict["HeaderKey"];

                                                try
                                                {
                                                    if ("Accept".Equals(key)) _HttpWebRequest.Accept = _dict["HeaderValue"];
                                                    else if ("Referer".Equals(key)) _HttpWebRequest.Referer = _dict["HeaderValue"];
                                                    else if ("User-Agent".Equals(key)) _HttpWebRequest.UserAgent = _dict["HeaderValue"];
                                                    else if ("Content-Type".Equals(key)) _HttpWebRequest.ContentType = _dict["HeaderValue"];
                                                    else if ("Connection".Equals(key)) continue;
                                                    else if ("Accept-Language".Equals(key)) _HttpWebRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, _dict["HeaderValue"]);
                                                    else if ("Accept-Encoding".Equals(key)) _HttpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, _dict["HeaderValue"]);
                                                    else if ("Host".Equals(key)) {}//_HttpWebRequest.Host = _dict["HeaderValue"];
                                                    else if ("Content-Length".Equals(key)) { }
                                                    else if ("If-Modified-Since".Equals(key)) { }
                                                    else if ("Authorization".Equals(key))
                                                    {
                                                        if (_dict["HeaderValue"].Contains("NTLM"))
                                                        {
                                                            usingNTLM = true;
                                                            _HttpWebRequest.Headers.Add(_dict["HeaderKey"], _dict["HeaderValue"]);
                                                        }
                                                    }
                                                    else _HttpWebRequest.Headers.Add(_dict["HeaderKey"], _dict["HeaderValue"]);
                                                }
                                                catch (Exception) { }
                                            }

                                            _HttpWebRequest.CookieContainer = cookieJar;

                                            bool isRedirectExists = false;

                                            for (int red = 0; red < redirect_url_list.Count; red++)
                                            {
                                                if (redirect_url_list[red].Contains(_HttpWebRequest.RequestUri.LocalPath))
                                                {
                                                    isRedirectExists = true;
                                                    redirect_url_list[red] = "";
                                                    break;
                                                }
                                            }

                                            if (isRedirectExists)
                                            {
                                                isRedirectExists = false;
                                                goto endOFLoop;
                                            }

                                            try
                                            {
                                                _HttpWebRequest.KeepAlive = false;
                                                if (usingNTLM)
                                                {
                                                    string userName = "";
                                                    string password = "";
                                                    string domain = "";

                                                    for (int jj = 0; jj < param_dicts.Length; jj++)
                                                    {
                                                        if (param_dicts[jj]["ParamName"] == "NTLMuserName") userName = param_dicts[jj]["ParamValue"];
                                                        if (param_dicts[jj]["ParamName"] == "NTLMpassword") password = param_dicts[jj]["ParamValue"];
                                                        if (param_dicts[jj]["ParamName"] == "NTLMDomain") domain = param_dicts[jj]["ParamValue"];
                                                    }
                                                    _HttpWebRequest.Credentials = new NetworkCredential(userName, password, domain);
                                                }
                                                else
                                                {
                                                    _HttpWebRequest.UseDefaultCredentials = true;
                                                }                                                                                           
                                                webresponse = (HttpWebResponse)_HttpWebRequest.GetResponse();
                                            }
                                            catch (ProtocolViolationException) { }

                                            cookieJar = FixCookies(cookieJar, webresponse.Cookies, _HttpWebRequest.RequestUri);

                                            _HttpWebRequest.AllowAutoRedirect = false;
                                            if (webresponse != null)
                                            {
                                                if (webresponse.StatusCode == HttpStatusCode.Redirect || webresponse.StatusCode == HttpStatusCode.MovedPermanently)
                                                {
                                                CheckRedirectGet:

                                                    string newUrl = webresponse.Headers["Location"];

                                                    redirect_url_list.Add(newUrl);
                                                    _HttpWebRequest = (HttpWebRequest)WebRequest.Create(newUrl);
                                                    _HttpWebRequest.CookieContainer = cookieJar;
                                                    webresponse.Close();
                                                    webresponse = (HttpWebResponse)_HttpWebRequest.GetResponse();

                                                    cookieJar = FixCookies(cookieJar, webresponse.Cookies, _HttpWebRequest.RequestUri);

                                                    if (webresponse.StatusCode == HttpStatusCode.Redirect || webresponse.StatusCode == HttpStatusCode.MovedPermanently)
                                                    {
                                                        goto CheckRedirectGet;
                                                    }
                                                }
                                                http_status_code = (int)webresponse.StatusCode;

                                                using (var streamReader = new StreamReader(webresponse.GetResponseStream()))
                                                {
                                                    navigation_string = streamReader.ReadToEnd();
                                                    if (webresponse.ContentType.Contains("json"))
                                                    {
                                                        navigation_string = "<html><body><h3><I><font color='GREEN'>JSON Objects returned from server. This is success but cant display properly in this browser</font></I></h3></body></html>" + navigation_string;
                                                    }
                                                }
                                            }
                                            
                                            webresponse.Close();
                                        }
                                    endOFLoop:
                                        Amigo.App.http_response = navigation_string;
                                        responses.Add(request_id, navigation_string);
                                        setStatusCodeToTreeAmigo(http_status_code.ToString(), message + navigation_string);

                                        if (navigation_string.Contains("title"))
                                        {
                                            previous_response = navigation_string;
                                        }
                                    }
                                    catch (CookieException cx) { Console.Write(cx.Message); }
                                    catch (WebException ex)
                                    {
                                        if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                                        {
                                            var resp = (HttpWebResponse)ex.Response;
                                            string respString = "\nStatus Description:" + resp.StatusDescription + " for URI:" + resp.ResponseUri + "\nFull Error:" + ex.StackTrace;
                                            if (resp.StatusCode == HttpStatusCode.NotModified) // HTTP 404 
                                            {
                                                respString += "\nStatus Code:" + "HTTP 304";
                                                setStatusCodeToTreeAmigo("304", message + respString + "\n");
                                            }
                                            else if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404 
                                            {
                                                respString += "\nStatus Code:" + "HTTP 404";
                                                setStatusCodeToTreeAmigo("404", message + respString + "\n");
                                            }
                                            else if (resp.StatusCode == HttpStatusCode.ProxyAuthenticationRequired) // HTTP 407 
                                            {
                                                respString += "\nStatus Code:" + "HTTP 407";
                                                setStatusCodeToTreeAmigo("404", message + respString + "\n");
                                            }
                                            else if (resp.StatusCode == HttpStatusCode.InternalServerError) // HTTP 500
                                            {
                                                respString += "\nStatus Code:" + "HTTP 500";
                                                setStatusCodeToTreeAmigo("500", message + respString + "\n");
                                            }
                                            else if (resp.StatusCode == HttpStatusCode.BadGateway) // HTTP 502 
                                            {
                                                respString += "\nStatus Code:" + "HTTP 502";
                                                setStatusCodeToTreeAmigo("502", message + respString + "\n");
                                            }
                                            else if (resp.StatusCode == HttpStatusCode.GatewayTimeout) // HTTP 504
                                            {
                                                respString += "\nStatus Code:" + "HTTP 504";
                                                setStatusCodeToTreeAmigo("504", message + respString + "\n");
                                            }
                                            else
                                            {
                                                respString += "\nStatus Code:" + "HTTP Generic";
                                                setStatusCodeToTreeAmigo("Generic", message + respString + "\n");
                                            }
                                        }
                                        else
                                        {
                                            var resp = (HttpWebResponse)ex.Response;
                                            string respString = "\nStatus Description:" + ex.Message + "\nFull Error:" + ex.StackTrace;
                                            setStatusCodeToTreeAmigo("Generic","Details:"+respString+ "\nResponse"+message+"\nFull err:"+ ex.StackTrace);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.StackTrace);
                                    }                                    
                                }
                            }
                            catch (Exception genEx)
                            {
                                Console.Write(genEx.StackTrace);
                            }
                            if (_HttpWebRequest != null)
                            {
                                double_current_request_id = double.Parse(request_id);
                                worker.ReportProgress(Int32.Parse(request_id));
                            }
                        }
                    }
                    catch { }
                }
            }
            _status.test_now_button_status = true;
            _status.stop_now_button_status = false;
            _status.iteration_text_box_is_read_only = false;
            ProxyProgram.DoQuit();
        }
        private string getCSVCellValue(string CSVfilePath, string CSVColumn, int itr)
        {
            int fieldIndex = -1;
            string CSVColValue = "";
            using (CsvReader csv = new CsvReader(new StreamReader(CSVfilePath), true))
            {
                string[] headers = csv.GetFieldHeaders();

                for (int i = 0; i < headers.Length; i++ )
                {
                    if (headers[i].Trim() == CSVColumn.Trim())
                    {
                        fieldIndex = i;
                    }
                }

                if (fieldIndex > -1)
                {
                    int rowNum = 0;
                    while (csv.ReadNextRecord() && rowNum <= itr)
                    {                        
                        CSVColValue = csv[fieldIndex];
                        rowNum++;
                    }
                }
            }
            Console.WriteLine("CSVColValue=" + CSVColValue);
            return CSVColValue;
        }
        private CookieContainer FixCookies(CookieContainer cookieJar, CookieCollection cookieCollection,Uri uri)
        {
            CookieContainer container = new CookieContainer();
            CookieCollection jarColln = cookieJar.GetCookies(uri);
            Cookie[] gotCookies = new Cookie[cookieCollection.Count];
            Cookie[] jarCookies = new Cookie[jarColln.Count];

            cookieCollection.CopyTo(gotCookies,0);
            jarColln.CopyTo(jarCookies,0);


            for (int i = 0; i < gotCookies.Length; i++)
            {
                for (int j = 0; j < jarCookies.Length; j++)
                {
                    Cookie gotCookie = gotCookies[i];
                    Cookie jarCookie = jarCookies[j];
                    if (gotCookie.Name == jarCookie.Name)
                    {
                        jarCookie.Value = gotCookie.Value;
                        break;
                    }
                }
            }

            for (int i = 0; i < gotCookies.Length; i++)
            {
                jarColln.Add(gotCookies[i]);
            }

            container.Add(jarColln);
            return container;
        }
        private void setStatusCodeToTreeAmigo(string http_status_code, string message)
        {            
            Amigo.App.http_status_code = http_status_code;
            Amigo.App.http_status_message = message;
        }
        private void clearPreviousResults()
        {
            for (int i = 0; i < valid_request_ids.Count; i++)
            {
                try
                {
                    TextBlock foundTextBlock = FindChild<TextBlock>(replay_window, "ststus_lable_" + ((string)valid_request_ids[i]).Replace('.', '_'));
                    if (foundTextBlock != null)
                    {
                        foundTextBlock.Visibility = System.Windows.Visibility.Collapsed;
                        foundTextBlock.Text = "";
                    }

                    Image foundImage = FindChild<Image>(replay_window, "ststus_image_" + ((string)valid_request_ids[i]).Replace('.','_'));
                    if (foundImage != null)
                    {
                        foundImage.Source = new BitmapImage();
                    }
                }
                catch { };
            }
        }         
        private void prepareScripts()
        {
            List_param_dictionary_for_all_req = new Dictionary<string, List<Dictionary<string, string>>>();
            _HttpWebRequestList = new List<Dictionary<string, HttpWebRequest>>();

            string _context_path = "";
            List<Dictionary<string, string>> _headers = new List<Dictionary<string,string>>();

            for (int i = 0; i < request_ids.Count; i++)
            {
                string[] _host_dtls = DBProcessingLayer.getReferer_for_RequestID(request_ids[i].ToString());
                string[] retval = DBProcessingLayer.getMethod_for_request_ID(request_ids[i].ToString());
                string protocol = retval[0];
                string method = retval[1];
                try
                {
                    _context_path = "";
                    if (_request_contextPath_pairs.ContainsKey(Int32.Parse((request_ids[i].ToString()))))
                    {
                        _context_path = _request_contextPath_pairs[Int32.Parse((request_ids[i].ToString()))];
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }

                if (!_context_path.Equals(""))
                   {
                    DBProcessingLayer.getAllParams_for_request_ID(request_ids[i].ToString(), "ScriptReplay");
                    NotifiableObservableCollection<Parameter> _params = Amigo.App.Parameters_For_Replay;

                    //param_dictionary
                    List<Dictionary<string, string>> _parameter_list = new List<Dictionary<string, string>>();

                    for (int j = 0; j < _params.Count; j++)
                    {
                        Dictionary<string, string> param_dict = new Dictionary<string, string>();
                        param_dict.Add("ParamName",_params[j].ParamName.ToString());
                        param_dict.Add("ParamValue",_params[j].SubstututedParamValue);
                        param_dict.Add("SelectedCSVColumnName", _params[j].SelectedCSVColumnName);
                        param_dict.Add("ParameterizationSource", _params[j].ParameterizationSource.ToString());
                        param_dict.Add("LB", _params[j].LB);
                        param_dict.Add("RB", _params[j].RB);

                        _parameter_list. Add(param_dict);
                    }

                    List_param_dictionary_for_all_req.Add(request_ids[i].ToString(),_parameter_list);

                    HttpWebRequest _single_request = null;
                    ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
                    

                    if ("POST" == method) 
                    {
                        _single_request = (HttpWebRequest)WebRequest.Create(protocol + "://" + _host_dtls[0] + ":" + _host_dtls[1] + _context_path);
                        _single_request.Method = method;
                    }
                    else if ("GET" == method)
                    {
                        _single_request = (HttpWebRequest)WebRequest.Create(protocol + "://" + _host_dtls[0] + ":" + _host_dtls[1] + _context_path);
                        _single_request.Method = method;
                    }
                                                            

                    Dictionary<string, HttpWebRequest> _dictionary_req_id_HttpRequests_Map = new Dictionary<string, HttpWebRequest>();
                    _dictionary_req_id_HttpRequests_Map.Add(request_ids[i].ToString(), _single_request);
                    _HttpWebRequestList.Add(_dictionary_req_id_HttpRequests_Map);                        
                  }
            }            
        }
        public void SuppressScriptErrors(System.Windows.Controls.WebBrowser wb, bool Hide) 
        {
            FieldInfo fi = typeof(System.Windows.Controls.WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic); 
            if (fi != null)
            {
                object browser = fi.GetValue(wb);
                if (browser != null)
                {
                    browser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, browser, new object[] { Hide });
                }
            }
        }
        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid.  
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child 
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree 
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child.  
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search 
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name 
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found. 
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }        
        void foundImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.downSender = sender;
                this.downTime = DateTime.Now;
            }
        }
        void foundImage_MouseUp(object sender, MouseButtonEventArgs e)
        {            
            if (e.LeftButton == MouseButtonState.Released && sender == this.downSender)
            {
                TimeSpan timeSinceDown = DateTime.Now - this.downTime;
                if (timeSinceDown.TotalMilliseconds < 500)
                {
                    Image _image = (Image)sender;
                    StackPanel _parent = (StackPanel)_image.Parent;
                    TextBlock _message = (TextBlock)_parent.Children[1];
                    string _message_text = _message.Text;

                    showStatusWindow(_message_text);
                }
            }
        }
        static HttpStatusWindow _HttpStatusWindow = null;        
        private void showStatusWindow(string _message_text)
        {
            _HttpStatusWindow = new HttpStatusWindow((string)_message_text);

            _HttpStatusWindow.ContextMenu = getStatusWindowContextMenu();
            _HttpStatusWindow.Height = 300;
            _HttpStatusWindow.Width = 500;
            _HttpStatusWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            _HttpStatusWindow.ShowDialog();
        }
        private System.Windows.Controls.ContextMenu getStatusWindowContextMenu()
        {
            ContextMenu Menu = new ContextMenu();

            MenuItem mt1 = new MenuItem();
            mt1.Header = "Copy all contents";
            mt1.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(@"/Amigo;component/icons/copy.png", UriKind.Relative)),
                Height = 20,
                Width = 20
            };
            mt1.Click += new RoutedEventHandler(Copy_all_status_contents);//IsInEditMode = true;
            Menu.Items.Add(mt1);
            return Menu;
        }
        private void Copy_all_status_contents(object sender, RoutedEventArgs e)
        {
            string _message_text = ((HttpStatusWindow)_HttpStatusWindow).httpErrorTextBlock.Text;
            Clipboard.Clear();
            Clipboard.SetText(_message_text);
        }
        private void stop_now_button_Click(object sender, RoutedEventArgs e)
        {
            CancelProcess(sender, e);
            _status.test_now_button_status = true;
            _status.stop_now_button_status = false;
            _status.iteration_text_box_is_read_only = true;
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (Amigo.App.speed == 500)
            {
                Amigo.App.speed = 2000;
            }
            else if (Amigo.App.speed == 2000)
            {
                Amigo.App.speed = 500;
            }
        }
        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            iteration_text_box.Text = (Int32.Parse(iteration_text_box.Text) + 1).ToString();            
        }
        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.Parse(iteration_text_box.Text) > 1)
            {
                iteration_text_box.Text = (Int32.Parse(iteration_text_box.Text) - 1).ToString();                
            }
        }
        private void Enable_Yummy_Cookies(object sender, RoutedEventArgs e)
        {            
            if (e.RoutedEvent.Name== "Checked")
            {
                yummyCookiesEnabled = true;
            }
            else 
            {
                yummyCookiesEnabled = false;
            }
        }
        private void script_root_tree_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }
    }    
}