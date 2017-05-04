// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver.Remote
{
    public class RemoteTargetLocator : ITargetLocator
    {
        private readonly WebDriver driver;

        public RemoteTargetLocator(WebDriver driver)
        {
            this.driver = driver;
        }

        public async Task<IWebElement> ActiveElement(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver?.browserClient == null)
                throw new WebDriverException("no browserClient");

            var res = await driver.browserClient.GetActiveElement(cancellationToken);
            return driver.GetElementFromResponse(res);
        }

        public Task<IAlert> Alert(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<IWebDriver> DefaultContent(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<IWebDriver> Frame(int frameIndex,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver?.browserClient == null)
                throw new WebDriverException("no browserClient");

            var res =
                await driver.browserClient.SwitchToFrame(frameIndex.ToString(), cancellationToken: cancellationToken);
            return driver;
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