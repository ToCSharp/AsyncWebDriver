// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Threading;
using System.Threading.Tasks;

namespace Zu.WebBrowser.AsyncInteractions
{
    /// <summary>
    ///     Provides methods representing basic keyboard actions.
    /// </summary>
    public interface IKeyboard
    {
        /// <summary>
        ///     Sends a sequence of keystrokes to the target.
        /// </summary>
        /// <param name="keySequence">A string representing the keystrokes to send.</param>
        Task SendKeys(string keySequence, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Presses a key.
        /// </summary>
        /// <param name="keyToPress">The key value representing the key to press.</param>
        /// <remarks>The key value must be one of the values from the <see cref="Keys" /> class.</remarks>
        Task PressKey(string keyToPress, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Releases a key.
        /// </summary>
        /// <param name="keyToRelease">The key value representing the key to release.</param>
        /// <remarks>The key value must be one of the values from the <see cref="Keys" /> class.</remarks>
        Task ReleaseKey(string keyToRelease, CancellationToken cancellationToken = new CancellationToken());
    }
}