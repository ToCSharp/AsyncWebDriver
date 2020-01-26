// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver.Internal
{
    /// <summary>
    ///     Defines the interface through which the user finds elements by their cascading style sheet (CSS) selector.
    /// </summary>
    public interface IFindsByCssSelector
    {
        /// <summary>
        ///     Finds the first element matching the specified CSS selector.
        /// </summary>
        /// <param name="cssSelector">The id to match.</param>
        /// <returns>The first <see cref="IWebElement" /> matching the criteria.</returns>
        Task<IWebElement> FindElementByCssSelector(string cssSelector,
            CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Finds all elements matching the specified CSS selector.
        /// </summary>
        /// <param name="cssSelector">The CSS selector to match.</param>
        /// <returns>
        ///     A <see cref="ReadOnlyCollection{T}" /> containing all
        ///     <see cref="IWebElement">IWebElements</see> matching the criteria.
        /// </returns>
        Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelector(string cssSelector,
            CancellationToken cancellationToken = new CancellationToken());

        Task<IWebElement> FindElementByCssSelector(string cssSelector, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByCssSelector(string cssSelector, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByCssSelector(string cssSelector, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByCssSelector(string cssSelector, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByCssSelectorOrDefault(string cssSelector, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByCssSelectorOrDefault(string cssSelector, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByCssSelectorOrDefault(string cssSelector, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByCssSelectorOrDefault(string cssSelector, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));

        Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelector(string cssSelector, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelector(string cssSelector, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelector(string cssSelector, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelector(string cssSelector, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelectorOrDefault(string cssSelector, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelectorOrDefault(string cssSelector, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelectorOrDefault(string cssSelector, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelectorOrDefault(string cssSelector, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
    }
}