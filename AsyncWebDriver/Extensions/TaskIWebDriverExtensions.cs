// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Remote;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BrowserOptions;

namespace Zu.AsyncWebDriver
{
    public static class TaskIWebDriverExtensions
    {
        public static async Task<string> GetUrl(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.GetUrl(cancellationToken).ConfigureAwait(false);
        }
        public static async Task<string> GoToUrl(this Task<IWebDriver> elementTask, string url, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.GoToUrl(url, cancellationToken).ConfigureAwait(false);
        }

        public static async Task<string> Title(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.Title(cancellationToken).ConfigureAwait(false);
        }

        public static async Task<string> PageSource(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.PageSource(cancellationToken).ConfigureAwait(false);
        }

        public static async Task<string> CurrentWindowHandle(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.CurrentWindowHandle(cancellationToken).ConfigureAwait(false);
        }

        public static async Task<List<string>> WindowHandles(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.WindowHandles(cancellationToken).ConfigureAwait(false);
        }

        public static async Task<IWebDriver> Open(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            await el.Open(cancellationToken).ConfigureAwait(false);
            return el;
        }

        public static async Task<IWebDriver> Close(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            await el.Close(cancellationToken).ConfigureAwait(false);
            return el;
        }

        public static async Task<IWebDriver> Quit(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            await el.Quit(cancellationToken).ConfigureAwait(false);
            return el;
        }

        //public static async Task<IWebElement> WaitForElementWithId(this Task<IWebDriver> elementTask, string id, string notWebElementId = null, int attemptsCount = 20, int delayMs = 500,
        //    CancellationToken cancellationToken = new CancellationToken())
        //{
        //    var el = await elementTask.ConfigureAwait(false);
        //    return await el.WaitForElementWithId(id, notWebElementId, attemptsCount, delayMs, cancellationToken).ConfigureAwait(false);
        //}

        //public static async Task<IWebElement> WaitForElementWithName(this Task<IWebDriver> elementTask, string name, string notWebElementId = null, int attemptsCount = 20, int delayMs = 500,
        //    CancellationToken cancellationToken = new CancellationToken())
        //{
        //    var el = await elementTask.ConfigureAwait(false);
        //    return await el.WaitForElementWithName(name, notWebElementId, attemptsCount, delayMs, cancellationToken).ConfigureAwait(false);
        //}

        //public static async Task<IWebElement> WaitForElementWithCssSelector(this Task<IWebDriver> elementTask, string cssSelector, string notWebElementId = null, int attemptsCount = 20, int delayMs = 500,
        //    CancellationToken cancellationToken = new CancellationToken())
        //{
        //    var el = await elementTask.ConfigureAwait(false);
        //    return await el.WaitForElementWithCssSelector(cssSelector, notWebElementId, attemptsCount, delayMs, cancellationToken).ConfigureAwait(false);
        //}

        //public static async Task<IWebElement> WaitForWebElement(this Task<IWebDriver> elementTask, Func<Task<IWebElement>> func, int attemptsCount = 20, int delayMs = 500,
        //    CancellationToken cancellationToken = new CancellationToken())
        //{
        //    var el = await elementTask.ConfigureAwait(false);
        //    return await el.WaitForWebElement(func, attemptsCount, delayMs, cancellationToken).ConfigureAwait(false);
        //}
        //public static async Task<IWebElement> WaitForWebElement(this Task<IWebDriver> elementTask, Func<Task<IWebElement>> func, IWebElement notWebElement, int attemptsCount = 20, int delayMs = 500,
        //    CancellationToken cancellationToken = new CancellationToken())
        //{
        //    var el = await elementTask.ConfigureAwait(false);
        //    return await el.WaitForWebElement(func, notWebElement, attemptsCount, delayMs, cancellationToken).ConfigureAwait(false);
        //}

        //public static async Task<IWebElement> WaitForWebElement(this Task<IWebDriver> elementTask, Func<Task<IWebElement>> func, string notWebElementId, int attemptsCount = 20, int delayMs = 500,
        //    CancellationToken cancellationToken = new CancellationToken())
        //{
        //    var el = await elementTask.ConfigureAwait(false);
        //    return await el.WaitForWebElement(func, notWebElementId, attemptsCount, delayMs, cancellationToken).ConfigureAwait(false);
        //}

        public static async Task<IOptions> Options(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return el.Options();
        }

        public static async Task<INavigation> Navigate(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return el.Navigate();
        }

        public static async Task<RemoteTargetLocator> SwitchTo(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return el.SwitchTo();
        }

        public static async Task<IWebElement> FindElement(this Task<IWebDriver> elementTask, By by, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.FindElement(by, cancellationToken).ConfigureAwait(false);
        }

        public static async Task<ReadOnlyCollection<IWebElement>> FindElements(this Task<IWebDriver> elementTask, By by, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.FindElements(by, cancellationToken).ConfigureAwait(false);
        }

    }
}