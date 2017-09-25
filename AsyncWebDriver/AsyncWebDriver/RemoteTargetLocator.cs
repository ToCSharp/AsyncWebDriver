// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver.Remote
{
    public class RemoteTargetLocator : IWebDriverTargetLocator
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

            var res = await driver.browserClient.Elements.GetActiveElement(cancellationToken);
            return driver.GetElementFromResponse(res);
        }

        public Task<IAlert> Alert(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<IWebDriver> DefaultContent(CancellationToken cancellationToken = new CancellationToken())
        {
            await driver.browserClient.TargetLocator.SwitchToFrame(null);
            return driver;
        }

        public async Task<IWebDriver> Frame(int frameIndex,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver?.browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                await driver.browserClient.TargetLocator.SwitchToFrame(frameIndex, cancellationToken: cancellationToken);
            }
            catch { throw; }
            return driver;
        }

        public async Task<IWebDriver> Frame(string frameName, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver?.browserClient == null)
                throw new WebDriverException("no browserClient");
            try
            {
                await driver.browserClient.TargetLocator.SwitchToFrame(frameName, cancellationToken: cancellationToken);
            }
            catch { throw; }
            return driver;
        }

        public async Task<IWebDriver> Frame(IWebElement frameElement,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (frameElement == null)
            {
                throw new ArgumentNullException("frameElement", "Frame element cannot be null");
            }
            if (driver?.browserClient == null)
                throw new WebDriverException("no browserClient");
            try
            {
                await driver.browserClient.TargetLocator.SwitchToFrameByElement(frameElement.Id, cancellationToken: cancellationToken);
            }
            catch { throw; }
            return driver;
        }

        public async Task<IWebDriver> ParentFrame(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver?.browserClient == null)
                throw new WebDriverException("no browserClient");

            await driver.browserClient.TargetLocator.SwitchToParentFrame(cancellationToken);
            return driver;
        }

        public async Task<IWebDriver> Window(string windowName, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver?.browserClient == null)
                throw new WebDriverException("no browserClient");

            await driver.browserClient.TargetLocator.SwitchToWindow(windowName, cancellationToken);
            return driver;
        }
    }
}