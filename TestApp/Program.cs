using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

        static void Main(string[] args)
        {
            Example();
            Console.ReadLine();
        }
    }
}
