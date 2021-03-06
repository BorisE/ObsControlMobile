﻿using System;
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
	public partial class ObsStatusPage : ContentPage
	{
        public ObsStatusViewModel viewModel;

        public ObsStatusPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ObsStatusViewModel(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}