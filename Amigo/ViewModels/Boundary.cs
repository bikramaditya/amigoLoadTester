using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amigo.ViewModels
{
    public class Boundary
    {
        private bool isChecked = false;
        private string lb = "";
        private string rb = "";

        public bool ISChecked { get; set; }
        public string LB { get; set; }        
        public string RB { get; set; }
    }
}
