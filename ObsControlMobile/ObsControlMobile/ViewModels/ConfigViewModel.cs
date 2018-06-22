using ObsControlMobile.Services;
using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace ObsControlMobile.ViewModels
{
    public class ConfigViewModel : BaseViewModel
    {
        public ICommand SaveButtonClickCommand { get; }

        public ConfigViewModel()
        {
            Title = "Config";

            SaveButtonClickCommand = new Command(() => SaveSettings());
        }

        internal void InitConfigValues()
        {
            IQPURL = Settings.IQPURL;
            Login = Settings.Login;
            Pass = Settings.Pass;
        }


        #region IQP
        public string IQPURL1111111
         {
            get => Settings.IQPURL;
            set
            {
                if (Settings.IQPURL == value)
                    return;
                if (value == "")
                {
                    ErrorMessage = "Empty URL, resetting to default...";
                    Settings.IQPURL = Settings.DefaultIQPURL;
                }
                else
                {
                    if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
                    {
                        ErrorMessage = "Invalid URL, IQP probably wouldn't work";
                    }
                    else
                    {
                        ErrorMessage = "";
                    }
                }
                OnPropertyChanged();
            }
         }


        string _iqpurl = "";
        public string IQPURL
        {
            get { return _iqpurl; }
            set {
                if (_iqpurl == value)
                    return;

                if (value == "")
                {
                    ErrorMessage = "Empty URL will be reset to default one on save";
                }
                else
                {
                    if (!Uri.IsWellFormedUriString(value, UriKind.Absolute))
                    {
                        ErrorMessage = "Invalid URL, IQP probably wouldn't work";
                    }
                    else
                    {
                        ErrorMessage = "";
                    }
                }
                SetProperty(ref _iqpurl, value);
            }
        }
        #endregion IQP    

        #region Astohostel Login
        public string Login111
        {
            get => Settings.Login;
            set
            {
                if (Settings.Login == value)
                    return;

                Settings.Login = value;
                OnPropertyChanged();
            }
        }

        string _login = "";
        public string Login
        {
            get { return _login; }
            set { SetProperty(ref _login, value); }
        }

        string _pass = "";
        public string Pass
        {
            get { return _pass; }
            set { SetProperty(ref _pass, value); }
        }
        public string Pass11111
        {
            get => Settings.Pass;
            set
            {
                if (Settings.Pass == value)
                    return;

                Settings.Pass = value;
                OnPropertyChanged();
            }
        }
        #endregion Astohostel Login

        
        string _errormessage = "";
        public string ErrorMessage
        {
            get { return _errormessage; }
            set { SetProperty(ref _errormessage, value); }
        }

        private void SaveSettings()
        {
            // IQP URL
            if (IQPURL == "")
            {
                ErrorMessage = "Empty URL, resetting to default...";
                IQPURL = Settings.DefaultIQPURL;
            }

            if (!Uri.IsWellFormedUriString(IQPURL, UriKind.Absolute))
            {
                ErrorMessage = "Invalid URL, IQP probably wouldn't work";
            }
            else
            {
                ErrorMessage = "";
            }
            Settings.IQPURL = IQPURL;

            //Login
            if (Login == "")
            {
                Login = Settings.DefaultLogin;
                ErrorMessage += (ErrorMessage != "" ? Environment.NewLine : "") + "Empty login, resetting to default...";
            }
            Settings.Login = Login;

            //Password
            Settings.Pass= Pass;

            ErrorMessage += (ErrorMessage!=""?Environment.NewLine:"") + "Settings saved";
        }

    }
}