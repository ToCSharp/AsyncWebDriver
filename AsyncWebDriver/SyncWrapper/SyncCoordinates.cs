// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncWebDriver.Remote
{
    public class SyncCoordinates 
    {
        private ICoordinates coordinates;

        public SyncCoordinates(ICoordinates coordinates)
        {
            this.coordinates = coordinates;
        }

        public WebPoint LocationOnScreen()
        {
            WebPoint res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () => 
            {
                res = await coordinates.LocationOnScreen();
                MRes.Set();
            });
            MRes.Wait();
            return res;
        }

        public WebPoint LocationInViewport()
        {
            WebPoint res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () =>
            {
                res = await coordinates.LocationInViewport();
                MRes.Set();
            });
            MRes.Wait();
            return res;
        }

        public WebPoint LocationInDom()
        {
            WebPoint res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Task.Run(async () =>
            {
                res = await coordinates.LocationInDom();
                MRes.Set();
            });
            MRes.Wait();
            return res;
        }

        public string AuxiliaryLocator => coordinates.AuxiliaryLocator;

    }
}