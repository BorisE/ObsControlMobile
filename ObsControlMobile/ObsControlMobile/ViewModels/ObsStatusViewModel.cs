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
using Xamarin.Forms;

namespace ObsControlMobile.ViewModels
{
    public class ObsStatusViewModel : BaseViewModel
    {
        Page ParentPage;

        public DownloadResult GetDataResult = DownloadResult.Undefined;

        public ObsStatus_JSON_ByObservatoryClass ObsStatus1 = new ObsStatus_JSON_ByObservatoryClass();
        public ObsStatus_JSON_ByObservatoryClass ObsStatus2 = new ObsStatus_JSON_ByObservatoryClass();

        //public ObservableCollection<ObsStatus_LV_Element_Class> ObsStatus_LVsource { get; set; }

        public ObservableCollection<ObsStatus_LV_Group> ObsStatus_LV_Grouped_source { get; set; }


        public Command LoadObsStatusCommand { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ExtPP"></param>
        public ObsStatusViewModel(Page ExtPP)
        {
            ParentPage = ExtPP;

            Title = "Status";

            //AllSkyTapCommand = new Command(() => RefreshObsStatus());
            //RefreshAllSkyCommand = new Command(() => RefreshObsStatus());

            //ObsStatus_LVsource = new ObservableCollection<ObsStatus_LV_Element_Class>();

            ObsStatus_LV_Grouped_source = new ObservableCollection<ObsStatus_LV_Group>();

            //ObsStatus_LVsource = new ObservableCollection<ObsStatus_Element_Class>
            //{
            //    new ObsStatus_Element_Class {
            //        Name = "Roof",
            //        Value = 1,
            //        Date = new DateTime(2018, 07, 01, 03, 01, 01)
            //    },
            //    new ObsStatus_Element_Class {
            //        Name = "Inside1",
            //        Value = 30.0,
            //        Date = new DateTime(2018, 07, 01, 03, 01, 11)
            //    },
            //};

            LoadObsStatusCommand = new Command(async () => await RefreshObsStatus());

            //MessagingCenter.Subscribe<Page, ObsStatus_Element_Class>(this, "AddItem", async (obj, item) =>
            //{
            //    var _item = item as ObsStatus_Element_Class;
            //    ObsStatus_LVsource.Add(_item);
            //});

            //ObsStatus Data
            //RefreshObsStatus();
        }


        #region OBSERVATORY 1 STATUS
        //IR
        public string Obs1_IR
        {
            get => ObsStatus1.ir.Value.ToString("0.0");
        }

        public string Obs1_IR_Updated
        {
            get => ObsStatus1.ir.Date.ToString("HH:mm:ss dd-MM-yyyy");
        }
        public string Obs1_IR_Updated_Color
        {
            get => RecentColor(ObsStatus1.ir.ValueIsRecent);
        }

        //Humdiity
        public string Obs1_Humidity
        {
            get => ObsStatus1.humidity.Value.ToString("0.0");
        }

        public string Obs1_Humidity_Updated
        {
            get => ObsStatus1.humidity.Date.ToString("HH:mm:ss dd-MM-yyyy");
        }
        public string Obs1_Humidity_Updated_Color
        {
            get => RecentColor(ObsStatus1.humidity.ValueIsRecent);
        }

        //OBS2 Inside temp
        public string Obs1_Inside
        {
            get => ObsStatus1.inside.Value.ToString("0.0");
        }

        public string Obs1_Inside_Updated
        {
            get => ObsStatus1.inside.Date.ToString("HH:mm:ss dd-MM-yyyy");
        }
        public string Obs1_Inside_Updated_Color
        {
            get => RecentColor(ObsStatus1.inside.ValueIsRecent);
        }
        //OBS1 AKB
        public string Obs1_Akb
        {
            get => ObsStatus1.akb.Value.ToString("0.0");
        }

        public string Obs1_Akb_Updated
        {
            get => ObsStatus1.akb.Date.ToString("HH:mm:ss dd-MM-yyyy");
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
                if (ObsStatus2.roof.Value == 0) return "Closed";
                else if (ObsStatus2.roof.Value == 1) return "Opened";
                else return "Unknown";
            }
        }
        public string Obs2_Roof_Color
        {
            get=>(ObsStatus2.roof.Value == 1 ? "lime" : (ObsStatus2.roof.Value == 0 ? "silver" : "crimson"));
        }
        public string Obs2_Roof_Updated
        {
            get=>ObsStatus2.roof.Date.ToString("HH:mm:ss dd-MM-yyyy");
        }
        public string Obs2_Roof_Updated_Color
        {
            get=>RecentColor(ObsStatus2.roof.ValueIsRecent);
        }


