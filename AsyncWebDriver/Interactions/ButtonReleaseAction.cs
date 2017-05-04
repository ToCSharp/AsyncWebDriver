// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Interactions.Internal;

namespace Zu.AsyncWebDriver.Interactions
{
    /// <summary>
    ///     Defines an action for releasing the currently held mouse button.
    /// </summary>
    /// <remarks>
    ///     This action can be called for an element different than the one
    ///     ClickAndHoldAction was called for. However, if this action is
    ///     performed out of sequence (without holding down the mouse button,
    ///     for example) the results will be different.
    /// </remarks>
    public class ButtonReleaseAction : MouseAction, IAction
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ButtonReleaseAction" /> class.
        /// </summary>
        /// <param name="mouse">The <see cref="IMouse" /> with which the action will be performed.</param>
        /// <param name="actionTarget">An <see cref="ILocatable" /> describing an element at which to perform the action.</param>
        public ButtonReleaseAction(IMouse mouse, ILocatable actionTarget) : base(mouse, actionTarget)
        {
        }

        /// <summary>
        ///     Performs this action.
        /// </summary>
        public async Task Perform(CancellationToken cancellationToken = new CancellationToken())
        {
            // Releases the mouse button currently left held.
            // between browsers.
            await MoveToLocation(cancellationToken);
            await Mouse.MouseUp(ActionLocation, cancellationToken);
        }
    }
}