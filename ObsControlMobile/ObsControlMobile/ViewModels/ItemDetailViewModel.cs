using System;

using ObsControlMobile.Models;

namespace ObsControlMobile.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public IQPItem Item { get; set; }
        public ItemDetailViewModel(IQPItem item = null)
        {
            Title = item?.FITSFileName;
            Item = item;
        }
    }
}
