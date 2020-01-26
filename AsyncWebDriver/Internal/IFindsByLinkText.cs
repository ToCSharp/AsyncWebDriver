// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver.Internal
{
    /// <summary>
    ///     Defines the interface through which the user finds elements by their link text.
    /// </summary>
    public interface IFindsByLinkText
    {
        /// <summary>
        ///     Finds the first element matching the specified link text.
        /// </summary>
        /// <param name="linkText">The link text to match.</param>
        /// <returns>The first <see cref="IWebElement" /> matching the criteria.</returns>
        Task<IWebElement> FindElementByLinkText(string linkText,
            CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Finds all elements matching the specified link text.
        /// </summary>
        /// <param name="linkText">The link text to match.</param>
        /// <returns>
        ///     A <see cref="ReadOnlyCollection{T}" /> containing all
        ///     <see cref="IWebElement">IWebElements</see> matching the criteria.
        /// </returns>
        Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkText(string linkText,
            CancellationToken cancellationToken = new CancellationToken());

        Task<IWebElement> FindElementByLinkText(string linkText, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByLinkText(string linkText, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByLinkText(string linkText, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByLinkText(string linkText, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByLinkTextOrDefault(string linkText, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByLinkTextOrDefault(string linkText, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByLinkTextOrDefault(string linkText, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByLinkTextOrDefault(string linkText, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));

        Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkText(string linkText, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkText(string linkText, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkText(string linkText, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkText(string linkText, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkTextOrDefault(string linkText, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkTextOrDefault(string linkText, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkTextOrDefault(string linkText, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkTextOrDefault(string linkText, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
    }
}