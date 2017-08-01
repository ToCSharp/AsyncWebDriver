// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver
{
    /// <summary>
    ///     Provides access to input devices for advanced user interactions.
    /// </summary>
    public interface IHasInputDevices
    {
        /// <summary>
        ///     Gets an <see cref="IKeyboard" /> object for sending keystrokes to the browser.
        /// </summary>
        IKeyboard Keyboard { get; }

        /// <summary>
        ///     Gets an <see cref="IMouse" /> object for sending mouse commands to the browser.
        /// </summary>
        IMouse Mouse { get; }
    }
}