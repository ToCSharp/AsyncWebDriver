// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

namespace Zu.AsyncWebDriver.Html5
{
    /// <summary>
    ///     Interface allowing the user to determine if the driver instance supports application cache.
    /// </summary>
    public interface IHasApplicationCache
    {
        /// <summary>
        ///     Gets a value indicating whether manipulating the application cache is supported for this driver.
        /// </summary>
        bool HasApplicationCache { get; }

        /// <summary>
        ///     Gets an <see cref="IApplicationCache" /> object for managing application cache.
        /// </summary>
        IApplicationCache ApplicationCache { get; }
    }
}