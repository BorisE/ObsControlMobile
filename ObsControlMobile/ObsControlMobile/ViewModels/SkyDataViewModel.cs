using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Windows.Input;

using AsrtoUtils;
using Newtonsoft.Json;
using ObsControlMobile.Models;
using ObsControlMobile.Services;
using Xamarin.Forms;

namespace ObsControlMobile.ViewModels
{


    public class SkyDataViewModel : BaseViewModel
    {
        Page ParentPage;

        public DownloadResult GetDataResult = DownloadResult.Undefined;


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

        bool isdownloading = false;
        public bool IsDownloading
        {
            get { return isdownloading; }
            set { SetProperty(ref isdownloading, value); }
        }
        #endregion AllSky

        #region Meteoblue
        string meteoblueiframe = "";
        public string MeteoBlueIFrame
        {
            get { return meteoblueiframe; }
            set { SetProperty(ref meteoblueiframe, value); }
        }
        #endregion meteoblue    



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

            AllSkyTapCommand = new Command(() => RefreshAllSkyImage());
            RefreshAllSkyCommand = new Command(() => RefreshAllSkyImage());
            OpenSiteCommand = new Command(() => Device.OpenUri(new Uri("http://www.astromania.info/observatory/")));

            //Allsky Data
            RefreshAllSkyImage();

            //Meteoblue frame
            MeteoBlueIFrame = "<p><iframe style=\"width: 520px; height: 709px;\" src=\"https://www.meteoblue.com/en/weather/widget/seeing/il%27skiy_russia_556951?geoloc=fixed\" width=\"300\" height=\"150\" frameborder=\"0\" scrolling=\"NO\" sandbox=\"allow-same-origin allow-scripts allow-popups\"></iframe></p>";

            //Timings
            RecalculateTimes();
        }

        public void RefreshAllSkyImage()
        {
            //Allsky URL propertie with cache tweak
            AllSkyURL = "http://astro.milantiev.com/allsky-current.jpg?time=" + DateTime.Now.Ticks;
            //Set CurrentDate propertie
            CurrentDate = DateTime.Now.ToString("HH:mm:ss");
            //Download status data
            GetAllSkyJSONData();
        }

        private async void GetAllSkyJSONData()
        {
            Debug.WriteLine("GetAllSkyJSONData enter");
            if (IsBusy)
            {
                Debug.WriteLine("GetAllSkyJSONData already busy, return");
                return;
            }
            IsBusy = true;
            GetDataResult = DownloadResult.Undefined;
            Debug.WriteLine("GetAllSkyJSONData started, DownloadResult:" + GetDataResult);

            // Check network status  
            if (NetworkCheck.IsConnectedToInternet())
            {
                try
                {
                    //1. Download data
                    Uri geturi = new Uri("http://astro.milantiev.com/allsky/stat.php"); //replace your xml url  
                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = await client.GetAsync(geturi);

                    Debug.WriteLine("GetAllSkyJSONData download completead, StatusCode:" + response.StatusCode);

                    //2. If data ok
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //2.2. Read string
                        string responseSt = await response.Content.ReadAsStringAsync();
                        Debug.WriteLine("GetAllSkyJSONData DOWNLOAD DATA:" + responseSt);

                        try
                        {
                            //3.1 Set JSON parse options
                            JsonSerializerSettings JSONSettings = new JsonSerializerSettings();
                            JSONSettings.Culture = new CultureInfo("ru-RU");
                            JSONSettings.Culture.NumberFormat.NumberDecimalSeparator = ".";
                            JSONSettings.NullValueHandling = NullValueHandling.Ignore;

                            //3.2. JSON convert
                            AllSkyDataClass objResponse = JsonConvert.DeserializeObject<AllSkyDataClass>(responseSt, JSONSettings);

                            //4. Data setting
                            DateTime ASDT = ServiceClass.UnixTimeStampToDateTime(objResponse.timestamp);
                            AllSkyDate = ServiceClass.ConvertToLocal(ASDT).ToString("HH:mm:ss");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Exception in GetAllSkyJSONData JSON parsing");
                            Debug.WriteLine(ex);
                            GetDataResult = DownloadResult.ParseError;
                        }
                    }
                    else
                    {
                        GetDataResult = DownloadResult.DownloadError;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception in GetAllSkyJSONData download");
                    Debug.WriteLine(ex);
                    GetDataResult = DownloadResult.DownloadError;
                }
            }
            else
            {
                await ParentPage.DisplayAlert("Get allsky data", "No network is available.", "Ok");
            }
            IsBusy = false;
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