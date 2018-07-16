using System;
using System.Collections.Generic;
using System.Text;

namespace ObsControlMobile.Models
{
    public class PowerStatusItem
    {
        public string Title { get; set; }
        public bool Status { get; set; }

        public int StatusNumeric
        {
            get => (Status ? 1 : 0 );
            set {
                Status = (value >= 1 ? true : false);
            }
        }
    }

    public class JSONPowerStatusListClass: Dictionary<string, int>
    {
    }

}
