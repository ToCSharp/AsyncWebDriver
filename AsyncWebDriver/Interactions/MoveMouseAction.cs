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
    ///     Defines an action for moving the mouse to a specified location.
    /// </summary>
    public class MoveMouseAction : MouseAction, IAction
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MoveMouseAction" /> class.
        /// </summary>
        /// <param name="mouse">The <see cref="IMouse" /> with which the action will be performed.</param>
        /// <param name="actionTarget">An <see cref="ILocatable" /> describing an element at which to perform the action.</param>
        public MoveMouseAction(IMouse mouse, ILocatable actionTarget) : base(mouse, actionTarget)
        {
            if (actionTarget == null)
                throw new ArgumentException("Must provide a location for a move action.", "actionTarget");
        }

        /// <summary>
        ///     Performs this action.
        /// </summary>
        public async Task Perform(CancellationToken cancellationToken = new CancellationToken())
        {
            await Mouse.MouseMove(ActionLocation, cancellationToken);
        }
    }
}