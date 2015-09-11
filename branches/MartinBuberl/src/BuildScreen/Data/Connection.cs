using System.ComponentModel;
using BuildScreen.Core.Utilities;

namespace BuildScreen.Data
{
    public class Connection : INotifyPropertyChanged,  IDataErrorInfo
    {
        private string _domain;
        private string _port;
        private bool _secureSocketlayer;
        private string _userName;
        private string _password;

        public string Domain
        {
            get
            {
                return _domain;
            }
            set
            {
                _domain = value;

                OnPropertyChanged("Domain");
                OnPropertyChanged("Preview");
            }
        }

        // TODO: I had this property as integer but then the reaction speed of the textbox was too slow, somwhow this should be fixed
        public string Port
        {
            get
            {
                return _port;
            }
            set
            {
                _port = value;

                OnPropertyChanged("Port");
                OnPropertyChanged("Preview");
            }
        }

        public bool SecureSocketLayer
        {
            get
            {
                return _secureSocketlayer;
            }
            set
            {
                _secureSocketlayer = value;

                OnPropertyChanged("Preview");
            }
        }

        public bool IgnoreInvalidCertificate { get; set; }

        public string Preview
        {
            get
            {
                string preview = SecureSocketLayer ? "https://" : "http://";

                preview += Domain;

                if (!string.IsNullOrEmpty(Port))
                    preview += ":" + Port;

                return preview + "/";
            }
            set
            {
            }
        }

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;

                OnPropertyChanged("UserName");
            }
        }

        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;

                OnPropertyChanged("Password");
            }
        }

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        #endregion

        #region Implementation of IDataErrorInfo

        public string this[string columnName]
        {
            get
            {
                string result = null;

                switch (columnName)
                {
                    case "Domain":
                        if (string.IsNullOrEmpty(_domain) || (!Validation.IsDomain(_domain) && !Validation.IsIPv4(_domain)))
                            result = "TODO: Validation Resource Domain";
                        break;
                    case "Port":
                        if (!string.IsNullOrEmpty(_port) && !Validation.IsPort(_port))
                            result = "TODO: Validation Resource Port 0 to 65535";
                        break;
                }

                return result;
            }
        }

        public string Error
        {
            get
            {
                return null;
            }
        }

        #endregion
    }
}
