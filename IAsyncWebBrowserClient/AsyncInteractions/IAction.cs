// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Threading;
using System.Threading.Tasks;

namespace Zu.WebBrowser.AsyncInteractions
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