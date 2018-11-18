// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver
{
    /// <summary>
    ///     Defines the interface used to search for elements.
    /// </summary>
    public static class TaskISearchContextExtensions
    {
        public static async Task<IWebElement> FindElement(this Task<ISearchContext> elementTask, By by,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.FindElement(by, cancellationToken);
        }

        public static async Task<ReadOnlyCollection<IWebElement>> FindElements(this Task<ISearchContext> elementTask, By by,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask;
            return await el.FindElements(by, cancellationToken);
        }
    }
}