// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Interactions.Internal;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver.Interactions
{
    /// <summary>
    ///     Presses a touch screen at a given location.
    /// </summary>
    public class ScreenReleaseAction : TouchAction, IAction
    {
        private readonly int x;
        private readonly int y;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScreenReleaseAction" /> class.
        /// </summary>
        /// <param name="touchScreen">The <see cref="ITouchScreen" /> with which the action will be performed.</param>
        /// <param name="x">The x coordinate relative to the view port.</param>
        /// <param name="y">The y coordinate relative to the view port.</param>
        public ScreenReleaseAction(ITouchScreen touchScreen, int x, int y) : base(touchScreen, null)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        ///     Performs the action.
        /// </summary>
        public async Task Perform(CancellationToken cancellationToken = new CancellationToken())
        {
            await TouchScreen.Up(x, y, cancellationToken);
        }
    }
}