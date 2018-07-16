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
        
        public ObservableCollection<PowerStatusItem> PowerStatusItems { get; set; }

        private JSONPowerStatusListClass PowerStatusList;
        

        public DownloadResult GetDataResult = DownloadResult.Undefined;


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

            PowerStatusItems = new ObservableCollection<PowerStatusItem>();
            PowerStatusList = new JSONPowerStatusListClass();

            LoadPowerStatusCommand = new Command(async () => await GetPowerStatus());
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

                //Download status data
                PowerStatusList = new JSONPowerStatusListClass
                {
                    ["spartak_pc"] = 0,
                    ["spartak_scope"] = 0,
                    ["boris2_pc"] = 1,
                    ["boris2_scope"] = 1,
                    ["boris2_ccd"] = 0,
                };


                // Clear data
                PowerStatusItems.Clear();

                foreach (KeyValuePair<string, int> entry in PowerStatusList)
                {
                    PowerStatusItem El = new PowerStatusItem
                    {
                        Title = entry.Key,
                        Status = (entry.Value == 1)
                    };
                    PowerStatusItems.Add(El);
                }
                

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


        public async Task GetPowerStatus()
        {
            Debug.WriteLine("GetPowerStatus enter");

            if (IsBusy)
            {
                Debug.WriteLine("GetPowerStatus already busy, return");
                return;
            }
            IsBusy = true;


            //Set CurrentDate propertie
            CurrentDate = DateTime.Now.ToString("HH:mm:ss");


            try
            {
                ////Download data
                //Tuple<JSONPowerStatusListClass, DownloadResult> PowerStatRet;
                //PowerStatRet = await NetworkServices.GetJSON<JSONPowerStatusListClass>(Settings.PowerStatusURL);
                ////Debug
                //string stout = JsonConvert.SerializeObject(PowerStatRet);
                //Debug.Write("Dump PowerStatRet: ");
                //Debug.WriteLine(stout);

                //var client = new WebClient { Credentials = new NetworkCredential("borise", "astro11") };
                //string response = await client.DownloadStringTaskAsync("http://astrohostel.ru/rest/power");

                //JSONPowerStatusListClass PowerStatRet = JsonConvert.DeserializeObject<JSONPowerStatusListClass>(response);


                // Clear data
                PowerStatusItems.Clear();

                //Download data
                Tuple<JSONPowerStatusListClass, DownloadResult> PowerStatusRet;
                NetworkCredential givenCredentials = new NetworkCredential(Settings.Login, Settings.Pass);
                PowerStatusRet = await NetworkServices.GetJSONCredential<JSONPowerStatusListClass>(Settings.PowerStatusURL, givenCredentials);


                // Check for errors
                if (PowerStatusRet.Item2 == DownloadResult.NoNetwork)
                {
                    await ParentPage.DisplayAlert("Get Power Status", "No network is available.", "Ok");
                }
                else if (PowerStatusRet.Item2 == DownloadResult.DownloadError)
                {
                    await ParentPage.DisplayAlert("Get Power Status", "Download error", "Ok");
                }
                else if (PowerStatusRet.Item2 == DownloadResult.AuthError)
                {
                    await ParentPage.DisplayAlert("Get Power Status", "Bad username/passwords", "Ok");
                }
                else if (PowerStatusRet.Item2 == DownloadResult.HttpError)
                {
                    await ParentPage.DisplayAlert("Get Power Status", "Web error", "Ok");
                }
                else if (PowerStatusRet.Item2 == DownloadResult.Success)
                {

                    foreach (KeyValuePair<string, int> entry in PowerStatusRet.Item1)
                    {
                        PowerStatusItem El = new PowerStatusItem
                        {
                            Title = entry.Key,
                            Status = (entry.Value == 1)
                        };
                        PowerStatusItems.Add(El);
                    }
                }
                else
                {
                    await ParentPage.DisplayAlert("Get Power Status", "Unknown error", "Ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("GetPowerStatus Exception");
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
                //this.IsDownloading = false;
            }
        }
    }
}