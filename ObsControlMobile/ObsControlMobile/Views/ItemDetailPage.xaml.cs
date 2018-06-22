using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ObsControlMobile.Models;
using ObsControlMobile.ViewModels;

namespace ObsControlMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemDetailPage : ContentPage
	{
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var item = new IQPItem
            { Id = Guid.NewGuid().ToString(), StarsNumber = 250, SkyBackground = 0.09, MeanRadius = 3.78673095436, AspectRatio = 0.931, DateObsUTC = DateTime.Now,
                ImageExposure = 600d, ImageFilter = "R", ImageType = "Light Frame", ImageBinningX = 1, ImageBinningY = 1, ImageSetTemp = -20.0, ImageTemp = -20.0299995523, CameraPixelSizeX = 5.4, CameraPixelSizeY = 5.4,
                ObjName = "M60", ObjRA = "12 42 35.0", ObjDec = "+11 40 02.0", ObjAlt = 48.9, ObjAz = 310.0, CameraName = "ArtemisHSC", Observer = "Boris Emchenko", TelescopeName = "SW250", TelescopeFocusLen = 1000d, TelescopeDiameter = 250d,
                FITSFileName = "M20_20180612_L_600s_1x1_-20degC_0.0degN_000008769.FIT", PixelResolution = 1.113831, FWHM = 4.21777832562,
            };

            viewModel = new ItemDetailViewModel(item);
            BindingContext = viewModel;
        }
    }
}