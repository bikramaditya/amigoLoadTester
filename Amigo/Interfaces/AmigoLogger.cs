using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Amigo.ViewModels
{
    public class AmigoLogger
    {
        private static StreamWriter w = File.AppendText("Response_Load_Test.txt");
        public static void Log(string logMessage)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),DateTime.Now.ToLongDateString());
            w.WriteLine("  :");
            w.WriteLine("  :{0}", logMessage);
            w.WriteLine("-------------------------------");
        }       
    }
}
