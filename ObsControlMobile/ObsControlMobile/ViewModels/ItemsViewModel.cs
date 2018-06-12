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
            if (NetworkCheck.IsConnectedToInternet() && false)
            {
                this.IsDownloading = true;

                //Download
                Uri geturi = new Uri("http://localhost/astropublisher/getcurrentsession.php"); //replace your xml url  
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(geturi);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseSt = await response.Content.ReadAsStringAsync();
                    var list = JsonConvert.DeserializeObject<IEnumerable<IQPItem>>(await response.Content.ReadAsStringAsync());

                    foreach (IQPItem item in list)
                        Items.Add(item);
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