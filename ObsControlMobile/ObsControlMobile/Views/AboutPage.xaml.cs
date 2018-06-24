using ObsControlMobile.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ObsControlMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPage : ContentPage
	{
        public AboutViewModel viewModel;

        public AboutPage ()
		{
			InitializeComponent ();

            BindingContext = viewModel = new AboutViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.ErrorMessage = "";
            viewModel.InitConfigValues();
        }

        async void GoConfig_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new ConfigPage()));
        }
    }
}