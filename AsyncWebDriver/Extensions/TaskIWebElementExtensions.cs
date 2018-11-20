// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncWebDriver
{
    public static class TaskIWebElementExtensions
    {
        public static async Task<string> TagName(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.TagName(cancellationToken).ConfigureAwait(false);
        }

        public static async Task<string> Text(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.Text(cancellationToken).ConfigureAwait(false);
        }

        public static async Task<bool> Enabled(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.Enabled(cancellationToken).ConfigureAwait(false);
        }

        public static async Task<bool> Selected(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.Selected(cancellationToken).ConfigureAwait(false);
        }

        public static async Task<WebPoint> Location(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.Location(cancellationToken).ConfigureAwait(false);
        }

        public static async Task<WebSize> Size(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.Size(cancellationToken).ConfigureAwait(false);
        }

        public static async Task<bool> Displayed(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.Displayed(cancellationToken).ConfigureAwait(false);
        }

        public static async Task<IWebElement> Clear(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            await el.Clear(cancellationToken).ConfigureAwait(false);
            return el;
        }

        public static async Task<IWebElement> SendKeys(this Task<IWebElement> elementTask, string text, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            await el.SendKeys(text, cancellationToken).ConfigureAwait(false);
            return el;
        }

        public static async Task<IWebElement> Submit(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            await el.Submit(cancellationToken).ConfigureAwait(false);
            return el;
        }

        public static async Task<IWebElement> Click(this Task<IWebElement> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            await el.Click(cancellationToken).ConfigureAwait(false);
            return el;
        }

        public static async Task<string> GetAttribute(this Task<IWebElement> elementTask, string attributeName, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.GetAttribute(attributeName, cancellationToken).ConfigureAwait(false);
        }

        public static async Task<string> GetCssValue(this Task<IWebElement> elementTask, string propertyName, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.GetCssValue(propertyName, cancellationToken).ConfigureAwait(false);
        }

        public static async Task<IWebElement> FindElement(this Task<IWebElement> elementTask, By by, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.FindElement(by, cancellationToken).ConfigureAwait(false);
        }

        public static async Task<ReadOnlyCollection<IWebElement>> FindElements(this Task<IWebElement> elementTask, By by, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.FindElements(by, cancellationToken).ConfigureAwait(false);
        }

    }
}