using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Fiddler;
using System.Text.RegularExpressions;
using Amigo.ViewModels;
using System.Collections;
using LumenWorks.Framework.IO.Csv;
using System.IO;
using System.Text;
using System.Diagnostics;


namespace Amigo
{
    class DBProcessingLayer
    {
        public void storeSessionToDB(List<Fiddler.Session> oAllSessions)
        {
            int recording_id = ScriptScenarioDAO.getRecordingSessionID();
            int project_id = getCurrentProjectID();
            double request_id = 0;
            int counter = 0;
            string protocol = "";
            string method = "";
            string host_name = "" ;
            int host_port = 0;
            string request_path = "";
            string[] headers = { };
            bool titleAssigned = false;
            string response_body = "";

            //first add new recording id for the project. One recording will have multiple requests.
            int retVal = ScriptScenarioDAO.insert_to_recording_sessions(recording_id, project_id,Amigo.App.recordingSessionName);
            
                //iterate the requests for details
            try
            {
                TransactionName._status.start_button_status = false;
                TransactionName._status.end_button_status = false;
                TransactionName._status.script_name_text_status = false;
                StatusBar.totalCount = oAllSessions.Count;
                Monitor.Enter(oAllSessions);
                foreach (Session oS in oAllSessions)
                {
                    try
                    {
                        request_id = getNextRequestID();
                        if (oS.isFTP) { protocol = "ftp"; }
                        else if (oS.isHTTPS) { protocol = "https"; }
                        else { protocol = "http"; }

                        method = oS.oRequest.headers.HTTPMethod;
                        host_name = oS.hostname;
                        host_port = oS.port;
                        request_path = oS.oRequest.headers.RequestPath;
                        if (oS.oResponse.headers.HTTPResponseCode >= 400)
                        { goto statusUpdate; }
                        string title = "";
                        if (oS.bHasResponse)
                        {
                            response_body = oS.GetResponseBodyAsString();

                            title = GetTitle(response_body);
                            if (title.Length > 80)
                            {
                                title = title.Substring(0, 80);
                            }
                            if (title.Length > 0)
                            {
                                title = "Page(Txn)" + request_id + "-" + title;
                            }
                            else if (!titleAssigned && (method == "get" || method == "GET" || method == "post" || method == "POST"))
                            {
                                title = "Page(Txn)" + request_id + "-" + title;
                                titleAssigned = true;
                            }
                        }

                        byte[] request_body = oS.RequestBody;
                        string body_string = Encoding.ASCII.GetString(request_body);
                        string request_path_body = "";
                        bool isSOAP = false;
                        if (body_string.StartsWith("<?xml"))
                        {
                            isSOAP = true;
                        }
                        if (string.Equals(method, "post", StringComparison.OrdinalIgnoreCase))
                        {
                            if (request_path.Contains('?'))
                            {
                                if (request_path.Contains('&'))
                                {
                                    if (isSOAP)
                                    {
                                        request_path_body = request_path + "&SOAPBody=" + body_string;
                                    }
                                    else
                                    {
                                        request_path_body = request_path + "&" + body_string;
                                    }
                                }
                                else if (request_path.IndexOf('?') == request_path.Length - 1)
                                {
                                    if (isSOAP)
                                    {
                                        request_path_body = request_path + "SOAPBody=" + body_string;
                                    }
                                    else
                                    {
                                        request_path_body = request_path + body_string;
                                    }
                                }
                                else if (!request_path.Contains('&') && !request_path.Contains('?'))
                                {
                                    if (isSOAP)
                                    {
                                        request_path_body = request_path + "?SOAPBody=" + body_string;
                                    }
                                }
                            }
                            else
                            {
                                if (isSOAP)
                                {
                                    request_path_body = request_path + "?SOAPBody=" + body_string;
                                }
                                else
                                {
                                    request_path_body = request_path + "?" + body_string;
                                }
                            }
                            request_path = request_path_body;
                        }

                        List<string[]> header_list = new List<string[]>();

                        for (int i = 0; i < oS.oRequest.headers.Count(); i++)
                        {
                            HTTPHeaderItem http_header = oS.oRequest.headers[i];
                            String[] nameValue = { "", "" };
                            nameValue[0] = http_header.Name;
                            nameValue[1] = http_header.Value;
                            header_list.Add(nameValue);
                            if ("Authorization" == http_header.Name)
                            {
                                if (http_header.Value.Contains("NTLM"))
                                {
                                    if (request_path.IndexOf("?") >= 0)
                                    {
                                        request_path = request_path + "&NTLMuserName=EnterUserName" + "&NTLMpassword=NTLMpassword" + "&NTLMDomain=NTLMDomain";
                                    }
                                    else
                                    {
                                        request_path = request_path + "?NTLMuserName=EnterUserName" + "&NTLMpassword=NTLMpassword" + "&NTLMDomain=NTLMDomain";
                                    }
                                }
                            }
                        }

                        saveRequestHeaders(header_list, request_id, recording_id, project_id);
                        string full_host = host_name + ":" + host_port;

                        if (!full_host.Equals(request_path))
                        {
                            request_path = System.Web.HttpUtility.UrlDecode(request_path);
                            saveRequestPathsAndParams(request_path, request_id, recording_id, project_id);
                        }
                        retVal = ScriptScenarioDAO.insert_to_request_referers(request_id, recording_id, project_id, protocol, method, host_name, host_port, "", "", oS.responseCode, title, response_body);

                    statusUpdate:
                        double percentage = 100 - 100 * (StatusBar.totalCount - counter) / StatusBar.totalCount;



                        TransactionName._status.StatusNow = (int)percentage;
                        counter++;
                        if (counter == StatusBar.totalCount)
                        {
                            TransactionName._status.StatusNow = 100;
                        }
                    }
                    catch { }
                }
            }
            finally
            {
                TransactionName._status.start_button_status = true;
                TransactionName._status.end_button_status = false;
                TransactionName._status.script_name_text_status = true;
                Monitor.Exit(oAllSessions);
            }
        }

