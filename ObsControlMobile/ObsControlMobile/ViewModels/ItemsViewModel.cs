using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using ObsControlMobile.Models;
using ObsControlMobile.Views;
using ObsControlMobile.Services;
using System.Net.Http;
using System.Collections.Generic;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Globalization;

namespace ObsControlMobile.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<IQPItem> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        Page ParentPage;

        public ItemsViewModel(Page ExtPP)
        {
            ParentPage = ExtPP;

            Title = "IQP";
            Items = new ObservableCollection<IQPItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, IQPItem>(this, "AddItem", async (obj, item) =>
            {
                var _item = item as IQPItem;
                Items.Add(_item);
                await DataStore.AddItemAsync(_item);
            });
        }


        
        bool isdownloading = false;
        public bool IsDownloading
        {
            get { return isdownloading; }
            set { SetProperty(ref isdownloading, value); }
        }


        DateTime lastsessiondate;
        public DateTime LastSessionDate
        {
            get { return lastsessiondate; }
            set { SetProperty(ref lastsessiondate, value); }
        }


        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }


                //Try to download
                //I don't know where to place it
                GetJSON();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void GetJSON()
        {
            //Check network status  
            if (NetworkCheck.IsConnectedToInternet())
            {
                this.IsDownloading = true;

                //Download
                Uri geturi = new Uri("http://astromania.info/iqp_data/getcurrentsession.php"); //replace your xml url  
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(geturi);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseSt = await response.Content.ReadAsStringAsync();

                    //await ParentPage.DisplayAlert("Get IQP Data", responseSt, "Ok");

                    JsonSerializerSettings JSONSettings = new JsonSerializerSettings();
                    JSONSettings.Culture = new CultureInfo("ru-RU");
                    JSONSettings.Culture.NumberFormat.NumberDecimalSeparator = ".";
                    JSONSettings.NullValueHandling = NullValueHandling.Ignore;

                    List<IQPItem> list = JsonConvert.DeserializeObject<List<IQPItem>>(await response.Content.ReadAsStringAsync(), JSONSettings);

                    DateTime curSess = DateTime.MinValue;
                    foreach (IQPItem item in list)
                    {
                        Items.Add(item);
                        curSess = (item.DateObsUTC > curSess ? item.DateObsUTC : curSess);
                    }
                    //update session name
                    LastSessionDate = curSess.AddHours(AsrtoUtils.AstroUtilsProp.SiteTimeZone);
                }

                this.IsDownloading = false;
            }
            else
            {
                await ParentPage.DisplayAlert("Get IQP Data", "No network is available.", "Ok");
            }
            //Hide loader after server response  
            //ProgressLoader.IsRunning = false;
        }


    }
}