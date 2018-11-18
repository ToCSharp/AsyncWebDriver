// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver.Remote
{
    public class SyncRemoteTargetLocator
    {
        private readonly RemoteTargetLocator locator;
        //private ITargetLocator targetLocator;
        public SyncRemoteTargetLocator(RemoteTargetLocator locator)
        {
            this.locator = locator;
        }

        //public SyncRemoteTargetLocator(ITargetLocator targetLocator)
        //{
        //    this.targetLocator = targetLocator;
        //}
        public SyncWebElement ActiveElement()
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await locator.ActiveElement().ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncAlert Alert()
        {
            SyncAlert res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await locator.Alert().ConfigureAwait(false);
                    res = new SyncAlert(r);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebDriver DefaultContent()
        {
            SyncWebDriver res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await locator.DefaultContent().ConfigureAwait(false);
                    if (r is WebDriver)
                        res = new SyncWebDriver(r as WebDriver);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebDriver Frame(int frameIndex)
        {
            SyncWebDriver res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await locator.Frame(frameIndex).ConfigureAwait(false);
                    if (r is WebDriver)
                        res = new SyncWebDriver(r as WebDriver);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebDriver Frame(string frameName)
        {
            SyncWebDriver res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await locator.Frame(frameName).ConfigureAwait(false);
                    if (r is WebDriver)
                        res = new SyncWebDriver(r as WebDriver);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebDriver Frame(SyncWebElement frameElement)
        {
            SyncWebDriver res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await locator.Frame(frameElement.AsyncElement).ConfigureAwait(false);
                    if (r is WebDriver)
                        res = new SyncWebDriver(r as WebDriver);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebDriver ParentFrame()
        {
            SyncWebDriver res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await locator.ParentFrame().ConfigureAwait(false);
                    if (r is WebDriver)
                        res = new SyncWebDriver(r as WebDriver);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebDriver Window(string windowName)
        {
            SyncWebDriver res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await locator.Window(windowName).ConfigureAwait(false);
                    if (r is WebDriver)
                        res = new SyncWebDriver(r as WebDriver);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }
    }
}