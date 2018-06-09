using System;
using System.Windows.Input;

using AsrtoUtils;

using Xamarin.Forms;

namespace ObsControlMobile.ViewModels
{
    public class DataViewModel : BaseViewModel
    {

        #region AllSky

        string allskyurl = "";
        public string AllSkyURL
        {
            get { return allskyurl; }
            set { SetProperty(ref allskyurl, value); }
        }

        string allskydatest;
        public string AllSkyDate
        {
            get { return allskydatest; }
            set { SetProperty(ref allskydatest, value);}
        }


        #endregion AllSky

        #region Timings

        string sunsettimest = "";
        public string SunsetTimeSt
        {
            get { return sunsettimest; }
            set { SetProperty(ref sunsettimest, value); }
        }

        string civtwilightendtimest = "";
        public string CivTwilightEndTimeSt
        {
            get { return civtwilightendtimest; }
            set { SetProperty(ref civtwilightendtimest, value); }
        }

        string navtwilightendtimest = "";
        public string NavTwilightEndTimeSt
        {
            get { return navtwilightendtimest; }
            set { SetProperty(ref navtwilightendtimest, value); }
        }

        string astrotwilightendtimest = "";
        public string AstroTwilightEndTimeSt
        {
            get { return astrotwilightendtimest; }
            set { SetProperty(ref astrotwilightendtimest, value); }
        }

        string astrotwilightbegtimest = "";
        public string AstroTwilightBegTimeSt
        {
            get { return astrotwilightbegtimest; }
            set { SetProperty(ref astrotwilightbegtimest, value); }
        }

        string navtwilightbegtimest = "";
        public string NavTwilightBegTimeSt
        {
            get { return navtwilightbegtimest; }
            set { SetProperty(ref navtwilightbegtimest, value); }
        }

        string civtwilightbegtimest = "";
        public string CivTwilightBegTimeSt
        {
            get { return civtwilightbegtimest; }
            set { SetProperty(ref civtwilightbegtimest, value); }
        }

        string sunrisetimest = "";
        public string SunriseTimeSt
        {
            get { return sunrisetimest; }
            set { SetProperty(ref sunrisetimest, value); }
        }

        #endregion Timings


        public DataViewModel()
        {
            Title = "AstroData";
            RefreshAllSkyImage();

            RefreshAllSkyCommand = new Command(() => Device.OpenUri(new Uri("http://www.astromania.info/observatory/")));

            //Timings
            RecalculateTimes();
        }

        public void RefreshAllSkyImage()
        {
            //Allsky
            AllSkyURL = "http://astro.milantiev.com/allsky-current.jpg?time=" + DateTime.Now.Ticks;
            AllSkyDate = DateTime.Now.ToString("HH:mm:ss");
        }

        public void RecalculateTimes()
        {
            SunsetTimeSt = "Sunset: " + AstroUtilsProp.SunSetDateTime().ToString("HH:mm:ss"); 
            CivTwilightEndTimeSt = "Civ end: " + AstroUtilsProp.CivilTwilightSetDateTime().ToString("HH:mm:ss");
            NavTwilightEndTimeSt = "Nav end: " + AstroUtilsProp.NautTwilightSetDateTime().ToString("HH:mm:ss");
            AstroTwilightEndTimeSt = "Astro end: " + AstroUtilsProp.AstronTwilightSetDateTime().ToString("HH:mm:ss");

            AstroTwilightBegTimeSt = "Asto beg: " + AstroUtilsProp.AstronTwilightRiseDateTime().ToString("HH:mm:ss");
            NavTwilightBegTimeSt = "Nav beg: " + AstroUtilsProp.NautTwilightRiseDateTime().ToString("HH:mm:ss");
            CivTwilightBegTimeSt = "Civ beg: " + AstroUtilsProp.CivilTwilightRiseDateTime().ToString("HH:mm:ss");
            SunriseTimeSt = "Sunrise: " + AstroUtilsProp.SunRiseDateTime().ToString("HH:mm:ss");
        }

        public ICommand RefreshAllSkyCommand { get; }
    }
}