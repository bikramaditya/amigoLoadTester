using System.Windows;
using Amigo.ViewModels;
using System.Windows.Threading;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {        
        public static bool isRecording = true;
        public static string recordingSessionName = "";
        public static int currentProjectID = -1;
        public static string http_status_code = "";
        public static string http_status_message = "";
        public static string http_response = "";        
        public static Project _project = new Project("",false);
        public static ButtonsStatus _buttons = new ButtonsStatus();
        public static NotifiableObservableCollection<Parameter> Parameters_For_Replay = new NotifiableObservableCollection<Parameter>();
        public static int speed = 500;
        public static bool preview_browser_IsVisible = true;        
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (ProxyProgram.oAllSessions != null)
            ProxyProgram.DoQuit();
        }

        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }

        public static int Buttonsize40 { get; set; }

        public static int Imgsize32 { get; set; }
    }    
}
