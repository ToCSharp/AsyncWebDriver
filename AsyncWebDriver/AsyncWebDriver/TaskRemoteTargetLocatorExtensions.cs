// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.
using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver.Remote
{
    public static class TaskRemoteTargetLocatorExtensions
    {
        public static async Task<IWebElement> ActiveElement(this Task<RemoteTargetLocator> task, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await task;
            return await el.ActiveElement(cancellationToken);
        }

        public static async Task<IAlert> Alert(this Task<RemoteTargetLocator> task, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await task;
            return await el.Alert(cancellationToken);
        }

        public static async Task<IWebDriver> DefaultContent(this Task<RemoteTargetLocator> task, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await task;
            return await el.DefaultContent(cancellationToken);
        }

        public static async Task<IWebDriver> Frame(this Task<RemoteTargetLocator> task, int frameIndex, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await task;
            return await el.Frame(frameIndex, cancellationToken);
        }

        public static async Task<IWebDriver> Frame(this Task<RemoteTargetLocator> task, string frameName, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await task;
            return await el.Frame(frameName, cancellationToken);
        }

        public static async Task<IWebDriver> Frame(this Task<RemoteTargetLocator> task, IWebElement frameElement, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await task;
            return await el.Frame(frameElement, cancellationToken);
        }

        public static async Task<IWebDriver> ParentFrame(this Task<RemoteTargetLocator> task, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await task;
            return await el.ParentFrame(cancellationToken);
        }

        public static async Task<IWebDriver> Window(this Task<RemoteTargetLocator> task, string windowName, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await task;
            return await el.Window(windowName, cancellationToken);
        }
    }
}