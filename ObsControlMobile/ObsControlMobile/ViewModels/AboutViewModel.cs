using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace ObsControlMobile.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("http://astrohostel.ru")));
        }

        public ICommand OpenWebCommand { get; }
    }
}