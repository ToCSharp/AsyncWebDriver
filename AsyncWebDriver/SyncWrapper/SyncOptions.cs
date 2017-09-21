// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Zu.WebBrowser.BrowserOptions;

namespace Zu.AsyncWebDriver.Remote
{
    public class SyncOptions
    {
        private IOptions options;

        public SyncOptions(IOptions options)
        {
            this.options = options;
        }

        public SyncCookieJar Cookies => new SyncCookieJar(options.Cookies);
        public SyncWindow Window => new SyncWindow(options.Window);
        public SyncLogs Logs => new SyncLogs(options.Logs);
        public SyncTimeouts Timeouts => new SyncTimeouts(options.Timeouts);

    }
}