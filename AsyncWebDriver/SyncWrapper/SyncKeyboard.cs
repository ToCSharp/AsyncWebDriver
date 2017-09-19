// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
            Task.Run(async () => { await keyboard.PressKey(keyToPress);  MRes.Set(); });
            MRes.Wait();
        }
        public void SendKeys(string keySequence)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () => { await keyboard.SendKeys(keySequence); MRes.Set(); });
            MRes.Wait();
        }
        public void ReleaseKey(string keyToRelease)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () => { await keyboard.ReleaseKey(keyToRelease); MRes.Set(); });
            MRes.Wait();
        }
    }
}