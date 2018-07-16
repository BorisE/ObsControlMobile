using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ObsControlMobile.Models;
using ObsControlMobile.Views;
using ObsControlMobile.ViewModels;
using System.Diagnostics;

namespace ObsControlMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PowerPage : ContentPage
	{
        public PowerViewModel viewModel;

        public PowerPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new PowerViewModel(this);

                       
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            // Manually deselect item.
            //ObsStatusListView.SelectedItem = null;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            base.OnAppearing();

            //if empty, reload
            if (viewModel.PowerStatusItems.Count == 0)
                viewModel.LoadPowerStatusCommand.Execute(null);
        }

    }
}