// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver
{
    public static class TaskISearchContextExtensions
    {
        public static async Task<IWebElement> FindElement(this Task<ISearchContext> elementTask, By by,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.FindElement(by, cancellationToken).ConfigureAwait(false);
        }

        public static async Task<ReadOnlyCollection<IWebElement>> FindElements(this Task<ISearchContext> elementTask, By by,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await elementTask.ConfigureAwait(false);
            return await el.FindElements(by, cancellationToken).ConfigureAwait(false);
        }
    }
}