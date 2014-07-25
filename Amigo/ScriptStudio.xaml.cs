using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using Amigo.ViewModels;
using System.Collections;
using LumenWorks.Framework.IO.Csv;
using System.IO;
using System.Threading;
using System.Xml;
using System.Windows.Markup;
using System.Diagnostics;
using System.Data.SQLite;

namespace Amigo
{
    /// <summary>
    /// Interaction logic for Tab1.xaml
    /// </summary>
    public partial class ScriptStudio : UserControl
    {
        public static MessagePage _message = new MessagePage();
        //public List<object> dataContextList = new List<object>();        
        Thread _thread = null;

        public ScriptStudio()
        {
            InitializeComponent();
            LeftBorder.Width = App.ScreenWidth * 0.24305555;
            RightBorder.Width = App.ScreenWidth * 0.20138888;
            this.DataContext = Amigo.App._project;
            request_referers.Expanded +=new RoutedEventHandler(request_referers_Expanded);
            recording_sessions.Expanded += new RoutedEventHandler(recording_sessions_Expanded);
            param_data_grid.Loaded +=new RoutedEventHandler(param_data_grid_Loaded);
            header_data_grid.Loaded += new RoutedEventHandler(header_data_grid_Loaded);            
        }
      
        void header_data_grid_Loaded(object sender, RoutedEventArgs e)
        {
            header_data_grid.Columns[0].MinWidth = 140;
            header_data_grid.Columns[1].MinWidth = 140;
            header_data_grid.IsReadOnly = true;
            header_data_grid.CanUserSortColumns = true;
            header_data_grid.CanUserAddRows = false;
            header_data_grid.CanUserResizeColumns = true;
            header_data_grid.CanUserDeleteRows = false;
            header_data_grid.CanUserReorderColumns = false;
            header_data_grid.CanUserResizeRows = false;
        }

        void param_data_grid_Loaded(object sender, RoutedEventArgs e)
        {
            param_data_grid.Columns[0].Width = 90;
            param_data_grid.Columns[1].Width = 140;
            param_data_grid.Columns[2].Width = 130;
            param_data_grid.Columns[2].Header = "Parameter Data\nSource";
            param_data_grid.Columns[3].Width = 160;
            param_data_grid.Columns[4].Width = 130;
            param_data_grid.Columns[5].Width = 155;

            param_data_grid.Columns[0].IsReadOnly = true;
            param_data_grid.Columns[1].IsReadOnly = true;
            
            param_data_grid.CanUserSortColumns = true;
            param_data_grid.CanUserAddRows = false;
            param_data_grid.CanUserResizeColumns = true;
            param_data_grid.CanUserDeleteRows = false;
            param_data_grid.CanUserReorderColumns = false;
            param_data_grid.CanUserResizeRows = false;
        }     

        void param_data_grid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            string caption = "";
            Parameter _currentParam = null;
            string filename = "";
            NotifiableObservableCollection<string> _csv_columns = new NotifiableObservableCollection<string>();

            try
            {
                _currentParam = (Parameter)e.Row.Item;
                caption = "" + e.Column.Header.ToString();
                filename = _currentParam.SubstututedParamValue;
            }
            catch { }

