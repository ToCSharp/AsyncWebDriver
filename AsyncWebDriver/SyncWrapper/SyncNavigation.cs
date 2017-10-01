// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver.Remote
{
    public class SyncNavigation
    {
        private INavigation navigation;

        public SyncNavigation(INavigation navigation)
        {
            this.navigation = navigation;
        }

        public void Back()
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await navigation.Back();
                    await Task.Delay(50);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }

        public void GoToUrl(string url)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await navigation.GoToUrl(url);
                    //todo: remove delay
                    await Task.Delay(200);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }

        public void GoToUrl(Uri url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await navigation.GoToUrl(url);
                    await Task.Delay(50);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }

        public void Forward()
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await navigation.Forward();
                    await Task.Delay(50);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }

        public void Refresh()
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await navigation.Refresh();
                    await Task.Delay(50);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
    }
}