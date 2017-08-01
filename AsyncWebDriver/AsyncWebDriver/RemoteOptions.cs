// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver.Remote
{
    public class RemoteOptions : IOptions
    {
        private WebDriver driver;

        public RemoteOptions(WebDriver driver)
        {
            this.driver = driver;
        }

        public ICookieJar Cookies => throw new NotImplementedException();

        public IWindow Window => throw new NotImplementedException();

        public ILogs Logs => throw new NotImplementedException();

        public ITimeouts Timeouts()
        {
            throw new NotImplementedException();
        }
    }
}