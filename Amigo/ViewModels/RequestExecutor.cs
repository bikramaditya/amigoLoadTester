using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using System.Threading;
using Amigo.Interfaces;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Amigo.ViewModels
{
    public class RequestExecutor
    {        
        private List<KeyValuePair<double, List<Dictionary<string, string>>>> _parametersList = null;
        private List<KeyValuePair<double, List<Dictionary<string, string>>>> _headersList = null;
        private List<KeyValuePair<double, string>> _requestsList = null;
        public static List<KeyValuePair<string, int>> _CSVValuePositionList = new List<KeyValuePair<string,int>>();
        
        double parent_request_id = 0;
        int iteration_no = 0;
        double child_request = 0;
        double request_id = 0;
        int thread_id = 0;
        public static CookieContainer cookieJar = new CookieContainer();
        private string message="";
        private List<Dictionary<string, string>> parameters;
        private List<Dictionary<string, string>> header_list;

        public RequestExecutor(ThreadParameters ThreadParam)
        {
            this._headersList = ExecutionStudio.headersList;
            this._parametersList = ExecutionStudio.parametersList;
            this._requestsList = ExecutionStudio.requestsList;
            this.parent_request_id = ThreadParam.parent_request;
            this.child_request = ThreadParam.child;
            this.iteration_no = ThreadParam.itrNo;
            this.thread_id = ThreadParam.threadID;
        }
        public void executeRequest()
        {            
            request_id = getRequestID();
            HttpWebRequest _HttpWebRequest = null; 
            List<string> redirect_url_list = new List<string>();
            string navigation_string = "";

            Stopwatch stopwatch = new Stopwatch();
            double response_time= 0 ;
            DateTime startTime = new DateTime();            
            DateTime endTime = new DateTime();
            double response_size = 0;
            double request_size = 0;
            string content_type = "";
            string previous_response = "";
            bool usingNTLM = false;

            //------------------------------------------parameters--------------------------------------//

            for (int i = 0; i < _parametersList.Count; i++)
            {
                KeyValuePair<double, List<Dictionary<string, string>>> param_key_val = _parametersList[i];
                if (request_id == param_key_val.Key)
                {
                    parameters = param_key_val.Value;
                    break;
                }
            }


            Dictionary<string, string>[] param_dicts = null;
            if(parameters !=null) param_dicts = (Dictionary<string, string>[])parameters.ToArray();            

            string keyValues = "";

            for (int i = 0; param_dicts != null && i < param_dicts.Length; i++)
            {
                if (param_dicts[i]["ParamName"] == "NTLMuserName" || param_dicts[i]["ParamName"] == "NTLMpassword" || param_dicts[i]["ParamName"] == "NTLMDomain")
                {
                    continue;
                }
                if (("" + param_dicts[i]["SelectedCSVColumnName"]).Length > 0)
                {
                    string CSVfilePath = param_dicts[i]["ParamValue"];
                    string CSVColumn = param_dicts[i]["SelectedCSVColumnName"];
                    string paramValue = getCSVCellValue(CSVfilePath, CSVColumn, thread_id);
                    keyValues = keyValues + param_dicts[i]["ParamName"] + "=" + paramValue.Trim();
                }
                else if (("" + param_dicts[i]["ParameterizationSource"]) == ParamSources.AutoCorrelation.ToString())
                {
                    string LB = param_dicts[i]["LB"];
                    string RB = param_dicts[i]["RB"];
                    string capture = "";

                    Regex r = new Regex(Regex.Escape(LB) + "(.*?)" + Regex.Escape(RB));
                    previous_response = ExecutionStudio.responses_for_correlation[iteration_no];

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



            ////---------------------create the web request--------------------------------------------------------------------------
            //retrieve method and protocol
            string[] retval = DBProcessingLayer.getMethod_for_request_ID(request_id.ToString());
            string protocol = retval[0];
            string method = retval[1];
            string fullURL = "";
            for (int i = 0; i < _requestsList.Count; i++)
            {
                KeyValuePair<double, string> req_key_val_pair = _requestsList[i];
                if (req_key_val_pair.Key == request_id)
                {
                    fullURL = req_key_val_pair.Value;
                }
            }

            _HttpWebRequest = (HttpWebRequest)WebRequest.Create(protocol + "://" + fullURL);
            _HttpWebRequest.Method = method;
            



            ////------------------------------------------update with latest cookies-------------------------------------------------

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

            ////--------------------------supress https certificate errors---------------------------------
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);




            ////------------------------------------start execution-------------------------------------
            try
            {
                _HttpWebRequest.UseDefaultCredentials = true;
                _HttpWebRequest.AllowAutoRedirect = false;
                _HttpWebRequest.CookieContainer = cookieJar;
                _HttpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
                _HttpWebRequest.Expect = null;

                for (int i = 0; i < _headersList.Count; i++)
                {
                    KeyValuePair<double, List<Dictionary<string, string>>> headers_key_val = _headersList[i];
                    if (request_id == headers_key_val.Key)
                    {
                        header_list = headers_key_val.Value;
                        break;
                    }
                }

                int http_status_code = 0;
                if (_HttpWebRequest.Method == "POST" || _HttpWebRequest.Method == "post")
                {

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
                                        cookieJar.Add(cookie);
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

                    if (keyValues.Length > 0)
                    keyValues = keyValues.Remove(0, 1);
                    
                    if (keyValues.StartsWith("SOAPBody="))
                    {
                        keyValues = keyValues.Replace("SOAPBody=", "");
                        if (keyValues.EndsWith("&"))
                            keyValues = keyValues.Remove(keyValues.Length - 1, 1);
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
                    
                    _HttpWebRequest.ContentLength = keyValues.Length;

                    startTime = DateTime.Now;
                    stopwatch.Start();
                    StreamWriter newStream = new StreamWriter(_HttpWebRequest.GetRequestStream());
                    newStream.Write(keyValues, 0, keyValues.Length);
                    newStream.Close();

                    HttpWebResponse webresponse = (HttpWebResponse)_HttpWebRequest.GetResponse();

                    cookieJar = FixCookies(cookieJar, webresponse.Cookies, _HttpWebRequest.RequestUri);

                    if (webresponse.StatusCode == HttpStatusCode.Redirect || webresponse.StatusCode == HttpStatusCode.MovedPermanently)
                    {
                    CheckRedirect:

                        string newUrl = webresponse.Headers["Location"];

                        redirect_url_list.Add(newUrl);

                        _HttpWebRequest = (HttpWebRequest)WebRequest.Create(newUrl);
                        _HttpWebRequest.CookieContainer = cookieJar;
                        _HttpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
                        webresponse = (HttpWebResponse)_HttpWebRequest.GetResponse();

                        cookieJar = FixCookies(cookieJar, webresponse.Cookies, _HttpWebRequest.RequestUri);

                        if (webresponse.StatusCode == HttpStatusCode.Redirect || webresponse.StatusCode == HttpStatusCode.MovedPermanently)
                        {
                            goto CheckRedirect;
                        }
                    }
                    http_status_code = (int)webresponse.StatusCode;
                    StreamReader reader = new StreamReader(webresponse.GetResponseStream());
                    stopwatch.Stop();
                    response_time = stopwatch.Elapsed.TotalMilliseconds;

                    endTime = DateTime.Now;
                    navigation_string = reader.ReadToEnd();

                    byte[] bytes = new byte[navigation_string.Length * sizeof(char)];
                    response_size = (bytes.Length) / 1024;                    
                    content_type = webresponse.ContentType;

                    reader.Close();
                    newStream.Close();
                    webresponse.Close();
                }
                else if (_HttpWebRequest.Method == "GET" || _HttpWebRequest.Method == "get")
                {
                    _HttpWebRequest = (HttpWebRequest)WebRequest.Create(_HttpWebRequest.RequestUri + keyValues);
                    
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
                            else if ("Connection".Equals(key)) {}
                            else if ("Accept-Language".Equals(key)) _HttpWebRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, _dict["HeaderValue"]);
                            else if ("Accept-Encoding".Equals(key)) _HttpWebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, _dict["HeaderValue"]);
                            else if ("Host".Equals(key)) {}//_HttpWebRequest.Host = _dict["HeaderValue"];
                            else if ("Content-Length".Equals(key)) { }
                            else if ("If-Modified-Since".Equals(key)) { }
                            else if ("Proxy-Connection".Equals(key)) {}
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
                    HttpWebResponse webresponse = null;

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

                    try
                    {
                        _HttpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
                        _HttpWebRequest.MaximumAutomaticRedirections = 10;
                        startTime = DateTime.Now;
                        stopwatch.Start();
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
                        }
                        stopwatch.Stop();
                        response_time = stopwatch.Elapsed.TotalMilliseconds;
                        endTime = DateTime.Now;
                        byte[] bytes = new byte[navigation_string.Length * sizeof(char)];
                        response_size = (bytes.Length)/1024;
                        content_type = webresponse.ContentType;
                    }
                }
            endOFLoop:
            Amigo.App.http_response = navigation_string;
            request_size = _HttpWebRequest.ContentLength;
            
            if (navigation_string.Contains("title"))
            {
                ExecutionStudio.responses_for_correlation.Add(iteration_no,navigation_string);
            }

            performPostRequestActivities("200", message + navigation_string, startTime, endTime, request_size, response_size, content_type, response_time);
            }
            catch (CookieException cx) { Console.Write(cx.Message); }
            catch (WebException ex)
            {
                message = ex.Message;
                endTime = DateTime.Now;
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    string respString = "\nStatus Description:" + resp.StatusDescription + " for URI:" + resp.ResponseUri;                    

                    if (resp.StatusCode == HttpStatusCode.NotModified) // HTTP 404 
                    {
                        respString += "\nStatus Code:" + "HTTP 304";
                        performPostRequestActivities("304", message + respString + "\n", startTime, endTime, request_size, response_size, content_type, response_time);
                    }
                    else if (resp.StatusCode == HttpStatusCode.NotFound) // HTTP 404 
                    {
                        respString += "\nStatus Code:" + "HTTP 404";
                        performPostRequestActivities("404", message + respString + "\n", startTime, endTime, request_size, response_size, content_type, response_time);
                    }
                    else if (resp.StatusCode == HttpStatusCode.ProxyAuthenticationRequired) // HTTP 407 
                    {
                        respString += "\nStatus Code:" + "HTTP 407";
                        performPostRequestActivities("404", message + respString + "\n", startTime, endTime, request_size, response_size, content_type, response_time);
                    }
                    else if (resp.StatusCode == HttpStatusCode.InternalServerError) // HTTP 500
                    {
                        respString += "\nStatus Code:" + "HTTP 500";
                        performPostRequestActivities("500", message + respString + "\n", startTime, endTime, request_size, response_size, content_type, response_time);
                    }
                    else if (resp.StatusCode == HttpStatusCode.BadGateway) // HTTP 502 
                    {
                        respString += "\nStatus Code:" + "HTTP 502";
                        performPostRequestActivities("502", message + respString + "\n", startTime, endTime, request_size, response_size, content_type, response_time);
                    }
                    else if (resp.StatusCode == HttpStatusCode.GatewayTimeout) // HTTP 504
                    {
                        respString += "\nStatus Code:" + "HTTP 504";
                        performPostRequestActivities("504", message + respString + "\n", startTime, endTime, request_size, response_size, content_type, response_time);
                    }
                    else
                    {
                        respString += "\nStatus Code:" + "HTTP Generic";
                        performPostRequestActivities("000", message + respString + "\n", startTime, endTime, request_size, response_size, content_type, response_time);
                    }
                }
                else
                {
                    performPostRequestActivities("000", message, startTime, endTime, request_size, response_size, content_type, response_time);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        private double getRequestID()
        {
            if (child_request < 0)
            {
                return parent_request_id;
            }
            else
            {
                return child_request;
            }
        }
        private bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        private CookieContainer FixCookies(CookieContainer cookieJar, CookieCollection cookieCollection, Uri uri)
        {
            CookieContainer container = new CookieContainer();
            CookieCollection jarColln = cookieJar.GetCookies(uri);
            Cookie[] gotCookies = new Cookie[cookieCollection.Count];
            Cookie[] jarCookies = new Cookie[jarColln.Count];

            cookieCollection.CopyTo(gotCookies, 0);
            jarColln.CopyTo(jarCookies, 0);

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
        private void performPostRequestActivities(string http_status_code, string message, DateTime startTime, DateTime endTime,double request_size,double response_size, string content_type,double response_time)
        {
            if (http_status_code.StartsWith("2"))
            {
                message = "";
            }
            //AmigoLogger.Log(http_status_code + ":" + message);
            ExecutionDAO _DBResponseWriter = new ExecutionDAO();
            if (request_id == parent_request_id)
            {
                parent_request_id = -1;
            }
            _DBResponseWriter.addResponseDetails(request_id, parent_request_id, thread_id, iteration_no, ExecutionStudio._Scenario.ProjectID,
                                ExecutionStudio._Scenario.ScenarioID, startTime, endTime, Int32.Parse(http_status_code), message, request_size, response_size, content_type, response_time);
        }
        private string getCSVCellValue(string CSVfilePath, string CSVColumn, int threadID)
        {
            int itr = getNextPosition(CSVfilePath, CSVColumn, threadID);
            int fieldIndex = -1;
            string CSVColValue = "";
            using (CsvReader csv = new CsvReader(new StreamReader(CSVfilePath), true))
            {
                string[] headers = csv.GetFieldHeaders();

                for (int i = 0; i < headers.Length; i++)
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
            return CSVColValue;
        }
        object csvLock = new object();        
        private int getNextPosition(string CSVfilePath, string CSVColumn, int threadID)
        {
            lock (csvLock)
            {
                string this_key = threadID + "-" + CSVfilePath + "-" + CSVColumn;
                int position = -1;
                KeyValuePair<string, int> _csv_key_val;

                for (int i = 0; i < _CSVValuePositionList.Count; i++)
                {
                    _csv_key_val = _CSVValuePositionList[i];
                    if (this_key == _csv_key_val.Key)
                    {
                        position = _csv_key_val.Value;
                        _CSVValuePositionList.Remove(_csv_key_val);
                        break;
                    }
                }
                position++;
                _csv_key_val = new KeyValuePair<string, int>(this_key, position);
                _CSVValuePositionList.Add(_csv_key_val);
            
            return position;
            }
        }
    }
}