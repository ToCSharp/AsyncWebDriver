// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Firefox
{
    public class FirefoxDriverConfig : DriverConfig
    {
        public FirefoxDriverConfig()
            :base()
        {
        }
        public FirefoxDriverConfig(DriverConfig config)
            :this()
        {
            UserDir = config.UserDir;
            IsTempProfile = config.IsTempProfile;
            IsDefaultProfile = config.IsDefaultProfile;
            TempDirCreateDelay = config.TempDirCreateDelay;
            Port = config.Port;
            Headless = config.Headless;
            WindowSize = config.WindowSize;
            DoNotOpenChromeProfile = config.DoNotOpenChromeProfile;
        }

        public bool OpenOffline { get; set; } = false;

        public string ProfileName { get => UserDir; set => this.SetUserDir(value); }

        public Dictionary<string, string> UserPreferences { get; set; }
    }

    public static class FirefoxDriverConfigFluent
    {
        public static T SetProfileName<T>(this T dc, string profileName = "default") where T: FirefoxDriverConfig
        {
            dc.ProfileName = profileName;
            return dc;
        }

        public static T SetOpenOffline<T>(this T dc, bool offline = true) where T : FirefoxDriverConfig
        {
            dc.OpenOffline = offline;
            return dc;
        }

        public static T SetUserPreferences<T>(this T dc, Dictionary<string, string> prefs) where T : FirefoxDriverConfig
        {
            dc.UserPreferences = prefs;
            return dc;
        }

    }
}