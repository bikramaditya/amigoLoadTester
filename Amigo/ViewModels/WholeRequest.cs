using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amigo.ViewModels
{
    public class WholeRequest
    {
        private string _recording_session_id;
        private string _recording_session_name;
        private List<int> _request_ids;
        private List<string> _request_referers;
        private List<string> _request_params;
        private List<string> _request_headers;

        public string Recording_Session_ID
        {
            get { return _recording_session_id; }
            set { _recording_session_id = value; }
        }
        public string Recording_Session_Name
        {
            get { return _recording_session_name; }
            set { _recording_session_name = value; }
        }
        public List<int> Request_IDs
        {
            get { return _request_ids; }
            set { _request_ids = value; }
        }
        public List<string> Request_Referers
        {
            get { return _request_referers; }
            set { _request_referers = value; }
        }
        public List<string> Request_Params
        {
            get { return _request_params; }
            set { _request_params = value; }
        }
        public List<string> Request_Headers
        {
            get { return _request_headers; }
            set { _request_headers = value; }
        }
    }
}