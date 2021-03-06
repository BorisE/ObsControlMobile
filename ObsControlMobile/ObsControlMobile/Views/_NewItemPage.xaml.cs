﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ObsControlMobile.Models;

namespace ObsControlMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewItemPage : ContentPage
    {
        public IQPItem Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new IQPItem
            {
                FITSFileName = "Item name",
                Description = "This is an item description."
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }
    }
}