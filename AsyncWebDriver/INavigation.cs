// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver
{
    /// <summary>
    ///     Defines an interface allowing the user to access the browser's history and to
    ///     navigate to a given URL.
    /// </summary>
    public interface INavigation
    {
        /// <summary>
        ///     Move back a single entry in the browser's history.
        /// </summary>
        Task Back(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Move a single "item" forward in the browser's history.
        /// </summary>
        /// <remarks>Does nothing if we are on the latest page viewed.</remarks>
        Task Forward(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load. It is best to use a fully qualified URL</param>
        /// <remarks>
        ///     Calling the <see cref="GoToUrl(string)" /> method will load a new web page in the current browser window.
        ///     This is done using an HTTP GET operation, and the method will block until the
        ///     load is complete. This will follow redirects issued either by the server or
        ///     as a meta-redirect from within the returned HTML. Should a meta-redirect "rest"
        ///     for any duration of time, it is best to wait until this timeout is over, since
        ///     should the underlying page change while your test is executing the results of
        ///     future calls against this interface will be against the freshly loaded page.
        /// </remarks>
        Task GoToUrl(string url, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Load a new web page in the current browser window.
        /// </summary>
        /// <param name="url">The URL to load.</param>
        /// <remarks>
        ///     Calling the <see cref="GoToUrl(System.Uri)" /> method will load a new web page in the current browser window.
        ///     This is done using an HTTP GET operation, and the method will block until the
        ///     load is complete. This will follow redirects issued either by the server or
        ///     as a meta-redirect from within the returned HTML. Should a meta-redirect "rest"
        ///     for any duration of time, it is best to wait until this timeout is over, since
        ///     should the underlying page change while your test is executing the results of
        ///     future calls against this interface will be against the freshly loaded page.
        /// </remarks>
        Task GoToUrl(Uri url, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Refreshes the current page.
        /// </summary>
        Task Refresh(CancellationToken cancellationToken = new CancellationToken());
    }
}