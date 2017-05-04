// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Drawing;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Interactions.Internal;

namespace Zu.AsyncWebDriver.Remote
{
    public class RemoteCoordinates : ICoordinates
    {
        private AsyncWebElement remoteWebElement;

        public RemoteCoordinates(AsyncWebElement remoteWebElement)
        {
            this.remoteWebElement = remoteWebElement;
        }

        public Task<Point> LocationOnScreen => throw new NotImplementedException();

        public Task<Point> LocationInViewport => throw new NotImplementedException();

        public Task<Point> LocationInDom => throw new NotImplementedException();

        public Task<object> AuxiliaryLocator => throw new NotImplementedException();
    }
}