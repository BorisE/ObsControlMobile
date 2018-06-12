using System;
using System.Collections.Generic;
using System.Text;

using Plugin.Connectivity;

namespace ObsControlMobile.Services
{
    public class NetworkCheck
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
    }
}
