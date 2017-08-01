// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.BasicTypes;

namespace Zu.WebBrowser.AsyncInteractions
{
    /// <summary>
    ///     Defines the interface through which the user can discover where an element is on the screen.
    /// </summary>
    public interface ILocatable
    {
        /// <summary>
        ///     Gets the coordinates identifying the location of this element using
        ///     various frames of reference.
        /// </summary>
        ICoordinates Coordinates { get; }

        /// <summary>
        ///     Gets the location of an element on the screen, scrolling it into view
        ///     if it is not currently on the screen.
        /// </summary>
        Task<WebPoint> LocationOnScreenOnceScrolledIntoView(CancellationToken cancellationToken = new CancellationToken());
    }
}