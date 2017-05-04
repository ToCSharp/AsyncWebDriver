// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

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