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
    ///     Creates a flick gesture on a touch screen.
    /// </summary>
    public class FlickAction : TouchAction, IAction
    {
        private readonly int offsetX;
        private readonly int offsetY;
        private readonly int speed;
        private readonly int speedX;
        private readonly int speedY;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FlickAction" /> class.
        /// </summary>
        /// <param name="touchScreen">The <see cref="ITouchScreen" /> with which the action will be performed.</param>
        /// <param name="speedX">The horizontal speed in pixels per second.</param>
        /// <param name="speedY">The vertical speed in pixels per second.</param>
        public FlickAction(ITouchScreen touchScreen, int speedX, int speedY) : base(touchScreen, null)
        {
            this.speedX = speedX;
            this.speedY = speedY;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FlickAction" /> class for use with the specified element.
        /// </summary>
        /// <param name="touchScreen">The <see cref="ITouchScreen" /> with which the action will be performed.</param>
        /// <param name="actionTarget">An <see cref="ILocatable" /> describing an element at which to perform the action.</param>
        /// <param name="offsetX">The x offset relative to the viewport.</param>
        /// <param name="offsetY">The y offset relative to the viewport.</param>
        /// <param name="speed">The speed in pixels per second.</param>
        public FlickAction(ITouchScreen touchScreen, ILocatable actionTarget, int offsetX, int offsetY,
            int speed) : base(touchScreen, actionTarget)
        {
            if (actionTarget == null)
                throw new ArgumentException("Must provide a location for a single tap action.", "actionTarget");
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.speed = speed;
        }

        /// <summary>
        ///     Performs the action.
        /// </summary>
        public async Task Perform(CancellationToken cancellationToken = new CancellationToken())
        {
            if (ActionLocation != null)
                await TouchScreen.Flick(ActionLocation, offsetX, offsetY, speed, cancellationToken);
            else
                await TouchScreen.Flick(speedX, speedY, cancellationToken);
        }
    }
}