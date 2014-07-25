using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Collections;
using System.IO;

namespace Amigo.Interfaces
{
    public class ExecutionDAO
    {
        String connString = "";
        public ExecutionDAO()
        {
            connString = "Data Source="+Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
        }
        public int addTestExecution(int project_id, int scenario_id)
        {
            int execution_id = getNextExecutionID();
            DateTime start_date_time = DateTime.Now;
            String query = "insert into test_executions (execution_id, project_id, scenario_id,start_date_time) values(@execution_id,@project_id,@scenario_id,@start_date_time)";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.Add(new SQLiteParameter("execution_id", execution_id));
                        command.Parameters.Add(new SQLiteParameter("project_id", project_id));
                        command.Parameters.Add(new SQLiteParameter("scenario_id", scenario_id));
                        command.Parameters.Add(new SQLiteParameter("start_date_time", start_date_time));
                        command.ExecuteNonQuery();
                    }
                }
            }catch { }
            return execution_id;
        }
        private int getNextExecutionID()
        {
            int NextExecutionID = 0;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT max(execution_id) from test_executions";

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
        public int addTestEvent(int execution_id, int project_id, int scenario_id, string event_name)//event_id,execution_id,project_id,scenario_id,date_time,event_name
        {
            DateTime date_time = DateTime.Now;
            String query = "insert into test_execution_events (event_id, execution_id, project_id, scenario_id,date_time,event_name) values(@event_id,@execution_id,@project_id,@scenario_id,@date_time,@event_name)";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.Add(new SQLiteParameter("event_id", null));
                        command.Parameters.Add(new SQLiteParameter("execution_id", execution_id));
                        command.Parameters.Add(new SQLiteParameter("project_id", project_id));
                        command.Parameters.Add(new SQLiteParameter("scenario_id", scenario_id));
                        command.Parameters.Add(new SQLiteParameter("date_time", date_time));
                        command.Parameters.Add(new SQLiteParameter("event_name", event_name));
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch { }
            return execution_id;
        }
        public void addResponseDetails(double request_id, double parent_id, int thread_id, int iteration_no, int project_id,
            int scenario_id, DateTime start_date_time, DateTime end_date_time, int response_code, string error_message, double request_size, double response_size, string content_type, double response_time)
        {
            response_time = (double)response_time / 1000;
            int current_execution_id = ExecutionStudio.current_execution_id;

            String query = "insert into test_execution_metrics (request_id,parent_id,thread_id,iteration_no,project_id,scenario_id,execution_id,start_date_time,start_millisec,end_date_time,end_millisec,response_code,error_message,response_size,content_type,response_time,request_size)" +
                " values(@request_id,@parent_id,@thread_id,@iteration_no,@project_id,@scenario_id,@execution_id,@start_date_time,@start_millisec,@end_date_time,@end_millisec,@response_code,@error_message,@response_size,@content_type,@response_time,@request_size)";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.Add(new SQLiteParameter("request_id",request_id ));
                        command.Parameters.Add(new SQLiteParameter("parent_id",parent_id ));
                        command.Parameters.Add(new SQLiteParameter("thread_id", thread_id));
                        command.Parameters.Add(new SQLiteParameter("iteration_no", iteration_no));
                        command.Parameters.Add(new SQLiteParameter("project_id",project_id ));
                        command.Parameters.Add(new SQLiteParameter("scenario_id",scenario_id ));
                        command.Parameters.Add(new SQLiteParameter("execution_id", current_execution_id));
                        command.Parameters.Add(new SQLiteParameter("start_date_time",start_date_time));
                        command.Parameters.Add(new SQLiteParameter("start_millisec", start_date_time.Millisecond));                        
                        command.Parameters.Add(new SQLiteParameter("end_date_time", end_date_time));
                        command.Parameters.Add(new SQLiteParameter("end_millisec", end_date_time.Millisecond));
                        command.Parameters.Add(new SQLiteParameter("response_code", response_code));
                        command.Parameters.Add(new SQLiteParameter("error_message", error_message));
                        command.Parameters.Add(new SQLiteParameter("response_size",response_size ));
                        command.Parameters.Add(new SQLiteParameter("content_type", content_type));
                        command.Parameters.Add(new SQLiteParameter("response_time", response_time));
                        command.Parameters.Add(new SQLiteParameter("request_size", request_size));                        
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch { }
        }
        public static Object respLock = new Object();
        internal List<KeyValuePair<double, double>> getResponseTimeFor_request(double request_id)
        {
            lock (respLock)
            {
                DateTime start_time = getTestStartTime();
                List<KeyValuePair<double, double>> responseTimeList= new List<KeyValuePair<double, double>>();

                ArrayList iteration_numbers = getAllIterations_For_Request(request_id);
                int factor = (int)Math.Ceiling((double)(iteration_numbers.Count) / 25);
                if (factor < 5) factor = 5;
                for (int i = 0; iteration_numbers!=null && i < iteration_numbers.Count; i++)
                {
                    if (i % factor == 0)
                    {
                        DateTime time_then = getThisExecutionTime((int)iteration_numbers[i]);
                        TimeSpan _TimeSpan = time_then - start_time;
                        double minutes = _TimeSpan.TotalMinutes;
                        minutes = Math.Round(minutes,1);
                        double resp_time = getResponseTime_for_Iteration((int)iteration_numbers[i]);
                        KeyValuePair<double, double> _resp_key_val = new KeyValuePair<double, double>(minutes, (double)(resp_time / 1000));
                        responseTimeList.Add(_resp_key_val);
                    }
                }            
                return responseTimeList;
            }
        }
        private DateTime getThisExecutionTime(int iteration_no)
        {
            DateTime _this_time = new DateTime();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = " select MAX(start_date_time) from test_execution_metrics " +
                                    " where project_id = " + ExecutionStudio._Scenario.ProjectID +
                                    " and scenario_id = " + ExecutionStudio._Scenario.ScenarioID +
                                    " and execution_id = " + ExecutionStudio.current_execution_id +
                                    " and iteration_no = "+iteration_no;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                _this_time = dr.GetDateTime(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.Write(ex.Message); }
            return _this_time;
        }

        private double getResponseTime_for_Iteration(int iteration_no)
        {
            double parent_resp_time = getParentRequest_Resp_Time(iteration_no);
            double child_resp_time = getChildRequest_Resp_Time(iteration_no);
            return parent_resp_time + child_resp_time;
        }

        private double getChildRequest_Resp_Time(int iteration_no)
        {
            double max_resp_time = 0;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select MIN(start_date_time),MAX(end_date_time) from test_execution_metrics " +
                                    " where project_id = " + ExecutionStudio._Scenario.ProjectID +
                                    " and scenario_id = " + ExecutionStudio._Scenario.ScenarioID +
                                    " and execution_id = " + ExecutionStudio.current_execution_id +
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
                                    
                                    if(obj.ToString() != "")
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
            catch (Exception) {}
            return max_resp_time;
        }

        private double getParentRequest_Resp_Time(int iteration_no)
        {
            DateTime startDateTime = new DateTime();
            DateTime endDateTime = new DateTime();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select start_date_time,end_date_time from test_execution_metrics " +
                                    " where project_id = " + ExecutionStudio._Scenario.ProjectID +
                                    " and scenario_id = " + ExecutionStudio._Scenario.ScenarioID +
                                    " and  execution_id = " + ExecutionStudio.current_execution_id +
                                    " and response_code = 200 " +
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

        public DateTime getTestStartTime()
        {
            DateTime _start_time = new DateTime();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select start_date_time from test_executions " +
                                    " where project_id = " + ExecutionStudio._Scenario.ProjectID +
                                    " and scenario_id = " + ExecutionStudio._Scenario.ScenarioID +
                                    " and  execution_id = " + ExecutionStudio.current_execution_id;

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

        private ArrayList getAllIterations_For_Request(double request_id)
        {
            int iteration = -1;
            ArrayList iterations = new ArrayList();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select distinct(iteration_no) from test_execution_metrics "+
                                    " where project_id = "+ExecutionStudio._Scenario.ProjectID+
                                    " and scenario_id = "+ExecutionStudio._Scenario.ScenarioID+
                                    " and  execution_id = "+ExecutionStudio.current_execution_id+
                                    " and (request_id = " + request_id + " or parent_id = " + request_id + ") " +
                                    " and response_code = 200 "+
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
            catch (Exception ex){Console.Write(ex.Message);}
            return iterations;
        }

        internal NotifiableObservableCollection<ViewModels.TransactionMetrics> getTransactionSummaryMetrices()
        {
            NotifiableObservableCollection<ViewModels.TransactionMetrics> _TransactionMetrics = new NotifiableObservableCollection<ViewModels.TransactionMetrics>();
            for (int i = 0; ExecutionStudio.txnNameList != null && i < ExecutionStudio.txnNameList.Count; i++)
            {
                double request_id = ((KeyValuePair<double, string>)ExecutionStudio.txnNameList[i]).Key;
                List<KeyValuePair<double, double>> _resp_List = getResponseTimeFor_request(request_id);
                _resp_List.Sort(Compare2);
                if (_resp_List.Count > 0)
                {
                    ViewModels.TransactionMetrics _singleMetric = new ViewModels.TransactionMetrics();
                    _singleMetric.TransactionID = request_id;
                    _singleMetric.TransactionName = "Series_" + request_id;
                    _singleMetric.DisplayName = (ExecutionStudio.txnNameList.Find(x => x.Key == request_id)).Value;
                    _singleMetric.RespTimeMin = Math.Round((_resp_List.Min<KeyValuePair<double, double>>(x => x.Value)), 2).ToString();
                    _singleMetric.RespTimeMax = Math.Round((_resp_List.Max<KeyValuePair<double, double>>(x => x.Value)),2).ToString();
                    _singleMetric.RespTimeAvg = Math.Round((_resp_List.Average(x => x.Value)), 2).ToString();

                    int pos85 = (int)Math.Floor((0.85) * _resp_List.Count + 0.5) - 1;
                    int pos90 = (int)Math.Floor((0.90) * _resp_List.Count + 0.5) - 1;
                    int pos95 = (int)Math.Floor((0.95) * _resp_List.Count + 0.5) - 1;

                    _singleMetric.RespTime85 = Math.Round(_resp_List[pos85].Value,2).ToString();
                    _singleMetric.RespTime90 = Math.Round(_resp_List[pos90].Value, 2).ToString();
                    _singleMetric.RespTime95 = Math.Round(_resp_List[pos95].Value, 2).ToString();
                    _TransactionMetrics.Add(_singleMetric);
                }
            }
            return _TransactionMetrics;
        }
        static int Compare2(KeyValuePair<double, double> a, KeyValuePair<double, double> b)
        {
            return a.Value.CompareTo(b.Value);
        }

        internal KeyValuePair<int, int> getLatestTPS()
        {
            int graphTime = 0;
            int count = 0;
            if (ExecutionStudio._ResponseTimePointList != null && ExecutionStudio._ResponseTimePointList.Count > 0)
            {
                try
                {
                    DateTime max_date_time = getMAXTxn_DateTime();
                    if (max_date_time == new DateTime())
                    {
                        return new KeyValuePair<int, int>(0, 0);
                    }
                    TimeSpan _interval = new TimeSpan(0, 0, 15);
                    DateTime b4_date_time = max_date_time.Subtract(_interval);
                    count = getIntervalTXNCount(b4_date_time);
                    DateTime start_time = getTestStartTime();
                    TimeSpan minutes = max_date_time - start_time;
                    graphTime = (int)minutes.TotalMinutes;
                    return new KeyValuePair<int, int>((int)graphTime, (int)(count / 15)); 
                }
                catch { return new KeyValuePair<int, int>((int)graphTime, (int)(count / 15)); }
            }
            else return new KeyValuePair<int, int>(0, 0);            
        }

        private int getIntervalTXNCount(DateTime b4_date_time)
        {
            int count = 0;
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connString))
                {
                    String query = "select count(1) from test_execution_metrics " +
                                        " where project_id = " + ExecutionStudio._Scenario.ProjectID +
                                        " and scenario_id = " + ExecutionStudio._Scenario.ScenarioID +
                                        " and  execution_id = " + ExecutionStudio.current_execution_id +
                                        " and end_date_time > @end_time";

                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.Add(new SQLiteParameter("end_time", b4_date_time));
                        using (SQLiteDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                count = dr.GetInt32(0);
                            }
                        }
                    }
                }
            }catch { }
            return count;
        }

        private DateTime getMAXTxn_DateTime()
        {
            DateTime _start_time = new DateTime();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select max(end_date_time) from test_execution_metrics " +
                                    " where project_id = " + ExecutionStudio._Scenario.ProjectID +
                                    " and scenario_id = " + ExecutionStudio._Scenario.ScenarioID +
                                    " and  execution_id = " + ExecutionStudio.current_execution_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                string obj = dr.GetValue(0).ToString();
                                if (obj == "")
                                {
                                    _start_time = new DateTime();
                                }
                                else
                                { _start_time = dr.GetDateTime(0);}                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.Write(ex.Message); }
            return _start_time;
        }

        internal string getAverageRespTime_For_All_Txn()
        {
            double resp_time = 0;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select avg(response_time) from test_execution_metrics " +
                                    " where project_id = " + ExecutionStudio._Scenario.ProjectID +
                                    " and scenario_id = " + ExecutionStudio._Scenario.ScenarioID +
                                    " and  execution_id = " + ExecutionStudio.current_execution_id;

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Object obj = dr.GetValue(0);
                                if (obj.ToString() != "")
                                {
                                    resp_time = Double.Parse(obj.ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.Write(ex.Message); }
            return Math.Round(resp_time, 3).ToString();
        }

        internal string getErrorCount()
        {
            string errorCount = "0";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select count(1) from test_execution_metrics where response_code not like '2%' and response_code not like '3%' " +
                                    " and project_id = " + ExecutionStudio._Scenario.ProjectID +
                                    " and scenario_id = " + ExecutionStudio._Scenario.ScenarioID +
                                    " and  execution_id = " + ExecutionStudio.current_execution_id;

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

        internal string getTotalTxnCount()
        {
            string totalCount = "0";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select count(1) from test_execution_metrics " +
                                    " where project_id = " + ExecutionStudio._Scenario.ProjectID +
                                    " and scenario_id = " + ExecutionStudio._Scenario.ScenarioID +
                                    " and  execution_id = " + ExecutionStudio.current_execution_id;

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

        internal KeyValuePair<double, double> getThroughPut(int interval_in_sec)
        {
            int graphTime = 0;
            double totalKB = 0;
            if (ExecutionStudio._ResponseTimePointList != null && ExecutionStudio._ResponseTimePointList.Count > 0)
            {
                try
                {
                    DateTime max_date_time = getMAXTxn_DateTime();
                    if (max_date_time == new DateTime())
                    {
                        return new KeyValuePair<double, double>(0, 0);
                    }
                    TimeSpan _interval = new TimeSpan(0, 0, interval_in_sec);
                    DateTime b4_date_time = max_date_time.Subtract(_interval);
                    
                    DateTime start_time = getTestStartTime();
                    TimeSpan minutes = max_date_time - start_time;
                    graphTime = (int)minutes.TotalMinutes;

                    totalKB = getTotalDataTransfered(b4_date_time, max_date_time);

                    return new KeyValuePair<double, double>((int)graphTime, (double)(totalKB / interval_in_sec));
                }
                catch { return new KeyValuePair<double, double>((int)graphTime, (double)(totalKB / interval_in_sec)); }
            }
            else return new KeyValuePair<double, double>(0, 0);
        }

        private double getTotalDataTransfered(DateTime b4_date_time, DateTime max_date_time)
        {
            double request_size = 0;
            double response_size = 0;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select sum(response_size), sum(request_size) from test_execution_metrics " +
                                    " where project_id = " + ExecutionStudio._Scenario.ProjectID +
                                    " and scenario_id = " + ExecutionStudio._Scenario.ScenarioID +
                                    " and  execution_id = " + ExecutionStudio.current_execution_id +
                                    " and start_date_time >= @b4_date_time" +
                                    " and end_date_time <= @max_date_time";

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add(new SQLiteParameter("b4_date_time", b4_date_time));
                        cmd.Parameters.Add(new SQLiteParameter("max_date_time", max_date_time));
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {                                
                                response_size = dr.GetDouble(0);
                                request_size = dr.GetDouble(1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Console.Write(ex.Message); }
            return Math.Abs(response_size) + Math.Abs(request_size);
        }

        internal KeyValuePair<double, double> getLatestTHPUT()
        {
            return getThroughPut(15);
        }

        internal KeyValuePair<int, int> getLatestError(int interval_in_sec)
        {
            int errorCount = 0;         
            int graphTime = 0;

            try
            {
                DateTime _max_time = getMAXTxn_DateTime();
                if (_max_time == new DateTime())
                {
                    return new KeyValuePair<int, int>(0, 0); ;
                }
                TimeSpan t = new TimeSpan(0, 0, interval_in_sec);
                DateTime b4_date_time = _max_time - t;

                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "select count(1) from test_execution_metrics where response_code not like '2%' and response_code not like '3%' " +
                                    " and project_id = " + ExecutionStudio._Scenario.ProjectID +
                                    " and scenario_id = " + ExecutionStudio._Scenario.ScenarioID +
                                    " and execution_id = " + ExecutionStudio.current_execution_id +
                                    " and start_date_time >= @b4_date_time" +
                                    " and start_date_time <= @max_date_time";

                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        cmd.Parameters.Add(new SQLiteParameter("b4_date_time", b4_date_time));
                        cmd.Parameters.Add(new SQLiteParameter("max_date_time", _max_time));
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                errorCount = dr.GetInt32(0);                                
                            }
                        }
                    }
                }
                DateTime start_time = getTestStartTime();
                TimeSpan minutes = _max_time - start_time;
                graphTime = (int)minutes.TotalMinutes;
            }
            catch (Exception ex) { Console.Write(ex.Message); }
            return new KeyValuePair<int, int>((int)graphTime, (int)(errorCount / interval_in_sec)); ;
        }

        internal static int checkUniqueMonitor(string monitor_name, int scenario_id)
        {
            int count = 0;
            String connString = "Data Source=" + Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT count(1) from server_monitors where scenario_id = " + scenario_id + " and monitor_name = " + monitor_name;
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

        internal static void saveMonitor(ViewModels.ServerMonitor monitor)
        {
            String connString = "Data Source=" + Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "insert into server_monitors(monitor_id,scenario_id,monitor_name,host,port,pollingFrequency) values(NULL,@scenario_id,@monitor_name,@host,@port,@pollingFrequency)";
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {                                                
                        cmd.Parameters.Add(new SQLiteParameter("scenario_id", monitor.ScenarioId));
                        cmd.Parameters.Add(new SQLiteParameter("monitor_name", monitor.MonitorName));
                        cmd.Parameters.Add(new SQLiteParameter("host", monitor.Host));
                        cmd.Parameters.Add(new SQLiteParameter("port", monitor.Port));
                        cmd.Parameters.Add(new SQLiteParameter("pollingFrequency", monitor.PollingFrequency));

                        cmd.ExecuteNonQuery();                        
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);                
            }
        }

        internal void saveStatsToDB(int monitor_id, int execution_id, string measure, string value, double time, string monitor_name)
        {
            String connString = "Data Source=" + Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "insert into server_monitor_metrics(monitor_id,execution_id,measure,value,time,monitor_name) values(@monitor_id,@execution_id,@measure,@value,@time,@monitor_name)";
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("monitor_id", monitor_id));
                        cmd.Parameters.Add(new SQLiteParameter("execution_id", execution_id));
                        cmd.Parameters.Add(new SQLiteParameter("measure", measure));
                        cmd.Parameters.Add(new SQLiteParameter("value", value));
                        cmd.Parameters.Add(new SQLiteParameter("time", time));
                        cmd.Parameters.Add(new SQLiteParameter("monitor_name", monitor_name));

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        internal static List<KeyValuePair<double, double>> getMonitorValueList_from_DB(int monitor_id, int execution_id, string measure)
        {
            List<KeyValuePair<double, double>> value_list = new List<KeyValuePair<double, double>>();
            String connString = "Data Source=" + Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "SELECT time,value from server_monitor_metrics where monitor_id = " + monitor_id + " and execution_id = " + execution_id + " and measure = '"+measure+"'";
                    using (SQLiteCommand cmd = new SQLiteCommand(query.ToString(), conn))
                    {
                        conn.Open();
                        using (SQLiteDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {                                
                                double time = dr.GetDouble(0);
                                string val = dr.GetString(1);
                                KeyValuePair<double, double> key_val = new KeyValuePair<double, double>(time,double.Parse(val));
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

        internal static void saveExecutionGraph(int current_execution_id, byte[] bytes, string graphName)
        {
            String connString = "Data Source=" + Path.GetFullPath(".\\..\\..\\DataBase\\TeraLoadDB.s3db");
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connString))
                {
                    String query = "insert into test_execution_graphs(execution_id,graph_name,graph_data) values(@execution_id,@graph_name,@graph_data)";
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.Add(new SQLiteParameter("execution_id", current_execution_id));
                        cmd.Parameters.Add(new SQLiteParameter("graph_name", graphName));
                        cmd.Parameters.Add(new SQLiteParameter("graph_data", bytes));                        

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }
    }
}
