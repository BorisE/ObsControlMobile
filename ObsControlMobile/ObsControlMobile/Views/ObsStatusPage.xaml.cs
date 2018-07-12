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
	public partial class ObsStatusPage : ContentPage
	{
        public ObsStatusViewModel viewModel;

        public ObsStatusPage()
        {
            BindingContext = viewModel = new ObsStatusViewModel(this);

            if (viewModel.ObsStatus_LV_Grouped_source.Count == 0)
                viewModel.LoadObsStatusCommand.Execute(null);

            InitializeComponent();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            // Manually deselect item.
            //ObsStatusListView.SelectedItem = null;

            ObsStatus_LV_Element_Class selectedPhone = args.SelectedItem as ObsStatus_LV_Element_Class;
            if (selectedPhone != null)
                await DisplayAlert("Выбранная модель", $"{selectedPhone.Name} - {selectedPhone.Value}", "OK");

            //await DisplayAlert("Alert", "You have been alerted", "OK");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                //if empty, reload
                if (viewModel.ObsStatus_LV_Grouped_source.Count == 0)
                    viewModel.LoadObsStatusCommand.Execute(null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("OnAppearing Exception");
                Debug.WriteLine(ex);
            }
        }

    }
}