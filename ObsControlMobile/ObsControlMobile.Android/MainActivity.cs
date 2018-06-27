using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Android.Content;
using ObsControlMobile.Services;

namespace ObsControlMobile.Droid
{
    [Activity(Label = "ObsControl Mobile", Icon = "@mipmap/logo_observ2", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public DateTime FromUnixTime(long unixTimeMillis)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTimeMillis);
        }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Context context = this.ApplicationContext;
            var version = context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
            VersionData.Version = version;
            long JavaDate = context.PackageManager.GetPackageInfo(context.PackageName, 0).LastUpdateTime;

            VersionData.Other = FromUnixTime(JavaDate).ToString("dd-MM-yyyy HH:mm:ss");

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

