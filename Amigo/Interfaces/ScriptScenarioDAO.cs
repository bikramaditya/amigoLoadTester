using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Collections;
using Amigo.ViewModels;
using System.IO;
using System.Text;

namespace Amigo
{
    class ScriptScenarioDAO
    {
        public static int insert_to_projects(int project_id, string project_name)
        {
            int retVal = 0;
            DateTime date = DateTime.Now;

            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            String query = "insert into projects (project_id, project_name, date) values(@project_id,@project_name,@date)";           

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.Add(new SQLiteParameter("project_id", project_id));
                    command.Parameters.Add(new SQLiteParameter("project_name", project_name));
                    command.Parameters.Add(new SQLiteParameter("date", date));
                    retVal = command.ExecuteNonQuery();
                }
            }
            return retVal;
        }

        public static int insert_to_recording_sessions(int recording_id, int project_id, string _session_name)
        {
            int retVal = 0;
            double sequence_id = double.Parse(recording_id.ToString());
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "insert into recording_sessions (recording_id ,project_id,session_name,sequence_id) values(" + recording_id + "," + project_id + ",'" + _session_name + "',"+sequence_id+")";
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        retVal = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
            return retVal;
        }


        public static int insert_to_request_referers
        (
        double request_id,
        int recording_id ,
        int project_id ,
        string protocol ,
        string method ,
        string host_name ,
        int host_port ,
        string parameterized_host_name ,
        string parameterized_host_port,
        int responseCode,
        string title,
        string response_body
        )
        {
            int retVal = 0;
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");

            byte[] byte_resp_body = Encoding.ASCII.GetBytes(response_body);


            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "insert into request_referers "+
                          "(request_id ,recording_id ,project_id ,protocol ,method ,host_name ,host_port ,parameterized_host_name ,parameterized_host_port,response_code,title,response_body) "+
                    "values(@request_id , @recording_id , @project_id ,@protocol , @method , @host_name , @host_port , @host_name , @host_port ,@responseCode , @title , @response_body )";
                    
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {

                        cmd.Parameters.Add(new SQLiteParameter("request_id" , request_id));
                        cmd.Parameters.Add(new SQLiteParameter("recording_id" , recording_id));
                        cmd.Parameters.Add(new SQLiteParameter("project_id" ,project_id));
                        cmd.Parameters.Add(new SQLiteParameter("protocol" , protocol));
                        cmd.Parameters.Add(new SQLiteParameter("method" , method));
                        cmd.Parameters.Add(new SQLiteParameter("host_name" , host_name));
                        cmd.Parameters.Add(new SQLiteParameter("host_port" , host_port));
                        cmd.Parameters.Add(new SQLiteParameter("host_name" , host_name));
                        cmd.Parameters.Add(new SQLiteParameter("host_port" ,host_port));
                        cmd.Parameters.Add(new SQLiteParameter("responseCode" , responseCode));
                        cmd.Parameters.Add(new SQLiteParameter("title" , title));
                        cmd.Parameters.Add(new SQLiteParameter("response_body", byte_resp_body));
                                                

                        retVal = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
            return retVal;
        }

        public static int insert_to_resource_paths
        (
        int resource_path_id ,
        double request_id,
        int recording_id ,
        int project_id ,
        string resource_path ,
        string parameterized_resource_path 
        )
        {
            int retVal = 0;
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "insert into resource_paths values(" + resource_path_id + "," + request_id + "," + recording_id + "," + project_id + ",'" + resource_path + "',''" + ")";
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        retVal = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
            return retVal;
        }

        public static int insert_to_request_params
        (
        int param_id ,
        double request_id,
        int recording_id ,
        int project_id ,
        string http_param_name ,
        string original_param_value ,
        string substituted_param_name ,
        string datapool_csv_name ,
        string csv_column_name 
        )
        {
            int retVal = 0;
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "insert into request_params values( NULL " + "," + request_id + "," + recording_id + "," + project_id + ",'" + http_param_name + "','" + original_param_value + "'" + ",''" + ",''" + ",''" + ",'" + original_param_value+ "'" + ",'','')";
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        retVal = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
            return retVal;
        }
        public static int insert_to_request_headers
        (
        int header_item_id ,
        double request_id,
        int recording_id ,
        int project_id ,
        string item_name ,
        string item_value
        )
        {
            int retVal = 0;
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            byte[] byte_item_value = Encoding.ASCII.GetBytes(item_value);
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "insert into request_headers values(NULL ,@request_id ,@recording_id ,@project_id ,@item_name ,@item_value)";
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("request_id", request_id));
                        cmd.Parameters.Add(new SQLiteParameter("recording_id", recording_id));
                        cmd.Parameters.Add(new SQLiteParameter("project_id", project_id));
                        cmd.Parameters.Add(new SQLiteParameter("item_name", item_name));
                        cmd.Parameters.Add(new SQLiteParameter("item_value", byte_item_value));

                        retVal = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
            return retVal;
        }
        public static int getRecordingSessionID()
        {
            int recordingSessionID = 0;
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT max(recording_id) from recording_sessions where project_id=" + Amigo.App.currentProjectID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                recordingSessionID = dr.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return recordingSessionID;
            }
            return recordingSessionID + 1;
        }


        internal static List<Dictionary<string, string>> getAllProjects()
        {
            List<Dictionary<string, string>> projects = new List<Dictionary<string, string>>();
            Dictionary<string, string> projectRow;            

            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT project_id, project_name,date from projects order by date desc";

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                projectRow = new Dictionary<string, string>();
                                int val = dr.GetInt32(0);
                                projectRow.Add("ID",val.ToString());
                                projectRow.Add("Name", dr.GetString(1));                                
                                projectRow.Add("Date", dr.GetValue(2).ToString());                                                         
                                projects.Add(projectRow);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return projects;
        }

        internal static int getMaxProjectID()
        {
            int maxProjectID=0;

            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT max(project_id) from projects";

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                maxProjectID = dr.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return maxProjectID;
            }
            return maxProjectID;
        }



        internal static List<Dictionary<string, string>> retrieveRequestReferers()
        {
            List<Dictionary<string, string>> _referers = new List<Dictionary<string, string>>();
            
            Dictionary<string, string> _referer;


            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select distinct parameterized_host_name from request_referers where project_id = "+Amigo.App.currentProjectID +
                        " and ( method='GET' or method='POST' or method='get' or method='post' )";

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                _referer = new Dictionary<string, string>();                                
                                _referer.Add("RefererName", dr.GetString(0));                                
                                _referers.Add(_referer);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return _referers;
        }

        internal static ArrayList getAllRequestIDs_forreferer(string referer, int project_id)
        {
            ArrayList _request_ids = new ArrayList();
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select request_id from request_referers where project_id =" + project_id + " and parameterized_host_name='" + referer + "'";

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                _request_ids.Add(dr.GetDouble(0));                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return _request_ids;
            }
            return _request_ids;
        }

        internal static ArrayList getAllPaths_for_Request_ID(double req, int project_id)
        {
            ArrayList paths = new ArrayList();            

            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select resource_path from resource_paths where project_id ="+project_id+" and request_id="+req;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                paths.Add(dr.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return paths;
            }
            return paths;
        }

        internal static ArrayList getAllRequestIDs_for_session(int _session_id, int project_id)
        {
            ArrayList _request_ids = new ArrayList();
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select request_id from request_referers where project_id = " + project_id + " and recording_id = " + _session_id + " order by request_id";

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                _request_ids.Add(dr.GetDouble(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return _request_ids;
            }
            return _request_ids;
        }

        internal static List<Dictionary<string, string>> retrieveRecordingSessions()
        {
            List<Dictionary<string, string>> _sessions = new List<Dictionary<string, string>>();

            Dictionary<string, string> _session;


            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select recording_id, session_name, sequence_id from recording_sessions where project_id = " + Amigo.App.currentProjectID + " order by sequence_id";//SequenceID

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                _session = new Dictionary<string, string>();
                                _session.Add("ID", ""+dr.GetInt32(0));
                                _session.Add("Name", dr.GetString(1));
                                _session.Add("SequenceID", dr.GetDouble(2).ToString());                                
                                _sessions.Add(_session);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return _sessions;
        }

        internal static double getNextRequestID()
        {
            double maxRequestID = 0;
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT max(request_id) from request_referers";

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                maxRequestID = dr.GetDouble(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return maxRequestID + (double)1.0;
        }

        internal static List<Dictionary<string, string>> getAllParams_for_request_ID(double request_id)
        {
            List<Dictionary<string, string>> _params = new List<Dictionary<string, string>>();
            Dictionary<string, string> _single_param = null;

            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select param_id, param_name,original_param_value,substituted_param_data_source,csv_column_name,iteration_mode,substututed_param_value,LB,RB from request_params where request_id = " + request_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                _single_param = new Dictionary<string, string>();
                                _single_param.Add("ParamID", "" + dr.GetInt32(0).ToString());
                                _single_param.Add("ParamName", dr.GetString(1));
                                _single_param.Add("OriginalParamValue", dr.GetString(2));
                                _single_param.Add("ParameterizationSource", dr.GetString(3));
                                _single_param.Add("CSVColumnName", dr.GetString(4));
                                _single_param.Add("IterationMode", dr.GetString(5));
                                _single_param.Add("DataPoolCSVName", dr.GetString(6));
                                _single_param.Add("LB", dr.GetValue(7).ToString());
                                _single_param.Add("RB", dr.GetValue(8).ToString());

                                _params.Add(_single_param);
                            }                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return _params;
        }

        internal static string[] getReferer_for_RequestID(double request_id)
        {
            string host_name = "";
            string port = "";
            string[] retVal = {"",""};

            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT parameterized_host_name ,parameterized_host_port from request_referers where request_id = " + request_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                host_name = dr.GetString(0);
                                port = ""+dr.GetInt32(1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            retVal[0] = host_name;
            retVal[1] = port;
            return retVal;
        }

        internal static void updateParameterizedHostPort(string host_name_text, string port_text, string _request_id)
        {
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "update request_referers set parameterized_host_name = '" + host_name_text + "'," +
                        " parameterized_host_port = " + Int32.Parse(port_text) +
                        " where project_id = (select project_id from request_referers where request_id = " + _request_id + " ) " +
                        " and host_name = (select host_name from request_referers where request_id = " + _request_id + " ) ";


                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
        }
        
        internal static void saveAll_Parameterized_Values(string ParamID, string _str_ParameterizationSource, string SubstututedParamValue, string SelectedCSVColumnName, string _str_SelectedIterationType,string LB, string RB)
        {
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query =  "update request_params set " +
                                    "substituted_param_data_source = " + "'" + _str_ParameterizationSource + "'," +
                                    "substututed_param_value = " + "'" + SubstututedParamValue + "'," +
                                    "csv_column_name = " + "'" + SelectedCSVColumnName + "'," +
                                    "iteration_mode = " + "'" + _str_SelectedIterationType + "', " +
                                    "LB = " + "'" + LB + "', " +
                                    "RB = " + "'" + RB + "'" +
                                    " where param_id = " + ParamID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
        }

        internal static List<Dictionary<string, string>> getAllHeaders_for_request_ID(string _request_id)
        {
            List<Dictionary<string, string>> _headers = new List<Dictionary<string, string>>();
            Dictionary<string, string> _single_header;

            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select item_name, item_value from request_headers where request_id = " + _request_id;
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                _single_header = new Dictionary<string, string>();
                                _single_header.Add("HeaderKey", dr.GetString(0));
                                byte[] headerVal = (byte[])dr.GetValue(1);
                                _single_header.Add("HeaderValue", Encoding.ASCII.GetString(headerVal));

                                _headers.Add(_single_header);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return _headers;
        }

        internal static int getRecordingSession_for_Request_id(double _request_id)
        {
            int recordingSessionID = 0;
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT recording_id from request_referers where request_id = " + _request_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                recordingSessionID = dr.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return recordingSessionID;
            }
            return recordingSessionID;
        }

        internal static string[] getMethod_for_request_ID(double request_id)
        {            
            string[] retval={"",""};
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT protocol,method from request_referers where request_id = " + request_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                retval[0] = dr.GetString(0);
                                retval[1] = dr.GetString(1);
                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return retval;
            }
            return retval;
        }

        public static string getTitle_for_request_ID(string request_id) 
        {
            string title = "";
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT title from request_referers where request_id = " + request_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                title = dr.GetString(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return title;
            }
            return title;
        }

        internal static void copyRequestHeaders(string copied_req_id, string new_req_id)
        {
            List<string> statements = new List<string>();

            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT * from request_headers where request_id = " + copied_req_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                int recording_id = dr.GetInt32(2);
                                int project_id = dr.GetInt32(3);
                                string item_name = dr.GetString(4);
                                string item_value = dr.GetString(5);
                                string statement =
                                "insert into request_headers(header_item_id,request_id,recording_id,project_id,item_name,item_value) values (NULL," +
                                new_req_id + " , " + recording_id + " , " + project_id + " , '" + item_name + "' , '" + item_value + "')";
                                statements.Add(statement);
                            }
                        }
                    }
                }

                for (int i = 0; i < statements.Count; i++)
                {
                    using (SQLiteConnection conn = new SQLiteConnection(connString))
                    {
                        String query = statements[i];
                        using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        internal static void copyRequestParams(string copied_req_id, string new_req_id)
        {
            List<string> statements = new List<string>();

            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT * from request_params where request_id = " + copied_req_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                int recording_id = dr.GetInt32(2);
                                int project_id = dr.GetInt32(3);
                                string param_name = dr.GetString(4);
                                string original_param_value = dr.GetString(5);
                                string substituted_param_data_source = dr.GetString(6);
                                string csv_column_name = dr.GetString(7);
                                string iteration_mode = dr.GetString(8);
                                string substututed_param_value = dr.GetString(9);

                                
                                string statement =
                                "insert into request_params(param_id,request_id,recording_id,project_id,param_name,original_param_value,"+
                                "substituted_param_data_source,csv_column_name,iteration_mode,substututed_param_value) values (NULL," +
                                new_req_id + " , " + 
                                recording_id + " , " + project_id + 
                                " , '" + param_name + 
                                "' , '" + original_param_value +
                                "' , '" + substituted_param_data_source +
                                "' , '" + csv_column_name +
                                "' , '" + iteration_mode +
                                "' , '" + substututed_param_value + 
                                "')";
                                statements.Add(statement);
                            }
                        }
                    }
                }

                for (int i = 0; i < statements.Count; i++)
                {
                    using (SQLiteConnection conn = new SQLiteConnection(connString))
                    {
                        String query = statements[i];
                        using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        internal static void copyRequestReferers(string copied_req_id, string new_req_id)
        {
            List<string> statements = new List<string>();

            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT * from request_referers where request_id = " + copied_req_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                int recording_id = dr.GetInt32(1);
                                int project_id = dr.GetInt32(2);
                                string protocol = dr.GetString(5);
                                string method = dr.GetString(6);
                                string host_name = dr.GetString(7);
                                int host_port = dr.GetInt32(8);
                                string parameterized_host_name = dr.GetString(9);
                                int parameterized_host_port = dr.GetInt32(10);
                                int response_code = dr.GetInt32(11);
                                string title = dr.GetString(12);

                                string statement =
                                "insert into request_referers(request_id,recording_id,project_id,protocol,method," +
                                "host_name,host_port,parameterized_host_name,parameterized_host_port,response_code,title) values (" +
                                new_req_id + " , " +
                                recording_id + " , " + 
                                project_id +
                                " , '" + protocol +
                                "' , '" + method +
                                "' , '" + host_name +
                                "' , " + host_port +
                                " , '" + parameterized_host_name +
                                "' , " + parameterized_host_port +
                                " , " + response_code +
                                " , '" + title +
                                "')";
                                statements.Add(statement);
                            }
                        }
                    }
                }

                for (int i = 0; i < statements.Count; i++)
                {
                    using (SQLiteConnection conn = new SQLiteConnection(connString))
                    {
                        String query = statements[i];
                        using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        internal static void copyResourcePaths(string copied_req_id, string new_req_id)
        {
            List<string> statements = new List<string>();

            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT * from resource_paths where request_id = " + copied_req_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                int recording_id = dr.GetInt32(2);
                                int project_id = dr.GetInt32(3);
                                string resource_path = dr.GetString(4);
                                string parameterized_resource_path = dr.GetString(5);                                

                                string statement =
                                "insert into resource_paths(resource_path_id,request_id,recording_id,project_id,resource_path,parameterized_resource_path) values (NULL," +
                                new_req_id + " , " +
                                recording_id + " , " +
                                project_id +
                                " , '" + resource_path +
                                "' , '" + parameterized_resource_path +                                
                                "')";
                                statements.Add(statement);
                            }
                        }
                    }
                }

                for (int i = 0; i < statements.Count; i++)
                {
                    using (SQLiteConnection conn = new SQLiteConnection(connString))
                    {
                        String query = statements[i];
                        using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                        {
                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        internal static void deleteRequest(string cut_req_id)
        {
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "delete from request_headers where request_id = " + cut_req_id;
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "delete from request_params where request_id = " + cut_req_id;
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "delete from request_referers where request_id = " + cut_req_id;
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "delete from resource_paths where request_id = " + cut_req_id;
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
        }

        internal static void update_project_name(int project_id, string project_name)
        {            
            DateTime date = DateTime.Now;

            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            String query = "update projects set project_name = '" + project_name + "' where project_id=" + project_id;

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        internal static void deleteProject(int project_id)
        {
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            String[] queries = {
                                 "delete from projects where project_id=" + project_id,
                                 "delete from recording_sessions where project_id=" + project_id,
                                 "delete from request_headers where project_id=" + project_id,
                                 "delete from request_params where project_id=" + project_id,
                                 "delete from request_referers where project_id=" + project_id,
                                 "delete from resource_paths where project_id=" + project_id,
                             };
            for (int i = 0; i < queries.Length; i++)
            {
                using (SQLiteConnection connection = new SQLiteConnection(connString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(queries[i], connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        internal static void deleteScript(int recording_id)
        {
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            String[] queries = {                                 
                                 "delete from recording_sessions where recording_id=" + recording_id,
                                 "delete from request_headers where recording_id=" + recording_id,
                                 "delete from request_params where recording_id=" + recording_id,
                                 "delete from request_referers where recording_id=" + recording_id,
                                 "delete from resource_paths where recording_id=" + recording_id,
                             };
            for (int i = 0; i < queries.Length; i++)
            {
                using (SQLiteConnection connection = new SQLiteConnection(connString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(queries[i], connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        internal static double getPreviousSequenceID(double SequenceID)
        {
            double previousSequence = 0;
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT max(sequence_id) from recording_sessions where sequence_id < " + SequenceID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                previousSequence = dr.GetDouble(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return previousSequence;
            }
            return previousSequence;
        }

        internal static void updateTitle(string req, string renamed_text)
        {
            DateTime date = DateTime.Now;

            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            String query = "update request_referers set title = '" + renamed_text + "' where request_id=" + req;

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        internal static string getPreviousRequestID(string copied_req_id)
        {
            double PreviousRequestID = 0;
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT max(request_id) from request_referers where request_id < " + copied_req_id;
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                PreviousRequestID = dr.GetDouble(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return PreviousRequestID.ToString();
            }
            return PreviousRequestID.ToString();
        }

        internal static string getNextRequestID(string copied_req_id)
        {
            double NextRequestID = 0;
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT min(request_id) from request_referers where request_id > " + copied_req_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                NextRequestID = dr.GetDouble(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return NextRequestID.ToString();
            }
            return NextRequestID.ToString();
        }

        internal static ViewModels.Scenario createScenario(string scenario_name, int project_id)
        {
            Scenario scenario = new Scenario();

            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "insert into scenario_runtime (ScenarioId,ProjectId,ScenarioName) values(NULL," + project_id + ",'" + scenario_name + "')";
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT MAX(ScenarioID) from scenario_runtime";
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                scenario.ScenarioID = dr.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return scenario;
        }

        internal static Scenario getScenario_for_ScenarioID(int ScenarioID, int project_id)
        {
            Scenario scenario = new Scenario();
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT * from scenario_runtime where ScenarioID = " + ScenarioID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                scenario.ScenarioID = ScenarioID;
                                scenario.ProjectID = project_id;
                                scenario.ScenarioName = dr.GetString(2);
                                scenario.TotalNumberOfUsers = dr.GetInt32(3);
                                scenario.ExecutionTime = dr.GetInt32(4);
                                scenario.isNumberOfUsersinPercent = dr.GetBoolean(5);
                                scenario.isLoggingEnabled = Boolean.Parse(dr.GetValue(6).ToString());
                                scenario.logLevel = dr.GetInt32(7);
                                scenario.TargetHost = dr.GetString(8);
                                scenario.TargetPort = dr.GetInt32(9);
                                scenario.errorLevel = dr.GetInt32(10);
                                scenario.wanEmulation = dr.GetInt32(11);                                
                                string str = dr.GetValue(12).ToString();
                                if (str == "1")
                                {
                                    scenario.simulateBrowserCache = true;
                                }
                                else
                                    scenario.simulateBrowserCache = false;
                                scenario.proxyHost = dr.GetString(13);
                                scenario.proxyPort = dr.GetInt32(14);
                                scenario.isTargetBasedOrLoadBased = dr.GetString(15);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return scenario;
        }

        internal static NotifiableObservableCollection<Scenario> getAllScenarios_for_Project_ID(int project_id)
        {
            NotifiableObservableCollection<Scenario> scenariosList = new NotifiableObservableCollection<Scenario>();
            Scenario scenario = null;
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT * from scenario_runtime where ProjectID = " + project_id + " order by ScenarioID DESC";

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                scenario = new Scenario();
                                scenario.ScenarioID = dr.GetInt32(0);
                                scenario.ProjectID = dr.GetInt32(1);
                                scenario.ScenarioName = dr.GetString(2);
                                scenario.TotalNumberOfUsers = dr.GetInt32(3);
                                scenario.ExecutionTime = dr.GetInt32(4);
                                int per = Int32.Parse(dr.GetValue(5).ToString());
                                if (per == 1)
                                {
                                    scenario.isNumberOfUsersinPercent = true;
                                }
                                else
                                { 
                                    scenario.isNumberOfUsersinPercent = false; 
                                }
                                scenario.isLoggingEnabled = Boolean.Parse(dr.GetValue(6).ToString());
                                scenario.logLevel = dr.GetInt32(7);
                                scenario.TargetHost = ""+dr.GetValue(8).ToString();
                                try
                                {
                                    scenario.TargetPort = Int32.Parse(dr.GetValue(9).ToString());
                                }
                                catch { scenario.TargetPort = 0; }
                                scenario.errorLevel = dr.GetInt32(10);
                                scenario.wanEmulation = dr.GetInt32(11);

                                int cache = Int32.Parse(dr.GetValue(12).ToString());
                                if (cache == 1)
                                {
                                    scenario.simulateBrowserCache = true;
                                }
                                else
                                {
                                    scenario.simulateBrowserCache = false;
                                }
                                scenario.proxyHost = "" + dr.GetValue(13).ToString();
                                
                                try
                                {
                                    scenario.proxyPort = Int32.Parse(dr.GetValue(14).ToString());
                                }
                                catch { scenario.proxyPort = 0; }
                                
                                scenario.isTargetBasedOrLoadBased = dr.GetString(15);
                                scenario.rampUpTime = dr.GetInt32(16);
                                scenario.rampDownTime = dr.GetInt32(17);
                                scenario.rampUserStep = dr.GetInt32(18);
                                scenario.rampTimeStep = dr.GetInt32(19);
                                try
                                {
                                    scenario.TPS = Int32.Parse(dr.GetInt32(20).ToString());
                                }
                                catch { scenario.TPS = 0; }
                                scenariosList.Add(scenario);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return scenariosList;
        }

        internal static ArrayList createScriptFromSession(int session_id, int scenario_id)
        {
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "insert into script_runtime (script_id,sequence_id,scenario_id) values(" + session_id + " , " + "NULL" + " , " + scenario_id + " )";
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }

            return retriveScripts_for_Scenario(scenario_id);
        }

        internal static ArrayList retriveScripts_for_Scenario(int scenario_id)
        {
            Script script = null;
            ArrayList Arrscript = new ArrayList();
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select * from script_runtime where scenario_id = " + scenario_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                script = new Script();
                                
                                script.ScriptID = Int32.Parse(dr.GetValue(0).ToString());
                                script.SequenceID = Int32.Parse(dr.GetValue(1).ToString());
                                script.ScenarioID = Int32.Parse(dr.GetValue(2).ToString());
                                script.NumberOfUsers = Int32.Parse(dr.GetValue(3).ToString());
                                script.StartAfter = Int32.Parse(dr.GetValue(4).ToString());
                                script.NumberOfIterations = Int32.Parse(dr.GetValue(5).ToString());
                                script.DelayBetweenIterationMin = Int32.Parse(dr.GetValue(6).ToString());
                                script.DelayBetweenIterationMax = Int32.Parse(dr.GetValue(7).ToString());
                                script.ThinkTimeMin = Int32.Parse(dr.GetValue(8).ToString());
                                script.ThinkTimeMax = Int32.Parse(dr.GetValue(9).ToString());

                                script.ScriptName = getScriptName(script.ScriptID);
                                Arrscript.Add(script);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return Arrscript;
        }

        private static string getScriptName(int recording_id)
        {
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            string scenario_name = "";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT session_name from recording_sessions where recording_id = " + recording_id + " and project_id = "+App.currentProjectID;
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                scenario_name = dr.GetString(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return scenario_name;
        }

        internal static void updateScript(Script srcipt)
        {
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "update script_runtime set " +
                    " NumberOfUsers = " + srcipt.NumberOfUsers + " , " +
                    " StartAfter = " + srcipt.StartAfter + " , " +
                    " NumberOfIterations = " + srcipt.NumberOfIterations + " , " +
                    " DelayBetweenIterationMin = " + srcipt.DelayBetweenIterationMin + " , " +
                    " DelayBetweenIterationMax = " + srcipt.DelayBetweenIterationMax + " , " +
                    " ThinkTimeMin = " + srcipt.ThinkTimeMin + " , " +
                    " ThinkTimeMax = " + srcipt.ThinkTimeMax + " where sequence_id = " + srcipt.SequenceID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.StackTrace);
            }
        }

        internal static void deleteScript_Runtime(int sequence_id)
        {
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            String[] queries = { "delete from script_runtime where sequence_id=" + sequence_id };
            for (int i = 0; i < queries.Length; i++)
            {
                using (SQLiteConnection connection = new SQLiteConnection(connString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(queries[i], connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        internal static void delete_Scenario_Runtime(int scenario_id)
        {
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            String[] queries = { "delete from scenario_runtime where ScenarioID = " + scenario_id, 
                                   "delete from script_runtime where scenario_id = "+scenario_id };
            for (int i = 0; i < queries.Length; i++)
            {
                using (SQLiteConnection connection = new SQLiteConnection(connString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(queries[i], connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        internal static void saveScenario(Scenario _Scenario)
        {
            try
            {
                int intNumberOfUsersinPercent = 0;
                if (_Scenario.isNumberOfUsersinPercent)
                {
                    intNumberOfUsersinPercent = 1;
                }
                else
                {
                    intNumberOfUsersinPercent = 0;
                }

                int intSimulateBrowserCache = 0;
                if (_Scenario.simulateBrowserCache)
                {
                    intSimulateBrowserCache = 1;
                }
                else 
                {
                    intSimulateBrowserCache = 0;
                }

                String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
                String query = "update scenario_runtime set "+
                                "TotalNumberOfUsers = " + _Scenario.TotalNumberOfUsers +
                                ",ExecutionTime = " + _Scenario.ExecutionTime +
                                ",isNumberOfUsersinPercent = " + intNumberOfUsersinPercent+
                                ",isLoggingEnabled = '" + _Scenario.isLoggingEnabled.ToString()+"' " +
                                ",logLevel = " + _Scenario.logLevel +
                                ",TargetHost = '" + _Scenario.TargetHost +"' "+
                                ",TargetPort = " + _Scenario.TargetPort +
                                ",errorLevel = " + _Scenario.errorLevel +
                                ",wanEmulation = " + _Scenario.wanEmulation +
                                ",simulateBrowserCache = " + intSimulateBrowserCache +
                                ",proxyHost = '" + _Scenario.proxyHost +"' "+
                                ",proxyPort = " + _Scenario.proxyPort +
                                ",isTargetBasedOrLoadBased = '" + _Scenario.isTargetBasedOrLoadBased +"' "+
                                ",rampUpTime = " + _Scenario.rampUpTime +
                                ",rampDownTime = " + _Scenario.rampDownTime +
                                ",rampUserStep = " + _Scenario.rampUserStep +
                                ",rampTimeStep = " + _Scenario.rampTimeStep +
                                ",TPS = " + _Scenario.TPS +
                                " where ScenarioID = "+_Scenario.ScenarioID;
                
                using (SQLiteConnection connection = new SQLiteConnection(connString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }                
            }
            catch { }
        }

        internal static ArrayList retrieveServerResourceMonitors(int scenario_id)
        {
            String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");            
            ArrayList monitors = new ArrayList();            
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT * from server_monitors where scenario_id = " + scenario_id;
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ServerMonitor singleMonitor = new ServerMonitor();
                                
                                singleMonitor.MonitorId = dr.GetInt32(0);
                                singleMonitor.ScenarioId = dr.GetInt32(1);
                                singleMonitor.MonitorName = dr.GetString(2);
                                singleMonitor.Host = dr.GetString(3);
                                singleMonitor.Port = dr.GetInt32(4);
                                singleMonitor.PollingFrequency = dr.GetInt32(5);

                                monitors.Add(singleMonitor);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return monitors;
        }

        internal static string getPreviousResponse(double request_id)
        {
            double previous_request_id = getPreviousRequestID_with_title(request_id);

            if (previous_request_id == -1) return "";

            String connString = "Data Source=" + Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            string response = "";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select response_body from request_referers where request_id = " + previous_request_id;
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                response = ASCIIEncoding.ASCII.GetString((byte[])dr.GetValue(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return response;           
        }

        private static double getPreviousRequestID_with_title(double request_id)
        {
            String connString = "Data Source=" + Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            double previous_id = -1;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT max(request_id) from request_referers where request_id < " + request_id+
                        " and title != ''";
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Object obj = dr.GetValue(0);
                                if (obj.ToString() != "") previous_id = double.Parse(obj.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return previous_id;
        }

        internal static string getProjectName(int curr_project_id)
        {
            string project_name = "";
            String connString = "Data Source=" + Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT project_name from projects where project_id=" + curr_project_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                project_name = dr.GetString(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return project_name;
            }
            return project_name;
        }
    }   
}