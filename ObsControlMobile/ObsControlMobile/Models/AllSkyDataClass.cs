using System;
using System.Collections.Generic;
using System.Text;

namespace ObsControlMobile.Models
{
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
