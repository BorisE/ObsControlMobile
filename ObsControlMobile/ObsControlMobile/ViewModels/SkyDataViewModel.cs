using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

using AsrtoUtils;
using AsrtoUtils.Conversion;
using AstroUtils;
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

        #region Moon

        string _moonlefttimest = "";
        public string MoonLeftTimeSt
        {
            get { return _moonlefttimest; }
            set { SetProperty(ref _moonlefttimest, value); }

        }

        string _moonrighttimest = "";
        public string MoonRightTimeSt
        {
            get { return _moonrighttimest; }
            set { SetProperty(ref _moonrighttimest, value); }
        }

        int _moon_left_colspan = 1;
        public int Moon_Left_ColSpan
        {
            get { return _moon_left_colspan; }
            set { SetProperty(ref _moon_left_colspan, value); }
        }

        int _moon_right_colspan = 1;
        public int Moon_Right_ColSpan
        {
            get { return 8 - _moon_left_colspan; }
        }

        public int Moon_Right_ColStart
        {
            get { return 0 + _moon_left_colspan; }
        }

        const string COLOR_MoonBelow = "DarkBlue";
        const string COLOR_MoonAbove = "LightSkyBlue";

        string _moon_left_color = COLOR_MoonBelow;
        public string Moon_Left_Color
        {
            get { return _moon_left_color; }
            set { SetProperty(ref _moon_left_color, value); }
        }

        string _moon_right_color = COLOR_MoonBelow;
        public string Moon_Right_Color
        {
            get { return _moon_right_color; }
            set { SetProperty(ref _moon_right_color, value); }
        }

        #endregion Moon

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

        private async void GetAllSkyJSONData_old()
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
            if (NetworkServices.IsConnectedToInternet())
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
                            DateTime ASDT = DateTimeUtils.UnixTimeStampToDateTime(objResponse.timestamp);
                            AllSkyDate = DateTimeUtils.ConvertToLocal(ASDT).ToString("HH:mm:ss");
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


        private async void GetAllSkyJSONData()
        {
            Debug.WriteLine("GetAllSkyJSONData enter");
            if (IsBusy)
            {
                Debug.WriteLine("GetAllSkyJSONData: GetJSON already busy, return");
                return;
            }
            IsBusy = true;
           
            // Check network status  
            if (NetworkServices.IsConnectedToInternet())
            {
                AllSkyDataClass allSkyDownloadedDataDef = new AllSkyDataClass();
                DownloadResult allSkyDownloadResultDef = DownloadResult.Undefined;
                Tuple<AllSkyDataClass, DownloadResult> allskyret = new Tuple<AllSkyDataClass, DownloadResult>(allSkyDownloadedDataDef, allSkyDownloadResultDef);
                try
                {
                    //string stout1 = JsonConvert.SerializeObject(allskyret);
                    //Debug.Write("Dump allskyret bedore: ");
                    //Debug.WriteLine(stout1);

                    allskyret = await Task.Run(() => NetworkServices.GetJSON<AllSkyDataClass>(Settings.AllskyStatusURL, allSkyDownloadedDataDef));
                    ////Debug
                    //string stout = JsonConvert.SerializeObject(allskyret);
                    //Debug.Write("Dump allskyret: ");
                    //Debug.WriteLine(stout);

                    //4. Data setting
                    DateTime ASDT = DateTimeUtils.UnixTimeStampToDateTime(allskyret.Item1.timestamp);
                    AllSkyDate = DateTimeUtils.ConvertToLocal(ASDT).ToString("HH:mm:ss");

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception in GetAllSkyJSONData");
                    Debug.WriteLine("Ex: " + ex);
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

            AstroTwilightBegTimeSt = AstroUtilsProp.AstronTwilightRiseDateTime(1).ToString("HH:mm");
            NavTwilightBegTimeSt = AstroUtilsProp.NautTwilightRiseDateTime(1).ToString("HH:mm");
            CivTwilightBegTimeSt = AstroUtilsProp.CivilTwilightRiseDateTime(1).ToString("HH:mm");
            SunriseTimeSt = AstroUtilsProp.SunRiseDateTime(1).ToString("HH:mm");

            DateTime MoonRise, MoonSet, MoonEvent = DateTime.MinValue;
            int SessionDayShift = 0;


            int DebugDayShift = 0;

            if (DateTime.Now.AddDays(DebugDayShift).Hour >= 0 && DateTime.Now.AddDays(DebugDayShift).Hour < 12) SessionDayShift-=1;

            SessionDayShift += DebugDayShift; //add debug dayshift


            DateTime GivenDate = DateTime.Now.AddDays(DebugDayShift);

            AstroUtilsProp.getMoonTimesForSession(GivenDate, AstroUtilsProp.Latitude, AstroUtilsProp.Longitude, 3, out MoonRise, out MoonSet);

            DateTime SunSet = AstroUtilsProp.SunSetDateTime(SessionDayShift);
            DateTime SunRise = AstroUtilsProp.SunRiseDateTime(SessionDayShift+1);
            if (MoonRise > SunSet && MoonRise < SunRise)
            {
                //Rise is here
                MoonRightTimeSt = MoonRise.ToString("HH:mm");
                MoonLeftTimeSt = "";
                Moon_Right_Color = COLOR_MoonAbove;
                Moon_Left_Color = COLOR_MoonBelow;
                MoonEvent = MoonRise;
            }
            else if (MoonSet > SunSet && MoonSet < SunRise)
            {
                //Set is here
                MoonLeftTimeSt = MoonSet.ToString("HH:mm");
                MoonRightTimeSt = "";
                Moon_Left_Color = COLOR_MoonAbove;
                Moon_Right_Color = COLOR_MoonBelow;
                MoonEvent = MoonSet;
            }
            else if (MoonRise < SunSet && MoonSet > SunRise)
            {
                //always above
                MoonLeftTimeSt = MoonRise.ToString("HH:mm");
                MoonRightTimeSt = MoonSet.ToString("HH:mm");
                Moon_Left_Color = COLOR_MoonAbove;
                Moon_Right_Color = COLOR_MoonAbove;
                MoonEvent = DateTime.MaxValue;
            }
            else 
            {
                //always below
                MoonLeftTimeSt = MoonSet.ToString("HH:mm");
                MoonRightTimeSt = MoonRise.ToString("HH:mm");
                Moon_Left_Color = COLOR_MoonBelow;
                Moon_Right_Color = COLOR_MoonBelow;
                MoonEvent = DateTime.MaxValue;
            }

            DateTime CivilSet = AstroUtilsProp.CivilTwilightSetDateTime(SessionDayShift);
            DateTime NauSet = AstroUtilsProp.NautTwilightSetDateTime(SessionDayShift);
            DateTime AstrSet = AstroUtilsProp.AstronTwilightSetDateTime(SessionDayShift);

            DateTime AstrRise = AstroUtilsProp.AstronTwilightRiseDateTime(SessionDayShift+1);
            DateTime NauRise = AstroUtilsProp.NautTwilightRiseDateTime(SessionDayShift + 1);
            DateTime CivilRise = AstroUtilsProp.CivilTwilightRiseDateTime(SessionDayShift + 1);

            if (MoonEvent < CivilSet)
            {
                Moon_Left_ColSpan = 1;
            }
            else if (MoonEvent < NauSet)
            {
                Moon_Left_ColSpan = 2;
            }
            else if (MoonEvent < AstrSet)
            {
                Moon_Left_ColSpan = 3;
            }
            else if (MoonEvent < AstrRise)
            {
                Moon_Left_ColSpan = 4;
            }
            else if (MoonEvent < NauRise)
            {
                Moon_Left_ColSpan = 5;
            }
            else if (MoonEvent < CivilRise)
            {
                Moon_Left_ColSpan = 6;
            }
            else if (MoonEvent < SunRise)
            {
                Moon_Left_ColSpan = 7;
            }
            else
            {
                Moon_Left_ColSpan = 4;
            }


        }

        public ICommand RefreshAllSkyCommand { get; }
        public ICommand AllSkyTapCommand { get; }

        public ICommand OpenSiteCommand { get; }
        
    }
}