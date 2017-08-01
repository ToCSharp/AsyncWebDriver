// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using Zu.WebBrowser.AsyncInteractions;

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