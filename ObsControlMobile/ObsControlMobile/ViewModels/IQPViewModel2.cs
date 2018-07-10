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
    public class IQPViewModel2 : BaseViewModel
    {
        public ObservableCollection<ObsStatus_LV_Element_Class> IQPItems { get; set; }
        public Command LoadIQPItemsCommand2 { get; set; }

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

        public IQPViewModel2(Page ExtPP)
        {
            ParentPage = ExtPP;

            Title = "IQP";

            IQPItems = new ObservableCollection<ObsStatus_LV_Element_Class>();
            LoadIQPItemsCommand2 = new Command(async () => await ExecuteLoadItemsCommand());

            //MessagingCenter.Subscribe<NewItemPage, IQPItem>(this, "AddItem", async (obj, item) =>
            //{
            //    var _item = item as IQPItem;
            //    IQPItems.Add(_item);
            //    await DataStore.AddItemAsync(_item);
            //});
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
   


                // Add loaded data into binded list
                // Also loop all data to determine last file data
                IQPItems.Add(
                    new ObsStatus_LV_Element_Class
                    {
                        Id = Guid.NewGuid().ToString(),
                        NameEl ="roof",
                        valueEl = 1,
                        dateEl = DateTime.Now
                    }
                );


                DateTime curSess = DateTime.Now;
                //update session name
                DateTime.SpecifyKind(curSess, DateTimeKind.Utc);
                LastSessionDate = AsrtoUtils.Conversion.DateTimeUtils.ConvertToLocal(curSess);

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