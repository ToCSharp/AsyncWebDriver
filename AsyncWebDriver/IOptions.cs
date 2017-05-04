// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Zu.AsyncWebDriver
{
    /// <summary>
    ///     Defines an interface allowing the user to set options on the browser.
    /// </summary>
    public interface IOptions
    {
        /// <summary>
        ///     Gets an object allowing the user to manipulate cookies on the page.
        /// </summary>
        ICookieJar Cookies { get; }

        /// <summary>
        ///     Gets an object allowing the user to manipulate the currently-focused browser window.
        /// </summary>
        /// <remarks>
        ///     "Currently-focused" is defined as the browser window having the window handle
        ///     returned when IWebDriver.CurrentWindowHandle is called.
        /// </remarks>
        IWindow Window { get; }

        /// <summary>
        ///     Gets an object allowing the user to examing the logs for this driver instance.
        /// </summary>
        ILogs Logs { get; }

        /// <summary>
        ///     Provides access to the timeouts defined for this driver.
        /// </summary>
        /// <returns>An object implementing the <see cref="ITimeouts" /> interface.</returns>
        ITimeouts Timeouts();
    }
}