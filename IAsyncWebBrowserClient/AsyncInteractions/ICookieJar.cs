// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.BasicTypes;

namespace Zu.WebBrowser.AsyncInteractions
{
    /// <summary>
    ///     Defines an interface allowing the user to manipulate cookies on the current page.
    /// </summary>
    public interface ICookieJar
    {
        /// <summary>
        ///     Gets all cookies defined for the current page.
        /// </summary>
        Task<ReadOnlyCollection<Cookie>> AllCookies { get; }

        /// <summary>
        ///     Adds a cookie to the current page.
        /// </summary>
        /// <param name="cookie">The <see cref="Cookie" /> object to be added.</param>
        Task AddCookie(Cookie cookie, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Gets a cookie with the specified name.
        /// </summary>
        /// <param name="name">The name of the cookie to retrieve.</param>
        /// <returns>
        ///     The <see cref="Cookie" /> containing the name. Returns <see langword="null" />
        ///     if no cookie with the specified name is found.
        /// </returns>
        Task<Cookie> GetCookieNamed(string name, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Deletes the specified cookie from the page.
        /// </summary>
        /// <param name="cookie">The <see cref="Cookie" /> to be deleted.</param>
        Task DeleteCookie(Cookie cookie, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Deletes the cookie with the specified name from the page.
        /// </summary>
        /// <param name="name">The name of the cookie to be deleted.</param>
        Task DeleteCookieNamed(string name, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Deletes all cookies from the page.
        /// </summary>
        Task DeleteAllCookies(CancellationToken cancellationToken = new CancellationToken());
    }
}