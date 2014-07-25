using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amigo.ViewModels
{
    public class TransactionMetrics
    {
        public double TransactionID { get; set; }
        public string TransactionName { get; set; }
        public string DisplayName { get; set; }
        public string RespTimeMin { get; set; }
        public string RespTimeMax { get; set; }
        public string RespTimeAvg { get; set; }
        public string RespTime85 { get; set; }
        public string RespTime90 { get; set; }
        public string RespTime95 { get; set; }        
    }
}
