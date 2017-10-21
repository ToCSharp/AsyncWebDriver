// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver.Remote
{
    public class SyncKeyboard
    {
        private IKeyboard keyboard;
        public SyncKeyboard(IKeyboard keyboard)
        {
            this.keyboard = keyboard;
        }

        public void PressKey(string keyToPress)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await keyboard.PressKey(keyToPress).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                MRes.Set();
            }

            );
            MRes.Wait();
            if (exception != null)
                throw exception;
        }

        public void SendKeys(string keySequence)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await keyboard.SendKeys(keySequence).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                MRes.Set();
            }

            );
            MRes.Wait();
            if (exception != null)
                throw exception;
        }

        public void ReleaseKey(string keyToRelease)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await keyboard.ReleaseKey(keyToRelease).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                MRes.Set();
            }

            );
            MRes.Wait();
            if (exception != null)
                throw exception;
        }
    }
}