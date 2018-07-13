using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AsrtoUtils;
using Newtonsoft.Json;



namespace TestApp
{
    class Program
    {
        public static async Task<Tuple<T, DownloadResult>> GetJSON<T>(string stURL)
        {
            //Return object
            T objResponse = default(T);
            DownloadResult retDataResult = DownloadResult.Undefined;

            Console.WriteLine("Type: "+typeof(T));

            try
            {
                //1. Download data
                Uri geturi = new Uri(stURL); //replace your xml url  
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(geturi);


                //2. If data ok
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //2.2. Read string
                    string responseSt = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine("GetJSON DOWNLOAD DATA:" + responseSt);

                    try
                    {
                        //3.1 Set JSON parse options
                        JsonSerializerSettings JSONSettings = new JsonSerializerSettings();
                        JSONSettings.Culture = new CultureInfo("ru-RU");
                        JSONSettings.Culture.NumberFormat.NumberDecimalSeparator = ".";
                        JSONSettings.NullValueHandling = NullValueHandling.Ignore;

                        //3.2. JSON convert
                        objResponse = JsonConvert.DeserializeObject<T>(responseSt, JSONSettings);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception in GetJSON JSON parsing");
                        Debug.WriteLine(ex);
                        retDataResult = DownloadResult.ParseError;
                    }
                }
                else
                {
                    retDataResult = DownloadResult.DownloadError;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in GetAllSkyJSONData download");
                Debug.WriteLine(ex);
                retDataResult = DownloadResult.DownloadError;
            }

            return new Tuple<T, DownloadResult>(objResponse,retDataResult);
        }

        static async void Example()
        {
            // This method runs asynchronously.
            DownloadResult res;
            Tuple<AllSkyDataClass,DownloadResult> t2;
            AllSkyDataClass t;
            t2 = await Task.Run(() => GetJSON<AllSkyDataClass>("http://astro.milantiev.com/allsky/stat.php"));
            Console.WriteLine("Compute: " + t2.Item1.timestamp);
            Console.WriteLine("Res: " + t2.Item2);
        }

        static void ParseArrayExample()
        {
            string responseSt = "{\"1\":{\"ir\":{\"date\":\"2018-06-13 21:45\",\"value\":27.42},\"humidity\":{\"date\":\"2018-06-13 21:45\",\"value\":31.79},\"inside\":{\"date\":\"2018-06-13 21:45\",\"value\":32.18},\"akb\":{\"date\":\"2018-03-18 16:59\",\"value\":15.56}},\"2\":{\"ir\":{\"date\":\"2017-05-24 10:38\",\"value\":14.79},\"humidity\":{\"date\":\"2018-06-30 10:50\",\"value\":32.5},\"inside\":{\"date\":\"2018-06-30 10:50\",\"value\":35.6},\"roof\":{\"date\":\"2018-06-30 10:50\",\"value\":0}}}";

            Dictionary<string, ObsStatusElement_Class> objResponse = JsonConvert.DeserializeObject<Dictionary<string,ObsStatusElement_Class>>(responseSt);

            string stout = JsonConvert.SerializeObject(objResponse);
            Console.Write("Dump: ");
            Console.WriteLine(stout);

        }

        static void TimeConversionExample()
        {
            string SunsetTimeSt = AstroUtilsProp.SunSetDateTime().ToString("HH:mm");
            Console.WriteLine("SunsetTimeSt:" + SunsetTimeSt);
            string CivTwilightEndTimeSt = AstroUtilsProp.CivilTwilightSetDateTime().ToString("HH:mm");
            Console.WriteLine("CivTwilightEndTimeSt: "+ CivTwilightEndTimeSt);
            string NavTwilightEndTimeSt = AstroUtilsProp.NautTwilightSetDateTime().ToString("HH:mm");
            Console.WriteLine("NavTwilightEndTimeSt: " + NavTwilightEndTimeSt);
            string AstroTwilightEndTimeSt = AstroUtilsProp.AstronTwilightSetDateTime().ToString("HH:mm");
            Console.WriteLine("AstroTwilightEndTimeSt: " + AstroTwilightEndTimeSt);

            string AstroTwilightBegTimeSt = AstroUtilsProp.AstronTwilightRiseDateTime().ToString("HH:mm");
            Console.WriteLine("AstroTwilightBegTimeSt: " + AstroTwilightBegTimeSt);

            //NavTwilightBegTimeSt = AstroUtilsProp.NautTwilightRiseDateTime().ToString("HH:mm");
            //CivTwilightBegTimeSt = AstroUtilsProp.CivilTwilightRiseDateTime().ToString("HH:mm");
            //SunriseTimeSt = AstroUtilsProp.SunRiseDateTime().ToString("HH:mm");

        }


        static void BaseAuthExample()
        {
            var client = new WebClient { Credentials = new NetworkCredential("borise", "astro11") };
            string response = client.DownloadString("http://astrohostel.ru/rest/power");

            Dictionary<string, int> values = JsonConvert.DeserializeObject<Dictionary<string, int>>(response);

            foreach (KeyValuePair<string, int> pair in values)
            { Console.WriteLine(pair.Key + "=" + pair.Value);  }

                Console.WriteLine(values);
        }

        static void Main(string[] args)
        {
            //TimeConversionExample();
            //ParseArrayExample();
            BaseAuthExample();
            Console.ReadLine();
        }
    }
}
