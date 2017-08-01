// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver.Html5
{
    /// <summary>
    ///     Represents the local storage for the site currently opened in the browser.
    ///     Each site has its own separate storage area.
    /// </summary>
    public interface ILocalStorage
    {
        /// <summary>
        ///     Gets the number of items in local storage.
        /// </summary>
        Task<int> Count { get; }

        /// <summary>
        ///     Returns value of the local storage given a key.
        /// </summary>
        /// <param name="key">key to for a local storage entry</param>
        /// <returns>Value of the local storage entry as <see cref="string " /> given a key.</returns>
        Task<string> GetItem(string key, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Returns the set of keys associated with local storage.
        /// </summary>
        /// <returns>Returns the set of keys associated with local storage as <see cref="HashSet{T}" />.</returns>
        Task<ReadOnlyCollection<string>> KeySet(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Adds key/value pair to local storage.
        /// </summary>
        /// <param name="key">storage key</param>
        /// <param name="value">storage value</param>
        Task SetItem(string key, string value, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Removes key/value pair from local storage.
        /// </summary>
        /// <param name="key">key to remove from storage</param>
        /// <returns>Value from local storage as <see cref="string ">string</see> for the given key.</returns>
        Task<string> RemoveItem(string key, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Clears local storage.
        /// </summary>
        Task Clear(CancellationToken cancellationToken = new CancellationToken());
    }
}