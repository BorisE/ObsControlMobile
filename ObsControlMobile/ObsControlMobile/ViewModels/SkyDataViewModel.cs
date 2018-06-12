using System;
using System.Windows.Input;

using AsrtoUtils;
using Newtonsoft.Json;
using ObsControlMobile.Services;
using Xamarin.Forms;

namespace ObsControlMobile.ViewModels
{
    public class AllSkyDataJSON
    {
        //{"filename":"allsky-current.jpg","timestamp":1528686365}
        public string filename = "";
        public int timestamp = 0;
    }

    public class SkyDataViewModel : BaseViewModel
    {
        Page ParentPage;



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

        string currentdate;
        public string CurrentDate
        {
            get { return currentdate; }
            set { SetProperty(ref currentdate, value); }
        }
        


        #endregion AllSky



        string meteoblueiframe = "";
        public string MeteoBlueIFrame
        {
            get { return meteoblueiframe; }
            set { SetProperty(ref meteoblueiframe, value); }
        }

        bool isdownloading = false;
        public bool IsDownloading
        {
            get { return isdownloading; }
            set { SetProperty(ref isdownloading, value); }
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


        public SkyDataViewModel(Page ExtPP)
        {
            ParentPage = ExtPP;

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
            CurrentDate = DateTime.Now.ToString("HH:mm:ss");

            GetJSON();

        }

        public async void GetJSON()
        {
            // Check network status  
            if (NetworkCheck.IsConnectedToInternet())
            {
                IsDownloading = true;

                var client = new System.Net.Http.HttpClient();
                var response = await client.GetAsync("http://astro.milantiev.com/allsky/stat.php");
                string contactsJson = await response.Content.ReadAsStringAsync(); //Getting response  

                AllSkyDataJSON ObjContactList = new AllSkyDataJSON();
                if (contactsJson != "")
                {
                    //Converting JSON Array Objects into generic list  
                    ObjContactList = JsonConvert.DeserializeObject<AllSkyDataJSON>(contactsJson);
                }

                DateTime ASDT = ServiceClass.UnixTimeStampToDateTime(ObjContactList.timestamp);

                AllSkyDate = ServiceClass.ConvertToLocal(ASDT).ToString("HH:mm:ss");

                IsDownloading = false;
            }
            else
            {
                await ParentPage.DisplayAlert("Get allsky data", "No network is available.", "Ok");
            }
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