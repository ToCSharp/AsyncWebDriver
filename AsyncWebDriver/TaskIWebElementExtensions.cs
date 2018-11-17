// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncWebDriver
{
    public static class TaskIWebElementExtensions
    {
        public static async Task<string> TagName(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.TagName(cancellationToken);
        }

        public static async Task<string> Text(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.Text(cancellationToken);
        }

        public static async Task<bool> Enabled(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.Enabled(cancellationToken);
        }

        public static async Task<bool> Selected(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.Selected(cancellationToken);
        }

        public static async Task<WebPoint> Location(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.Location(cancellationToken);
        }

        public static async Task<WebSize> Size(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.Size(cancellationToken);
        }

        public static async Task<bool> Displayed(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.Displayed(cancellationToken);
        }

        public static async Task<IWebElement> Clear(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            await el.Clear(cancellationToken);
            return el;
        }

        public static async Task<IWebElement> SendKeys(this Task<IWebElement> elementTask, string text, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            await el.SendKeys(text, cancellationToken);
            return el;
        }

        public static async Task<IWebElement> Submit(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            await el.Submit(cancellationToken);
            return el;
        }

        public static async Task<IWebElement> Click(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            await el.Click(cancellationToken);
            return el;
        }

        public static async Task<string> GetAttribute(this Task<IWebElement> elementTask, string attributeName, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.GetAttribute(attributeName, cancellationToken);
        }

        public static async Task<string> GetCssValue(this Task<IWebElement> elementTask, string propertyName, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.GetCssValue(propertyName, cancellationToken);
        }
    }
}