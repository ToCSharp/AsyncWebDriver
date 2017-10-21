// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver.Interactions.Internal
{
    /// <summary>
    ///     Defines an action for mouse interaction with the browser.
    /// </summary>
    public class MouseAction : WebDriverAction
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref = "MouseAction"/> class.
        /// </summary>
        /// <param name = "mouse">The <see cref = "IMouse"/> with which the action will be performed.</param>
        /// <param name = "target">An <see cref = "ILocatable"/> describing an element at which to perform the action.</param>
        public MouseAction(IMouse mouse, ILocatable target): base (target)
        {
            Mouse = mouse;
        }

        /// <summary>
        ///     Gets the coordinates at which to perform the mouse action.
        /// </summary>
        protected ICoordinates ActionLocation => ActionTarget?.Coordinates;
        /// <summary>
        ///     Gets the mouse with which to perform the action.
        /// </summary>
        protected IMouse Mouse
        {
            get;
        }

        /// <summary>
        ///     Moves the mouse to the location at which to perform the action.
        /// </summary>
        protected async Task MoveToLocation(CancellationToken cancellationToken = new CancellationToken())
        {
            // Only call MouseMove if an actual location was provided. If not,
            // the action will happen in the last known location of the mouse
            // cursor.
            if (ActionLocation != null)
                await Mouse.MouseMove(ActionLocation, cancellationToken).ConfigureAwait(false);
        }
    }
}