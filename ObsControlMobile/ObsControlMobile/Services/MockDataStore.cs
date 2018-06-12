using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ObsControlMobile.Models;

[assembly: Xamarin.Forms.Dependency(typeof(ObsControlMobile.Services.MockDataStore))]
namespace ObsControlMobile.Services
{
    public class MockDataStore : IDataStore<IQPItem>
    {
        List<IQPItem> items;

        public MockDataStore()
        {
            items = new List<IQPItem>();
            var mockItems = new List<IQPItem>
            {
                new IQPItem { Id = Guid.NewGuid().ToString(), StarsNumber=250, SkyBackground = 0.09, MeanRadius=3.78673095436,AspectRatio=0.931, DateObsUTC=DateTime.Now,
                    ImageExposure =600d, ImageFilter="R", ImageType="Light Frame", ImageBinningX=1, ImageBinningY=1,ImageSetTemp=-20.0, ImageTemp=-20.0299995523, CameraPixelSizeX=5.4, CameraPixelSizeY=5.4,
                    ObjName="M60", ObjRA="12 42 35.0",ObjDec="+11 40 02.0", ObjAlt=48.9, ObjAz=310.0, CameraName="ArtemisHSC", Observer="Boris Emchenko", TelescopeName="SW250", TelescopeFocusLen=1000d, TelescopeDiameter=250d,
                    FITSFileName="M20_20180612_L_600s_1x1_-20degC_0.0degN_000008769.FIT", PixelResolution=1.113831, FWHM=4.21777832562 },
            
                new IQPItem { Id = Guid.NewGuid().ToString(), StarsNumber=250, SkyBackground = 0.09, MeanRadius=3.78673095436,AspectRatio=0.931, DateObsUTC=DateTime.Now,
                    ImageExposure =600d, ImageFilter="L", ImageType="Light Frame", ImageBinningX=1, ImageBinningY=1,ImageSetTemp=-20.0, ImageTemp=-20.0299995523, CameraPixelSizeX=5.4, CameraPixelSizeY=5.4,
                    ObjName="M60", ObjRA="12 42 35.0",ObjDec="+11 40 02.0", ObjAlt=51.9, ObjAz=310.0, CameraName="ArtemisHSC", Observer="Boris Emchenko", TelescopeName="SW250", TelescopeFocusLen=1000d, TelescopeDiameter=250d,
                    FITSFileName="M20_20180612_L_600s_1x1_-20degC_0.0degN_000008769.FIT", PixelResolution=1.113831, FWHM=4.01777832562 },
            };

            foreach (var item in mockItems)
            {
                items.Add(item);
            }
        }

        public async Task<bool> AddItemAsync(IQPItem item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(IQPItem item)
        {
            var _item = items.Where((IQPItem arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(_item);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var _item = items.Where((IQPItem arg) => arg.Id == id).FirstOrDefault();
            items.Remove(_item);

            return await Task.FromResult(true);
        }

        public async Task<IQPItem> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<IQPItem>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}