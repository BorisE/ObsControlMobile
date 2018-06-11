using System;
using System.Windows.Input;

using AsrtoUtils;

using Xamarin.Forms;

namespace ObsControlMobile.ViewModels
{
    public class SkyDataViewModel : BaseViewModel
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

        

        string meteoblueiframe = "";
        public string MeteoBlueIFrame
        {
            get { return meteoblueiframe; }
            set { SetProperty(ref meteoblueiframe, value); }
        }

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


        public SkyDataViewModel()
        {
            Title = "Sky";
            RefreshAllSkyImage();

            MeteoBlueIFrame = "<p><iframe style=\"width: 520px; height: 709px;\" src=\"https://www.meteoblue.com/en/weather/widget/seeing/il%27skiy_russia_556951?geoloc=fixed\" width=\"300\" height=\"150\" frameborder=\"0\" scrolling=\"NO\" sandbox=\"allow-same-origin allow-scripts allow-popups\"></iframe></p>";


            AllSkyTapCommand = new Command(() => RefreshAllSkyImage());
            RefreshAllSkyCommand = new Command(() => RefreshAllSkyImage());
            OpenSiteCommand = new Command(() => Device.OpenUri(new Uri("http://www.astromania.info/observatory/")));

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
            SunsetTimeSt = AstroUtilsProp.SunSetDateTime().ToString("HH:mm"); 
            CivTwilightEndTimeSt = AstroUtilsProp.CivilTwilightSetDateTime().ToString("HH:mm");
            NavTwilightEndTimeSt = AstroUtilsProp.NautTwilightSetDateTime().ToString("HH:mm");
            AstroTwilightEndTimeSt = AstroUtilsProp.AstronTwilightSetDateTime().ToString("HH:mm");

            AstroTwilightBegTimeSt = AstroUtilsProp.AstronTwilightRiseDateTime().ToString("HH:mm");
            NavTwilightBegTimeSt = AstroUtilsProp.NautTwilightRiseDateTime().ToString("HH:mm");
            CivTwilightBegTimeSt = AstroUtilsProp.CivilTwilightRiseDateTime().ToString("HH:mm");
            SunriseTimeSt = AstroUtilsProp.SunRiseDateTime().ToString("HH:mm");
        }

        public ICommand RefreshAllSkyCommand { get; }
        public ICommand AllSkyTapCommand { get; }

        public ICommand OpenSiteCommand { get; }
        
    }
}