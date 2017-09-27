// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MyCommunicationLib.Communication.MarionetteComands;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.Firefox
{
    internal class FirefoxDriverNavigation: INavigation
    {
        private IAsyncFirefoxDriver asyncFirefoxDriver;

        public FirefoxDriverNavigation(IAsyncFirefoxDriver asyncFirefoxDriver)
        {
            this.asyncFirefoxDriver = asyncFirefoxDriver;
        }

        public async Task Back(CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new GoBackCommand();
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new Exception(comm1.Error.ToString());
        }

        public async Task Forward(CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new GoForwardCommand();
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new Exception(comm1.Error.ToString());
        }

        public async Task<string> GetUrl(CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new GetCurrentUrlCommand();
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new Exception(comm1.Error.ToString());
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task GoToUrl(string url, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                await asyncFirefoxDriver.CheckConnected(cancellationToken);
                await asyncFirefoxDriver.SetContextContent();
                if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
                var comm1 = new GetCommand(url);
                await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
                if (comm1.Error != null) throw new Exception(comm1.Error.ToString());
            }
            catch { throw; }
        }

        public Task GoToUrl(Uri url, CancellationToken cancellationToken = default(CancellationToken))
        {
            return GoToUrl(url.ToString(), cancellationToken);
        }

        public async Task Refresh(CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new RefreshCommand();
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new Exception(comm1.Error.ToString());
        }
    }
}