        static string GetTitle(string file)
        {
            Match m = Regex.Match(file, @"<title>\s*(.+?)\s*</title>");
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            else
            {
                return "";
            }
        }

        private double getNextRequestID()
        {
            return ScriptScenarioDAO.getNextRequestID();
        }

        public void saveRequestHeaders(List<string[]> fullHeader, double request_id, int recording_id, int project_id)
        {
            for (int i = 0; i < fullHeader.Count; i++)
            {
                string[] nvpair = fullHeader[i];
                int retVal = ScriptScenarioDAO.insert_to_request_headers(i, request_id, recording_id, project_id, nvpair[0], nvpair[1]);
            }
        }

        public void saveRequestPathsAndParams(string request_path, double request_id, int recording_id, int project_id)
        {
            string allParams = "";
            string allPaths = "";
            string[] arrAllParams={};
            string[] arrPaths={};
            int position = -10;
            int lastPosition = request_path.Length;
            bool isSOAP = false;
            try
            {
                if (request_path.Contains("SOAPBody"))
                {
                    isSOAP = true;
                }
                position = request_path.IndexOf("?");

                if (position >= 0)
                {
                    allParams = request_path.Substring(position + 1, lastPosition - position - 1);

                    allPaths = request_path.Substring(0, position);

                    arrAllParams = allParams.Split('&');
                    arrPaths = allPaths.Split('/');
                }
                else
                {
                    allPaths = request_path;
                    arrPaths = allPaths.Split('/');
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            if (allParams.Contains('&') || isSOAP)
            {
                saveRequestParams(arrAllParams, request_id, recording_id, project_id, isSOAP);
            }
            if (allPaths.Contains('/'))
            {
                saveRequestPaths(arrPaths, request_id, recording_id, project_id);
            }
        }

        public void saveRequestParams(string[] arrAllParams, double request_id, int recording_id, int project_id, bool isSOAP)
        {
            for(int i=0; i< arrAllParams.Length; i++)
            {
                if (arrAllParams[i].Contains("="))
                {
                    char[] splitter = {'='};
                    string[] nvPair = arrAllParams[i].Split(splitter, 2);
                    string http_param_name = nvPair[0];
                    string original_param_value = nvPair[1];
                    int retVal = ScriptScenarioDAO.insert_to_request_params(i, request_id, recording_id, project_id, http_param_name, original_param_value, "", "", "");
                }
            }
        }

        public void saveRequestPaths(string[] arrPaths, double request_id, int recording_id, int project_id)
        {
            for(int i=0; i<arrPaths.Length; i++)
            {
                
                
                    int retVal = ScriptScenarioDAO.insert_to_resource_paths(i, request_id, recording_id, project_id, arrPaths[i], "");
                
            }
        }

        public int getCurrentProjectID()
        {
            return Amigo.App.currentProjectID;
        }
        
        public int getMaxProjectID()
        {
            return 0;
        }

        internal string validateAndSaveProjectName(string project_name)
        {
            List<Dictionary<string, string>> projects = getAllProjects();
            Dictionary<string, string> projectRow;

            for (int i = 0; i < projects.Count; i++)
            {
                projectRow = projects[i];
                if (projectRow["Name"].Equals(project_name))
                {
                    return "Project name already exists";
                }
            }
            
                int project_id = ScriptScenarioDAO.getMaxProjectID()+1;
                ScriptScenarioDAO.insert_to_projects (project_id, project_name);
                Amigo.App.currentProjectID = project_id;                
                return "success";            
        }


        internal string validateAndRenameProjectName(string project_name)
        {
            List<Dictionary<string, string>> projects = getAllProjects();
            Dictionary<string, string> projectRow;

            for (int i = 0; i < projects.Count; i++)
            {
                projectRow = projects[i];
                if (projectRow["Name"].Equals(project_name))
                {
                    return "Project name already exists";
                }
            }

            int project_id = Amigo.App.currentProjectID;
            ScriptScenarioDAO.update_project_name(project_id, project_name);
            return "success";
        }

        public static List<Dictionary<string, string>> getAllProjects()
        {
            List<Dictionary<string, string>> projects = ScriptScenarioDAO.getAllProjects();
            return projects;
        }

        internal static void retrieveRequestReferers()
        {
            Amigo.App._project.RequestReferers.Clear();
            List<Dictionary<string, string>> _referers = ScriptScenarioDAO.retrieveRequestReferers();
            NotifiableObservableCollection<RequestReferer> _new_referers = new NotifiableObservableCollection<RequestReferer>();
            RequestReferer _new_referer;

            for (int i = 0; i < _referers.Count; i++)
            {
                Dictionary<string, string> _referer = _referers[i];
                //Amigo.App._project.RequestReferers 
                _new_referer = new RequestReferer();
                _new_referer.Referer = _referer["RefererName"];
                _new_referers.Add(_new_referer);
            }
            Amigo.App._project.RequestReferers = _new_referers;
        }

        internal static void retrieveRecordingSessions()
        {
            List<Dictionary<string, string>> _sessions = ScriptScenarioDAO.retrieveRecordingSessions();
            NotifiableObservableCollection<RecordingSession> _new_sessions = new NotifiableObservableCollection<RecordingSession>();
            RecordingSession _new_session;

            for (int i = 0; i < _sessions.Count; i++)
            {
                Dictionary<string, string> _session = _sessions[i];
                _new_session = new RecordingSession();
                _new_session.SessionID = Int32.Parse(_session["ID"]);
                _new_session.SessionName = "Script - " + _session["Name"];
                _new_session.SequenceID = double.Parse(_session["SequenceID"]);
                _new_sessions.Add(_new_session);
            }
            Amigo.App._project.RecordingSessions = _new_sessions;
        }

        internal static ArrayList getAllRequestIDs_for_Referer(string _referer)
        {
            ArrayList _request_ids = ScriptScenarioDAO.getAllRequestIDs_forreferer(_referer, Amigo.App.currentProjectID);
            return _request_ids;
        }

        internal static ArrayList getAllPaths_for_Request_ID(double req)
        {
            ArrayList paths = ScriptScenarioDAO.getAllPaths_for_Request_ID(req, Amigo.App.currentProjectID);
            return paths;
        }

        internal static ArrayList getAllRequestIDs_for_Sessions(int _session_id)
        {
            ArrayList _request_ids = ScriptScenarioDAO.getAllRequestIDs_for_session(_session_id, Amigo.App.currentProjectID);
            return _request_ids;
        }

        internal static void getAllParams_for_request_ID(string _lable_content, string from_method)
        {
            double request_id = double.Parse(_lable_content);
            List<Dictionary<string, string>> _params = ScriptScenarioDAO.getAllParams_for_request_ID(request_id);
            NotifiableObservableCollection<Parameter> _new_parameters = new NotifiableObservableCollection<Parameter>();
            Parameter _new_single_parameter;

            for (int i = 0; i < _params.Count; i++)
            {
                Dictionary<string, string> _param = _params[i];
                _new_single_parameter = new Parameter();

                _new_single_parameter.ParamID = _param["ParamID"];
                _new_single_parameter.ParamName = _param["ParamName"];
                _new_single_parameter.OriginalParamValue = _param["OriginalParamValue"];
                

                string _str_ParameterizationSource = _param["ParameterizationSource"];

                if (_str_ParameterizationSource.Equals(ParamSources.CSV.ToString()))
                {
                    _new_single_parameter.ParameterizationSource = ParamSources.CSV;
                }
                else if (_str_ParameterizationSource.Equals(ParamSources.KeepOriginal.ToString()))
                {
                    _new_single_parameter.ParameterizationSource = ParamSources.KeepOriginal;
                }
                else if (_str_ParameterizationSource.Equals(ParamSources.AutoCorrelation.ToString()))
                {
                    _new_single_parameter.ParameterizationSource = ParamSources.AutoCorrelation;
                }
                _param["DataPoolCSVName"] = _param["DataPoolCSVName"].Replace(@"\\", @"\");
                _param["DataPoolCSVName"] = _param["DataPoolCSVName"].Replace(@"\\", @"\");
                _new_single_parameter.SubstututedParamValue = _param["DataPoolCSVName"];


                _new_single_parameter.SelectedCSVColumnName = _param["CSVColumnName"];
                string _itr = _param["IterationMode"];
                if (_itr.Equals(IterationType.None.ToString()))
                {
                    _new_single_parameter.SelectedIterationType = IterationType.None;
                }
                else if (_itr.Equals(IterationType.Random.ToString()))
                {
                    _new_single_parameter.SelectedIterationType = IterationType.Random;
                }
                else if (_itr.Equals(IterationType.Sequential.ToString()))
                {
                    _new_single_parameter.SelectedIterationType = IterationType.Sequential;
                }
                else if (_itr.Equals(IterationType.Unique.ToString()))
                {
                    _new_single_parameter.SelectedIterationType = IterationType.Unique;
                }

                //retrive csv columns start

                try
                {
                    string filename = _new_single_parameter.SubstututedParamValue;
                    if (!"".Equals(filename) && _new_single_parameter.SelectedCSVColumnName.Length > 0)
                    {
                        NotifiableObservableCollection<string> _csv_columns = new NotifiableObservableCollection<string>();

                        using (CsvReader csv = new CsvReader(new StreamReader(filename), true))
                        {
                            int fieldCount = csv.FieldCount;
                            string[] headers = csv.GetFieldHeaders();

                            for (int ii = 0; ii < fieldCount; ii++)
                            {
                                _csv_columns.Add(headers[ii]);
                            }
                            _new_single_parameter.CSVColumns = _csv_columns;
                        }
                    }
                }
                catch (Exception csvex) { Console.Write(csvex.Message); }

                //retrive csv columns end

                _new_single_parameter.LB = _param["LB"];
                _new_single_parameter.RB = _param["RB"];

                _new_parameters.Add(_new_single_parameter);
            }
            if (from_method.Equals("ScriptStudio"))
            {
                Amigo.App._project.Parameters = _new_parameters;
            }
            else if (from_method.Equals("ScriptReplay"))
            {
                Amigo.App.Parameters_For_Replay = _new_parameters;
            }
        }

        internal static string[] getReferer_for_RequestID(string _lable_content)
        {
            string[] retVal = ScriptScenarioDAO.getReferer_for_RequestID(Double.Parse(_lable_content));
            return retVal;
        }

        internal static void updateParameterizedHostPort(string host_name_text, string port_text, string _request_id)
        {

            ScriptScenarioDAO.updateParameterizedHostPort(host_name_text, port_text, _request_id);
            Amigo.App._project.SelectedRequestPart1 = host_name_text + ":" + port_text;            
        }

        internal static void saveAll_Parameterized_Values()
        {
            NotifiableObservableCollection<ViewModels.Parameter> _parameters = Amigo.App._project.Parameters;

            for (int i = 0; i < _parameters.Count; i++)
            {
                string ParamID = _parameters[i].ParamID;
                string ParamName = _parameters[i].ParamName;
                string OriginalParamValue = _parameters[i].OriginalParamValue;
                ParamSources ParameterizationSource = _parameters[i].ParameterizationSource;
                string _str_ParameterizationSource = "";

                if (ParameterizationSource.Equals(ParamSources.CSV))
                {
                    _str_ParameterizationSource = "CSV";
                }
                else if (ParameterizationSource.Equals(ParamSources.KeepOriginal))
                {
                    _str_ParameterizationSource = "KeepOriginal";
                }
                else if (ParameterizationSource.Equals(ParamSources.AutoCorrelation))
                {
                    _str_ParameterizationSource = "AutoCorrelation";
                }

                string SubstututedParamValue = _parameters[i].SubstututedParamValue;
                SubstututedParamValue = SubstututedParamValue.Replace(@"\",@"\\");

                string SelectedCSVColumnName = _parameters[i].SelectedCSVColumnName;
                IterationType SelectedIterationType = _parameters[i].SelectedIterationType;
                string _str_SelectedIterationType = "";

                if (SelectedIterationType.Equals(IterationType.None))
                {
                    _str_SelectedIterationType = "None";
                }
                else if (SelectedIterationType.Equals(IterationType.Random))
                {
                    _str_SelectedIterationType = "Random";
                }
                else if (SelectedIterationType.Equals(IterationType.Sequential))
                {
                    _str_SelectedIterationType = "Sequential";
                }
                else if (SelectedIterationType.Equals(IterationType.Unique))
                {
                    _str_SelectedIterationType = "Unique";
                }

                ScriptScenarioDAO.saveAll_Parameterized_Values(ParamID,_str_ParameterizationSource, SubstututedParamValue, SelectedCSVColumnName, _str_SelectedIterationType,_parameters[i].LB,_parameters[i].RB);
            }                
        }

        internal static void getAllHeaders_for_request_ID(string _request_id)
        {
            NotifiableObservableCollection<ViewModels.SingleHeader> _request_headers = new NotifiableObservableCollection<SingleHeader>();
            SingleHeader _single_header;

            string _key = "";
            string _value = "";

            List <Dictionary<string , string>> header_list = ScriptScenarioDAO.getAllHeaders_for_request_ID(_request_id);
            

            for(int i=0 ; i < header_list.Count ; i++)
            {
                _key   = header_list[i]["HeaderKey"];
                _value = header_list[i]["HeaderValue"];
                
                _single_header = new SingleHeader();
                _single_header.HeaderKey = _key;
                _single_header.HeaderValue = _value;

                _request_headers.Add(_single_header);
            }
            Amigo.App._project.RequestHeaders = _request_headers;
        }

        internal static int getRecordingSession_for_Request_id(string _lable_content)
        {
            return ScriptScenarioDAO.getRecordingSession_for_Request_id(double.Parse(_lable_content));
        }

        internal static string[] getMethod_for_request_ID(string request_id)
        {
            return ScriptScenarioDAO.getMethod_for_request_ID(double.Parse(request_id));
        }

        internal static string getTitle_for_request_ID(string request_id)
        {
            return ScriptScenarioDAO.getTitle_for_request_ID(request_id);
        }

        internal static void copyRequest(string copied_req_id, string new_req_id)
        {
            ScriptScenarioDAO.copyRequestHeaders(copied_req_id, new_req_id);
            ScriptScenarioDAO.copyRequestParams(copied_req_id, new_req_id);
            ScriptScenarioDAO.copyRequestReferers(copied_req_id, new_req_id);
            ScriptScenarioDAO.copyResourcePaths(copied_req_id, new_req_id);
        }

        internal static void deleteRequest(string cut_req_id)
        {
            ScriptScenarioDAO.deleteRequest(cut_req_id);
        }

        internal static void deleteProject(int project_id)
        {
            ScriptScenarioDAO.deleteProject(project_id);
        }

        internal static void deleteScript(int recording_id)
        {
            ScriptScenarioDAO.deleteScript(recording_id);
        }

        internal static void updateTitle(string req, string renamed_text)
        {
            ScriptScenarioDAO.updateTitle(req, renamed_text);
        }

        internal static void createHTMLfromURLS()
        {
            //// create URL file start ------
            string htmlstart = "<html><body><center><h2>Recently Accessed Links</h2></center><hr/><h4><OL>";
            string line;
            string URLLinks="";
            System.IO.StreamReader file = new System.IO.StreamReader(".\\..\\..\\Interfaces\\frequentURLs.txt");
            while ((line = file.ReadLine()) != null)
            {
                URLLinks = "<LI><a href='" + line + "'>" + line + "</a></LI><br/>" + URLLinks;
            }
            file.Close();
            Console.ReadLine();
            string htmlend = "</OL></h4></body></html>";
            string html = htmlstart + URLLinks + htmlend;
            System.IO.File.WriteAllText(".\\..\\..\\Interfaces\\browse.html", html);
            //// create URL file end --------
        }

        internal static string getPreviousRequestID(string copied_req_id)
        {
            return ScriptScenarioDAO.getPreviousRequestID(copied_req_id);
        }

        internal static string getNextRequestID(string copied_req_id)
        {
            return ScriptScenarioDAO.getNextRequestID(copied_req_id);
        }

        internal static Scenario createScenario(string scenario_name, int project_id)
        {
            Scenario scenario = ScriptScenarioDAO.createScenario(scenario_name, project_id);
            return ScriptScenarioDAO.getScenario_for_ScenarioID(scenario.ScenarioID, project_id);
        }

        internal static NotifiableObservableCollection<Scenario> getAllScenarios_for_Project_ID(int project_id)
        {
            return ScriptScenarioDAO.getAllScenarios_for_Project_ID(project_id);
        }

        internal static ArrayList createScriptFromSession(int session_id, int scenario_id)
        {
            return ScriptScenarioDAO.createScriptFromSession(session_id,scenario_id);
        }

        internal static ArrayList retriveScripts_for_Scenario(int scenario_id)
        {
            return ScriptScenarioDAO.retriveScripts_for_Scenario(scenario_id);
        }

        internal static void updateScript(Script srcipt)
        {
            ScriptScenarioDAO.updateScript(srcipt);
        }

        internal static void deleteScript_Runtime(int sequence_id)
        {
            ScriptScenarioDAO.deleteScript_Runtime(sequence_id);
        }

        internal static void delete_Scenario_Runtime(int scenario_id)
        {
            ScriptScenarioDAO.delete_Scenario_Runtime(scenario_id);
        }

        internal static void saveScenario(Scenario _Scenario)
        {
            ScriptScenarioDAO.saveScenario(_Scenario);
        }

        internal static ArrayList retrieveServerResourceMonitors(int scenario_id)
        {
            return ScriptScenarioDAO.retrieveServerResourceMonitors(scenario_id);
        }        
    }
}