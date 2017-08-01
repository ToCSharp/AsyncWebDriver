// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Threading.Tasks;

namespace Zu.AsyncWebDriver.Html5
{
    /// <summary>
    ///     Defines an interface allowing the user to access application cache status
    /// </summary>
    public interface IApplicationCache
    {
        /// <summary>
        ///     Gets the current state of the application cache.
        /// </summary>
        Task<AppCacheStatus> Status { get; }
    }
}