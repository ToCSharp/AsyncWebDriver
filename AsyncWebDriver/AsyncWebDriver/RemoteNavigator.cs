// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver.Remote
{
    public class RemoteNavigator : INavigation
    {
        private readonly WebDriver driver;

        public RemoteNavigator(WebDriver driver)
        {
            this.driver = driver;
        }

        public async Task Back(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            var res = await driver.browserClient.GoBack(cancellationToken);
        }

        public async Task Forward(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            var res = await driver.browserClient.GoForward(cancellationToken);
        }

        public async Task GoToUrl(string url, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            var res = await driver.browserClient.GetUrl(url, cancellationToken);
        }

        public async Task GoToUrl(Uri url, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            var res = await driver.browserClient.GetUrl(url.ToString(), cancellationToken);
        }

        public async Task Refresh(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            var res = await driver.browserClient.Refresh(cancellationToken);
        }
    }
}