// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Zu.AsyncWebDriver.Html5
{
    /// <summary>
    ///     Represents the application cache status.
    /// </summary>
    public enum AppCacheStatus
    {
        /// <summary>
        ///     AppCache status is uncached
        /// </summary>
        Uncached = 0,

        /// <summary>
        ///     AppCache status is idle
        /// </summary>
        Idle = 1,

        /// <summary>
        ///     AppCache status is checkint
        /// </summary>
        Checking,

        /// <summary>
        ///     AppCache status is downloading
        /// </summary>
        Downloading,

        /// <summary>
        ///     AppCache status is updated-ready
        /// </summary>
        UpdateReady,

        /// <summary>
        ///     AppCache status is obsolete
        /// </summary>
        Obsolete
    }
}