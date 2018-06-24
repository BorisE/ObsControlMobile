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

namespace ObsControlMobile.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IQPPage : ContentPage
	{
        public IQPViewModel viewModel;

        public IQPPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new IQPViewModel(this);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as IQPItem;
            if (item == null)
                return;

            await Navigation.PushAsync(new IQPItemDetailPage(new IQPItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListViewIQP.SelectedItem = null;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            //if empty, reload
            if (viewModel.IQPItems.Count == 0)
                viewModel.LoadIQPItemsCommand.Execute(null);
        }
    }
}