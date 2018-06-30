using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

using AsrtoUtils;
using AsrtoUtils.Conversion;
using Newtonsoft.Json;
using ObsControlMobile.Models;
using ObsControlMobile.Services;
using Xamarin.Forms;

namespace ObsControlMobile.ViewModels
{
        public class ObsStatusViewModel : BaseViewModel
    {
        Page ParentPage;

        public DownloadResult GetDataResult = DownloadResult.Undefined;

        public ObsStatusElement_Class ObsStatus1 = new ObsStatusElement_Class();
        public ObsStatusElement_Class ObsStatus2 = new ObsStatusElement_Class();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ExtPP"></param>
        public ObsStatusViewModel(Page ExtPP)
        {
            ParentPage = ExtPP;

            Title = "Status";

            AllSkyTapCommand = new Command(() => RefreshObsStatus());
            RefreshAllSkyCommand = new Command(() => RefreshObsStatus());

            //ObsStatus Data
            RefreshObsStatus();
        }


        #region OBSERVATORY 1 STATUS
        double _ir1;
        public string Obs1_IR
        {
            get {
                return _ir1.ToString("0.0");
            }
            set {
                 if (DoubleConversionUtils.TryParseToDouble(value, out _ir1)) 
                        OnPropertyChanged();
            }
        }
        DateTime _ir1_updated;
        public string Obs1_IR_Updated
        {
            get
            {
                return _ir1_updated.ToString("HH:mm:ss MM-dd-YYYY");
            }
            set
            {
                _ir1_updated=DateTime.Parse(value);
                OnPropertyChanged();
            }
        }
        double _humidity1;
        public string Obs1_Humidity
        {
            get
            {
                return _humidity1.ToString("0");
            }
            set
            {
                if (DoubleConversionUtils.TryParseToDouble(value, out _humidity1))
                    OnPropertyChanged();
            }
        }
        DateTime _humidity1_updated;
        public string Humidity_Updated
        {
            get
            {
                return _humidity1_updated.ToString("HH:mm:ss MM-dd-YYYY");
            }
            set
            {
                _humidity1_updated = DateTime.Parse(value);
                OnPropertyChanged();
            }
        }
        double _inside1;
        public string Obs1_Inside
        {
            get
            {
                return _inside1.ToString("0.0");
            }
            set
            {
                if (DoubleConversionUtils.TryParseToDouble(value, out _inside1))
                    OnPropertyChanged();
            }
        }
        DateTime _inside1_updated;
        public string Obs1_Inside_Updated
        {
            get
            {
                return _inside1_updated.ToString("HH:mm:ss MM-dd-YYYY");
            }
            set
            {
                _inside1_updated = DateTime.Parse(value);
                OnPropertyChanged();
            }
        }
        double _akb1;
        public string Obs1_Akb
        {
            get
            {
                return _akb1.ToString("0.00");
            }
            set
            {
                if (DoubleConversionUtils.TryParseToDouble(value, out _akb1))
                    OnPropertyChanged();
            }
        }
        DateTime _akb1_updated;
        public string Obs1_Akb_Updated
        {
            get
            {
                return _akb1_updated.ToString("HH:mm:ss MM-dd-YYYY");
            }
            set
            {
                _akb1_updated = DateTime.Parse(value);
                OnPropertyChanged();
            }
        }
        #endregion OBSERVATORY 1 STATUS

        #region OBSERVATORY 2 STATUS
        string _roofopen2;
        public string Obs2_Roof
        {
            get
            {
                //if (_roofopen2 == "0") return "Closed";
                //else if (_roofopen2 == "1") return "Opened";
                //else return "Unknown";
                if (ObsStatus2.roof.value == 0) return "Closed";
                else if (ObsStatus2.roof.value == 1) return "Opened";
                else return "Unknown";
            }
            //set
            //{
            //    //SetProperty(ref _roofopen2, value);
            //    _roofopen2 = value;
            //    OnPropertyChanged();
            //    OnPropertyChanged("Obs2_Roof_Color");
            //}
        }
        public string Obs2_Roof_Color
        {
            get
            {
                return (ObsStatus2.roof.value == 1 ? "lime" : (ObsStatus2.roof.value == 0 ? "silver" : "crimson"));
            }
        }
        DateTime _roof2_updated;
        public string Obs2_Roof_Updated
        {
            get
            {
                //return _roof2_updated.ToString("HH:mm:ss dd-MM-yyyy");
                return ObsStatus2.roof.date.ToString("HH:mm:ss dd-MM-yyyy");
            }
            //set
            //{
            //    _roof2_updated = DateTime.Parse(value);
            //    OnPropertyChanged();
            //    OnPropertyChanged("Obs2_Roof_Updated_Color");
            //}
        }
        public string Obs2_Roof_Updated_Color
        {
            get
            {
                //Debug.WriteLine("Here");
                //string stout = JsonConvert.SerializeObject(ObsStatus2);
                //Debug.WriteLine("Obj: " + stout);
                //bool flag = ObsStatus2.roof.ValueIsRecent;

                //double passed = (DateTime.Now - ObsStatus2.roof.date).TotalSeconds;
                //bool flag2 = (passed < DateValuePair.VALID_DATETIME_MAX_SECONDS_SINCE_NOW);

                //Debug.WriteLine("Date: "+ ObsStatus2.roof.date + ", _roof2_updated: "+ _roof2_updated+", Passed: " + passed + ", VALID_DATETIME_MAX_SECONDS_SINCE_NOW=" + DateValuePair.VALID_DATETIME_MAX_SECONDS_SINCE_NOW + ", flag:" + flag2 + ",orig: " + flag);

                return (ObsStatus2.roof.ValueIsRecent ? "blue" : "crimson");
            }
        }

