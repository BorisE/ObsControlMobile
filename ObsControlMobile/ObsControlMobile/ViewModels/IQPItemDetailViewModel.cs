using System;

using ObsControlMobile.Models;

namespace ObsControlMobile.ViewModels
{
    public class IQPItemDetailViewModel : BaseViewModel
    {
        public IQPItem Item { get; set; }
        public IQPItemDetailViewModel(IQPItem item = null)
        {
            Title = item?.FITSFileName;
            Item = item;
        }
    }
}
