// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver.Remote
{
    public class SyncRemoteTargetLocator 
    {
        private readonly RemoteTargetLocator locator;

        public SyncRemoteTargetLocator(RemoteTargetLocator locator)
        {
            this.locator = locator;
        }

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

        public Task<IAlert> Alert(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<IWebDriver> DefaultContent(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
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

        public Task<IWebDriver> Frame(string frameName, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<IWebDriver> Frame(IWebElement frameElement,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<IWebDriver> ParentFrame(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<IWebDriver> Window(string windowName, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }
    }
}