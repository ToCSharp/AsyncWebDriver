// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.
using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Interactions.Internal;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver.Interactions
{
    /// <summary>
    ///     Creates a double tap gesture on a touch screen.
    /// </summary>
    public class ScrollAction : TouchAction, IAction
    {
        private readonly int offsetX;
        private readonly int offsetY;
        /// <summary>
        ///     Initializes a new instance of the <see cref = "ScrollAction"/> class.
        /// </summary>
        /// <param name = "touchScreen">The <see cref = "ITouchScreen"/> with which the action will be performed.</param>
        /// <param name = "offsetX">The horizontal offset relative to the view port.</param>
        /// <param name = "offsetY">The vertical offset relative to the view port.</param>
        public ScrollAction(ITouchScreen touchScreen, int offsetX, int offsetY): this (touchScreen, null, offsetX, offsetY)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref = "ScrollAction"/> class for use with the specified element.
        /// </summary>
        /// <param name = "touchScreen">The <see cref = "ITouchScreen"/> with which the action will be performed.</param>
        /// <param name = "actionTarget">An <see cref = "ILocatable"/> describing an element at which to perform the action.</param>
        /// <param name = "offsetX">The x coordinate relative to the view port.</param>
        /// <param name = "offsetY">The y coordinate relative to the view port.</param>
        public ScrollAction(ITouchScreen touchScreen, ILocatable actionTarget, int offsetX, int offsetY): base (touchScreen, actionTarget)
        {
            if (actionTarget == null)
                throw new ArgumentException("Must provide a location for a single tap action.", "actionTarget");
            this.offsetX = offsetX;
            this.offsetY = offsetY;
        }

        /// <summary>
        ///     Performs the action.
        /// </summary>
        public async Task Perform(CancellationToken cancellationToken = new CancellationToken())
        {
            await TouchScreen.Scroll(ActionLocation, offsetX, offsetY, cancellationToken).ConfigureAwait(false);
        }
    }
}