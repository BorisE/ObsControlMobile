using System;
using System.Collections.Generic;
using System.Text;

namespace ObsControlMobile.Models
{
    public class PowerStatusItem
    {
        public string Title { get; set; }
        public bool Status { get; set; }
    }

    public class JSONPowerStatusListClass: Dictionary<string, int>
    {
    }

}
