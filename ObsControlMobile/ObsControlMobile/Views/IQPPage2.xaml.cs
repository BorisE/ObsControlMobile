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
	public partial class IQPPage2 : ContentPage
	{
        public IQPViewModel2 viewModel;

        public IQPPage2()
        {
            InitializeComponent();

            BindingContext = viewModel = new IQPViewModel2(this);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            //if empty, reload
            if (viewModel.IQPItems.Count == 0)
                viewModel.LoadIQPItemsCommand2.Execute(null);
        }
    }
}