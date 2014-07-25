using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Amigo.Utils
{
    public class MyWebServer
    {
        private TcpListener myListener;
        private int port = 5050; // Select any free port you wish
        private String sMyWebServerRoot = Path.GetFullPath(".\\..\\..\\Templates\\");
        public static bool isListeningEnabled = true;
        public MyWebServer()
        {
            isListeningEnabled = true;
            try
            {
                myListener = new TcpListener(port);
                myListener.Start();            
                Thread th = new Thread(new ThreadStart(StartListen));
                th.Start();
            }
            catch (Exception e)
            {
                //Console.WriteLine("An Exception Occurred while Listening :" + e.ToString());
            }
        }

        public string GetTheDefaultFileName(string sLocalDirectory)
        {
            StreamReader sr;
            String sLine = "";
            try
            {                
                sr = new StreamReader(sMyWebServerRoot + "Data\\Default.Dat");
                while ((sLine = sr.ReadLine()) != null)
                {
                    //Look for the default file in the web server root folder
                    if (File.Exists(sLocalDirectory + sLine) == true)
                        break;
                }
            }
            catch (Exception e)
            {
                //Console.WriteLine("An Exception Occurred : " + e.ToString());
            }
            if (File.Exists(sLocalDirectory + sLine) == true)
                return sLine;
            else
                return "";
        }



        /// <summary>
        /// This function takes FileName as Input and returns the mime type..
        /// </summary>
        /// <param name="sRequestedFile">To indentify the Mime Type</param>
        /// <returns>Mime Type</returns>
        public string GetMimeType(string sRequestedFile)
        {
            StreamReader sr;
            String sLine = "";
            String sMimeType = "";
            String sFileExt = "";
            String sMimeExt = "";

            // Convert to lowercase
            sRequestedFile = sRequestedFile.ToLower();
            int iStartPos = sRequestedFile.IndexOf(".");
            sFileExt = sRequestedFile.Substring(iStartPos);
            try
            {
                sr = new StreamReader(sMyWebServerRoot+"Data\\Mimes.Dat");
                while ((sLine = sr.ReadLine()) != null)
                {
                    sLine.Trim();
                    if (sLine.Length > 0)
                    {
                        iStartPos = sLine.IndexOf(";");
                        sLine = sLine.ToLower();
                        sMimeExt = sLine.Substring(0, iStartPos);
                        sMimeType = sLine.Substring(iStartPos + 1);
                        if (sMimeExt == sFileExt)
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An Exception Occurred : " + e.ToString());
            }

            if (sMimeExt == sFileExt)
                return sMimeType;
            else
                return "";
        }

        public string GetLocalPath(string sMyWebServerRoot, string sDirName)
        {
            return sMyWebServerRoot + "\\" + sDirName;
            StreamReader sr;
            String sLine = "";
            String sVirtualDir = "";
            String sRealDir = "";
            int iStartPos = 0;

            sDirName.Trim();
            sMyWebServerRoot = sMyWebServerRoot.ToLower();            
            sDirName = sDirName.ToLower();

            try
            {            
                sr = new StreamReader(sMyWebServerRoot + "Data\\VDirs.Dat");
                while ((sLine = sr.ReadLine()) != null)
                {
                    sLine.Trim();
                    if (sLine.Length > 0)
                    {
                        iStartPos = sLine.IndexOf(";");
                        sLine = sLine.ToLower();
                        sVirtualDir = sLine.Substring(0, iStartPos);
                        sRealDir = sLine.Substring(iStartPos + 1);
                        if (sVirtualDir == sDirName)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An Exception Occurred : " + e.ToString());
            }
            if (sVirtualDir == sDirName)
                return sRealDir;
            else
                return "";
        }



        /// <summary>
        /// This function send the Header Information to the client (Browser)
        /// </summary>
        /// <param name="sHttpVersion">HTTP Version</param>
        /// <param name="sMIMEHeader">Mime Type</param>
        /// <param name="iTotBytes">Total Bytes to be sent in the body</param>
        /// <param name="mySocket">Socket reference</param>
        /// <returns></returns>
        public void SendHeader(string sHttpVersion, string sMIMEHeader, int iTotBytes, string sStatusCode, ref Socket mySocket)
        {
            String sBuffer = "";
            if (sMIMEHeader.Length == 0)
            {
                sMIMEHeader = "text/html";  // Default Mime Type is text/html
            }

            sBuffer = sBuffer + sHttpVersion + sStatusCode + "\r\n";
            sBuffer = sBuffer + "Server: cx1193719-b\r\n";
            sBuffer = sBuffer + "Content-Type: " + sMIMEHeader + "\r\n";
            sBuffer = sBuffer + "Accept-Ranges: bytes\r\n";
            sBuffer = sBuffer + "Content-Length: " + iTotBytes + "\r\n\r\n";

            Byte[] bSendData = Encoding.ASCII.GetBytes(sBuffer);

            SendToBrowser(bSendData, ref mySocket);            
        }



        /// <summary>
        /// Overloaded Function, takes string, convert to bytes and calls 
        /// overloaded sendToBrowserFunction.
        /// </summary>
        /// <param name="sData">The data to be sent to the browser(client)</param>
        /// <param name="mySocket">Socket reference</param>
        public void SendToBrowser(String sData, ref Socket mySocket)
        {
            SendToBrowser(Encoding.ASCII.GetBytes(sData), ref mySocket);
        }



        /// <summary>
        /// Sends data to the browser (client)
        /// </summary>
        /// <param name="bSendData">Byte Array</param>
        /// <param name="mySocket">Socket reference</param>
        public void SendToBrowser(Byte[] bSendData, ref Socket mySocket)
        {
            int numBytes = 0;

            try
            {
                if (mySocket.Connected)
                {
                    if ((numBytes = mySocket.Send(bSendData, bSendData.Length, 0)) == -1)
                        Console.WriteLine("Socket Error cannot Send Packet");
                    else
                    {
                        Console.WriteLine("No. of bytes send {0}", numBytes);
                    }
                }
                else
                    Console.WriteLine("Connection Dropped....");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Occurred : {0} ", e);

            }
        }


        public void StopListen()
        {
            isListeningEnabled = false;
        }

        //This method Accepts new connection and
        //First it receives the welcome massage from the client,
        //Then it sends the Current date time to the Client.
        public void StartListen()
        {

            int iStartPos = 0;
            String sRequest;
            String sDirName;
            String sRequestedFile;
            String sErrorMessage;
            String sLocalDir;            
            String sPhysicalFilePath = "";
            String sFormattedMessage = "";
            String sResponse = "";

            while (isListeningEnabled)
            {
                Socket mySocket = myListener.AcceptSocket();

                if (mySocket.Connected)
                {                
                    Byte[] bReceive = new Byte[1024];
                    int i = mySocket.Receive(bReceive, bReceive.Length, 0);                 
                    string sBuffer = Encoding.ASCII.GetString(bReceive);
                    var byteArray = bReceive;
                    var x1 = System.Convert.ToBase64String(byteArray, 0, byteArray.Length);
                    var x2 = Encoding.UTF8.GetString(byteArray);

                    if (sBuffer.Substring(0, 3) != "GET")
                    {
                        Console.WriteLine("Only Get Method is supported..");
                        mySocket.Close();
                        return;
                    }

                    // Look for HTTP request
                    iStartPos = sBuffer.IndexOf("HTTP", 1);

                    // Get the HTTP text and version e.g. it will return "HTTP/1.1"
                    string sHttpVersion = sBuffer.Substring(iStartPos, 8);

                    // Extract the Requested Type and Requested file/directory
                    sRequest = sBuffer.Substring(0, iStartPos - 1);

                    //Replace backslash with Forward Slash, if Any
                    sRequest.Replace("\\", "/");

                    //If file name is not supplied add forward slash to indicate 
                    //that it is a directory and then we will look for the 
                    //default file name..
                    if ((sRequest.IndexOf(".") < 1) && (!sRequest.EndsWith("/")))
                    {
                        sRequest = sRequest + "/";
                    }


                    //Extract the requested file name
                    iStartPos = sRequest.LastIndexOf("/") + 1;
                    sRequestedFile = sRequest.Substring(iStartPos);


                    //Extract The directory Name
                    sDirName = sRequest.Substring(sRequest.IndexOf("/"), sRequest.LastIndexOf("/") - 3);

                    /////////////////////////////////////////////////////////////////////
                    // Identify the Physical Directory
                    /////////////////////////////////////////////////////////////////////
                    if (sDirName == "/")
                        sLocalDir = sMyWebServerRoot;
                    else
                    {
                        //Get the Virtual Directory
                        sLocalDir = GetLocalPath(sMyWebServerRoot, sDirName);
                    }


                    Console.WriteLine("Directory Requested : " + sLocalDir);

                    //If the physical directory does not exists then
                    // dispaly the error message
                    if (sLocalDir.Length == 0)
                    {
                        sErrorMessage = "<H2>Error!! Requested Directory does not exists</H2><Br>";
                        //sErrorMessage = sErrorMessage + "Please check data\\Vdirs.Dat";

                        //Format The Message
                        SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", ref mySocket);

                        //Send to the browser
                        SendToBrowser(sErrorMessage, ref mySocket);

                        mySocket.Close();

                        continue;
                    }


                    /////////////////////////////////////////////////////////////////////
                    // Identify the File Name
                    /////////////////////////////////////////////////////////////////////

                    //If The file name is not supplied then look in the default file list
                    if (sRequestedFile.Length == 0)
                    {
                        // Get the default filename
                        sRequestedFile = GetTheDefaultFileName(sLocalDir);

                        if (sRequestedFile == "")
                        {
                            sErrorMessage = "<H2>Error!! No Default File Name Specified</H2>";
                            SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", ref mySocket);
                            SendToBrowser(sErrorMessage, ref mySocket);

                            mySocket.Close();

                            return;

                        }
                    }

                    /////////////////////////////////////////////////////////////////////
                    // Get TheMime Type
                    /////////////////////////////////////////////////////////////////////

                    String sMimeType = GetMimeType(sRequestedFile);
                    sPhysicalFilePath = sLocalDir + sRequestedFile;
                    Console.WriteLine("File Requested : " + sPhysicalFilePath);

                    if (File.Exists(sPhysicalFilePath) == false)
                    {
                        sErrorMessage = "<H2>404 Error! File Does Not Exists...</H2>";
                        SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", ref mySocket);
                        SendToBrowser(sErrorMessage, ref mySocket);
                        Console.WriteLine(sFormattedMessage);
                    }

                    else
                    {
                        int iTotBytes = 0;
                        sResponse = "";
                        FileStream fs = new FileStream(sPhysicalFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                        // Create a reader that can read bytes from the FileStream.
                        BinaryReader reader = new BinaryReader(fs);
                        byte[] bytes = new byte[fs.Length];
                        int read;
                        while ((read = reader.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            sResponse = sResponse + Encoding.ASCII.GetString(bytes, 0, read);
                            iTotBytes = iTotBytes + read;
                        }
                        reader.Close();
                        fs.Close();
                        SendHeader(sHttpVersion, sMimeType, iTotBytes, " 200 OK", ref mySocket);
                        SendToBrowser(bytes, ref mySocket);
                    }
                    mySocket.Close();
                }
            }
        }
    }
}
