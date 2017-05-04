// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Interactions.Internal;

namespace Zu.AsyncWebDriver.Interactions
{
    /// <summary>
    ///     Defines an action for moving the mouse to a specified offset from its current location.
    /// </summary>
    public class MoveToOffsetAction : MouseAction, IAction
    {
        private readonly int offsetX;
        private readonly int offsetY;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MoveToOffsetAction" /> class.
        /// </summary>
        /// <param name="mouse">The <see cref="IMouse" /> with which the action will be performed.</param>
        /// <param name="actionTarget">An <see cref="ILocatable" /> describing an element at which to perform the action.</param>
        /// <param name="offsetX">The horizontal offset from the origin of the target to which to move the mouse.</param>
        /// <param name="offsetY">The vertical offset from the origin of the target to which to move the mouse.</param>
        public MoveToOffsetAction(IMouse mouse, ILocatable actionTarget, int offsetX, int offsetY) : base(mouse,
            actionTarget)
        {
            this.offsetX = offsetX;
            this.offsetY = offsetY;
        }

        /// <summary>
        ///     Performs this action.
        /// </summary>
        public async Task Perform(CancellationToken cancellationToken = new CancellationToken())
        {
            await Mouse.MouseMove(ActionLocation, offsetX, offsetY, cancellationToken);
        }
    }
}