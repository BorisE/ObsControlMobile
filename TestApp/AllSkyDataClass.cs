using System;
using System.Collections.Generic;
using System.Text;

namespace TestApp
{

    public enum DownloadResult { Success = 1, Undefined = 0, NoNetwork = -1, DownloadError = -2, ParseError = -3 };

    /// <summary>
    /// JSON object to return from site
    /// </summary>
    public class AllSkyDataClass
    {
        //{"filename":"allsky-current.jpg","timestamp":1528686365}
        public string filename = "";
        public int timestamp = 0;
    }
}
