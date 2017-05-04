// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Zu.AsyncWebDriver.Html5
{
    /// <summary>
    ///     Interface allowing the user to determine if the driver instance supports web storage.
    /// </summary>
    public interface IHasWebStorage
    {
        /// <summary>
        ///     Gets a value indicating whether web storage is supported for this driver.
        /// </summary>
        bool HasWebStorage { get; }

        /// <summary>
        ///     Gets an <see cref="IWebStorage" /> object for managing web storage.
        /// </summary>
        IWebStorage WebStorage { get; }
    }
}