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
        public int VALID_DATETIME_MAX_SECONDS_SINCE_NOW = 5*60; //5 min

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

            //Set individual VALID_DATETIME_MAX_SECONDS_SINCE_NOW
            akb.VALID_DATETIME_MAX_SECONDS_SINCE_NOW = 10 * 60;
            roof.VALID_DATETIME_MAX_SECONDS_SINCE_NOW = 2 * 60;
        }
    }

    public class ObsStatus_LV_Class
    {
        public DateValuePair ir;
        public DateValuePair humidity1;
        public DateValuePair inside1;
        public DateValuePair humidity2;
        public DateValuePair inside2;
        public DateValuePair akb;
        public DateValuePair roof;

        public ObsStatus_LV_Class()
        {
            ir = new DateValuePair();
            humidity1 = new DateValuePair();
            inside1 = new DateValuePair();
            humidity2 = new DateValuePair();
            inside2 = new DateValuePair();
            akb = new DateValuePair();
            roof = new DateValuePair();

            //Set individual VALID_DATETIME_MAX_SECONDS_SINCE_NOW
            akb.VALID_DATETIME_MAX_SECONDS_SINCE_NOW = 10 * 60;
            roof.VALID_DATETIME_MAX_SECONDS_SINCE_NOW = 2 * 60;
        }
    }

    public class ObsStatus_LV_Element_Class2: DateValuePair
    {
        public string Id { get; set; }
        public string NameEl = "";

        public ObsStatus_LV_Element_Class2(): base()
        {
        }
    }

    public class ObsStatus_LV_Element_Class
    {
        public string Id { get; set; }
        public string NameEl { get; set; }
        public double valueEl { get; set; }
        public DateTime dateEl { get; set; }

        public ObsStatus_LV_Element_Class()
        {
            NameEl = "";
            valueEl = -100.0;
            dateEl = DateTime.Now;
        }
    }
}
