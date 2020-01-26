// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver.Internal
{
    /// <summary>
    ///     Defines the interface through which the user finds elements by their CSS class.
    /// </summary>
    public interface IFindsByClassName
    {
        /// <summary>
        ///     Finds the first element matching the specified CSS class.
        /// </summary>
        /// <param name="className">The CSS class to match.</param>
        /// <returns>The first <see cref="IWebElement" /> matching the criteria.</returns>
        Task<IWebElement> FindElementByClassName(string className,
            CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Finds all elements matching the specified CSS class.
        /// </summary>
        /// <param name="className">The CSS class to match.</param>
        /// <returns>
        ///     A <see cref="ReadOnlyCollection{T}" /> containing all
        ///     <see cref="IWebElement">IWebElements</see> matching the criteria.
        /// </returns>
        Task<ReadOnlyCollection<IWebElement>> FindElementsByClassName(string className,
            CancellationToken cancellationToken = new CancellationToken());

        Task<IWebElement> FindElementByClassName(string className, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByClassName(string className, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByClassName(string className, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByClassName(string className, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByClassNameOrDefault(string className, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByClassNameOrDefault(string className, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByClassNameOrDefault(string className, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<IWebElement> FindElementByClassNameOrDefault(string className, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));

        Task<ReadOnlyCollection<IWebElement>> FindElementsByClassName(string className, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByClassName(string className, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByClassName(string className, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByClassName(string className, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByClassNameOrDefault(string className, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByClassNameOrDefault(string className, int timeoutMs, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByClassNameOrDefault(string className, string notElementId, CancellationToken cancellationToken = default(CancellationToken));
        Task<ReadOnlyCollection<IWebElement>> FindElementsByClassNameOrDefault(string className, TimeSpan timeout, CancellationToken cancellationToken = default(CancellationToken));

    }
}