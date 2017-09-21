// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.BasicTypes;
using Zu.WebBrowser.BrowserOptions;

namespace Zu.AsyncWebDriver.Remote
{
    public class SyncCookieJar
    {
        private ICookieJar cookies;

        public SyncCookieJar(ICookieJar cookies)
        {
            this.cookies = cookies;
        }

        public void AddCookie(Cookie cookie)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () =>
            {
                await cookies.AddCookie(cookie);
                MRes.Set();
            });
            MRes.Wait();
        }
        public ReadOnlyCollection<Cookie> AllCookies()
        {
            ReadOnlyCollection<Cookie> res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () =>
            {
                res = await cookies.AllCookies();
                MRes.Set();
            });
            MRes.Wait();
            return res;

        }
        public void DeleteAllCookies()
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () =>
            {
                await cookies.DeleteAllCookies();
                MRes.Set();
            });
            MRes.Wait();
        }
        public void DeleteCookie(Cookie cookie)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () =>
            {
                await cookies.DeleteCookie(cookie);
                MRes.Set();
            });
            MRes.Wait();
        }
        public void DeleteCookieNamed(string name)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () =>
            {
                await cookies.DeleteCookieNamed(name);
                MRes.Set();
            });
            MRes.Wait();

        }
        public Cookie GetCookieNamed(string name)
        {
            Cookie res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () =>
            {
                res = await cookies.GetCookieNamed(name);
                MRes.Set();
            });
            MRes.Wait();
            return res;
        }

    }
}