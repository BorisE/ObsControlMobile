using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ObsControlMobile.Models;

[assembly: Xamarin.Forms.Dependency(typeof(ObsControlMobile.Services.IQPDataStore))]
namespace ObsControlMobile.Services
{
    public enum DownloadResult { Success = 1, Undefined = 0, NoNetwork = -1, DownloadError = -2, ParseError = -3 };

    public class IQPDataStore : IDataStore<IQPItem>
    {
        List<IQPItem> dataStoreIQPitems;

        public DownloadResult GetDataResult = DownloadResult.Undefined;

        private bool _UseTestItems = false;

        public IQPDataStore()
        {
            dataStoreIQPitems = new List<IQPItem>();
            if (_UseTestItems)
                AddTestItems();
        }

        /// <summary>
        /// Add example records for testing
        /// </summary>
        private void AddTestItems()
        {
            var testItems = new List<IQPItem>
            {
                new IQPItem { Id = Guid.NewGuid().ToString(), StarsNumber=250, SkyBackground = 0.09, MeanRadius=3.78673095436,AspectRatio=0.931, DateObsUTC=DateTime.Now,
                    ImageExposure =600d, ImageFilter="R", ImageType="Light Frame", ImageBinningX=1, ImageBinningY=1,ImageSetTemp=-20.0, ImageTemp=-20.0299995523, CameraPixelSizeX=5.4, CameraPixelSizeY=5.4,
                    ObjName="M60", ObjRA="12 42 35.0",ObjDec="+11 40 02.0", ObjAlt=48.9, ObjAz=310.0, CameraName="ArtemisHSC", Observer="Boris Emchenko", TelescopeName="SW250", TelescopeFocusLen=1000d, TelescopeDiameter=250d,
                    FITSFileName="M20_20180612_L_600s_1x1_-20degC_0.0degN_000008769.FIT", PixelResolution=1.113831, FWHM=4.21777832562 },

                new IQPItem { Id = Guid.NewGuid().ToString(), StarsNumber=250, SkyBackground = 0.09, MeanRadius=3.78673095436,AspectRatio=0.931, DateObsUTC=DateTime.Now,
                    ImageExposure =600d, ImageFilter="G", ImageType="Light Frame", ImageBinningX=1, ImageBinningY=1,ImageSetTemp=-20.0, ImageTemp=-20.0299995523, CameraPixelSizeX=5.4, CameraPixelSizeY=5.4,
                    ObjName="M60", ObjRA="12 42 35.0",ObjDec="+11 40 02.0", ObjAlt=51.9, ObjAz=310.0, CameraName="ArtemisHSC", Observer="Boris Emchenko", TelescopeName="SW250", TelescopeFocusLen=1000d, TelescopeDiameter=250d,
                    FITSFileName="M20_20180612_L_600s_1x1_-20degC_0.0degN_000008769.FIT", PixelResolution=1.113831, FWHM=4.01777832562 },

                new IQPItem { Id = Guid.NewGuid().ToString(), StarsNumber=250, SkyBackground = 0.09, MeanRadius=3.78673095436,AspectRatio=0.931, DateObsUTC=DateTime.Now,
                    ImageExposure =600d, ImageFilter="B", ImageType="Light Frame", ImageBinningX=1, ImageBinningY=1,ImageSetTemp=-20.0, ImageTemp=-20.0299995523, CameraPixelSizeX=5.4, CameraPixelSizeY=5.4,
                    ObjName="M60", ObjRA="12 42 35.0",ObjDec="+11 40 02.0", ObjAlt=51.9, ObjAz=310.0, CameraName="ArtemisHSC", Observer="Boris Emchenko", TelescopeName="SW250", TelescopeFocusLen=1000d, TelescopeDiameter=250d,
                    FITSFileName="M20_20180612_L_600s_1x1_-20degC_0.0degN_000008769.FIT", PixelResolution=1.113831, FWHM=3.9777832562 },
            };

            foreach (var item in testItems)
            {
                dataStoreIQPitems.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(IQPItem item)
        {
            dataStoreIQPitems.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> ClearItemsAsync()
        {
            dataStoreIQPitems.Clear();

            return await Task.FromResult(true);
        }
        public async Task<bool> UpdateItemAsync(IQPItem item)
        {
            var _item = dataStoreIQPitems.Where((IQPItem arg) => arg.Id == item.Id).FirstOrDefault();
            dataStoreIQPitems.Remove(_item);
            dataStoreIQPitems.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = dataStoreIQPitems.Where((IQPItem arg) => arg.Id == id).FirstOrDefault();
            dataStoreIQPitems.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<IQPItem> GetItemAsync(string id)
        {
            return await Task.FromResult(dataStoreIQPitems.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<IQPItem>> GetItemsAsync(bool forceRefresh = false)
        {
            Debug.WriteLine("GetItemsAsync enter");

            if (forceRefresh)
            {
                GetDataResult = DownloadResult.Undefined;
                Debug.WriteLine("GetItemsAsync started, DownloadResult:"+ GetDataResult);

                //Check network status  
                if (NetworkCheck.IsConnectedToInternet())
                {
                    try
                    {
                        //1. Download data
                        Debug.WriteLine("GetItemsAsync download will start now, DownloadResult:" + GetDataResult);
                        Uri geturi = new Uri(Settings.IQPURL); //replace your xml url  
                        HttpClient client = new HttpClient();
                        HttpResponseMessage response = await client.GetAsync(geturi);

                        Debug.WriteLine("GetItemsAsync download completead, StatusCode:" + response.StatusCode);

                        //2. If data ok
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            //2.2. Read string
                            string responseSt = await response.Content.ReadAsStringAsync();
                            //await ParentPage.DisplayAlert("Get IQP Data", responseSt, "Ok");

                            Debug.WriteLine("GetItemsAsync DOWNLOADDATA:" + responseSt);

                            try
                            {
                                //3.1 Set JSON parse options
                                JsonSerializerSettings JSONSettings = new JsonSerializerSettings();
                                JSONSettings.Culture = new CultureInfo("ru-RU");
                                JSONSettings.Culture.NumberFormat.NumberDecimalSeparator = ".";
                                JSONSettings.NullValueHandling = NullValueHandling.Ignore;

                                //3.2 Convert string into IQP Object 
                                List<IQPItem> IQP_items_list = JsonConvert.DeserializeObject<List<IQPItem>>(responseSt, JSONSettings);
                                GetDataResult = DownloadResult.Success;

                                Debug.WriteLine("GetItemsAsync Converted to JSON, Count:" + IQP_items_list.Count());

                                //Clear current items
                                await ClearItemsAsync();
                                //Add test if needed
                                if (_UseTestItems) AddTestItems();
                                //Add downloaded
                                foreach (var _item in IQP_items_list)
                                {
                                    await AddItemAsync(_item);
                                }

                                Debug.WriteLine("GetItemsAsync Data added into dataStoreIQPitems, Count:" + dataStoreIQPitems.Count());
                                Debug.WriteLine("GetItemsAsync DownloadResult:" + GetDataResult);

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Exception in GetItemsAsync JSON parsing");
                                Debug.WriteLine(ex);
                                GetDataResult = DownloadResult.ParseError;
                            }
                        }
                        else
                        {
                            GetDataResult = DownloadResult.DownloadError;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception in GetItemsAsync download");
                        Debug.WriteLine(ex);
                        GetDataResult = DownloadResult.DownloadError;
                    }
                }
                else
                {
                    GetDataResult = DownloadResult.NoNetwork;
                }
            }

            return await Task.FromResult(dataStoreIQPitems);
        }

    }
}