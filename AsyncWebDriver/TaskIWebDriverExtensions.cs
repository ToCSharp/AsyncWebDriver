// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver
{
    public static class TaskIWebDriverExtensions
    {
        public static async Task<string> GetUrl(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.GetUrl(cancellationToken);
        }
        public static async Task<string> GoToUrl(this Task<IWebDriver> elementTask, string url, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.GoToUrl(url, cancellationToken);
        }

        public static async Task<string> Title(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.Title(cancellationToken);
        }

        public static async Task<string> PageSource(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.PageSource(cancellationToken);
        }

        public static async Task<string> CurrentWindowHandle(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.CurrentWindowHandle(cancellationToken);
        }

        public static async Task<List<string>> WindowHandles(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.WindowHandles(cancellationToken);
        }

        public static async Task<IWebDriver> Open(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            await el.Open(cancellationToken);
            return el;
        }

        public static async Task<IWebDriver> Close(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            await el.Close(cancellationToken);
            return el;
        }

        public static async Task<IWebDriver> Quit(this Task<IWebDriver> elementTask, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            await el.Quit(cancellationToken);
            return el;
        }

        public static async Task<IWebElement> WaitForElementWithId(this Task<IWebDriver> elementTask, string id, string notWebElementId = null, int attemptsCount = 20, int delayMs = 500,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.WaitForElementWithId(id, notWebElementId, attemptsCount, delayMs,  cancellationToken);
        }

        public static async Task<IWebElement> WaitForElementWithName(this Task<IWebDriver> elementTask, string name, string notWebElementId = null, int attemptsCount = 20, int delayMs = 500,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.WaitForElementWithName(name, notWebElementId, attemptsCount, delayMs, cancellationToken);
        }

        public static async Task<IWebElement> WaitForElementWithCssSelector(this Task<IWebDriver> elementTask, string cssSelector, string notWebElementId = null, int attemptsCount = 20, int delayMs = 500,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.WaitForElementWithCssSelector(cssSelector, notWebElementId, attemptsCount, delayMs, cancellationToken);
        }

        public static async Task<IWebElement> WaitForWebElement(this Task<IWebDriver> elementTask, Func<Task<IWebElement>> func, int attemptsCount = 20, int delayMs = 500,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.WaitForWebElement(func, attemptsCount, delayMs, cancellationToken);
        }
        public static async Task<IWebElement> WaitForWebElement(this Task<IWebDriver> elementTask, Func<Task<IWebElement>> func, IWebElement notWebElement, int attemptsCount = 20, int delayMs = 500,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.WaitForWebElement(func, notWebElement, attemptsCount, delayMs, cancellationToken);
        }

        public static async Task<IWebElement> WaitForWebElement(this Task<IWebDriver> elementTask, Func<Task<IWebElement>> func, string notWebElementId, int attemptsCount = 20, int delayMs = 500,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.WaitForWebElement(func, notWebElementId, attemptsCount, delayMs, cancellationToken);
        }
    }
}