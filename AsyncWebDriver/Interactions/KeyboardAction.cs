// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver.Interactions.Internal
{
    /// <summary>
    ///     Defines an action for keyboard interaction with the browser.
    /// </summary>
    public class KeyboardAction : WebDriverAction
    {
        private readonly IMouse mouse;

        /// <summary>
        ///     Initializes a new instance of the <see cref="KeyboardAction" /> class.
        /// </summary>
        /// <param name="keyboard">The <see cref="IKeyboard" /> to use in performing the action.</param>
        /// <param name="mouse">The <see cref="IMouse" /> to use in setting focus to the element on which to perform the action.</param>
        /// <param name="actionTarget">An <see cref="ILocatable" /> object providing the element on which to perform the action.</param>
        protected KeyboardAction(IKeyboard keyboard, IMouse mouse, ILocatable actionTarget) : base(actionTarget)
        {
            Keyboard = keyboard;
            this.mouse = mouse;
        }

        /// <summary>
        ///     Gets the keyboard with which to perform the action.
        /// </summary>
        protected IKeyboard Keyboard { get; }

        /// <summary>
        ///     Focuses on the element on which the action is to be performed.
        /// </summary>
        protected async Task FocusOnElement(CancellationToken cancellationToken = new CancellationToken())
        {
            if (ActionTarget != null)
                await mouse.Click(ActionTarget.Coordinates, cancellationToken);
        }
    }
}