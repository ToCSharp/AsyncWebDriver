// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using MyCommunicationLib.Communication.MarionetteComands;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Interactions;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Firefox
{
    internal class FirefoxDriverActionExecutor : IActionExecutor
    {
        private AsyncFirefoxDriver asyncFirefoxDriver;
        private CancellationTokenSource performActionsCancellationTokenSource;
        public FirefoxDriverActionExecutor(AsyncFirefoxDriver asyncFirefoxDriver)
        {
            this.asyncFirefoxDriver = asyncFirefoxDriver;
        }

        public Task<bool> IsActionExecutor(CancellationToken cancellationToken = default (CancellationToken))
        {
            return Task.FromResult(true);
        }

        public async Task PerformActions(IList<ActionSequence> actionSequenceList, CancellationToken cancellationToken = default (CancellationToken))
        {
            performActionsCancellationTokenSource = new CancellationTokenSource();
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            if (asyncFirefoxDriver.ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            await asyncFirefoxDriver.SetContextContent().ConfigureAwait(false);
            List<object> objectList = new List<object>();
            foreach (ActionSequence sequence in actionSequenceList)
            {
                objectList.Add(sequence.ToDictionary());
            }

            var comm1 = new PerformActionsCommand(objectList);
            await asyncFirefoxDriver.ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
        }

        public async Task ResetInputState(CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            if (asyncFirefoxDriver.ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            await asyncFirefoxDriver.SetContextContent().ConfigureAwait(false);
            var comm1 = new ReleaseActionsCommand();
            await asyncFirefoxDriver.ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
        }
    }
}