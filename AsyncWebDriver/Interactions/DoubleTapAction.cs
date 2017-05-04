// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Interactions.Internal;

namespace Zu.AsyncWebDriver.Interactions
{
    /// <summary>
    ///     Creates a double tap gesture on a touch screen.
    /// </summary>
    public class DoubleTapAction : TouchAction, IAction
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DoubleTapAction" /> class.
        /// </summary>
        /// <param name="touchScreen">The <see cref="ITouchScreen" /> with which the action will be performed.</param>
        /// <param name="actionTarget">An <see cref="ILocatable" /> describing an element at which to perform the action.</param>
        public DoubleTapAction(ITouchScreen touchScreen, ILocatable actionTarget) : base(touchScreen, actionTarget)
        {
            if (actionTarget == null)
                throw new ArgumentException("Must provide a location for a single tap action.", "actionTarget");
        }

        /// <summary>
        ///     Performs the action.
        /// </summary>
        public async Task Perform(CancellationToken cancellationToken = new CancellationToken())
        {
            await TouchScreen.DoubleTap(ActionLocation, cancellationToken);
        }
    }
}