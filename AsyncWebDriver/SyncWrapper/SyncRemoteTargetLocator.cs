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
            Task.Run(async () =>
            {
                var r = await locator.ActiveElement();
                if (r is AsyncWebElement) res = new SyncWebElement(r as AsyncWebElement);
                mRes.Set();
            });
            mRes.Wait();
            return res;
        }

        public SyncAlert Alert()
        {
            SyncAlert res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Task.Run(async () =>
            {
                var r =  await locator.Alert();
                res = new SyncAlert(r);
                mRes.Set();
            });
            mRes.Wait();
            return res;
        }

        public SyncWebDriver DefaultContent()
        {
            SyncWebDriver res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Task.Run(async () =>
            {
                var r = await locator.DefaultContent();
                if (r is WebDriver) res = new SyncWebDriver(r as WebDriver);
                mRes.Set();
            });
            mRes.Wait();
            return res;
        }

        public SyncWebDriver Frame(int frameIndex)
        {
            SyncWebDriver res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Task.Run(async () =>
            {
                var r = await locator.Frame(frameIndex);
                if (r is WebDriver) res = new SyncWebDriver(r as WebDriver);
                mRes.Set();
            });
            mRes.Wait();
            return res;
        }

        public SyncWebDriver Frame(string frameName)
        {
            SyncWebDriver res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Task.Run(async () =>
            {
                var r = await locator.Frame(frameName);
                if (r is WebDriver) res = new SyncWebDriver(r as WebDriver);
                mRes.Set();
            });
            mRes.Wait();
            return res;
        }

        public SyncWebDriver Frame(IWebElement frameElement)
        {
            SyncWebDriver res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Task.Run(async () =>
            {
                var r = await locator.Frame(frameElement);
                if (r is WebDriver) res = new SyncWebDriver(r as WebDriver);
                mRes.Set();
            });
            mRes.Wait();
            return res;
        }

        public SyncWebDriver ParentFrame()
        {
            SyncWebDriver res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Task.Run(async () =>
            {
                var r = await locator.ParentFrame();
                if (r is WebDriver) res = new SyncWebDriver(r as WebDriver);
                mRes.Set();
            });
            mRes.Wait();
            return res;
        }

        public SyncWebDriver Window(string windowName)
        {
            SyncWebDriver res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Task.Run(async () =>
            {
                var r = await locator.Window(windowName);
                if (r is WebDriver) res = new SyncWebDriver(r as WebDriver);
                mRes.Set();
            });
            mRes.Wait();
            return res;
        }
    }
}