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


    public class DateValuePair
    {
        public static int VALID_DATETIME_MAX_SECONDS_SINCE_NOW = 5 * 60; //5 min

        public DateTime date = DateTime.MinValue;
        public double value = -100.0;

        public bool ValueIsRecent
        {
            get
            {
                return ((DateTime.Now - date).TotalSeconds > VALID_DATETIME_MAX_SECONDS_SINCE_NOW);
            }
        }
    }

    public class ObsStatusElement_Class
    {
        public DateValuePair ir;
        public DateValuePair humidity;
        public DateValuePair inside;
        public DateValuePair akb;
        public DateValuePair roof;
    }


    public class PowerStatusItem
    {
        public string Title { get; set; }
        public bool Status { get; set; }
    }

    public class JSONPowerStatusListClass : Dictionary<string, int>
    {
    }




}
