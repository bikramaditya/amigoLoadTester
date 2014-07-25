using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Collections;
using Amigo.ViewModels;
using System.Threading;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;

namespace Amigo.Interfaces
{
    public class ReportsDAO
    {
        static String connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");

        internal static void retrieveExecutions(int project_id, ref BackgroundWorker bw)
        {
            NotifiableObservableCollection<KeyValuePair<int, string>> _test_executions = new NotifiableObservableCollection<KeyValuePair<int, string>>();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT execution_id,scenario_id,start_date_time,end_date_time from test_executions where project_id = "+project_id +" order by execution_id desc";

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            int count = 0;
                            while (dr.Read())
                            {
                                int execution_id = dr.GetInt32(0);
                                int scenario_id = dr.GetInt32(1);
                                string start_date = dr.GetValue(2).ToString();
                                string end_date = dr.GetValue(3).ToString();
                                Scenario _Scenario = ScriptScenarioDAO.getScenario_for_ScenarioID(scenario_id, project_id);
                                string execution_name = execution_id + "-" + start_date+ "- to -" + end_date+ "- to - for " + _Scenario.ExecutionTime + " min and " + _Scenario.TotalNumberOfUsers + " users";
                                KeyValuePair<int, string> _key_val = new KeyValuePair<int, string>(execution_id, execution_name);
                                _test_executions.Add(_key_val);
                                count++;
                                bw.ReportProgress(count);                                
                                Thread.Sleep(10);
                            }
                        }
                    }
                }
                ReportsStudio._TestExecutionDetails.TestExecutions = _test_executions;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);             
            }
        }

        internal static void saveReport(Report _Report)
        {
            if (_Report.ReportID == -1) addReport(_Report);
            else updateReport(_Report);
        }

        private static void updateReport(Report _Report)
        {
            String query = "update test_reports set "+
            "ReportName = @ReportName, "+
            "YourLogoText = @YourLogoText, "+
            "YourLogoImage = @YourLogoImage, "+
            "CustomerLogoText = @CustomerLogoText, "+
            "CustomerLogoImage = @CustomerLogoImage, "+
            "TestDescription = @TestDescription, "+
            "TimeFrame = @TimeFrame, "+
            "DeploymentDiagramText = @DeploymentDiagramText, "+
            "DeploymentDiagramImage = @DeploymentDiagramImage, " +
            "WebServersText = @WebServersText, "+
            "AppServersText = @AppServersText, "+
            "DbServersText = @DbServersText, "+
            "OtherServersText = @OtherServersText, "+
            "SelectedSections = @SelectedSections, "+
            "ReportFormat = @ReportFormat where ReportID = "+_Report.ReportID;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.Add(new SQLiteParameter("ReportName", _Report.ReportName));
                        command.Parameters.Add(new SQLiteParameter("YourLogoText", _Report.YourLogoText));
                        command.Parameters.Add(new SQLiteParameter("YourLogoImage", _Report.YourLogoImage));
                        command.Parameters.Add(new SQLiteParameter("CustomerLogoText", _Report.CustomerLogoText));
                        command.Parameters.Add(new SQLiteParameter("CustomerLogoImage", _Report.CustomerLogoImage));                        
                        command.Parameters.Add(new SQLiteParameter("TestDescription", _Report.TestDescription));
                        command.Parameters.Add(new SQLiteParameter("TimeFrame", _Report.TimeFrame));
                        command.Parameters.Add(new SQLiteParameter("DeploymentDiagramText", _Report.DeploymentDiagramText));
                        command.Parameters.Add(new SQLiteParameter("DeploymentDiagramImage", _Report.DeploymentDiagramImage));
                        command.Parameters.Add(new SQLiteParameter("WebServersText", _Report.WebServersText));
                        command.Parameters.Add(new SQLiteParameter("AppServersText", _Report.AppServersText));
                        command.Parameters.Add(new SQLiteParameter("DbServersText", _Report.DbServersText));
                        command.Parameters.Add(new SQLiteParameter("OtherServersText", _Report.OtherServersText));
                        command.Parameters.Add(new SQLiteParameter("SelectedSections", _Report.SelectedSections));
                        command.Parameters.Add(new SQLiteParameter("ReportFormat", _Report.ReportFormat));

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }


        public static void addReport(Report _Report)
        {
            _Report.ReportID = getNextReportID();

            String query = "insert into test_reports (ReportName,YourLogoText,YourLogoImage,CustomerLogoText,CustomerLogoImage,ReportID,ProjectID,TestDescription,TimeFrame,DeploymentDiagramText,DeploymentDiagramImage,WebServersText,AppServersText,DbServersText,OtherServersText,SelectedSections,ReportFormat) values(@ReportName,@YourLogoText,@YourLogoImage,@CustomerLogoText,@CustomerLogoImage,@ReportID,@ProjectID,@TestDescription,@TimeFrame,@DeploymentDiagramText,@DeploymentDiagramImage,@WebServersText,@AppServersText,@DbServersText,@OtherServersText,@SelectedSections,@ReportFormat)";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.Add(new SQLiteParameter("ReportName", _Report.ReportName));
                        command.Parameters.Add(new SQLiteParameter("YourLogoText", _Report.YourLogoText));
                        command.Parameters.Add(new SQLiteParameter("YourLogoImage", _Report.YourLogoImage));
                        command.Parameters.Add(new SQLiteParameter("CustomerLogoText", _Report.CustomerLogoText));
                        command.Parameters.Add(new SQLiteParameter("CustomerLogoImage", _Report.CustomerLogoImage));
                        command.Parameters.Add(new SQLiteParameter("ReportID" , _Report.ReportID));
                        command.Parameters.Add(new SQLiteParameter("ProjectID", _Report.ProjectID));
                        command.Parameters.Add(new SQLiteParameter("TestDescription", _Report.TestDescription));
                        command.Parameters.Add(new SQLiteParameter("TimeFrame", _Report.TimeFrame));
                        command.Parameters.Add(new SQLiteParameter("DeploymentDiagramText", _Report.DeploymentDiagramText));
                        command.Parameters.Add(new SQLiteParameter("DeploymentDiagramImage", _Report.DeploymentDiagramImage));
                        command.Parameters.Add(new SQLiteParameter("WebServersText", _Report.WebServersText));
                        command.Parameters.Add(new SQLiteParameter("AppServersText", _Report.AppServersText));
                        command.Parameters.Add(new SQLiteParameter("DbServersText", _Report.DbServersText));
                        command.Parameters.Add(new SQLiteParameter("OtherServersText", _Report.OtherServersText));
                        command.Parameters.Add(new SQLiteParameter("SelectedSections", _Report.SelectedSections));
                        command.Parameters.Add(new SQLiteParameter("ReportFormat", _Report.ReportFormat));

                        command.ExecuteNonQuery();
                    }
                }
            }catch { }
        }

        private static int getNextReportID()
        {
            int NextExecutionID = 0;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT max(ReportID) from test_reports";

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                NextExecutionID = dr.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return NextExecutionID;
            }
            return NextExecutionID + 1;
        }

        internal void updateTestExecution(int current_execution_id, int project_id, int scenario_id)
        {
            DateTime end_date_time = DateTime.Now;
            String query = "update test_executions set end_date_time = @end_date_time where execution_id=@execution_id";

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.Add(new SQLiteParameter("end_date_time", end_date_time));
                    command.Parameters.Add(new SQLiteParameter("execution_id", current_execution_id));
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch { }
                }
            }
        }

        internal static int getExecutionCount(int p)
        {
            int count = 0;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT count(1) from test_executions where project_id = "+p;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                count = dr.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return count;
            }
            return count;
        }



        internal static Report retrieveReport(int project_id)
        {
            Report _Report = new Report();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT * from test_reports where ProjectID = " + project_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {                                
                                _Report.ReportName = dr.GetString(0);
                                _Report.YourLogoText = dr.GetString(1);
                                try
                                {
                                    _Report.YourLogoImage = (byte[])dr.GetValue(2);
                                }
                                catch { }
                                _Report.CustomerLogoText = dr.GetString(3);
                                try
                                {
                                    _Report.CustomerLogoImage = (byte[])dr.GetValue(4);
                                }
                                catch { }
                                _Report.ReportID = dr.GetInt32(5);
                                _Report.ProjectID = dr.GetInt32(6);
                                _Report.TestDescription = dr.GetString(7);
                                _Report.TimeFrame = dr.GetBoolean(8);
                                _Report.DeploymentDiagramText = dr.GetString(9);
                                try
                                {
                                    _Report.DeploymentDiagramImage = (byte[])dr.GetValue(10);
                                }
                                catch { }
                                _Report.WebServersText = dr.GetString(11);
                                _Report.AppServersText = dr.GetString(12);
                                _Report.DbServersText = dr.GetString(13);
                                _Report.OtherServersText = dr.GetString(14);
                                _Report.SelectedSections = dr.GetString(15);
                                _Report.ReportFormat = dr.GetString(16);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return _Report;
            }
            return _Report;
        }

        internal static ArrayList getPrinaryRequests_for_Execution(int selectedExecutionID)
        {
            ArrayList txnList = new ArrayList();

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select distinct(request_id) from test_execution_metrics where parent_id = -1 and execution_id = "+selectedExecutionID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                txnList.Add (dr.GetDouble(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return txnList;
            }            
            return txnList;
        }

        internal static ArrayList getIterations_for_RequestID(double request_id, int execution_id)
        {
            int iteration = -1;
            ArrayList iterations = new ArrayList();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select distinct(iteration_no) from test_execution_metrics " +
                                    " where execution_id = " + execution_id +
                                    " and (request_id = " + request_id + " or parent_id = " + request_id + ") " +
                                    " and response_code = 200 " +
                                    " order by iteration_no ";

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                iteration = dr.GetInt32(0);
                                iterations.Add(iteration);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.Write(ex.Message); }
            return iterations;
        }
        public static double getResponseTime_for_Iteration(int iteration_no, int execution_id)
        {            
            double parent_resp_time = getParentRequest_Resp_Time(iteration_no, execution_id);       
            double child_resp_time = getChildRequest_Resp_Time(iteration_no, execution_id);
            return parent_resp_time + child_resp_time;            
        }
        private static double getParentRequest_Resp_Time(int iteration_no, int execution_id)
        {
            DateTime startDateTime = new DateTime();
            DateTime endDateTime = new DateTime();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select start_date_time,end_date_time from test_execution_metrics " +
                                    " where execution_id = " + execution_id +
                                    " and response_code = 200 " +
                                    " and iteration_no = " + iteration_no +
                                    " and parent_id = -1 " +
                                    " order by iteration_no ";
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                startDateTime = dr.GetDateTime(0);
                                endDateTime = dr.GetDateTime(1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.Write(ex.Message); }
            TimeSpan time_diff = endDateTime - startDateTime;
            return time_diff.TotalMilliseconds;
        }
        private static double getChildRequest_Resp_Time(int iteration_no, int execution_id)
        {
            double max_resp_time = 0;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select MIN(start_date_time),MAX(end_date_time) from test_execution_metrics " +
                                    " where execution_id = " + execution_id +
                                    " and iteration_no = " + iteration_no +
                                    " and response_code = 200 " +
                                    " and parent_id != -1 " +
                                    " order by iteration_no ";
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.HasRows)
                            {
                                while (dr.Read())
                                {
                                    object obj = dr.GetValue(0);
                                    string str = obj.ToString();

                                    if (obj.ToString() != "")
                                    {
                                        DateTime startDateTime = Convert.ToDateTime(str);
                                        DateTime endDateTime = dr.GetDateTime(1);
                                        TimeSpan time_diff = endDateTime - startDateTime;
                                        if (time_diff.TotalMilliseconds > max_resp_time)
                                        { max_resp_time = time_diff.TotalMilliseconds; }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception) { }
            return max_resp_time;
        }

        internal static Scenario retrieveScenario_for_Execution_ID(int selectedExecutionID)
        {
            int scenario_id = getScenarioID_for_execution_ID(selectedExecutionID);
            return ScriptScenarioDAO.getScenario_for_ScenarioID(scenario_id,App.currentProjectID);
        }

        private static int getScenarioID_for_execution_ID(int selectedExecutionID)
        {
            int scenario_id = 0;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT scenario_id from test_executions where execution_id = " + selectedExecutionID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                scenario_id = dr.GetInt32(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return scenario_id;
            }
            return scenario_id;
        }

        internal static NotifiableObservableCollection<Script> getScripts_For_Scenario(Scenario _Scenario)
        {
            Scenario scn = _Scenario;
            ArrayList arrScript = DBProcessingLayer.retriveScripts_for_Scenario(scn.ScenarioID);
            NotifiableObservableCollection<Script> Scripts = new NotifiableObservableCollection<Script>();
            for (int j = 0; j < arrScript.Count; j++)
            {
                Scripts.Add((Script)arrScript[j]);
            }
            return Scripts;
        }

        internal static string getTotalTxnCount(int selectedExecutionID)
        {
            string totalCount = "0";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select count(1) from test_execution_metrics where execution_id = " + selectedExecutionID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                object obj = dr.GetValue(0);
                                if (obj.ToString() != "")
                                {
                                    totalCount = obj.ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.Write(ex.Message); }
            return totalCount;
        }

        internal static string getTotalPassCount(int selectedExecutionID)
        {
            string errorCount = "0";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select count(1) from test_execution_metrics where (response_code like '2%' or response_code like '3%') and execution_id = " +selectedExecutionID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                object obj = dr.GetValue(0);
                                if (obj.ToString() != "")
                                {
                                    errorCount = obj.ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.Write(ex.Message); }
            return errorCount;
        }

        internal static DateTime getTestStartTime(int selectedExecutionID)
        {
            DateTime _start_time = new DateTime();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select start_date_time from test_executions where execution_id = " + selectedExecutionID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                _start_time = dr.GetDateTime(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.Write(ex.Message); }
            return _start_time;
        }        

        internal static DateTime getIterationTime(int iteration_id,int execution_id)
        {
            DateTime _start_time = new DateTime();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select start_date_time from test_execution_metrics where iteration_no = " + iteration_id + " and execution_id = " + execution_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                _start_time = dr.GetDateTime(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.Write(ex.Message); }
            return _start_time;
        }

        internal static ArrayList getUniqueContentTypes(int selectedExecutionID)
        {
            ArrayList contentTypeList = new ArrayList();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select distinct(content_type) from test_execution_metrics where execution_id = " + selectedExecutionID;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                try
                                {
                                    Object obj = dr.GetValue(0);
                                    if (obj != null && obj.ToString() != "")
                                    {
                                        contentTypeList.Add(obj.ToString());
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.Write(ex.Message); }
            return contentTypeList;
        }

        internal static double getTotalRespSize_for_contentType(string contentType, int selectedExecutionID)
        {
            double size = 0.0;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select sum(response_size) from test_execution_metrics where execution_id = " + selectedExecutionID + " and content_type = '" + contentType + "'";

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                try
                                {                                    
                                    size = dr.GetDouble(0);                                    
                                }
                                catch { }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.Write(ex.Message); }
            return size;
        }

        internal static List<KeyValuePair<string, byte[]>> getAllStoredGraphs_for_execution(int selectedExecutionID)
        {
            List<KeyValuePair<string, byte[]>> value_list = new List<KeyValuePair<string, byte[]>>();
            String connString = "Data Source=" + Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT graph_name,graph_data from test_execution_graphs where execution_id = " + selectedExecutionID;
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string name = dr.GetString(0);
                                byte[] data = (byte[])dr.GetValue(1);
                                KeyValuePair<string, byte[]> key_val = new KeyValuePair<string, byte[]>(name, data);
                                value_list.Add(key_val);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return value_list;
            }
            return value_list;
        }
    }
}
