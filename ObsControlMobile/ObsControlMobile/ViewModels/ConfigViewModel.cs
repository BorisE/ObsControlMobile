using ObsControlMobile.Services;
using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace ObsControlMobile.ViewModels
{
    public class ConfigViewModel : BaseViewModel
    {
        public ICommand SaveButtonClickCommand { get; }

        #region IQP
         public string IQPURL
         {
            get => Settings.IQPURL;
            set
            {
                if (Settings.IQPURL == value)
                    return;

                Settings.IQPURL = value;
                OnPropertyChanged();
            }

         }
        #endregion IQP    

        #region Astohostel Login
        public string Login
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

        public string Pass
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

        public ConfigViewModel()
        {
            Title = "Config";

            SaveButtonClickCommand = new Command(() => SaveSettings());
        }

        private void SaveSettings()
        {

        }

    }
}