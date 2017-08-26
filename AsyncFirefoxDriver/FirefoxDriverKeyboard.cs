// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.Firefox
{
    internal class FirefoxDriverKeyboard : IKeyboard
    {
        private AsyncFirefoxDriver asyncFirefoxDriver;

        public FirefoxDriverKeyboard(AsyncFirefoxDriver asyncFirefoxDriver)
        {
            this.asyncFirefoxDriver = asyncFirefoxDriver;
        }

        public async Task PressKey(string keyToPress, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            throw new NotImplementedException();
        }

        public async Task ReleaseKey(string keyToRelease, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            throw new NotImplementedException();
        }

        public async Task SendKeys(string keySequence, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            throw new NotImplementedException();
        }
    }
}