using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ObsControlMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AboutPage : ContentPage
	{
		public AboutPage ()
		{
			InitializeComponent ();
		}

        async void GoConfig_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new ConfigPage()));
        }
    }
}