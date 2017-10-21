// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using MyCommunicationLib.Communication.MarionetteComands;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Firefox
{
    public class FirefoxDriverAlert : IAlert
    {
        private IAsyncFirefoxDriver asyncFirefoxDriver;
        public FirefoxDriverAlert(IAsyncFirefoxDriver asyncFirefoxDriver)
        {
            this.asyncFirefoxDriver = asyncFirefoxDriver;
        }

        public async Task<string> Text(CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            if (asyncFirefoxDriver.ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new GetTextFromDialogCommand();
            await asyncFirefoxDriver.ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task Accept(CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            if (asyncFirefoxDriver.ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new AcceptDialogCommand();
            await asyncFirefoxDriver.ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
        }

        public async Task Dismiss(CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            if (asyncFirefoxDriver.ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new DismissDialogCommand();
            await asyncFirefoxDriver.ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
        }

        public async Task SendKeys(string keysToSend, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            if (asyncFirefoxDriver.ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new SendKeysToDialogCommand(keysToSend);
            await asyncFirefoxDriver.ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
        }

        public Task SetAuthenticationCredentials(string userName, string password, CancellationToken cancellationToken = default (CancellationToken))
        {
            throw new System.NotImplementedException();
        }
    }
}