        ObsStatusElement_Class ObsStatus2t = new ObsStatusElement_Class();
        public DateValuePair Roof
        {
            get
            {
                return new DateValuePair() { value = 1.0, date = new DateTime(2010, 8, 18, 10, 10, 10)};
            
                //return ObsStatus2t.roof;
            }
            set
            {
                ObsStatus2t.roof.value = value.value;
                ObsStatus2t.roof.date = value.date;


                Debug.WriteLine("");
                OnPropertyChanged();
            }
        }


        double _ir2;
        public string Obs2_IR
        {
            get
            {
                return _ir2.ToString("0.0");
            }
            set
            {
                if (DoubleConversionUtils.TryParseToDouble(value, out _ir2))
                    OnPropertyChanged();
            }
        }
        DateTime _ir2_updated;
        public string Obs2_IR_Updated
        {
            get
            {
                return _ir2_updated.ToString("HH:mm:ss MM-dd-YYYY");
            }
            set
            {
                _ir2_updated = DateTime.Parse(value);
                OnPropertyChanged();
            }
        }
        double _humidity2;
        public string Obs2_Humidity
        {
            get
            {
                return _humidity2.ToString("0");
            }
            set
            {
                if (DoubleConversionUtils.TryParseToDouble(value, out _humidity2))
                    OnPropertyChanged();
            }
        }
        DateTime _humidity2_updated;
        public string Humidity2_Updated
        {
            get
            {
                return _humidity2_updated.ToString("HH:mm:ss MM-dd-YYYY");
            }
            set
            {
                _humidity2_updated = DateTime.Parse(value);
                OnPropertyChanged();
            }
        }
        double _inside2;
        public string Obs2_Inside
        {
            get
            {
                return _inside2.ToString("0.0");
            }
            set
            {
                if (DoubleConversionUtils.TryParseToDouble(value, out _inside2))
                    OnPropertyChanged();
            }
        }
        DateTime _inside2_updated;
        public string Obs2_Inside_Updated
        {
            get
            {
                return _inside2_updated.ToString("HH:mm:ss MM-dd-YYYY");
            }
            set
            {
                _inside2_updated = DateTime.Parse(value);
                OnPropertyChanged();
            }
        }
        #endregion OBSERVATORY 2 STATUS


        #region Misc fileds
        string _currentdate;
        public string CurrentDate
        {
            get { return _currentdate; }
            set { SetProperty(ref _currentdate, value); }
        }
        #endregion Misc fileds


        public void RefreshObsStatus()
        {
            //Set CurrentDate propertie
            CurrentDate = DateTime.Now.ToString("HH:mm:ss");
            
            //Download status data
            GetObsStatusJSONData();
        }

        private async void GetObsStatusJSONData()
        {
            Debug.WriteLine("GetObsStatusJSONData enter");
            if (IsBusy)
            {
                Debug.WriteLine("GetObsStatusJSONData already busy, return");
                return;
            }
            IsBusy = true;

            // Check network status  
            if (NetworkServices.IsConnectedToInternet())
            {
                try
                {
                    Tuple<Dictionary<string, ObsStatusElement_Class>, DownloadResult> obsstatret;
                    obsstatret = await  NetworkServices.GetJSON<Dictionary<string, ObsStatusElement_Class>>(Settings.ObsStatusURL);
                    //string stout=JsonConvert.SerializeObject(obsstatret);
                    //Debug.Write("Dump: ");
                    //Debug.WriteLine(stout);

                    ObsStatus1 = obsstatret.Item1["1"];
                    ObsStatus2 = obsstatret.Item1["2"];

                    //REFRESH PROTPERTIES
                    OnPropertyChanged("Obs2_Roof");
                    OnPropertyChanged("Obs2_Roof_Color");
                    OnPropertyChanged("Obs2_Roof_Updated");
                    OnPropertyChanged("Obs2_Roof_Updated_Color");





                    //Obs2_Roof = ObsStatus2.roof.value.ToString();
                    //Debug.WriteLine("RoofValue is recent: " + ObsStatus2.roof.ValueIsRecent.ToString());

                    //Obs2_Roof_Updated = ObsStatus2.roof.date.ToString();



                    //Roof = ObsStatus2.roof;

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception in GetObsStatusJSONData");
                    Debug.WriteLine(ex);
                }
            }
            else
            {
                await ParentPage.DisplayAlert("Get ObsStatus Data", "No network is available.", "Ok");
            }
            IsBusy = false;
        }


        public ICommand RefreshAllSkyCommand { get; }
        public ICommand AllSkyTapCommand { get; }
    }
}