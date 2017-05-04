// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Zu.AsyncWebDriver.Interactions.Internal
{
    /// <summary>
    ///     Defines an action for keyboard interaction with the browser.
    /// </summary>
    public class TouchAction : WebDriverAction
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TouchAction" /> class.
        /// </summary>
        /// <param name="touchScreen">The <see cref="ITouchScreen" /> to use in performing the action.</param>
        /// <param name="actionTarget">An <see cref="ILocatable" /> object providing the element on which to perform the action.</param>
        protected TouchAction(ITouchScreen touchScreen, ILocatable actionTarget) : base(actionTarget)
        {
            TouchScreen = touchScreen;
        }

        /// <summary>
        ///     Gets the touch screen with which to perform the action.
        /// </summary>
        protected ITouchScreen TouchScreen { get; }

        /// <summary>
        ///     Gets the location at which to perform the action.
        /// </summary>
        protected ICoordinates ActionLocation => ActionTarget?.Coordinates;
    }
}