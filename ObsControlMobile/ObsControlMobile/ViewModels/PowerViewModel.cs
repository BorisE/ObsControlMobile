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

        private JSONPowerStatusListClass PowerStatus_Downloaded;

        private PowerServiceClass PowerService;

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

            PowerService = new PowerServiceClass();

            PowerStatusItems = new ObservableCollection<PowerStatusItem>();

            LoadPowerStatusCommand = new Command(async () => await GetPowerStatus());
            SendPowerStatusCommand = new Command(async () => await SendPowerStatus());
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
                JSONPowerStatusListClass PowerStatusList = new JSONPowerStatusListClass
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
                // Clear data
                PowerStatusItems.Clear();

                //Download data
                Tuple<JSONPowerStatusListClass, DownloadResult> PowerStatusRet = await PowerService.GetStatusAsync();

                //Save it for future use
                PowerStatus_Downloaded = PowerStatusRet.Item1;

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

                    foreach (KeyValuePair<string, int> entry in PowerStatus_Downloaded)
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
            }
        }

        public async Task SendPowerStatus()
        {
            Debug.WriteLine("SendPowerStatus enter");

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
                ObservableCollection<PowerStatusItem> PowerStatusList = new ObservableCollection<PowerStatusItem>
                {
                    new PowerStatusItem{ Title="boris2_pc", StatusNumeric = 1 },
                    new PowerStatusItem{ Title="boris2_scope", StatusNumeric = 1 },
                };


                //PowerStatusItem PowerStatusEl = new PowerStatusItem
                //{
                //    Title = "boris2_scope",
                //    StatusNumeric = 1
                //};
                
                ////Download data
                //DownloadResult PowerStatusRet = await PowerService.SetItemStatusAsync(PowerStatusEl);

                //Download data
                DownloadResult PowerStatusRet = await PowerService.SetStatusAsync(PowerStatusItems);


                //Reread data
                await GetPowerStatus();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("SendPowerStatus Exception");
                Debug.WriteLine(ex);
            }
            finally
            {
            }

        }
    }
}