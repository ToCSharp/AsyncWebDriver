// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MyCommunicationLib.Communication.MarionetteComands;
using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.BrowserOptions;

namespace Zu.Firefox
{
    internal class FirefoxDriverTimeouts: ITimeouts
    {
        private IAsyncFirefoxDriver asyncFirefoxDriver;

        public FirefoxDriverTimeouts(IAsyncFirefoxDriver asyncFirefoxDriver)
        {
            this.asyncFirefoxDriver = asyncFirefoxDriver;
        }

        public Task<TimeSpan> GetAsynchronousJavaScript(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<TimeSpan> GetImplicitWait(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<TimeSpan> GetPageLoad(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task SetAsynchronousJavaScript(TimeSpan time, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new SetTimeoutsCommand(SetTimeoutsCommand.TimeoutType.script, (int)time.TotalMilliseconds);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new Exception(comm1.Error.ToString());
        }

        public async Task SetImplicitWait(TimeSpan implicitWait, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new SetTimeoutsCommand(SetTimeoutsCommand.TimeoutType.@implicit, (int)implicitWait.TotalMilliseconds);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new Exception(comm1.Error.ToString());
        }

        public async Task SetPageLoad(TimeSpan time, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new SetTimeoutsCommand(SetTimeoutsCommand.TimeoutType.page_load, (int)time.TotalMilliseconds);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new Exception(comm1.Error.ToString());
        }
    }
}