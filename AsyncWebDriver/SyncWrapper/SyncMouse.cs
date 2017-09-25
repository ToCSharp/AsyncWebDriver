// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
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
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await mouse.Click(where);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
        public void ContextClick(ICoordinates where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await mouse.ContextClick(where);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
        public void DoubleClick(ICoordinates where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await mouse.DoubleClick(where);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
        public void MouseDown(ICoordinates where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await mouse.MouseDown(where);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
        public void MouseMove(ICoordinates where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await mouse.MouseMove(where);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
        public void MouseUp(ICoordinates where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await mouse.MouseUp(where);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }

        public void Click(WebPoint where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await mouse.Click(where);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
        public void ContextClick(WebPoint where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await mouse.ContextClick(where);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
        public void DoubleClick(WebPoint where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await mouse.DoubleClick(where);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
        public void MouseDown(WebPoint where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await mouse.MouseDown(where);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
        public void MouseMove(WebPoint where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await mouse.MouseMove(where);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
        public void MouseUp(WebPoint where)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await mouse.MouseUp(where);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
    }
}