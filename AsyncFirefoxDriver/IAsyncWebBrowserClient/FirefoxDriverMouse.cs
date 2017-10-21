// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Firefox
{
    public class FirefoxDriverMouse : IMouse
    {
        private IAsyncFirefoxDriver asyncFirefoxDriver;
        public FirefoxDriverMouse(IAsyncFirefoxDriver asyncFirefoxDriver)
        {
            this.asyncFirefoxDriver = asyncFirefoxDriver;
        }

        public WebPoint MousePosition
        {
            get;
            set;
        }

        public async Task Click(ICoordinates where, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public async Task Click(WebPoint location, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public async Task ContextClick(ICoordinates where, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public async Task ContextClick(WebPoint location, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public async Task DoubleClick(ICoordinates where, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public async Task DoubleClick(WebPoint location, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public async Task MouseDown(ICoordinates where, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public async Task MouseDown(WebPoint location, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public async Task MouseMove(ICoordinates where, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public async Task MouseMove(ICoordinates where, int offsetX, int offsetY, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public async Task MouseMove(WebPoint location, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public async Task MouseUp(ICoordinates where, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException();
        }

        public async Task MouseUp(WebPoint location, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            throw new NotImplementedException();
        }
    }
}