            if ("Substituted value / CSV".Equals(caption) && _currentParam.ParameterizationSource.Equals(ParamSources.CSV))
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".csv";
                dlg.Filter = "CSV Files (.csv)|*.csv";
                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    filename = dlg.FileName;
                    ((Parameter)(param_data_grid.CurrentItem)).SubstututedParamValue = filename;
                    using (CsvReader csv = new CsvReader(new StreamReader(filename), true))
                    {
                        int fieldCount = csv.FieldCount;
                        string[] headers = csv.GetFieldHeaders();
                        
                        for (int i = 0; i < fieldCount; i++)
                        {                                         
                            _csv_columns.Add(headers[i]);
                        }
                        _currentParam.CSVColumns = _csv_columns;                        
                    }
                }
            }
            else if ("Substituted value / CSV".Equals(caption) && _currentParam.ParameterizationSource.Equals(ParamSources.AutoCorrelation))
            {
                Parameter currentPar = (Parameter)(param_data_grid.CurrentItem);

                Window window = new Window
                {
                    Title = "Automatic Correlation Window",
                    Content = new AutoCorrelationWindow(ref currentPar, global_selected_request_id),
                };
                window.Height = 650;
                window.Width = 470;
                window.Closing +=new CancelEventHandler(window_Closing);
                window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                window.ShowDialog();                
            }            
        }
        void window_Closing(object sender, CancelEventArgs e)
        {
            Window corr_window = (Window)sender;
            DataGrid _DataGrid = (DataGrid)((AutoCorrelationWindow)corr_window.Content).autoCorrelationGrid;
        }
        void recording_sessions_Expanded(object sender, RoutedEventArgs e)
        {
            try
            {
                TreeViewItem _item = (TreeViewItem)e.OriginalSource;

                if (_item.Header.ToString().Contains("RecordingSession"))
                {
                    //// read URL file start ------
                    List<string> urls_in_file = new List<string>();
                    bool isFirstURL = true;
                    int lineCounter = 0;
                    string line;
                    System.IO.StreamReader file = new System.IO.StreamReader(".\\..\\..\\Interfaces\\frequentURLs.txt");
                    while ((line = file.ReadLine()) != null)
                    {
                        urls_in_file.Add(line);
                        lineCounter++;
                    }
                    file.Close();
                    Console.ReadLine();
                    //// read URL file end --------

                    Amigo.App.recordingSessionName = ((RecordingSession)(_item.Header)).SessionName;
                    int _session_id = ((RecordingSession)(_item.Header)).SessionID;
                    int itemCount = _item.Items.Count;
                    for (int rem = 0; rem < itemCount; rem++)
                    {
                        _item.Items.RemoveAt(0);
                    }

                    ArrayList request_ids = DBProcessingLayer.getAllRequestIDs_for_Sessions(_session_id);
                    TreeViewItem titleItem = null;


                    for (int i = 0; i < request_ids.Count; i++)
                    {
                        try
                        {                           
                            string context_path = "";
                            double req = (double)request_ids[i];
                            ArrayList paths = DBProcessingLayer.getAllPaths_for_Request_ID(req);
                            for (int j = 0; j < paths.Count; j++)
                            {
                                context_path += "/" + paths[j];
                            }

                            context_path = context_path.Replace("//", "/");

                            if (paths.Count > 0)
                            {
                                Image _image = new Image();
                                Label _hidden_id = new Label();
                                _hidden_id.Visibility = System.Windows.Visibility.Collapsed;
                                _hidden_id.Content = "" + request_ids[i];
                                
                                _image.Height = 25;
                                _image.Width = 25;
                                bool isStatic = false;

                                if (context_path.EndsWith(".png"))
                                {
                                    _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/png.png", UriKind.Relative));
                                    isStatic = true;
                                }
                                else if (context_path.EndsWith(".jpg") || context_path.EndsWith(".jpeg"))
                                {
                                    _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/jpeg.png", UriKind.Relative));
                                    isStatic = true;
                                }
                                else if (context_path.EndsWith(".ico"))
                                {
                                    _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/ico.png", UriKind.Relative));
                                    isStatic = true;
                                }
                                else if (context_path.EndsWith(".bmp"))
                                {
                                    _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/bmp.png", UriKind.Relative));
                                    isStatic = true;
                                }
                                else if (context_path.EndsWith(".gif"))
                                {
                                    _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/gif.png", UriKind.Relative));
                                    isStatic = true;
                                }
                                else if (context_path.Equals("/"))
                                {
                                    _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/browser.png", UriKind.Relative));
                                    isStatic = false;
                                }
                                else if (context_path.EndsWith(".js"))
                                {
                                    _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/js.png", UriKind.Relative));
                                    isStatic = true;
                                }
                                else if (context_path.EndsWith(".css"))
                                {
                                    _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/css.png", UriKind.Relative));
                                    isStatic = true;
                                }
                                else
                                {
                                    _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/browser.png", UriKind.Relative));
                                }

                                if (!isStatic)
                                {
                                    _image.Height = 16;
                                    _image.Width = 16;
                                }

                                Label _label = new Label();
                                _label.Foreground = new SolidColorBrush(Colors.White);

                                string[] _host_dtls = DBProcessingLayer.getReferer_for_RequestID("" + req);
                                string _context_path_lbl = _host_dtls[0] + ":" + _host_dtls[1] + context_path;

                                _label.Content = _context_path_lbl;

                                _label.ContextMenu = getContextMenu_For_Dynamics();
                                _label.FontSize = 12;
                                _label.ContextMenu.Opened += new RoutedEventHandler(ContextMenu_Opened);

                                StackPanel _stackPanel = new StackPanel();
                                _stackPanel.Orientation = Orientation.Horizontal;
                                _stackPanel.Children.Add(_image);
                                _stackPanel.Children.Add(_label);
                                _stackPanel.Children.Add(_hidden_id);                                                            

                                TreeViewItem _new_item = new TreeViewItem();
                                _new_item.Foreground = new SolidColorBrush(Colors.White);

                                String title = ""+DBProcessingLayer.getTitle_for_request_ID(request_ids[i].ToString());

                                if (title.Length > 0)
                                {
                                    titleItem = new TreeViewItem();
                                    StackPanel titleStackPanel = new StackPanel();
                                    titleStackPanel.Orientation = Orientation.Horizontal;
                                    titleStackPanel.Height = 25;
                                    Image _title_image = new Image();
                                    _title_image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/web_page.png", UriKind.Relative));
                                    _title_image.Height = 20;
                                    _title_image.Width = 20;
                                    EditableTextBlock _title_lable = new EditableTextBlock();
                                    _title_lable.Foreground = new SolidColorBrush(Colors.White);
                                    _title_lable.FontSize = 12;
                                    _title_lable.Text = title;
                                    
                                    titleStackPanel.Children.Add(_title_image);
                                    titleStackPanel.Children.Add(_title_lable);

                                    titleItem.Header = titleStackPanel;
                                    titleItem.ToolTip = "Tripple click to rename";
                                }
                                if (titleItem != null)
                                {
                                    titleItem.ContextMenu = getPageContextMenu();
                                    
                                    _new_item.Header = _stackPanel;
                                    titleItem.Items.Add(_new_item);

                                    if (!_item.Items.Contains(titleItem))
                                    {
                                        _item.Items.Add(titleItem);
                                    }
                                    string[] protocolNmethod = DBProcessingLayer.getMethod_for_request_ID(request_ids[i].ToString());
                                    if (!urls_in_file.Contains(protocolNmethod[0] + "://" + _context_path_lbl) && !isStatic)
                                    {
                                        urls_in_file.Insert(0,protocolNmethod[0]+ "://" + _context_path_lbl);                                        
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message + ex.StackTrace);
                        }
                    }
                    urls_in_file.Reverse();
                    if (urls_in_file.Count > 20) urls_in_file.RemoveRange(20, urls_in_file.Count - 20 - 1);
                    string[] lines = (string[])urls_in_file.ToArray();
                    
                    System.IO.File.WriteAllLines(".\\..\\..\\Interfaces\\frequentURLs.txt", lines);
                }
            }
            catch (Exception ex)
            {
                EventLog Log = new EventLog();
                Log.Source = "ScriptStudio";
                Log.WriteEntry(ex.Message,
                         EventLogEntryType.Error);
            }
        }        

        private System.Windows.Controls.ContextMenu getPageContextMenu()
        {
            ContextMenu Menu = new ContextMenu();

            MenuItem mt1 = new MenuItem();
            mt1.Header = "Rename this Page";
            mt1.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(@"/Amigo;component/icons/rename.png", UriKind.Relative)),
                Height = 20,
                Width = 20
            };
            mt1.Click += new RoutedEventHandler(edit_page_text_box);

            MenuItem mt2 = new MenuItem();
            mt2.Header = "Expand";
            mt2.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(@"/Amigo;component/icons/expand.png", UriKind.Relative)),
                Height = 20,
                Width = 20
            };
            mt2.Click += new RoutedEventHandler(MenuItem_expand_page_click);

            MenuItem mt3 = new MenuItem();
            mt3.Header = "Collapse";
            mt3.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(@"/Amigo;component/icons/collapse.png", UriKind.Relative)),
                Height = 20,
                Width = 20
            };
            mt3.Click += new RoutedEventHandler(MenuItem_collapse_page_click);

            Menu.Items.Add(mt1);
            Menu.Items.Add(mt2);
            Menu.Items.Add(mt3);            

            return Menu;
        }

        private System.Windows.Controls.ContextMenu getContextMenu_For_Dynamics()
        {
            ContextMenu Menu = new ContextMenu();

            MenuItem mt1 = new MenuItem();
            mt1.Header = "Copy this request";
            mt1.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(@"/Amigo;component/icons/copy.png", UriKind.Relative)),
                Height = 20,
                Width = 20
            };
            mt1.Click += new RoutedEventHandler(copied_Click);

            MenuItem mt2 = new MenuItem();
            mt2.Header = "Cut this request";
            mt2.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(@"/Amigo;component/icons/cut.png", UriKind.Relative)),
                Height = 20,
                Width = 20
            };
            mt2.Click += new RoutedEventHandler(cut_Click);

            MenuItem mt3 = new MenuItem();
            mt3.Header = "Paste above this";
            mt3.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(@"/Amigo;component/icons/paste.png", UriKind.Relative)),
                Height = 20,
                Width = 20
            };
            mt3.Click += new RoutedEventHandler(paste_above_Click);

            MenuItem mt31 = new MenuItem();
            mt31.Header = "Paste below this";
            mt31.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(@"/Amigo;component/icons/paste.png", UriKind.Relative)),
                Height = 20,
                Width = 20
            };
            mt31.Click += new RoutedEventHandler(paste_below_Click);

            MenuItem mt4 = new MenuItem();
            mt4.Header = "Delete this request";
            mt4.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(@"/Amigo;component/icons/delete.png", UriKind.Relative)),
                Height = 20,
                Width = 20
            };
            mt4.Click += new RoutedEventHandler(delete_request_Click);

            MenuItem mt5 = new MenuItem();
            mt5.Header = "Share this request across scripts";
            mt5.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(@"/Amigo;component/icons/share.png", UriKind.Relative)),
                Height = 20,
                Width = 20
            };

            Menu.Items.Add(mt1);
            Menu.Items.Add(mt2);
            Menu.Items.Add(mt3);
            Menu.Items.Add(mt31);
            Menu.Items.Add(mt4);
            Menu.Items.Add(mt5);

            return Menu;
        }

        public static T Clone<T>(T from)
        {
            string objStr = XamlWriter.Save(from);
            StringReader stringReader = new StringReader(objStr);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            object clone = XamlReader.Load(xmlReader);
            return (T)clone;
        }

        TreeViewItem copiedItem = null;
        TreeViewItem cutItem = null;

        private void saveTreeViewItems(int currentIndex, TreeViewItem new_item, TreeViewItem cutItem, string aboveORbelow)
        {
            
            string copied_req_id = ((Label)((StackPanel)new_item.Header).Children[2]).Content.ToString();
            string previous_req_id = DBProcessingLayer.getPreviousRequestID(copied_req_id);
            string next_req_id = DBProcessingLayer.getNextRequestID(copied_req_id);

            double new_req_id = 0;

            if (aboveORbelow == "above")
            {
                new_req_id = double.Parse(previous_req_id) + (double.Parse(copied_req_id) - double.Parse(previous_req_id)) / 2;
            }
            else if (aboveORbelow == "below")
            {
                new_req_id = double.Parse(copied_req_id) + (double.Parse(next_req_id) - double.Parse(copied_req_id)) / 2;
            }

            ((Label)((StackPanel)new_item.Header).Children[2]).Content = new_req_id.ToString();
            DBProcessingLayer.copyRequest(copied_req_id, new_req_id.ToString());
            if (cutItem != null)
            {
                string cut_req_id = ((Label)((StackPanel)cutItem.Header).Children[2]).Content.ToString();
                DBProcessingLayer.deleteRequest(cut_req_id);
            }
        }

        void paste_above_Click(object sender, RoutedEventArgs e)
        {            
            int currentIndex = ((TreeViewItem)((TreeViewItem)script_root_tree.SelectedItem).Parent).Items.IndexOf((TreeViewItem)script_root_tree.SelectedItem);            
            if (cutItem != null)
            {
                var new_item = Clone<TreeViewItem>(cutItem);
                ((TreeViewItem)((TreeViewItem)script_root_tree.SelectedItem).Parent).Items.Insert(currentIndex, new_item);
                ((Label)((StackPanel)new_item.Header).Children[1]).ContextMenu = getContextMenu_For_Dynamics();
                ((Label)((StackPanel)new_item.Header).Children[1]).ContextMenu.Opened += new RoutedEventHandler(ContextMenu_Opened);

                saveTreeViewItems(currentIndex, (TreeViewItem)new_item, (TreeViewItem)cutItem,"above");

                ((TreeViewItem)cutItem.Parent).Items.Remove(cutItem);
                cutItem = null;
            }
            else if (copiedItem != null)
            {
                var new_item = Clone<TreeViewItem>(copiedItem);
                ((TreeViewItem)((TreeViewItem)script_root_tree.SelectedItem).Parent).Items.Insert(currentIndex, new_item);
                ((Label)((StackPanel)new_item.Header).Children[1]).ContextMenu = getContextMenu_For_Dynamics();
                ((Label)((StackPanel)new_item.Header).Children[1]).ContextMenu.Opened += new RoutedEventHandler(ContextMenu_Opened);

                saveTreeViewItems(currentIndex, (TreeViewItem)new_item, null, "above");

                copiedItem = null;
            }
        }        

        void paste_below_Click(object sender, RoutedEventArgs e)
        {
            int currentIndex = ((TreeViewItem)((TreeViewItem)script_root_tree.SelectedItem).Parent).Items.IndexOf((TreeViewItem)script_root_tree.SelectedItem) + 1;
            if (cutItem != null)
            {
                var new_item = Clone<TreeViewItem>(cutItem);
                ((TreeViewItem)((TreeViewItem)script_root_tree.SelectedItem).Parent).Items.Insert(currentIndex, new_item);
                ((Label)((StackPanel)new_item.Header).Children[1]).ContextMenu = getContextMenu_For_Dynamics();
                ((Label)((StackPanel)new_item.Header).Children[1]).ContextMenu.Opened += new RoutedEventHandler(ContextMenu_Opened);

                saveTreeViewItems(currentIndex, (TreeViewItem)new_item, (TreeViewItem)cutItem, "below");

                ((TreeViewItem)cutItem.Parent).Items.Remove(cutItem);
                cutItem = null;
            }
            else if (copiedItem != null)
            {
                var new_item = Clone<TreeViewItem>(copiedItem);
                ((TreeViewItem)((TreeViewItem)script_root_tree.SelectedItem).Parent).Items.Insert(currentIndex, new_item);
                ((Label)((StackPanel)new_item.Header).Children[1]).ContextMenu = getContextMenu_For_Dynamics();
                ((Label)((StackPanel)new_item.Header).Children[1]).ContextMenu.Opened += new RoutedEventHandler(ContextMenu_Opened);

                saveTreeViewItems(currentIndex, (TreeViewItem)new_item, null, "below");
                
                copiedItem = null;
            }
        }
        void delete_request_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("The Request will be permanently deleted!!!\n Are you sure?", "Confirmation !!!", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
            { 
                string request_id = ((Label)(((StackPanel)((TreeViewItem)script_root_tree.SelectedItem).Header)).Children[2]).Content.ToString();
                DBProcessingLayer.deleteRequest(request_id);
                ((TreeViewItem)((TreeViewItem)script_root_tree.SelectedItem).Parent).Items.Remove((TreeViewItem)script_root_tree.SelectedItem);
            }
        }
        void cut_Click(object sender, RoutedEventArgs e)
        {
            copiedItem = null;
            cutItem = (TreeViewItem)script_root_tree.SelectedItem;
        }

        void copied_Click(object sender, RoutedEventArgs e)
        {
            cutItem = null;
            copiedItem = (TreeViewItem)script_root_tree.SelectedItem;
        }

        void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            ContextMenu _ContextMenu = (ContextMenu)e.OriginalSource;

            if (copiedItem == null && cutItem == null)
            {
                ((MenuItem)_ContextMenu.Items[2]).IsEnabled = false;
                ((MenuItem)_ContextMenu.Items[3]).IsEnabled = false;
            }
            else
            {
                ((MenuItem)_ContextMenu.Items[2]).IsEnabled = true;
                ((MenuItem)_ContextMenu.Items[3]).IsEnabled = true;
            }
        }
        
        void request_referers_Expanded(object sender, RoutedEventArgs e)
        {
            try
            {
                TreeViewItem _item = (TreeViewItem)e.OriginalSource;
                if (_item.Header.ToString().Contains("RequestReferer"))
                {
                    String _referer = ((RequestReferer)(_item.Header)).Referer;                    
                    _item.Items.Clear();

                    ArrayList request_ids = DBProcessingLayer.getAllRequestIDs_for_Referer(_referer);

                    for (int i = 0; i < request_ids.Count; i++)
                    {
                        TreeViewItem _new_item = new TreeViewItem();
                        _new_item.Foreground = new SolidColorBrush(Colors.White);
                    
                        string context_path = "";
                        double req = (double)request_ids[i];
                        ArrayList paths = DBProcessingLayer.getAllPaths_for_Request_ID(req);
                        for (int j = 0; j < paths.Count; j++)
                        {
                            context_path += "/" + paths[j];
                        }

                        context_path = context_path.Replace("//","/");

                        if (paths.Count > 0)
                        {
                            Image _image = new Image();
                            _image.Height = 20;
                            _image.Width = 20;

                            if (context_path.EndsWith(".png"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/png.png", UriKind.Relative));
                            }
                            else if (context_path.EndsWith(".jpg") || context_path.EndsWith(".jpeg"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/jpeg.png", UriKind.Relative));
                            }
                            else if (context_path.EndsWith(".ico"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/ico.png", UriKind.Relative));
                            }
                            else if (context_path.EndsWith(".bmp"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/bmp.png", UriKind.Relative));
                            }
                            else if (context_path.EndsWith(".gif"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/gif.png", UriKind.Relative));
                            }
                            else if (context_path.Equals("/"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/browser.png", UriKind.Relative));
                            }
                            else if (context_path.EndsWith(".js"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/js.png", UriKind.Relative));
                            }
                            else if (context_path.EndsWith(".css"))
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/css.png", UriKind.Relative));
                            }
                            else
                            {
                                _image.Source = new BitmapImage(new Uri(@"/Amigo;component/icons/browser.png", UriKind.Relative));
                            }
                            Label _label = new Label();
                            _label.Foreground = new SolidColorBrush(Colors.White);
                            
                            _label.Content = context_path;

                            StackPanel _stackPanel = new StackPanel();
                            _stackPanel.Orientation = Orientation.Horizontal;
                            _stackPanel.Children.Add(_image);
                            _stackPanel.Children.Add(_label);

                            _new_item.Header = _stackPanel;

                            _item.Items.Add(_new_item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }        

        private void MenuItem_Close_Project_Click(object sender, RoutedEventArgs e)
        {
            recording_sessions.IsExpanded = false;
            Amigo.App._project.RecordingSessions = null;
            Amigo.App._project.ParamPanelsVisibility = System.Windows.Visibility.Hidden;
            Amigo.App._project.InfoPanelsVisibility = System.Windows.Visibility.Visible;                     
            Amigo.App._project.ProjectName = "";
            Amigo.App.currentProjectID = -1;
            Amigo.App._buttons.RecordButton = false;
            Amigo.App._buttons.SaveProject = false;
            Amigo.App._buttons.StopButton = false;
            host_parameterization_button.IsEnabled = false;
            App._project.ReplayButtonStatus = false;
            Amigo.App._project.toEnable = false;
            Amigo.App._project.Scenarios = null;//new NotifiableObservableCollection<Scenario>();
            Amigo.App._project.Scripts = null;// new NotifiableObservableCollection<Script>();
            Amigo.App._project.CurrentScenario = null;
            Amigo.App._project.isEnableInitializeLoadTestButton = false;            
        }

        private void script_root_tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            DBProcessingLayer.saveAll_Parameterized_Values();

            try
            {
                TreeViewItem new_value = null;
                bool goAhead = false;

                if(e.NewValue.ToString().Contains("TreeViewItem"))
                {                    
                    new_value = (TreeViewItem)e.NewValue;
                    renamed_item = new_value;//used for rename
                }

                if (script_root_tree.SelectedItem.ToString().Contains("TreeViewItem"))
                {
                    if (((TreeViewItem)script_root_tree.SelectedItem).Name == "")
                    {
                        goAhead = true;
                    }
                }

                if (new_value != null && new_value.Items.Count == 0 && goAhead)
                {
                    Amigo.App._project.ParamPanelsVisibility = System.Windows.Visibility.Visible;
                    Amigo.App._project.InfoPanelsVisibility = System.Windows.Visibility.Collapsed;

                    TreeView _tree = (TreeView)e.Source;
                    TreeViewItem _tree_item = (TreeViewItem)_tree.SelectedItem;
                    StackPanel _tree_item_stack = (StackPanel)_tree_item.Header;

                    Label _label_request_id = (Label)_tree_item_stack.Children[2];
                    String _lable_content = (string)_label_request_id.Content;

                    global_selected_request_id = _lable_content;

                    Label _request_string = (Label)_tree_item_stack.Children[1];
                    string _request_name = (string)_request_string.Content;
                    string[] host_dtls = DBProcessingLayer.getReferer_for_RequestID(_lable_content);

                    Amigo.App._project.SelectedRequestPart1 = host_dtls[0] + ":" + host_dtls[1];
                    Amigo.App._project.SelectedRequestPart2 = _request_name;

                    host_parameterization_button.IsEnabled = true;
                    validate_parameterization_button.IsEnabled = true;
                    DBProcessingLayer.getAllParams_for_request_ID(_lable_content, "ScriptStudio");
                    DBProcessingLayer.getAllHeaders_for_request_ID(_lable_content);
                    if (_thread==null || !_thread.IsAlive)
                    {
                        App._project.ReplayButtonStatus = true;
                    }
                }
            }
            catch { }
        }

        private void host_parameterization_button_Click(object sender, RoutedEventArgs e)
        {
            TreeViewItem _tree_item = (TreeViewItem)script_root_tree.SelectedItem;
            StackPanel _tree_item_stack = (StackPanel)_tree_item.Header;

            Label _label_request_id = (Label)_tree_item_stack.Children[2];
            String _lable_content_request_id = (string)_label_request_id.Content;

            Window window = new Window
            {
                Title = "Create new host and port",
                Content = new HostPortParameterization(_lable_content_request_id),                
            };            
            window.Height = 150;
            window.Width = 560;
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        private void validate_parameterization_button_Click(object sender, RoutedEventArgs e)
        {            
            DBProcessingLayer.saveAll_Parameterized_Values();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {                
                ParamSources _data_source = (ParamSources)e.AddedItems[0];

                if (_data_source != ParamSources.CSV) 
                {
                    ((Parameter)param_data_grid.CurrentItem).SelectedIterationType = IterationType.None;
                }

                ((Parameter)param_data_grid.CurrentItem)._parameterizationSource = _data_source;
            }
        }
        
        private void Itreation_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {
                IterationType _data_source = (IterationType)e.AddedItems[0];

                if (_data_source != IterationType.None && ((Parameter)param_data_grid.CurrentItem)._parameterizationSource != ParamSources.CSV)
                {
                    Xceed.Wpf.Toolkit.MessageBox.Show("This iteration type is valid only for Param Data Source Type = CSV", "Error !!!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                    
                    ComboBox combo = (ComboBox)sender;
                    combo.SelectedItem = e.RemovedItems[0];
                    return;
                }
                ((Parameter)param_data_grid.CurrentItem)._selectedIterationType = _data_source;
            }
        }

        private void CSV_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (param_data_grid.CurrentItem != null)
            {
                string _data_source = (string)e.AddedItems[0];
                ((Parameter)param_data_grid.CurrentItem)._selected_csv_columnName = _data_source;
            }
        }

        private void check_whole_script_button_Click(object sender, RoutedEventArgs e)
        {
            bool successFlag = false;
            int _recording_session_id = -1;            
            if (script_root_tree.SelectedItem != null && script_root_tree.SelectedItem.ToString().Contains("TreeViewItem"))
            {
                try
                {
                    TreeViewItem _tree_item = (TreeViewItem)script_root_tree.SelectedItem;
                    StackPanel _tree_item_stack = (StackPanel)_tree_item.Header;

                    Label _label_request_id = (Label)_tree_item_stack.Children[2];
                    String _lable_content = (string)_label_request_id.Content;
                    _recording_session_id = DBProcessingLayer.getRecordingSession_for_Request_id(_lable_content);

                    successFlag = true;
                }
                catch (Exception) { Xceed.Wpf.Toolkit.MessageBox.Show("Please select a valid request or session node"); }
            }
            else if (script_root_tree.SelectedItem != null && script_root_tree.SelectedItem.ToString().Contains("RecordingSession"))
            {
                try
                {
                    _recording_session_id = ((RecordingSession)(script_root_tree.SelectedItem)).SessionID;
                    Amigo.App.recordingSessionName = ((RecordingSession)(script_root_tree.SelectedItem)).SessionName;
                    successFlag = true;
                }
                catch (Exception) { Xceed.Wpf.Toolkit.MessageBox.Show("Please select a valid request or session node"); }
            }
            else 
            { 
                Xceed.Wpf.Toolkit.MessageBox.Show("Please select a request from the script tree"); 
            }

            if (successFlag == true && _recording_session_id > -1)
            {
                App._project.ReplayButtonStatus = false;
                _thread = new Thread(startReplayWindow);
                _thread.SetApartmentState(ApartmentState.STA);
                _thread.Start(_recording_session_id);
            }
        }

        private void startReplayWindow(Object _recording_session_id)
        {
            int recording_session_id = (Int32)_recording_session_id;
            Window window = new Window
            {
                Title = "Replay and verify the script",
                Content = new ScriptReplayWindow(recording_session_id)                 
            };
            window.Closed += new EventHandler(ScriptReplayWindow_Closed);
            window.Height = 750;
            window.Width = 1100;
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            try
            {
                window.ShowDialog();
            }
            catch { }
        }
        void ScriptReplayWindow_Closed(object sender, EventArgs e)
        {
            Amigo.App.preview_browser_IsVisible = false;
            App._project.ReplayButtonStatus = true;
            ProxyProgram.DoQuit();
        }
        
        Point _lastMouseDown;
        TreeViewItem draggedItem, _target;

        string target_req_str = "";
        string target_req_id = "";
        
        private void TreeView_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                _lastMouseDown = e.GetPosition(script_root_tree);
            }

        }
        private void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    Point currentPosition = e.GetPosition(script_root_tree);
                    
                    if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                        (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                    {
                        if (script_root_tree.SelectedItem.ToString().Contains("TreeViewItem"))
                        {
                            draggedItem = (TreeViewItem)script_root_tree.SelectedItem;
                        }
                        if (draggedItem != null)
                        {
                            DragDropEffects finalDropEffect = DragDrop.DoDragDrop(script_root_tree, script_root_tree.SelectedValue, DragDropEffects.Move);                         
                            if ((finalDropEffect == DragDropEffects.Move) && (_target != null))
                            {
                                // A Move drop was accepted
                                if (draggedItem.Header.ToString().Equals(_target.Header.ToString()))
                                {
                                    CopyItem(draggedItem, _target, currentPosition);
                                    _target = null;
                                    draggedItem = null;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                Point currentPosition = e.GetPosition(script_root_tree);
              
                if ((Math.Abs(currentPosition.X - _lastMouseDown.X) > 10.0) ||
                    (Math.Abs(currentPosition.Y - _lastMouseDown.Y) > 10.0))
                {
                    // Verify that this is a valid drop and then store the drop target
                    TreeViewItem item = GetNearestContainer(e.OriginalSource as UIElement);
                    if (draggedItem != null && item!=null)
                    {
                        if (CheckDropTarget(draggedItem, item))
                        {
                            e.Effects = DragDropEffects.Move;
                        }
                        else
                        {
                            e.Effects = DragDropEffects.None;
                        }
                    }
                }
                e.Handled = true;
            }
            catch (Exception)
            {
            }
        }
        private void treeView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;

                // Verify that this is a valid drop and then store the drop target
                //TreeViewItem TargetItem = GetNearestContainer(e.OriginalSource as UIElement);
                Border TargetItem = e.OriginalSource as Border;
                StackPanel stp=null;
                if (TargetItem != null)
                {
                    ContentPresenter _ContentPresenter = (ContentPresenter)TargetItem.Child;
                    Label lbl = (Label)_ContentPresenter.TemplatedParent;
                    stp = (StackPanel)lbl.Parent;
                    target_req_str = ((Label)stp.Children[1]).Content.ToString();
                    target_req_id = ((Label)stp.Children[2]).Content.ToString();
                }

                if (TargetItem != null && draggedItem != null )
                {
                    _target = (TreeViewItem)stp.Parent;
                    e.Effects = DragDropEffects.Move;
                }
            }
            catch (Exception)
            {
            }
        }
        private bool CheckDropTarget(TreeViewItem _sourceItem, TreeViewItem _targetItem)
        {
            //Check whether the target item is meeting your condition
            bool _isEqual = false;
            if (_sourceItem.Header.ToString().Equals(_targetItem.Header.ToString()))
            {
                _isEqual = true;
            }
            return _isEqual;

        }

        private void CopyItem(TreeViewItem _sourceItem, TreeViewItem _targetItem, Point currentPosition)
        {
            try
            {
                string source_string = ((Label)((StackPanel)_sourceItem.Header).Children[1]).Content.ToString();
                if (Xceed.Wpf.Toolkit.MessageBox.Show("Are you sure to move \"" + source_string + "\"", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                
                    int sourceIndex = ((TreeViewItem)_sourceItem.Parent).Items.IndexOf(_sourceItem);
                    int targetIndex = ((TreeViewItem)_targetItem.Parent).Items.IndexOf(_targetItem);
                        
                    ((TreeViewItem)_sourceItem.Parent).Items.RemoveAt(sourceIndex);
                    ((TreeViewItem)_targetItem.Parent).Items.Insert(targetIndex, _sourceItem);

                    _sourceItem.IsSelected = true;                
                }
            }
            catch
            {
            }
        }
        
        static TObject FindVisualParent<TObject>(UIElement child) where TObject : UIElement
        {
            if (child == null)
            {
                return null;
            }

            UIElement parent = VisualTreeHelper.GetParent(child) as UIElement;

            while (parent != null)
            {
                TObject found = parent as TObject;
                if (found != null)
                {
                    return found;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent) as UIElement;
                }
            }

            return null;
        }
        
        private TreeViewItem GetNearestContainer(UIElement element)
        {
            // Walk up the element tree to the nearest tree view item.
            TreeViewItem container = element as TreeViewItem;
            
            while ((container == null) && (element != null))
            {
                element = VisualTreeHelper.GetParent(element) as UIElement;
                container = element as TreeViewItem;
            }
            return container;
        }

        private void script_root_tree_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }

        private void recording_sessions_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ContextMenu _ContextMenu = recording_sessions.ContextMenu;

            if (Amigo.App._project.ProjectName.Length > 10)
            {
                ((MenuItem)_ContextMenu.Items[0]).IsEnabled = true;
                ((MenuItem)_ContextMenu.Items[1]).IsEnabled = true;
                ((MenuItem)_ContextMenu.Items[2]).IsEnabled = true;
                ((MenuItem)_ContextMenu.Items[3]).IsEnabled = true;
                ((MenuItem)_ContextMenu.Items[4]).IsEnabled = false;
                ((MenuItem)_ContextMenu.Items[5]).IsEnabled = true;
            }
            else
            {
                ((MenuItem)_ContextMenu.Items[0]).IsEnabled = false;
                ((MenuItem)_ContextMenu.Items[1]).IsEnabled = false;
                ((MenuItem)_ContextMenu.Items[2]).IsEnabled = false;
                ((MenuItem)_ContextMenu.Items[3]).IsEnabled = false;
                ((MenuItem)_ContextMenu.Items[4]).IsEnabled = true;
                ((MenuItem)_ContextMenu.Items[5]).IsEnabled = false;
            }
        }

        Process proc;        
        private static TreeViewItem renamed_item;
        private string global_selected_request_id = "";

        private void MenuItem_record_script_Into_project_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Proceed with recording a new session?", "Confirmation !!!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {                    
                App.isRecording = true;
                App._buttons.RecordButton = false;
                Thread.Sleep(500);
                ProcessStartInfo startInfo = new ProcessStartInfo("IExplore.exe");
                startInfo.WindowStyle = ProcessWindowStyle.Maximized;
                DBProcessingLayer.createHTMLfromURLS();
                startInfo.Arguments = Path.GetFullPath(".\\..\\..\\Interfaces\\browse.html");
                proc = Process.Start(startInfo);

                Window window = new Window
                {                   
                    Content = new TransactionName()                    
                };

                window.Height = 270;
                window.Width = 370;
                window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.Topmost = true;
                window.Closing += new CancelEventHandler(window_Closing);
                window.Closed += new EventHandler(window_Closed);                
                window.ShowDialog();
            }
            else
            {
                return;
            }
        }
        void window_Closed(object sender, EventArgs e)
        {
            try
            {
                if(!proc.HasExited)
                proc.Kill();
            }
            catch (Exception)
            {
                //Console.Write(procex.Message);
            }

            App._buttons.RecordButton = true;
            DBProcessingLayer.retrieveRecordingSessions();            
            DBProcessingLayer.retrieveRequestReferers();
        }        

        private void rename_project_click(object sender, RoutedEventArgs e)
        {
            Window window = new Window
            {
                Content = new Rename_Project()
            };

            window.Height = 200;
            window.Width = 300;
            window.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            window.WindowStyle = WindowStyle.SingleBorderWindow;
            window.Topmost = true;            
            window.ShowDialog();           
        }
        private void MenuItem_delete_Project_Click(object sender, RoutedEventArgs e)
        {
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Project will be permanently deleted!!!\n Are you sure?", "Confirmation", MessageBoxButton.YesNo,MessageBoxImage.Error) == MessageBoxResult.Yes)
            {
                DBProcessingLayer.deleteProject(Amigo.App.currentProjectID);
                MenuItem_Close_Project_Click(sender, e);    
            }            
        }
        private void MenuItem_delete_script_click(object sender, RoutedEventArgs e)
        {
            MenuItem _menu_item = (MenuItem)e.OriginalSource;
            ContextMenu _context_menu = (ContextMenu)_menu_item.Parent;
            Label _context_lable = (Label)_context_menu.PlacementTarget;
            TreeViewItem _script_node = FindVisualParent<TreeViewItem>((Label)_context_lable);
            RecordingSession _session = (RecordingSession)_script_node.Header;
            if (Xceed.Wpf.Toolkit.MessageBox.Show(_context_lable.Content + " will be permanently deleted!!!\n Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
            {
                DBProcessingLayer.deleteScript(_session.SessionID);
                Amigo.App._project.RecordingSessions.Remove(_session);
            }
        }
        private void MenuItem_expand_script_click(object sender, RoutedEventArgs e)
        {
            MenuItem _menu_item = (MenuItem)e.OriginalSource;
            ContextMenu _context_menu = (ContextMenu)_menu_item.Parent;
            Label _context_lable = (Label)_context_menu.PlacementTarget;
            TreeViewItem _script_node = FindVisualParent<TreeViewItem>((Label)_context_lable);
            _script_node.IsExpanded = true;
        }
        private void MenuItem_collapse_script_click(object sender, RoutedEventArgs e)
        {
            MenuItem _menu_item = (MenuItem)e.OriginalSource;
            ContextMenu _context_menu = (ContextMenu)_menu_item.Parent;
            Label _context_lable = (Label)_context_menu.PlacementTarget;
            TreeViewItem _script_node = FindVisualParent<TreeViewItem>((Label)_context_lable);
            _script_node.IsExpanded = false;
        }
        //RoutedEventHandler(edit_page_text_box);//IsInEditMode = true;
        private void edit_page_text_box(object sender, RoutedEventArgs e)
        {
            MenuItem _menu_item = (MenuItem)e.OriginalSource;
            ContextMenu _context_menu = (ContextMenu)_menu_item.Parent;
            TreeViewItem _script_node = (TreeViewItem)_context_menu.PlacementTarget;
            ((EditableTextBlock)((StackPanel)_script_node.Header).Children[1]).IsInEditMode = true;
        }
        public static bool saveRename()
        {
            try
            {
                TreeViewItem renamed_item = ScriptStudio.renamed_item;
                StackPanel _StackPanel = (StackPanel)renamed_item.Header;
                string renamed_text = ((TextBlock)_StackPanel.Children[1]).Text;

                TreeViewItem first_request = (TreeViewItem)renamed_item.Items[0];
                string req = ((Label)((StackPanel)first_request.Header).Children[2]).Content.ToString();
                DBProcessingLayer.updateTitle(req, renamed_text);
                ScriptStudio.renamed_item = null;
                return true;
            }
            catch (SQLiteException)
            {                
                Xceed.Wpf.Toolkit.MessageBox.Show("Please check if any special character present", "Error !!!", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return false;
            }
            catch (Exception)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show("Please first select the Page node then click rename.\nOr Just Tripple click the Page node to rename", "Error !!!",MessageBoxButton.OK, MessageBoxImage.Error,MessageBoxResult.OK);
                return false;
            }
        }
        private void MenuItem_expand_page_click(object sender, RoutedEventArgs e)
        {
            MenuItem _menu_item = (MenuItem)e.OriginalSource;
            ContextMenu _context_menu = (ContextMenu)_menu_item.Parent;
            TreeViewItem _script_node = (TreeViewItem)_context_menu.PlacementTarget;
            _script_node.IsExpanded = true;
        }
        private void MenuItem_collapse_page_click(object sender, RoutedEventArgs e)
        {
            MenuItem _menu_item = (MenuItem)e.OriginalSource;
            ContextMenu _context_menu = (ContextMenu)_menu_item.Parent;
            TreeViewItem _script_node = (TreeViewItem)_context_menu.PlacementTarget;
            _script_node.IsExpanded = false;
        }

        private void delete_all_request_for_this_server(object sender, RoutedEventArgs e)
        {
            MenuItem _MenuItem = (MenuItem)e.OriginalSource;
            ContextMenu _ContextMenu = (ContextMenu)_MenuItem.Parent;
            Label _Label = (Label)_ContextMenu.PlacementTarget;
            string _referer = _Label.Content.ToString();
            if (Xceed.Wpf.Toolkit.MessageBox.Show("Requests under " + _referer + " will be permanently deleted!!!\n Are you sure?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
            {
                ArrayList request_ids = DBProcessingLayer.getAllRequestIDs_for_Referer(_referer);
                for (int i = 0; i < request_ids.Count; i++)
                {
                    DBProcessingLayer.deleteRequest(request_ids[i].ToString());
                }
                DBProcessingLayer.retrieveRequestReferers();
                DBProcessingLayer.retrieveRecordingSessions();
            }
        }
    }
}