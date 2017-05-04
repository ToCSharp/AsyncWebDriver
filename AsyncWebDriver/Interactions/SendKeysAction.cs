// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Interactions.Internal;

namespace Zu.AsyncWebDriver.Interactions
{
    /// <summary>
    ///     Defines an action for sending a sequence of keystrokes to an element.
    /// </summary>
    public class SendKeysAction : KeyboardAction, IAction
    {
        private readonly string keysToSend;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SendKeysAction" /> class.
        /// </summary>
        /// <param name="keyboard">The <see cref="IKeyboard" /> to use in performing the action.</param>
        /// <param name="mouse">The <see cref="IMouse" /> to use in setting focus to the element on which to perform the action.</param>
        /// <param name="actionTarget">An <see cref="ILocatable" /> object providing the element on which to perform the action.</param>
        /// <param name="keysToSend">The key sequence to send.</param>
        public SendKeysAction(IKeyboard keyboard, IMouse mouse, ILocatable actionTarget, string keysToSend) : base(
            keyboard, mouse, actionTarget)
        {
            this.keysToSend = keysToSend;
        }

        /// <summary>
        ///     Performs this action.
        /// </summary>
        public async Task Perform(CancellationToken cancellationToken = new CancellationToken())
        {
            await FocusOnElement(cancellationToken);
            await Keyboard.SendKeys(keysToSend, cancellationToken);
        }
    }
}