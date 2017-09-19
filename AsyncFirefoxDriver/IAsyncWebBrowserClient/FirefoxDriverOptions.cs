// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Zu.WebBrowser.BrowserOptions;

namespace Zu.Firefox
{
    internal class FirefoxDriverOptions: IOptions
    {
        private IAsyncFirefoxDriver asyncFirefoxDriver;
        private FirefoxDriverWindow window;
        private FirefoxDriverTimeouts timeouts;

        public FirefoxDriverOptions(IAsyncFirefoxDriver asyncFirefoxDriver)
        {
            this.asyncFirefoxDriver = asyncFirefoxDriver;
        }

        public ICookieJar Cookies => throw new System.NotImplementedException();

        public IWindow Window { get { if (window == null) window = new FirefoxDriverWindow(asyncFirefoxDriver); return window; } }

        public ILogs Logs => throw new System.NotImplementedException();

        public ITimeouts Timeouts { get { if (timeouts == null) timeouts = new FirefoxDriverTimeouts(asyncFirefoxDriver); return timeouts; } }

        public bool HasLocationContext => throw new System.NotImplementedException();

        public ILocationContext LocationContext => throw new System.NotImplementedException();

        public bool HasApplicationCache => throw new System.NotImplementedException();

        public IApplicationCache ApplicationCache => throw new System.NotImplementedException();

        public ILocalStorage LocalStorage => throw new System.NotImplementedException();

        public ISessionStorage SessionStorage => throw new System.NotImplementedException();
    }
}