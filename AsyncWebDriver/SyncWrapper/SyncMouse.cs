// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncWebDriver.Remote
{
    public class SyncMouse
    {
        private IMouse mouse;

        public SyncMouse(IMouse mouse)
        {
            this.mouse = mouse;
        }

        public void Click(ICoordinates where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () => { await mouse.Click(where); MRes.Set(); });
            MRes.Wait();
        }
        public void ContextClick(ICoordinates where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () => { await mouse.ContextClick(where); MRes.Set(); });
            MRes.Wait();
        }
        public void DoubleClick(ICoordinates where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () => { await mouse.DoubleClick(where); MRes.Set(); });
            MRes.Wait();
        }
        public void MouseDown(ICoordinates where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () => { await mouse.MouseDown(where); MRes.Set(); });
            MRes.Wait();
        }
        public void MouseMove(ICoordinates where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () => { await mouse.MouseMove(where); MRes.Set(); });
            MRes.Wait();
        }
        public void MouseUp(ICoordinates where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () => { await mouse.MouseUp(where); MRes.Set(); });
            MRes.Wait();
        }

        public void Click(WebPoint where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () => { await mouse.Click(where); MRes.Set(); });
            MRes.Wait();
        }
        public void ContextClick(WebPoint where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () => { await mouse.ContextClick(where); MRes.Set(); });
            MRes.Wait();
        }
        public void DoubleClick(WebPoint where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () => { await mouse.DoubleClick(where); MRes.Set(); });
            MRes.Wait();
        }
        public void MouseDown(WebPoint where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () => { await mouse.MouseDown(where); MRes.Set(); });
            MRes.Wait();
        }
        public void MouseMove(WebPoint where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () => { await mouse.MouseMove(where); MRes.Set(); });
            MRes.Wait();
        }
        public void MouseUp(WebPoint where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () => { await mouse.MouseUp(where); MRes.Set(); });
            MRes.Wait();
        }
    }
}