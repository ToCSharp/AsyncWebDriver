// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver.Internal
{
    /// <summary>
    ///     Defines the interface through which the user finds elements by their ID.
    /// </summary>
    public interface IFindsById
    {
        /// <summary>
        ///     Finds the first element matching the specified id.
        /// </summary>
        /// <param name="id">The id to match.</param>
        /// <returns>The first <see cref="IWebElement" /> matching the criteria.</returns>
        Task<IWebElement> FindElementById(string id, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Finds all elements matching the specified id.
        /// </summary>
        /// <param name="id">The id to match.</param>
        /// <returns>
        ///     A <see cref="ReadOnlyCollection{T}" /> containing all
        ///     <see cref="IWebElement">IWebElements</see> matching the criteria.
        /// </returns>
        Task<ReadOnlyCollection<IWebElement>> FindElementsById(string id,
            CancellationToken cancellationToken = new CancellationToken());

        Task<IWebElement> FindElementById(string id, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementById(string id, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementById(string id, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementById(string id, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByIdEndsWith(string id, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByIdEndsWithOrDefault(string id, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByIdOrDefault(string id, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByIdOrDefault(string id, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByIdOrDefault(string id, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByIdOrDefault(string id, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByIdStartsWith(string id, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByIdStartsWithOrDefault(string id, CancellationToken cancellationToken = default(CancellationToken));

        Task<ReadOnlyCollection<IWebElement>> FindElementsById(string id, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsById(string id, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsById(string id, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsById(string id, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByIdEndsWith(string id, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByIdEndsWithOrDefault(string id, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByIdOrDefault(string id, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByIdOrDefault(string id, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByIdOrDefault(string id, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByIdOrDefault(string id, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByIdStartsWith(string id, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByIdStartsWithOrDefault(string id, CancellationToken cancellationToken = default(CancellationToken));
    }
}