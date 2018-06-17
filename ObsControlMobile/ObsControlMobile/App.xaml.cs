using System;
using Xamarin.Forms;
using ObsControlMobile.Views;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace ObsControlMobile
{
	public partial class App : Application
	{
		
		public App ()
		{
			InitializeComponent();


			MainPage = new MainPage();

            Debug.WriteLine("Program was started");
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
            // Handle when your app resumes
            //System.Collections.Generic.IList<Page> MP = ((TabbedPage)MainPage).Children;
            //foreach (Page pg in MP)
            //{
            //    if (pg.Title=="IQP")
            //        ((IQPPage)pg).viewModel.GetJSON();
            //}


        }
	}
}
