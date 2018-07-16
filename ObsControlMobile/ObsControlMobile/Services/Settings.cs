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
        public static string Other = "";
    }

    public static class Settings
    {
        #region Configuration settings
        public static string PowerStatusURL = "http://astrohostel.ru/rest/power";
        public static string ObsStatusURL = "http://astrohostel.ru/status/json";
        public static string AllskyStatusURL = "http://astro.milantiev.com/allsky/stat.php";
        #endregion


        #region Settings class properties
        public static string DefaultIQPURL = "http://astromania.info/iqp_data/getcurrentsession.php";
        public static string DefaultLogin = "BorisE";
        public static string DefaultPass = "1";

        /// <summary>
        /// Main Settings Object reference
        /// </summary>
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
        #endregion Settings class properties
    }
}
