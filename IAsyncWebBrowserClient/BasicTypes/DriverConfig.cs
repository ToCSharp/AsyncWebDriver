// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;

namespace Zu.WebBrowser.BasicTypes
{

    public class DriverConfig
    {
        public string UserDir { get; set; }
        public string CommandLineArgumets { get; set; }
        public bool IsTempProfile { get; set; } = true;
        public bool IsDefaultProfile { get; set; } = false;
        public int TempDirCreateDelay { get; set; } = 3000;
        public int Port { get; set; } = 0;
        public bool Headless { get; set; } = false;
        public WebSize WindowSize { get; set; } = null;
        public bool DoNotOpenChromeProfile { get; set; } = false;

        public DriverConfig()
        {
            this.SetIsTempProfile();
            Port = 11000 + new Random().Next(4000);
        }
    }

    public static class DriverConfigFluent
    { 
        public static T SetUserDir<T>(this T dc, string userDir) where T: DriverConfig
        {
            dc.IsTempProfile = false;
            dc.UserDir = userDir;
            return dc;
        }
        public static T SetCommandLineArgumets<T>(this T dc, string args) where T : DriverConfig
        {
            dc.CommandLineArgumets = args;
            return dc;
        }
        public static T SetIsTempProfile<T>(this T dc, bool isTempProfile = true) where T : DriverConfig
        {
            dc.IsTempProfile = isTempProfile;
            if(dc.IsTempProfile && string.IsNullOrWhiteSpace(dc.UserDir)) dc.UserDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            return dc;
        }
        public static T SetIsDefaultProfile<T>(this T dc, bool isDefaultProfile = true) where T : DriverConfig
        {
            dc.IsDefaultProfile = isDefaultProfile;
            if(isDefaultProfile) dc.IsTempProfile = false;
            dc.SetUserDir(null);
            return dc;
        }
        public static T SetPort<T>(this T dc, int port) where T : DriverConfig
        {
            dc.Port = port;
            return dc;
        }
        public static T SetHeadless<T>(this T dc, bool headless = true) where T : DriverConfig
        {
            dc.Headless = headless;
            if (dc.Headless && dc.WindowSize == null) dc.WindowSize = new WebSize(1200, 900);
            return dc;
        }
        public static T SetWindowSize<T>(this T dc, int width, int height) where T : DriverConfig
        {
            dc.WindowSize = new WebSize(width, height);
            return dc;
        }
        public static T SetDoNotOpenChromeProfile<T>(this T dc, bool doNotOpenChromeProfile = true) where T : DriverConfig
        {
            dc.DoNotOpenChromeProfile = doNotOpenChromeProfile;
            return dc;
        }
        public static T SetTempDirCreateDelay<T>(this T dc, int delay) where T : DriverConfig
        {
            dc.TempDirCreateDelay = delay;
            return dc;
        }
    }
}