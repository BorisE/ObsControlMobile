using ObsControlMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ObsControlMobile.Services
{
    /***********************************
 * Get all power status:
 *      http://astrohostel.ru/rest/power 
 * GET http://astrohostel.ru/rest/power/boris_pc - чтение одного порта
 * PUT http://astrohostel.ru/rest/power/boris_scope/1 - запись в один порт
 * 
 * BASIC HTTP AUTH логин / пароль.
 **********************************/

    public class PowerServiceClass
    {
        JSONPowerStatusListClass powerStatusSaveDict;


        public PowerServiceClass()
        {
            powerStatusSaveDict = new JSONPowerStatusListClass();
        }

        /// <summary>
        /// Return all power status
        /// </summary>
        /// <returns></returns>
        public async Task<Tuple<JSONPowerStatusListClass, DownloadResult>> GetStatusAsync()
        {
            powerStatusSaveDict.Clear();

            JSONPowerStatusListClass PowerStatusJSONListDef = new JSONPowerStatusListClass();
            DownloadResult downloadResultDef = DownloadResult.Undefined;
            Tuple<JSONPowerStatusListClass, DownloadResult> PowerStatusRet = new Tuple<JSONPowerStatusListClass, DownloadResult>(PowerStatusJSONListDef, downloadResultDef);

            NetworkCredential givenCredentials = new NetworkCredential(Settings.Login, Settings.Pass);

            //Debug.WriteLine("GetStatusAsync Got here 1");

            try
            {
                PowerStatusRet = await NetworkServices.GetJSONCredential<JSONPowerStatusListClass>(Settings.PowerStatusURL, givenCredentials, PowerStatusJSONListDef);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in PowerService.GetStatusAsync");
                Debug.WriteLine("Ex: " + ex);
            }

            //Debug.WriteLine("GetStatusAsync Got here 2");

            powerStatusSaveDict = PowerStatusRet.Item1;
            

            return PowerStatusRet;
        }

        public async Task<DownloadResult> SetStatusAsync(ObservableCollection<PowerStatusItem> powerStatusTargetList)
        {
            foreach(PowerStatusItem El in powerStatusTargetList)
            {
                if (powerStatusSaveDict[El.Title] != El.StatusNumeric)
                {
                    await SetItemStatusAsync(El);
                }
                else
                {
                    Debug.WriteLine("SetStatusAsync: skipping ["+ El.Title + "]=" + El.StatusNumeric);
                }
            }
            
            return DownloadResult.Undefined;

        }

        public async Task<DownloadResult> SetItemStatusAsync(PowerStatusItem powerStatusList)
        {
            Debug.WriteLine("SetItemStatusAsync enter");

            //Return object
            DownloadResult retDataResult = DownloadResult.Undefined;

            // Check network status  
            if (NetworkServices.IsConnectedToInternet())
            {
                try
                {
                    //1. Compose URL
                    Uri putURI = new Uri(Settings.PowerStatusURL + "/" + powerStatusList.Title + "/" + powerStatusList.StatusNumeric ); //replace your xml url  
                    Debug.WriteLine("SetItemStatusAsync url:" + putURI);
                    //2. Set credentials
                    NetworkCredential givenCredentials = new NetworkCredential(Settings.Login, Settings.Pass);
                    //3. Create Object
                    var client = new WebClient { Credentials = givenCredentials };
                    string datast = "";
                    //2. Run PUT Query
                    string responseSt = await client.UploadStringTaskAsync(putURI, "PUT", datast);

                    Debug.WriteLine("SetItemStatusAsync ["+putURI+"] response:" + responseSt);

                }
                catch (WebException we)
                {
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;
                    Debug.WriteLine("WebException in SetItemStatusAsync [" + Settings.PowerStatusURL + "/" + powerStatusList.Title + "/" + powerStatusList.StatusNumeric + "] download. StatusCode=" + response.StatusCode);
                    Debug.WriteLine(we);
                    retDataResult = DownloadResult.HttpError;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        retDataResult = DownloadResult.AuthError;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception in SetItemStatusAsync [" + Settings.PowerStatusURL + "/" + powerStatusList.Title + "/" + powerStatusList.StatusNumeric + "] download");
                    Debug.WriteLine(ex);
                    retDataResult = DownloadResult.DownloadError;
                }
            }
            else
            {
                Debug.WriteLine("SetItemStatusAsync  [" + Settings.PowerStatusURL + "/" + powerStatusList.Title + "/" + powerStatusList.StatusNumeric + "]- no network");
                retDataResult = DownloadResult.NoNetwork;
            }

            return retDataResult;

        }


    }
}
