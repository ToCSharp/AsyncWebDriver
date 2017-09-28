// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MyCommunicationLib.Communication.MarionetteComands;
using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Firefox
{
    internal class FirefoxDriverScreenshot: ITakesScreenshot
    {
        private IAsyncFirefoxDriver asyncFirefoxDriver;

        public FirefoxDriverScreenshot(IAsyncFirefoxDriver asyncFirefoxDriver)
        {
            this.asyncFirefoxDriver = asyncFirefoxDriver;
        }

        //TODO
        public Task<Screenshot> GetScreenshot(CancellationToken cancellationToken = default(CancellationToken))
        {
            return TakeScreenshot(null, null, null, null, cancellationToken);
        }

        public async Task<Screenshot> TakeScreenshot(string elementId, string highlights, string full, string hash,
    CancellationToken cancellationToken = new CancellationToken())
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new TakeScreenshotCommand(elementId, highlights, full, hash);
            await asyncFirefoxDriver.ClientMarionette.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            throw new NotImplementedException(nameof(TakeScreenshot));
        }


    }
}