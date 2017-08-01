// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Drawing;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Interactions.Internal;
using Zu.AsyncWebDriver.Internal;
using Zu.WebBrowser.BasicTypes;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver.Remote
{
    public class RemoteCoordinates : ICoordinates
    {
        private AsyncWebElement remoteWebElement;

        public RemoteCoordinates(AsyncWebElement remoteWebElement)
        {
            this.remoteWebElement = remoteWebElement;
        }

        public Task<WebPoint> LocationOnScreen => throw new NotImplementedException();

        public Task<WebPoint> LocationInViewport => remoteWebElement.LocationOnScreenOnceScrolledIntoView();

        public Task<WebPoint> LocationInDom => remoteWebElement.Location();

        public string AuxiliaryLocator
        {
            get
            {
                IWebElementReference elementReference = remoteWebElement as IWebElementReference;
                if (elementReference == null)
                {
                    return null;
                }

                // Note that the OSS dialect of the wire protocol for the Actions API
                // uses the raw ID of the element, not an element reference. To use this,
                // extract the ID using the well-known key to the dictionary for element
                // references.
                return elementReference.ElementReferenceId;
            }
        }
    }
}