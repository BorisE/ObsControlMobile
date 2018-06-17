using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace ObsControlMobile.ViewModels
{
    public class ConfigViewModel : BaseViewModel
    {
        public ConfigViewModel()
        {
            Title = "Config";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("http://astrohostel.ru")));
        }

        public ICommand OpenWebCommand { get; }
    }
}