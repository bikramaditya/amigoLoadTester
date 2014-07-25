/*
* This demo program shows how to use the FiddlerCore library.
* By default, it is compiled without support for the SAZ File format.
* If you want to add SAZ support, define the token SAZ_SUPPORT in the list of
* Conditional Compilation symbols on the project's BUILD tab.
* 
* You will need to add either SAZ-DOTNETZIP.cs or SAZXCEEDZIP.cs to your project,
* depending on which ZIP library you want to use. You must also ensure to set the 
* Fiddler.RequiredVersionAttribute on your assembly, or your transcoders will be 
* ignored.
*/

using System;
using Fiddler;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using System.Data.Common;
using Ionic.Zip;
using System.IO;
using System.Text;
using System.Diagnostics;


namespace Amigo
{
    public class ProxyProgram
    {
        public static List<Fiddler.Session> oAllSessions;
        public static string tempSAZfileName;        
        static Proxy oSecureEndpoint;
        static string sSecureEndpointHostname = "localhost";
        static int iSecureEndpointPort = 7777;

        public static void WriteCommandResponse(string s)
        {
            ConsoleColor oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(s);
            Console.ForegroundColor = oldColor;
        }

        public static void DoQuit()
        {
            if (null != oSecureEndpoint) oSecureEndpoint.Dispose();            
            Fiddler.FiddlerApplication.Shutdown();
        }
        private static string Ellipsize(string s, int iLen)
        {
            if (s.Length <= iLen) return s;
            return s.Substring(0, iLen - 3) + "...";
        }

       public static void ProxyStart()
        {            
            oAllSessions = new List<Fiddler.Session>();
            #region AttachEventListeners
            //           
            // by default, we must handle notifying the user ourselves.
            Fiddler.FiddlerApplication.OnNotification += delegate(object sender, NotificationEventArgs oNEA) { Console.WriteLine("** NotifyUser: " + oNEA.NotifyString); };
            Fiddler.FiddlerApplication.Log.OnLogString += delegate(object sender, LogEventArgs oLEA) { Console.WriteLine("** LogString: " + oLEA.LogString); };
            
           //FiddlerApplication.Log.LogString("bikram entered while loop");
           
           Fiddler.FiddlerApplication.BeforeRequest += delegate(Fiddler.Session oS)
            {                
                // the response to the client as the response comes in.
                oS.bBufferResponse = false;
                Monitor.Enter(oAllSessions);
                oAllSessions.Add(oS);
                Monitor.Exit(oAllSessions);

                if ((oS.oRequest.pipeClient.LocalPort == iSecureEndpointPort) && (oS.hostname == sSecureEndpointHostname))
                {
                    oS.utilCreateResponseAndBypassServer();
                    oS.oResponse.headers.HTTPResponseStatus = "200 Ok";
                    oS.oResponse["Content-Type"] = "text/html; charset=UTF-8";
                    oS.oResponse["Cache-Control"] = "private, max-age=0";
                    oS.utilSetResponseBody("<html><body>Request for httpS://"+sSecureEndpointHostname+ ":" + iSecureEndpointPort.ToString() + " received. Your request was:<br /><plaintext>" + oS.oRequest.headers.ToString());
                }
            };

           

            // Tell the system console to handle CTRL+C by calling our method that
            // gracefully shuts down the FiddlerCore.
            //
            // Note, this doesn't handle the case where the user closes the window with the close button.
            // See http://geekswithblogs.net/mrnat/archive/2004/09/23/11594.aspx for info on that...
            //
            Console.CancelKeyPress += new ConsoleCancelEventHandler(Console_CancelKeyPress);
            #endregion AttachEventListeners

            //string sSAZInfo = "NoSAZ";


            //Console.WriteLine(String.Format("Starting {0} ({1})...", Fiddler.FiddlerApplication.GetVersionString(), sSAZInfo));

            // For the purposes of this demo, we'll forbid connections to HTTPS 
            // sites that use invalid certificates. Change this from the default only
            // if you know EXACTLY what that implies.
            Fiddler.CONFIG.IgnoreServerCertErrors = false;

            // ... but you can allow a specific (even invalid) certificate by implementing and assigning a callback...
            // FiddlerApplication.OverrideServerCertificateValidation += new OverrideCertificatePolicyHandler(FiddlerApplication_OverrideServerCertificateValidation);

            FiddlerApplication.Prefs.SetBoolPref("fiddler.network.streaming.abortifclientaborts", true);

            // For forward-compatibility with updated FiddlerCore libraries, it is strongly recommended that you 
            // start with the DEFAULT options and manually disable specific unwanted options.
            FiddlerCoreStartupFlags oFCSF = FiddlerCoreStartupFlags.Default;
            // E.g. uncomment the next line if you don't want FiddlerCore to act as the system proxy
            // oFCSF = (oFCSF & ~FiddlerCoreStartupFlags.RegisterAsSystemProxy);
            // or uncomment the next line if you don't want to decrypt SSL traffic.
            // oFCSF = (oFCSF & ~FiddlerCoreStartupFlags.DecryptSSL);
            //
            // NOTE: Because we haven't disabled the option to decrypt HTTPS traffic, makecert.exe 
            // must be present in this executable's folder.

            // NOTE: In the next line, you can pass 0 for the port (instead of 8877) to have FiddlerCore auto-select an available port
            Fiddler.FiddlerApplication.Startup(8877, oFCSF);

            FiddlerApplication.Log.LogString("Starting with settings: [" + oFCSF + "]");
            FiddlerApplication.Log.LogString("Using Gateway: " + ((CONFIG.bForwardToGateway) ? "TRUE" : "FALSE"));

            //Console.WriteLine("Hit CTRL+C to end session.");

            oSecureEndpoint = FiddlerApplication.CreateProxyEndpoint(iSecureEndpointPort, true, sSecureEndpointHostname);
            if (null != oSecureEndpoint)
            {
                FiddlerApplication.Log.LogString("Created secure end point listening on port "+ iSecureEndpointPort.ToString() + ", using a HTTPS certificate for '" + sSecureEndpointHostname + "'");
            }
        }

        /// <summary>
        /// When the user hits CTRL+C, this event fires.  We use this to shut down and unregister our FiddlerCore.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            DoQuit();
        }
    }
}

