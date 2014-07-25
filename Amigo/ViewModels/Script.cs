using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Amigo.ViewModels
{
    public class Script
    {
        public string ScriptName { get; set; }
        public int ScriptID { get; set; }
        public int SequenceID { get; set; }
        public int ScenarioID { get; set; }
        public int NumberOfUsers { get; set; }        
        public int StartAfter { get; set; }
        public int NumberOfIterations { get; set; }        
        public int DelayBetweenIterationMin { get; set; }
        public int DelayBetweenIterationMax { get; set; }
        public int ThinkTimeMin { get; set; }
        public int ThinkTimeMax { get; set; }
    }
}

