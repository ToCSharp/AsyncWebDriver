// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver.Interactions
{
    /// <summary>
    ///     Provides methods by which an interaction with the browser can be performed.
    /// </summary>
    public interface IAction
    {
        /// <summary>
        ///     Performs this action on the browser.
        /// </summary>
        Task Perform(CancellationToken cancellationToken = new CancellationToken());
    }
}