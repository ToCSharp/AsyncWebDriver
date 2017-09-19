// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Firefox
{
    public class FirefoxDriverCoordinates: ICoordinates
    {
        private IAsyncFirefoxDriver asyncFirefoxDriver;

        public FirefoxDriverCoordinates(IAsyncFirefoxDriver asyncFirefoxDriver)
        {
            this.asyncFirefoxDriver = asyncFirefoxDriver;
        }

        public Task<WebPoint> LocationOnScreen => throw new System.NotImplementedException();

        public Task<WebPoint> LocationInViewport => throw new System.NotImplementedException();

        public Task<WebPoint> LocationInDom => throw new System.NotImplementedException();

        public string AuxiliaryLocator => throw new System.NotImplementedException();
    }
}