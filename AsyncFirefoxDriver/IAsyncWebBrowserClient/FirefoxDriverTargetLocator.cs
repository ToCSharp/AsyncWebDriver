// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MyCommunicationLib.Communication.MarionetteComands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Firefox
{
    internal class FirefoxDriverTargetLocator : ITargetLocator
    {
        private IAsyncFirefoxDriver asyncFirefoxDriver;

        public FirefoxDriverTargetLocator(IAsyncFirefoxDriver asyncFirefoxDriver)
        {
            this.asyncFirefoxDriver = asyncFirefoxDriver;
        }

        public async Task<string> GetWindowHandle(CancellationToken cancellationToken)
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new GetWindowHandleCommand();
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<List<string>> GetWindowHandles(CancellationToken cancellationToken)
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new GetWindowHandlesCommand();
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return ResultValueConverter.WindowHandles(comm1.Result);
        }

        public Task<string> SwitchToActiveElement(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task<IAlert> SwitchToAlert(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public Task SwitchToDefaultContent(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public async Task SwitchToFrame(int frameIndex, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new SwitchToFrameCommand(frameIndex);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
        }

        public Task SwitchToFrame(string frameName, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SwitchToFrame(frameName, null, true);
        }

        public async Task SwitchToFrame(string frameName, string element = null, bool doFocus = true, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (frameName == null)
            {
                if (element == null)
                {
                    await asyncFirefoxDriver.CheckConnected(cancellationToken);
                    if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
                    var comm1 = new SwitchToFrameCommand(frameName, element, doFocus);
                    await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
                    if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
                }
                else
                {
                    await asyncFirefoxDriver.CheckConnected(cancellationToken);
                    if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
                    var comm1 = new SwitchToFrameCommand(frameName, element, doFocus);
                    await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
                    if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
                }
            }
            else
            {
                string name = /*frameName; //*/ Regex.Replace(frameName, @"(['""\\#.:;,!?+<>=~*^$|%&@`{}\-/\[\]\(\)])", @"\$1");
                var json = await asyncFirefoxDriver.Elements.FindElements("css selector", "frame[name='" + name + "'],iframe[name='" + name + "']");
                var frameElements = FirefoxDriverElements.GetElementsFromResponse(json);
                if (frameElements.Count == 0)
                {
                    json = await asyncFirefoxDriver.Elements.FindElements("css selector", "frame#" + name + ",iframe#" + name);
                    frameElements = FirefoxDriverElements.GetElementsFromResponse(json);
                    if (frameElements.Count == 0)
                    {
                        var e = new WebBrowserException("No frame element found with name or id " + frameName);
                        e.Error = "No frame element found with name or id";
                        throw e; // NoSuchFrameException("No frame element found with name or id " + frameName);
                    }
                }

                await SwitchToFrame(null, frameElements[0], doFocus, cancellationToken);
            }
        }


        public Task SwitchToFrameByElement(string element, CancellationToken cancellationToken = default(CancellationToken))
        {
            return SwitchToFrame(null, element, true);
        }

        public async Task SwitchToParentFrame(CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new SwitchToParentFrameCommand();
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
        }

        public async Task SwitchToWindow(string windowName, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new SwitchToWindowCommand(windowName);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
        }
    }
}