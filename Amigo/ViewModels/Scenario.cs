using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amigo.ViewModels
{
    public class Scenario
    {
        public int ScenarioID { get; set; }
        public int ProjectID { get; set; }
        public string ScenarioName { get; set; }
        public int TotalNumberOfUsers { get; set; }
        public int ExecutionTime { get; set; }
        public bool isNumberOfUsersinPercent { get; set; }
        public bool isLoggingEnabled { get; set; }
        public int logLevel { get; set; }
        public string TargetHost { get; set; }
        public int TargetPort { get; set; }
        public int errorLevel { get; set; }
        public int wanEmulation { get; set; }
        public bool simulateBrowserCache { get; set; }
        public string proxyHost { get; set; }
        public int proxyPort { get; set; }
        public string isTargetBasedOrLoadBased { get; set; }
        public int rampUpTime { get; set; }
        public int rampDownTime { get; set; }
        public int rampUserStep { get; set; }
        public int rampTimeStep { get; set; }//tps_value
        public int TPS { get; set; }
    }
}
