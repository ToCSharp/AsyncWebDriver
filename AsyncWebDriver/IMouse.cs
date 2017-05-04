// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Interactions.Internal;

namespace Zu.AsyncWebDriver
{
    /// <summary>
    ///     Provides methods representing basic mouse actions.
    /// </summary>
    public interface IMouse
    {
        /// <summary>
        ///     Clicks at a set of coordinates using the primary mouse button.
        /// </summary>
        /// <param name="where">An <see cref="ICoordinates" /> describing where to click.</param>
        Task Click(ICoordinates where, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Double-clicks at a set of coordinates.
        /// </summary>
        /// <param name="where">A <see cref="ICoordinates" /> describing where to double-click.</param>
        Task DoubleClick(ICoordinates where, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Presses the primary mouse button at a set of coordinates.
        /// </summary>
        /// <param name="where">A <see cref="ICoordinates" /> describing where to press the mouse button down.</param>
        Task MouseDown(ICoordinates where, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Releases the primary mouse button at a set of coordinates.
        /// </summary>
        /// <param name="where">A <see cref="ICoordinates" /> describing where to release the mouse button.</param>
        Task MouseUp(ICoordinates where, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Moves the mouse to the specified set of coordinates.
        /// </summary>
        /// <param name="where">A <see cref="ICoordinates" /> describing where to move the mouse to.</param>
        Task MouseMove(ICoordinates where, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Moves the mouse to the specified set of coordinates.
        /// </summary>
        /// <param name="where">A <see cref="ICoordinates" /> describing where to click.</param>
        /// <param name="offsetX">A horizontal offset from the coordinates specified in <paramref name="where" />.</param>
        /// <param name="offsetY">A vertical offset from the coordinates specified in <paramref name="where" />.</param>
        Task MouseMove(ICoordinates where, int offsetX, int offsetY,
            CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Clicks at a set of coordinates using the secondary mouse button.
        /// </summary>
        /// <param name="where">A <see cref="ICoordinates" /> describing where to click.</param>
        Task ContextClick(ICoordinates where, CancellationToken cancellationToken = new CancellationToken());

        // TODO: Scroll wheel support
    }
}