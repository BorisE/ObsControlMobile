using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace ObsControlMobile.ViewModels
{
    public class DataViewModel : BaseViewModel
    {

        string sunrisetimest = "";
        public string SunriseTimeSt
        {
            get { return sunrisetimest; }
            set { SetProperty(ref sunrisetimest, value); }
        }


        public DataViewModel()
        {
            Title = "AstroData";
            SunriseTimeSt = "Sunrise: " + DateTime.Now.ToString("HH:mm:ss");
        }

        public ICommand OpenWebCommand { get; }
    }
}