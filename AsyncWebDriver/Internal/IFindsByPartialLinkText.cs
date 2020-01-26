// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver.Internal
{
    /// <summary>
    ///     Defines the interface through which the user finds elements by a partial match on their link text.
    /// </summary>
    public interface IFindsByPartialLinkText
    {
        /// <summary>
        ///     Finds the first element matching the specified partial link text.
        /// </summary>
        /// <param name="partialLinkText">The partial link text to match.</param>
        /// <returns>The first <see cref="IWebElement" /> matching the criteria.</returns>
        Task<IWebElement> FindElementByPartialLinkText(string partialLinkText,
            CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Finds all elements matching the specified partial link text.
        /// </summary>
        /// <param name="partialLinkText">The partial link text to match.</param>
        /// <returns>
        ///     A <see cref="ReadOnlyCollection{T}" /> containing all
        ///     <see cref="IWebElement">IWebElements</see> matching the criteria.
        /// </returns>
        Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkText(string partialLinkText,
            CancellationToken cancellationToken = new CancellationToken());

        Task<IWebElement> FindElementByPartialLinkText(string partialLinkText, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByPartialLinkText(string partialLinkText, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByPartialLinkText(string partialLinkText, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByPartialLinkText(string partialLinkText, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByPartialLinkTextOrDefault(string partialLinkText, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByPartialLinkTextOrDefault(string partialLinkText, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByPartialLinkTextOrDefault(string partialLinkText, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByPartialLinkTextOrDefault(string partialLinkText, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));

        Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkText(string partialLinkText, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkText(string partialLinkText, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkText(string partialLinkText, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkText(string partialLinkText, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkTextOrDefault(string partialLinkText, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkTextOrDefault(string partialLinkText, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkTextOrDefault(string partialLinkText, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkTextOrDefault(string partialLinkText, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
    }
}