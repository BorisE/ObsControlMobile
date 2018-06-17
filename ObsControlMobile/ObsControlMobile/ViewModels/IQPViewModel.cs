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
    public class IQPViewModel : BaseViewModel
    {
        public ObservableCollection<IQPItem> IQPItems { get; set; }
        public Command LoadIQPItemsCommand { get; set; }

        Page ParentPage;


        #region Binding properties
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

        #endregion Binding properties

        public IQPViewModel(Page ExtPP)
        {
            ParentPage = ExtPP;

            Title = "IQP";
            IQPItems = new ObservableCollection<IQPItem>();
            LoadIQPItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, IQPItem>(this, "AddItem", async (obj, item) =>
            {
                var _item = item as IQPItem;
                IQPItems.Add(_item);
                await DataStore.AddItemAsync(_item);
            });
        }

        
        async Task ExecuteLoadItemsCommand()
        {
            Debug.WriteLine("ExecuteLoadItemsCommand enter");

            if (IsBusy)
            {
                Debug.WriteLine("ExecuteLoadItemsCommand already busy, return");
                return;
            }
            IsBusy = true;
            //this.IsDownloading = true;

            try
            {
                // Clear data
                IQPItems.Clear();

                // Load data
                var items = await DataStore.GetItemsAsync(true);

                // Check for errors
                if (((IQPDataStore)DataStore).GetDataResult == DownloadResult.NoNetwork)
                {
                    await ParentPage.DisplayAlert("Get IQP Data", "No network is available.", "Ok");
                }
                else if (((IQPDataStore)DataStore).GetDataResult == DownloadResult.DownloadError)
                {
                    await ParentPage.DisplayAlert("Get IQP Data", "Download error", "Ok");
                }
                else if (((IQPDataStore)DataStore).GetDataResult == DownloadResult.ParseError)
                {
                    await ParentPage.DisplayAlert("Get IQP Data", "JSON Parse error", "Ok");
                }
                else
                {
                    // Add loaded data into binded list
                    // Also loop all data to determine last file data
                    DateTime curSess = DateTime.MinValue;
                    foreach (var item in items)
                    {
                        IQPItems.Add(item);
                        curSess = (item.DateObsUTC > curSess ? item.DateObsUTC : curSess);
                    }

                    //update session name
                    DateTime.SpecifyKind(curSess, DateTimeKind.Utc);
                    LastSessionDate = AsrtoUtils.ServiceClass.ConvertToLocal(curSess);
                }
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

    }
}