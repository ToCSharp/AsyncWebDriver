// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Interactions.Internal;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver.Interactions
{
    /// <summary>
    ///     Defines an action for releasing a modifier key (Shift, Alt, or Control) on the keyboard.
    /// </summary>
    public class KeyUpAction : SingleKeyAction, IAction
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="KeyUpAction" /> class.
        /// </summary>
        /// <param name="keyboard">The <see cref="IKeyboard" /> to use in performing the action.</param>
        /// <param name="mouse">The <see cref="IMouse" /> to use in setting focus to the element on which to perform the action.</param>
        /// <param name="actionTarget">An <see cref="ILocatable" /> object providing the element on which to perform the action.</param>
        /// <param name="key">
        ///     The modifier key (<see cref="Keys.Shift" />, <see cref="Keys.Control" />, <see cref="Keys.Alt" />) to
        ///     use in the action.
        /// </param>
        public KeyUpAction(IKeyboard keyboard, IMouse mouse, ILocatable actionTarget, string key) : base(keyboard,
            mouse, actionTarget, key)
        {
        }

        /// <summary>
        ///     Performs this action.
        /// </summary>
        public async Task Perform(CancellationToken cancellationToken = new CancellationToken())
        {
            await FocusOnElement(cancellationToken);
            await Keyboard.ReleaseKey(Key, cancellationToken);
        }
    }
}