        //IR
        public string Obs2_IR
        {
            get => ObsStatus2.ir.Value.ToString("0.0");
        }

        public string Obs2_IR_Updated
        {
            get => ObsStatus2.ir.Date.ToString("HH:mm:ss dd-MM-yyyy");
        }
        public string Obs2_IR_Updated_Color
        {
            get=>RecentColor(ObsStatus2.ir.ValueIsRecent);
        }

        //Humdiity
        public string Obs2_Humidity
        {
            get => ObsStatus2.humidity.Value.ToString("0.0");
        }

        public string Obs2_Humidity_Updated
        {
            get => ObsStatus2.humidity.Date.ToString("HH:mm:ss dd-MM-yyyy");
        }
        public string Obs2_Humidity_Updated_Color
        {
            get => RecentColor(ObsStatus2.humidity.ValueIsRecent);
        }

        //OBS2 Inside temp
        public string Obs2_Inside
        {
            get => ObsStatus2.inside.Value.ToString("0.0");
        }

        public string Obs2_Inside_Updated
        {
            get => ObsStatus2.inside.Date.ToString("HH:mm:ss dd-MM-yyyy");
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


        async Task RefreshObsStatus_LV_emulate()
        {
            Debug.WriteLine("RefreshObsStatus_LV enter");

            if (IsBusy)
            {
                Debug.WriteLine("RefreshObsStatus_LV already busy, return");
                return;
            }
            IsBusy = true;


            //Set CurrentDate propertie
            CurrentDate = DateTime.Now.ToString("HH:mm:ss");

            try
            {
                //Download status data

                var Group_Obs1 = new ObsStatus_LV_Group()
                {
                    GroupTitle = "Observatory 1",
                };
                ObsStatus_LV_Element_Class El1 = new ObsStatus_LV_Element_Class
                {
                    Name = "Inside",
                    Value = 30.0,
                    Date = new DateTime(2018, 07, 01, 03, 01, 11)
                };
                Group_Obs1.Add(El1);

                El1 = new ObsStatus_LV_Element_Class
                {
                    Name = "IR",
                    Value = 2.0,
                    Date = new DateTime(2018, 07, 01, 03, 01, 13)
                };
                Group_Obs1.Add(El1);

                El1 = new ObsStatus_LV_Element_Class
                {
                    Name = "Humidity",
                    Value = 30.1,
                    Date = new DateTime(2018, 07, 01, 03, 01, 14)
                };
                Group_Obs1.Add(El1);


                //Group for Observatory 2
                var Group_Obs2 = new ObsStatus_LV_Group()
                {
                    GroupTitle = "Observatory 2"
                };
                El1 = new ObsStatus_LV_Element_Class
                {
                    Name = "Inside",
                    Value = 24.6,
                    Date = new DateTime(2018, 06, 01, 03, 01, 11)
                };
                Group_Obs2.Add(El1);

                El1 = new ObsStatus_LV_Element_Class
                {
                    Name = "Humidity",
                    Value = 22.3,
                    Date = new DateTime(2018, 06, 01, 03, 01, 14)
                };
                Group_Obs2.Add(El1);

                ObsStatus_LV_Element_Class El2 = new ObsStatus_LV_Element_Class
                {
                    Name = "Roof",
                    Value = 1,
                    Date = new DateTime(2018, 07, 01, 03, 01, 01)
                };
                Group_Obs2.Add(El2);

                ObsStatus_LV_Grouped_source.Clear();
                ObsStatus_LV_Grouped_source.Add(Group_Obs1);
                ObsStatus_LV_Grouped_source.Add(Group_Obs2);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("ExecuteLoadItemsCommand Exception");
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

                    //Split for 2 obs
                    ObsStatus1 = obsstatret.Item1["1"];
                    ObsStatus2 = obsstatret.Item1["2"];
                    
                    //Create Group1
                    var Group_Obs1 = new ObsStatus_LV_Group()
                    {
                        GroupTitle = "Observatory 1",
                    };

                    var El1 = new ObsStatus_LV_Element_Class
                    {
                        Name = "IR",
                        Value = ObsStatus1.ir.Value,
                        Date = ObsStatus1.ir.Date
                    };
                    Group_Obs1.Add(El1);
                    El1 = new ObsStatus_LV_Element_Class
                    {
                        Name = "Inside",
                        Value = ObsStatus1.inside.Value,
                        Date = ObsStatus1.inside.Date
                    };
                    Group_Obs1.Add(El1);
                    El1 = new ObsStatus_LV_Element_Class
                    {
                        Name = "Humidity",
                        Value = ObsStatus1.humidity.Value,
                        Date = ObsStatus1.humidity.Date
                    };
                    Group_Obs1.Add(El1);

                            stout = JsonConvert.SerializeObject(Group_Obs1);
                            Debug.Write("Dump Group_Obs1: ");
                            Debug.WriteLine(stout);

                    //Group for Observatory 2
                    var Group_Obs2 = new ObsStatus_LV_Group()
                    {
                        GroupTitle = "Observatory 2"
                    };

                    El1 = new ObsStatus_LV_Element_Class
                    {
                        Name = "Roof",
                        Value = ObsStatus2.roof.Value,
                        Date = ObsStatus2.roof.Date
                    };
                    Group_Obs2.Add(El1);

                    El1 = new ObsStatus_LV_Element_Class
                    {
                        Name = "Inside",
                        Value = ObsStatus2.inside.Value,
                        Date = ObsStatus2.inside.Date
                    };
                    Group_Obs2.Add(El1);
                    
                    El1 = new ObsStatus_LV_Element_Class
                    {
                        Name = "Humidity",
                        Value = ObsStatus2.humidity.Value,
                        Date = ObsStatus2.humidity.Date
                    };
                    Group_Obs2.Add(El1);


                            stout = JsonConvert.SerializeObject(Group_Obs2);
                            Debug.Write("Dump Group_Obs2: ");
                            Debug.WriteLine(stout);

                    ObsStatus_LV_Grouped_source.Clear();
                            stout = JsonConvert.SerializeObject(ObsStatus_LV_Grouped_source);
                            Debug.Write("Dump1 ObsStatus_LV_Grouped_source: ");
                            Debug.WriteLine(stout);

                    ObsStatus_LV_Grouped_source.Add(Group_Obs1);
                            stout = JsonConvert.SerializeObject(ObsStatus_LV_Grouped_source);
                            Debug.Write("Dump2 ObsStatus_LV_Grouped_source: ");
                            Debug.WriteLine(stout);

                    ObsStatus_LV_Grouped_source.Add(Group_Obs2);
                            stout = JsonConvert.SerializeObject(ObsStatus_LV_Grouped_source);
                            Debug.Write("Dump3 ObsStatus_LV_Grouped_source: ");
                            Debug.WriteLine(stout);



                    //string stout = JsonConvert.SerializeObject(ObsStatus_LVsource);
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