using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ObsControlMobile.Models;
using Plugin.Connectivity;

namespace ObsControlMobile.Services
{
    public class NetworkServices
    {
        public static bool IsConnectedToInternet()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                return true;
            }
            else
            {
                // write your code if there is no Internet available    
                return false;
            }
        }

        public static async Task<Tuple<T, DownloadResult>> GetJSON<T>(string stURL)
        {
            Debug.WriteLine("GetJSON [" + typeof(T) + "] enter");

            //Return object
            T objResponse = default(T);
            DownloadResult retDataResult = DownloadResult.Undefined;
            // Check network status  
            if (NetworkServices.IsConnectedToInternet())
            {

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
                        Debug.WriteLine("GetJSON [" + typeof(T) + "] DOWNLOAD DATA:" + responseSt);

                        try
                        {
                            //3.1 Set JSON parse options
                            JsonSerializerSettings JSONSettings = new JsonSerializerSettings();
                            JSONSettings.Culture = new CultureInfo("ru-RU");
                            JSONSettings.Culture.NumberFormat.NumberDecimalSeparator = ".";
                            JSONSettings.NullValueHandling = NullValueHandling.Ignore;

                            //3.2. JSON convert
                            objResponse = JsonConvert.DeserializeObject<T>(responseSt, JSONSettings);
                            Debug.WriteLine("GetJSON [" + typeof(T) + "] was converted to JSON:" + objResponse.ToString());

                            retDataResult = DownloadResult.Success;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine("Exception in GetJSON [" + typeof(T) + "] JSON parsing");
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
                    Debug.WriteLine("Exception in GetJSON [" + typeof(T) + "] download");
                    Debug.WriteLine(ex);
                    retDataResult = DownloadResult.DownloadError;
                }
            }
            else
            {
                Debug.WriteLine("GetJSON  [" + typeof(T) + "]- no network");
                retDataResult = DownloadResult.NoNetwork;
            }
            Debug.WriteLine("GetJSON [" + typeof(T) + "] return status:" + retDataResult);

            return new Tuple<T, DownloadResult>(objResponse, retDataResult);
        }

        public static async Task<Tuple<T, DownloadResult>> GetJSONCredential<T>(string stURL, NetworkCredential givenCredentials)
        {
            Debug.WriteLine("GetJSON [" + typeof(T) + "] enter");

            //Return object
            T objResponse = default(T);
            DownloadResult retDataResult = DownloadResult.Undefined;
            // Check network status  
            if (NetworkServices.IsConnectedToInternet())
            {

                try
                {
                    //1. Download data
                    Uri geturi = new Uri(stURL); //replace your xml url  
                    var client = new WebClient { Credentials = givenCredentials };
                    string responseSt = await client.DownloadStringTaskAsync(geturi);
                    Debug.WriteLine("GetJSONCredential [" + typeof(T) + "] DOWNLOAD DATA:" + responseSt);

                    try
                    {
                        //3.1 Set JSON parse options
                        JsonSerializerSettings JSONSettings = new JsonSerializerSettings();
                        JSONSettings.Culture = new CultureInfo("ru-RU");
                        JSONSettings.Culture.NumberFormat.NumberDecimalSeparator = ".";
                        JSONSettings.NullValueHandling = NullValueHandling.Ignore;

                        //3.2. JSON convert
                        objResponse = JsonConvert.DeserializeObject<T>(responseSt, JSONSettings);
                        Debug.WriteLine("GetJSONCredential [" + typeof(T) + "] was converted to JSON:" + objResponse.ToString());

                        retDataResult = DownloadResult.Success;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception in GetJSONCredential [" + typeof(T) + "] JSON parsing");
                        Debug.WriteLine(ex);
                        retDataResult = DownloadResult.ParseError;
                    }
                }
                catch (WebException we)
                {
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;
                    Debug.WriteLine("WebException in GetJSONCredential [" + typeof(T) + "] download. StatusCode=" + response.StatusCode);
                    Debug.WriteLine(we);
                    retDataResult = DownloadResult.HttpError;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    { 
                        retDataResult = DownloadResult.AuthError;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception in GetJSONCredential [" + typeof(T) + "] download");
                    Debug.WriteLine(ex);
                    retDataResult = DownloadResult.DownloadError;
                }
            }
            else
            {
                Debug.WriteLine("GetJSONCredential  [" + typeof(T) + "]- no network");
                retDataResult = DownloadResult.NoNetwork;
            }
            Debug.WriteLine("GetJSONCredential [" + typeof(T) + "] return status:" + retDataResult);

            return new Tuple<T, DownloadResult>(objResponse, retDataResult);
        }




    }
}

