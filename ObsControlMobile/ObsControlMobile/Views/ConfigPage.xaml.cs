using ObsControlMobile.Services;
using ObsControlMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ObsControlMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfigPage : ContentPage
	{
        public ConfigViewModel viewModel;

        public ConfigPage ()
		{
			InitializeComponent ();

            BindingContext = viewModel = new ConfigViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.ErrorMessage = "";
            viewModel.InitConfigValues();
        }
    }
}