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
	public partial class SkyDataPage : ContentPage
	{
        SkyDataViewModel viewModel;

        public SkyDataPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SkyDataViewModel(this);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}