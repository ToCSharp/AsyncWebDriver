// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Collections.Generic;

namespace Zu.AsyncWebDriver.Internal
{
    /// <summary>
    /// Defines the interface through which the framework can serialize an element to the wire protocol.
    /// </summary>
    public interface IWebElementReference
    {
        /// <summary>
        /// Gets the internal ID of the element.
        /// </summary>
        string ElementReferenceId { get; }

        /// <summary>
        /// Converts an object into an object that represents an element for the wire protocol.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that represents an element in the wire protocol.</returns>
        Dictionary<string, object> ToDictionary();
    }
}