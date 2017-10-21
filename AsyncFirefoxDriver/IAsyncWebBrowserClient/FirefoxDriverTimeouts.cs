// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using MyCommunicationLib.Communication.MarionetteComands;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.BasicTypes;
using Zu.WebBrowser.BrowserOptions;

namespace Zu.Firefox
{
    internal class FirefoxDriverTimeouts : ITimeouts
    {
        private IAsyncFirefoxDriver asyncFirefoxDriver;
        public FirefoxDriverTimeouts(IAsyncFirefoxDriver asyncFirefoxDriver)
        {
            this.asyncFirefoxDriver = asyncFirefoxDriver;
        }

        public Task<TimeSpan> GetAsynchronousJavaScript(CancellationToken cancellationToken = default (CancellationToken))
        {
            return GetTimeout(SetTimeoutsCommand.GetTimeoutTypeStr(SetTimeoutsCommand.TimeoutType.script), cancellationToken);
        }

        public Task<TimeSpan> GetImplicitWait(CancellationToken cancellationToken = default (CancellationToken))
        {
            return GetTimeout(SetTimeoutsCommand.GetTimeoutTypeStr(SetTimeoutsCommand.TimeoutType.implicitWait), cancellationToken);
        }

        public Task<TimeSpan> GetPageLoad(CancellationToken cancellationToken = default (CancellationToken))
        {
            return GetTimeout(SetTimeoutsCommand.GetTimeoutTypeStr(SetTimeoutsCommand.TimeoutType.page_load), cancellationToken);
        }

        private async Task<TimeSpan> GetTimeout(string timeoutType, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            if (asyncFirefoxDriver.ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new GetTimeoutsCommand();
            await asyncFirefoxDriver.ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
            var res = comm1.Result?[timeoutType]?.ToString();
            if (res == null)
                throw new WebBrowserException("Specified timeout type not defined");
            return TimeSpan.FromMilliseconds(Convert.ToDouble(res, CultureInfo.InvariantCulture));
        }

        public async Task SetAsynchronousJavaScript(TimeSpan time, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            if (asyncFirefoxDriver.ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new SetTimeoutsCommand(SetTimeoutsCommand.TimeoutType.script, (int)time.TotalMilliseconds);
            await asyncFirefoxDriver.ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
        }

        public async Task SetImplicitWait(TimeSpan implicitWait, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            if (asyncFirefoxDriver.ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new SetTimeoutsCommand(SetTimeoutsCommand.TimeoutType.implicitWait, (int)implicitWait.TotalMilliseconds);
            await asyncFirefoxDriver.ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
        }

        public async Task SetPageLoad(TimeSpan time, CancellationToken cancellationToken = default (CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken).ConfigureAwait(false);
            if (asyncFirefoxDriver.ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new SetTimeoutsCommand(SetTimeoutsCommand.TimeoutType.page_load, (int)time.TotalMilliseconds);
            await asyncFirefoxDriver.ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
        }
    }
}