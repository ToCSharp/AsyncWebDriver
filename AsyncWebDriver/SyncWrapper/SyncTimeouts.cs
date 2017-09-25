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
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await timeouts.GetAsynchronousJavaScript();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }
        public TimeSpan GetImplicitWait()
        {
            TimeSpan res = default(TimeSpan);
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await timeouts.GetAsynchronousJavaScript();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }
        public TimeSpan GetPageLoad()
        {
            TimeSpan res = default(TimeSpan);
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await timeouts.GetAsynchronousJavaScript();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }
        public void SetAsynchronousJavaScript(TimeSpan time)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await timeouts.SetAsynchronousJavaScript(time);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
        public void SetImplicitWait(TimeSpan implicitWait)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await timeouts.SetImplicitWait(implicitWait);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
        public void SetPageLoad(TimeSpan time)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await timeouts.SetPageLoad(time);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }

    }
}