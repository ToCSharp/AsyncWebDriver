// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.BrowserOptions;

namespace Zu.AsyncWebDriver.Remote
{
    public class SyncTimeouts
    {
        private ITimeouts timeouts;

        public SyncTimeouts(ITimeouts timeouts)
        {
            this.timeouts = timeouts;
        }

        public TimeSpan GetAsynchronousJavaScript()
        {
            TimeSpan res = default(TimeSpan);
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () =>
            {
                res = await timeouts.GetAsynchronousJavaScript();
                MRes.Set();
            });
            MRes.Wait();
            return res;
        }
        public TimeSpan GetImplicitWait()
        {
            TimeSpan res = default(TimeSpan);
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () =>
            {
                res = await timeouts.GetAsynchronousJavaScript();
                MRes.Set();
            });
            MRes.Wait();
            return res;
        }
        public TimeSpan GetPageLoad()
        {
            TimeSpan res = default(TimeSpan);
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () =>
            {
                res = await timeouts.GetAsynchronousJavaScript();
                MRes.Set();
            });
            MRes.Wait();
            return res;
        }
        public void SetAsynchronousJavaScript(TimeSpan time)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () =>
            {
                await timeouts.SetAsynchronousJavaScript(time);
                MRes.Set();
            });
            MRes.Wait();
        }
        public void SetImplicitWait(TimeSpan implicitWait)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () =>
            {
                await timeouts.SetImplicitWait(implicitWait);
                MRes.Set();
            });
            MRes.Wait();
        }
        public void SetPageLoad(TimeSpan time)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () =>
            {
                await timeouts.SetPageLoad(time);
                MRes.Set();
            });
            MRes.Wait();
        }

    }
}