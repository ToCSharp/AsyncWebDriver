// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
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
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await cookies.AddCookie(cookie).ConfigureAwait(false);
                    MRes.Set();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
            }

            );
            MRes.Wait();
            if (exception != null)
                throw exception;
        }

        public ReadOnlyCollection<Cookie> AllCookies()
        {
            ReadOnlyCollection<Cookie> res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await cookies.AllCookies().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                MRes.Set();
            }

            );
            MRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public void DeleteAllCookies()
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await cookies.DeleteAllCookies().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                MRes.Set();
            }

            );
            MRes.Wait();
            if (exception != null)
                throw exception;
        }

        public void DeleteCookie(Cookie cookie)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await cookies.DeleteCookie(cookie).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                MRes.Set();
            }

            );
            MRes.Wait();
            if (exception != null)
                throw exception;
        }

        public void DeleteCookieNamed(string name)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await cookies.DeleteCookieNamed(name).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                MRes.Set();
            }

            );
            MRes.Wait();
            if (exception != null)
                throw exception;
        }

        public Cookie GetCookieNamed(string name)
        {
            Cookie res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await cookies.GetCookieNamed(name).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                MRes.Set();
            }

            );
            MRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }
    }
}