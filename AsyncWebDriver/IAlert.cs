// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver
{
    /// <summary>
    ///     Defines the interface through which the user can manipulate JavaScript alerts.
    /// </summary>
    public interface IAlert
    {
        /// <summary>
        ///     Gets the text of the alert.
        /// </summary>
        Task<string> Text { get; }

        /// <summary>
        ///     Dismisses the alert.
        /// </summary>
        Task Dismiss(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Accepts the alert.
        /// </summary>
        Task Accept(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Sends keys to the alert.
        /// </summary>
        /// <param name="keysToSend">The keystrokes to send.</param>
        Task SendKeys(string keysToSend, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Sets the user name and password in an alert prompting for credentials.
        /// </summary>
        /// <param name="userName">The user name to set.</param>
        /// <param name="password">The password to set.</param>
        Task SetAuthenticationCredentials(string userName, string password,
            CancellationToken cancellationToken = new CancellationToken());
    }
}