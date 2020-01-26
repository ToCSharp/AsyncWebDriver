// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver
{
    /// <summary>
    ///     Defines the interface used to search for elements.
    /// </summary>
    public interface ISearchContext
    {
        /// <summary>
        ///     Finds the first <see cref="IWebElement" /> using the given method.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>The first matching <see cref="IWebElement" /> on the current context.</returns>
        /// <exception cref="NoSuchElementException">If no element matches the criteria.</exception>
        Task<IWebElement> FindElement(By by, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Finds all <see cref="IWebElement">IWebElements</see> within the current context
        ///     using the given mechanism.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>
        ///     A <see cref="ReadOnlyCollection{T}" /> of all <see cref="IWebElement">WebElements</see>
        ///     matching the current criteria, or an empty list if nothing matches.
        /// </returns>
        Task<ReadOnlyCollection<IWebElement>> FindElements(By by,
            CancellationToken cancellationToken = new CancellationToken());

        Task<IWebElement> FindElement(string mechanism, string value, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElement(string mechanism, string value, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementOrDefault(By by, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementOrDefault(string mechanism, string value, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementOrDefault(string mechanism, string value, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));

        Task<ReadOnlyCollection<IWebElement>> FindElements(string mechanism, string value, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElements(string mechanism, string value, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsOrDefault(By by, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsOrDefault(string mechanism, string value, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsOrDefault(string mechanism, string value, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));

        Task<ReadOnlyCollection<IWebElement>> Children(CancellationToken cancellationToken = default(CancellationToken));
    }
}