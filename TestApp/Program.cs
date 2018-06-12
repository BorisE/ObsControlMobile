using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ObsControlMobile.Models;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string JSON = "[{\"StarsNumber\":\"1100\",\"SkyBackground\":\"0.0164\",\"MeanRadius\":\"4.23373794545\",\"AspectRatio\":null,\"DateObsUTC\":\"2018-03-14 17:24:32\",\"ImageExposure\":\"120\",\"ImageFilter\":\"L\",\"ImageType\":\"Light Frame\",\"ImageBinningX\":null,\"ImageBinningY\":\"1\",\"ImageSetTemp\":\"-30\",\"ImageTemp\":\"-30.019999329\",\"CameraPixelSizeX\":\"5.4\",\"CameraPixelSizeY\":\"5.4\",\"ObjName\":\"M46\",\"ObjRA\":\"07 41 50.0\",\"ObjDec\":\"-14 49 22.0\",\"ObjAlt\":\"30.2827\",\"ObjAz\":\"176.4517\",\"CameraName\":\"ArtemisHSC\",\"Observer\":\"Boris Emchenko\",\"TelescopeName\":\"SW250\",\"TelescopeFocusLen\":\"1000\",\"TelescopeDiameter\":\"250\",\"FITSFileName\":\"M46_20180314_L_120s_1x1_-30degC_0.0degN_000006375.FIT\",\"PixelResolution\":\"1.113831\",\"FWHM\":\"4.71566856952\"},{\"StarsNumber\":\"1022\",\"SkyBackground\":\"0.0166\",\"MeanRadius\":\"4.38267011742\",\"AspectRatio\":null,\"DateObsUTC\":\"2018-03-14 17:27:13\",\"ImageExposure\":\"120\",\"ImageFilter\":\"L\",\"ImageType\":\"Light Frame\",\"ImageBinningX\":null,\"ImageBinningY\":\"1\",\"ImageSetTemp\":\"-30\",\"ImageTemp\":\"-30.019999329\",\"CameraPixelSizeX\":\"5.4\",\"CameraPixelSizeY\":\"5.4\",\"ObjName\":\"M46\",\"ObjRA\":\"07 41 50.0\",\"ObjDec\":\"-14 49 22.0\",\"ObjAlt\":\"30.3081\",\"ObjAz\":\"177.2029\",\"CameraName\":\"ArtemisHSC\",\"Observer\":\"Boris Emchenko\",\"TelescopeName\":\"SW250\",\"TelescopeFocusLen\":\"1000\",\"TelescopeDiameter\":\"250\",\"FITSFileName\":\"M46_20180314_L_120s_1x1_-30degC_0.0degN_000006376.FIT\",\"PixelResolution\":\"1.113831\",\"FWHM\":\"4.88155383955\"},{\"StarsNumber\":\"1109\",\"SkyBackground\":\"0.0164\",\"MeanRadius\":\"4.24450955816\",\"AspectRatio\":null,\"DateObsUTC\":\"2018-03-14 17:29:54\",\"ImageExposure\":\"120\",\"ImageFilter\":\"L\",\"ImageType\":\"Light Frame\",\"ImageBinningX\":null,\"ImageBinningY\":\"1\",\"ImageSetTemp\":\"-30\",\"ImageTemp\":\"-30.019999329\",\"CameraPixelSizeX\":\"5.4\",\"CameraPixelSizeY\":\"5.4\",\"ObjName\":\"M46\",\"ObjRA\":\"07 41 50.0\",\"ObjDec\":\"-14 49 22.0\",\"ObjAlt\":\"30.3277\",\"ObjAz\":\"177.9544\",\"CameraName\":\"ArtemisHSC\",\"Observer\":\"Boris Emchenko\",\"TelescopeName\":\"SW250\",\"TelescopeFocusLen\":\"1000\",\"TelescopeDiameter\":\"250\",\"FITSFileName\":\"M46_20180314_L_120s_1x1_-30degC_0.0degN_000006377.FIT\",\"PixelResolution\":\"1.113831\",\"FWHM\":\"4.72766632568\"}]";

            //JSON.Replace('.', ',');

            var settings = new JsonSerializerSettings();
            settings.Culture = new CultureInfo("ru-RU");

            settings.Culture.NumberFormat.NumberDecimalSeparator = ".";
            settings.NullValueHandling = NullValueHandling.Ignore;

            var list = JsonConvert.DeserializeObject<IEnumerable<IQPItem>>(JSON, settings);

            Console.WriteLine(list.Count());
            
            foreach(IQPItem item in list)
            {
                Console.WriteLine(item.ToString());
            }

            Console.ReadLine();
        }
    }
}
