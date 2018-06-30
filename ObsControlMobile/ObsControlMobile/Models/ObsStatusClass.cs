using System;
using System.Collections.Generic;
using System.Text;

namespace ObsControlMobile.Models
{
    /*
     * {
       "1":
            {
            "ir":{"date":"2018-06-13 21:45","value":27.42},
            "humidity":{"date":"2018-06-13 21:45","value":31.79},
            "inside":{"date":"2018-06-13 21:45","value":32.18},
            "akb":{"date":"2018-03-18 16:59","value":15.56}},
       "2":
            {
            "ir":{"date":"2017-05-24 10:38","value":14.79},
            "humidity":{"date":"2018-06-27 01:26","value":36.18},
            "inside":{"date":"2018-06-27 01:26","value":25},
            "roof":{"date":"2018-06-27 01:26","value":0}
            }
       }
     */
    public class DateValuePair
    {
        public static int VALID_DATETIME_MAX_SECONDS_SINCE_NOW = 5*60; //5 min

        public DateTime date = DateTime.MinValue;
        public double value = -100.0;

        public bool ValueIsRecent
        {
            get {
                return ((DateTime.Now - date).TotalSeconds < VALID_DATETIME_MAX_SECONDS_SINCE_NOW);
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

        public ObsStatusElement_Class()
        {
            ir = new DateValuePair();
            humidity = new DateValuePair();
            inside = new DateValuePair();
            akb = new DateValuePair();
            roof = new DateValuePair();
        }
    }

}
