using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ObsControlMobile.Views;
using Xamarin.Forms;

namespace ObsControlMobile.ViewModels
{
    public class PowerViewModel : BaseViewModel
    {
        Page ParentPage;


        public DownloadResult GetDataResult = DownloadResult.Undefined;

        //{
        //   "boximer_pc": 0,
        //   "boximer_scope": 0,
        //   "boris_pc": 0,
        //   "boris_scope": 0,
        //   "roman_pc": 0,
        //   "roman_scope": 0,
        //   "spartak_pc": 0,
        //   "spartak_scope": 0,
        //   "boris2_pc": 0,
        //   "boris2_scope": 0,
        //   "boris2_ccd": 0
        //}

        public Dictionary<string, int> PowerStatusList;

        public Command LoadPowerStatusCommand { get; set; }

        public Command SendPowerStatusCommand { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ExtPP"></param>
        public PowerViewModel(Page ExtPP)
        {
            ParentPage = ExtPP;

            Title = "Status";

            PowerStatusList = new Dictionary<string, int>(); 
            LoadPowerStatusCommand = new Command(async () => await GetPowerStatus_emulate());
        }


        #region Misc fileds
        string _currentdate;
        public string CurrentDate
        {
            get { return _currentdate; }
            set { SetProperty(ref _currentdate, value); }
        }
        #endregion Misc fileds


        async Task GetPowerStatus_emulate()
        {
            Debug.WriteLine("GetPowerStatus_emulate enter");

            if (IsBusy)
            {
                Debug.WriteLine("GetPowerStatus_emulate already busy, return");
                return;
            }
            IsBusy = true;


            //Set CurrentDate propertie
            CurrentDate = DateTime.Now.ToString("HH:mm:ss");


            try
            {
                //Download status data
                PowerStatusList = new Dictionary<string, int>
                {
                    ["spartak_pc"] = 0,
                    ["spartak_scope"] = 0,
                    ["boris2_pc"] = 0,
                    ["boris2_scope"] = 0,
                    ["boris2_ccd"] = 0,
                };

                SwitchCell PowerCell1 = new SwitchCell
                {
                    Text = "Boris2 pc"
                };

                 ((PowerPage)ParentPage).PowerSwitchSection.Children.Add(PowerCell1);


            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetPowerStatus_emulate Exception");
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
                //this.IsDownloading = false;
            }

        }


        public async Task RefreshObsStatus()
        {
            //Download status data
            Debug.WriteLine("RefreshObsStatus enter");

            //Set CurrentDate propertie
            CurrentDate = DateTime.Now.ToString("HH:mm:ss");

            if (IsBusy)
            {
                Debug.WriteLine("RefreshObsStatus already busy, return");
                return;
            }
            IsBusy = true;

            // Check network status  
            if (NetworkServices.IsConnectedToInternet())
            {
                try
                {
                    //Download data
                    Tuple<Dictionary<string, ObsStatus_JSON_ByObservatoryClass>, DownloadResult> obsstatret;
                    obsstatret = await NetworkServices.GetJSON<Dictionary<string, ObsStatus_JSON_ByObservatoryClass>>(Settings.ObsStatusURL);
                            //Debug
                            string stout=JsonConvert.SerializeObject(obsstatret);
                            Debug.Write("Dump obsstatret: ");
                            Debug.WriteLine(stout);

                    //Debug.Write("Dump: ");
                    //Debug.WriteLine(stout);

                    //REFRESH PROTPERTIES
                    //RefreshBindingFields();
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
    }
}