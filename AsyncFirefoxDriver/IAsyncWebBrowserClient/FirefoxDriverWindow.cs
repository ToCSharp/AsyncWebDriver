// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MyCommunicationLib.Communication.MarionetteComands;
using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.BasicTypes;
using Zu.WebBrowser.BrowserOptions;

namespace Zu.Firefox
{
    internal class FirefoxDriverWindow : IWindow
    {
        private IAsyncFirefoxDriver asyncFirefoxDriver;

        public FirefoxDriverWindow(IAsyncFirefoxDriver asyncFirefoxDriver)
        {
            this.asyncFirefoxDriver = asyncFirefoxDriver;
        }

        public async Task<WebPoint> GetPosition(CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new GetWindowPositionCommand();
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return ResultValueConverter.ToWebPoint(comm1.Result);
        }

        public async Task SetPosition(WebPoint pos, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new SetWindowPositionCommand(pos.X, pos.Y);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
        }

        public async Task<WebSize> GetSize(CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new GetWindowSizeCommand();
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return ResultValueConverter.ToWebSize(comm1.Result);
        }

        public async Task SetSize(WebSize size, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new SetWindowSizeCommand(size.Width, size.Height);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
        }

        public async Task Maximize(CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new MaximizeWindowCommand();
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
        }
    }
}