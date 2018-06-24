using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObsControlMobile.Services
{
    public static class VersionData
    {
        public static string Version = "";
    }

    public static class Settings
    {
        public static string DefaultIQPURL = "http://astromania.info/iqp_data/getcurrentsession.php";
        public static string DefaultLogin = "BorisE";
        public static string DefaultPass = "1";

        private static ISettings AppSettings => CrossSettings.Current;

        public static string IQPURL
        {   
            get {
                string st= AppSettings.GetValueOrDefault(nameof(IQPURL), DefaultIQPURL);
                if (st == "") st = DefaultIQPURL;
                return st;
            }
            set => AppSettings.AddOrUpdateValue(nameof(IQPURL), value);
        }

        public static string Login
        {
            get => AppSettings.GetValueOrDefault(nameof(Login), DefaultLogin);
            set => AppSettings.AddOrUpdateValue(nameof(Login), value);
        }

        public static string Pass
        {
            get => AppSettings.GetValueOrDefault(nameof(Pass), DefaultPass);
            set => AppSettings.AddOrUpdateValue(nameof(Pass), value);
        }
    }
}
