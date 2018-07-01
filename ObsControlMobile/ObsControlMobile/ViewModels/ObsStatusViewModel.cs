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
        //IR
        public string Obs1_IR
        {
            get => ObsStatus1.ir.value.ToString("0.0");
        }

        public string Obs1_IR_Updated
        {
            get => ObsStatus1.ir.date.ToString("HH:mm:ss dd-MM-yyyy");
        }
        public string Obs1_IR_Updated_Color
        {
            get => RecentColor(ObsStatus1.ir.ValueIsRecent);
        }

        //Humdiity
        public string Obs1_Humidity
        {
            get => ObsStatus1.humidity.value.ToString("0.0");
        }

        public string Obs1_Humidity_Updated
        {
            get => ObsStatus1.humidity.date.ToString("HH:mm:ss dd-MM-yyyy");
        }
        public string Obs1_Humidity_Updated_Color
        {
            get => RecentColor(ObsStatus1.humidity.ValueIsRecent);
        }

        //OBS2 Inside temp
        public string Obs1_Inside
        {
            get => ObsStatus1.inside.value.ToString("0.0");
        }

        public string Obs1_Inside_Updated
        {
            get => ObsStatus1.inside.date.ToString("HH:mm:ss dd-MM-yyyy");
        }
        public string Obs1_Inside_Updated_Color
        {
            get => RecentColor(ObsStatus1.inside.ValueIsRecent);
        }
        //OBS1 AKB
        public string Obs1_Akb
        {
            get => ObsStatus1.akb.value.ToString("0.0");
        }

        public string Obs1_Akb_Updated
        {
            get => ObsStatus1.akb.date.ToString("HH:mm:ss dd-MM-yyyy");
        }
        public string Obs1_Akb_Updated_Color
        {
            get => RecentColor(ObsStatus1.akb.ValueIsRecent);
        }
        #endregion OBSERVATORY 1 STATUS

        #region OBSERVATORY 2 STATUS
        public string Obs2_Roof
        {
            get
            {
                if (ObsStatus2.roof.value == 0) return "Closed";
                else if (ObsStatus2.roof.value == 1) return "Opened";
                else return "Unknown";
            }
        }
        public string Obs2_Roof_Color
        {
            get=>(ObsStatus2.roof.value == 1 ? "lime" : (ObsStatus2.roof.value == 0 ? "silver" : "crimson"));
        }
        public string Obs2_Roof_Updated
        {
            get=>ObsStatus2.roof.date.ToString("HH:mm:ss dd-MM-yyyy");
        }
        public string Obs2_Roof_Updated_Color
        {
            get=>RecentColor(ObsStatus2.roof.ValueIsRecent);
        }


        //IR
        public string Obs2_IR
        {
            get => ObsStatus2.ir.value.ToString("0.0");
        }

        public string Obs2_IR_Updated
        {
            get => ObsStatus2.ir.date.ToString("HH:mm:ss dd-MM-yyyy");
        }
        public string Obs2_IR_Updated_Color
        {
            get=>RecentColor(ObsStatus2.ir.ValueIsRecent);
        }

        //Humdiity
        public string Obs2_Humidity
        {
            get => ObsStatus2.humidity.value.ToString("0.0");
        }

        public string Obs2_Humidity_Updated
        {
            get => ObsStatus2.humidity.date.ToString("HH:mm:ss dd-MM-yyyy");
        }
        public string Obs2_Humidity_Updated_Color
        {
            get => RecentColor(ObsStatus2.humidity.ValueIsRecent);
        }

        //OBS2 Inside temp
        public string Obs2_Inside
        {
            get => ObsStatus2.inside.value.ToString("0.0");
        }

        public string Obs2_Inside_Updated
        {
            get => ObsStatus2.inside.date.ToString("HH:mm:ss dd-MM-yyyy");
        }
        public string Obs2_Inside_Updated_Color
        {
            get => RecentColor(ObsStatus2.inside.ValueIsRecent);
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


        public void RefreshBindingFields()
        {
            OnPropertyChanged("Obs1_Akb");
            OnPropertyChanged("Obs1_Akb_Updated");
            OnPropertyChanged("Obs1_Akb_Updated_Color");

            OnPropertyChanged("Obs1_IR");
            OnPropertyChanged("Obs1_IR_Updated");
            OnPropertyChanged("Obs1_IR_Updated_Color");

            OnPropertyChanged("Obs1_Humidity");
            OnPropertyChanged("Obs1_Humidity_Updated");
            OnPropertyChanged("Obs1_Humidity_Updated_Color");

            OnPropertyChanged("Obs1_Inside");
            OnPropertyChanged("Obs1_Inside_Updated");
            OnPropertyChanged("Obs1_Inside_Updated_Color");


            OnPropertyChanged("Obs2_Roof");
            OnPropertyChanged("Obs2_Roof_Color");
            OnPropertyChanged("Obs2_Roof_Updated");
            OnPropertyChanged("Obs2_Roof_Updated_Color");

            OnPropertyChanged("Obs2_IR");
            OnPropertyChanged("Obs2_IR_Updated");
            OnPropertyChanged("Obs2_IR_Updated_Color");

            OnPropertyChanged("Obs2_Humidity");
            OnPropertyChanged("Obs2_Humidity_Updated");
            OnPropertyChanged("Obs2_Humidity_Updated_Color");

            OnPropertyChanged("Obs2_Inside");
            OnPropertyChanged("Obs2_Inside_Updated");
            OnPropertyChanged("Obs2_Inside_Updated_Color");
        }

        public string RecentColor(bool IsRecent)
        {
            return (IsRecent ? "azure" : "crimson");
        }

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
                    RefreshBindingFields();
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