using System;
using Xamarin.Forms;
using ObsControlMobile.Views;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using System.Collections.Generic;
using ObsControlMobile.ViewModels;

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
            try
            {
                TabbedPage MainPageTabbed = (TabbedPage)MainPage;
                Debug.WriteLine("OnResume, MainPageTabbed created");
                IList<Page> ChildPages = MainPageTabbed.Children;
                Debug.WriteLine("OnResume, ChildPages created");
                foreach (Page pg in ChildPages)
                {
                    if (pg.Title == "IQP")
                    {
                        Debug.WriteLine("OnResume, IQP found");
                        IQPPage _iqppage = (IQPPage)pg; //problem is here!!!
                        Debug.WriteLine("OnResume, IQPpage created");
                        IQPViewModel _iqpmodel = _iqppage.viewModel;
                        Debug.WriteLine("OnResume, IQPViewModel created");
                        _iqpmodel.LoadIQPItemsCommand.Execute(null);
                        Debug.WriteLine("OnResume, LoadIQPItemsCommand executed");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception onResume");
                Debug.WriteLine(ex);
            }

        }
    }
}
