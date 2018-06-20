using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ObsControlMobile.Services
{ 
    public static class Settings
    {

        private static ISettings AppSettings => CrossSettings.Current;

        public static string IQPURL
        {   
            get => AppSettings.GetValueOrDefault(nameof(IQPURL), "http://astrohoster.ru/test");
            set => AppSettings.AddOrUpdateValue(nameof(IQPURL), value);
        }

        public static string Login
        {
            get => AppSettings.GetValueOrDefault(nameof(Login), "BorisE");
            set => AppSettings.AddOrUpdateValue(nameof(Login), value);
        }

        public static string Pass
        {
            get => AppSettings.GetValueOrDefault(nameof(Pass), "");
            set => AppSettings.AddOrUpdateValue(nameof(Pass), value);
        }
    }
}
