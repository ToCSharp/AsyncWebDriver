// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver
{
    public static class TaskIWebDriverTargetLocatorExtensions
    {
        public static async Task<IWebDriver> Frame(this Task<IWebDriverTargetLocator> elementTask, int frameIndex, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.Frame(frameIndex, cancellationToken);
        }

        public static async Task<IWebDriver> Frame(this Task<IWebDriverTargetLocator> elementTask, string frameName, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.Frame(frameName, cancellationToken);
        }

        public static async Task<IWebDriver> Frame(this Task<IWebDriverTargetLocator> elementTask, IWebElement frameElement, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.Frame(frameElement, cancellationToken);
        }

        public static async Task<IWebDriver> ParentFrame(this Task<IWebDriverTargetLocator> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.ParentFrame(cancellationToken);
        }

        public static async Task<IWebDriver> Window(this Task<IWebDriverTargetLocator> elementTask, string windowName, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.Window(windowName, cancellationToken);
        }

        public static async Task<IWebDriver> DefaultContent(this Task<IWebDriverTargetLocator> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.DefaultContent(cancellationToken);
        }

        public static async Task<IWebElement> ActiveElement(this Task<IWebDriverTargetLocator> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.ActiveElement(cancellationToken);
        }

        public static async Task<IAlert> Alert(this Task<IWebDriverTargetLocator> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.Alert(cancellationToken);
        }
    }
}