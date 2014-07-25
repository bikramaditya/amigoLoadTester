using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amigo.ViewModels
{
    public class Report
    {
        private int _report_id = -1;

        public string ReportName { get; set; }

        public string YourLogoText { get; set; }

        public string CustomerLogoText { get; set; }

        public int ReportID 
        { 
            get { return _report_id; }
            set { _report_id = value; }
        }

        public int ProjectID { get; set; }

        public string TestDescription { get; set; }

        public bool TimeFrame { get; set; }

        public string DeploymentDiagramText { get; set; }

        public string WebServersText { get; set; }

        public string AppServersText { get; set; }

        public string DbServersText { get; set; }

        public string OtherServersText { get; set; }

        public string SelectedSections { get; set; }

        public string ReportFormat { get; set; }

        public byte[] YourLogoImage { get; set; }

        public byte[] CustomerLogoImage { get; set; }

        public byte[] DeploymentDiagramImage { get; set; }
    }
}
