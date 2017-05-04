// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Zu.AsyncWebDriver.Internal
{
    /// <summary>
    ///     Defines the interface through which the user can access the driver used to find an element.
    /// </summary>
    public interface IWrapsDriver
    {
        /// <summary>
        ///     Gets the <see cref="IWebDriver" /> used to find this element.
        /// </summary>
        IWebDriver WrappedDriver { get; }
    }
}