// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Zu.AsyncWebDriver
{
    /// <summary>
    ///     Interface implemented by each driver that allows access to touch screen capabilities.
    /// </summary>
    public interface IHasTouchScreen
    {
        /// <summary>
        ///     Gets the device representing the touch screen.
        /// </summary>
        ITouchScreen TouchScreen { get; }
    }
}