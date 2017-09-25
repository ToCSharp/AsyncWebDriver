// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

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
                    var r = await locator.ActiveElement();
                    if (r is AsyncWebElement) res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex) { exception = ex; }
                mRes.Set();
            });
            mRes.Wait();
            if (exception != null) throw exception;
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
                    var r = await locator.Alert();
                    res = new SyncAlert(r);
                }
                catch (Exception ex) { exception = ex; }
                mRes.Set();
            });
            mRes.Wait();
            if (exception != null) throw exception;
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
                    var r = await locator.DefaultContent();
                    if (r is WebDriver) res = new SyncWebDriver(r as WebDriver);
                }
                catch (Exception ex) { exception = ex; }
                mRes.Set();
            });
            mRes.Wait();
            if (exception != null) throw exception;
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
                    var r = await locator.Frame(frameIndex);
                    if (r is WebDriver) res = new SyncWebDriver(r as WebDriver);
                }
                catch (Exception ex) { exception = ex; }
                mRes.Set();
            });
            mRes.Wait();
            if (exception != null) throw exception;
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
                    var r = await locator.Frame(frameName);
                    if (r is WebDriver) res = new SyncWebDriver(r as WebDriver);
                }
                catch (Exception ex) { exception = ex; }
                mRes.Set();
            });
            mRes.Wait();
            if (exception != null) throw exception;
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
                    var r = await locator.Frame(frameElement.AsyncElement);
                    if (r is WebDriver) res = new SyncWebDriver(r as WebDriver);
                }
                catch (Exception ex) { exception = ex; }
                mRes.Set();
            });
            mRes.Wait();
            if (exception != null) throw exception;
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
                    var r = await locator.ParentFrame();
                    if (r is WebDriver) res = new SyncWebDriver(r as WebDriver);
                }
                catch (Exception ex) { exception = ex; }
                mRes.Set();
            });
            mRes.Wait();
            if (exception != null) throw exception;
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
                    var r = await locator.Window(windowName);
                    if (r is WebDriver) res = new SyncWebDriver(r as WebDriver);
                }
                catch (Exception ex) { exception = ex; }
                mRes.Set();
            });
            mRes.Wait();
            if (exception != null) throw exception;
            return res;
        }
    }
}