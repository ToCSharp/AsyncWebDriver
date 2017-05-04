// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;

namespace Zu.AsyncWebDriver.Html5
{
    /// <summary>
    ///     Represents both local and session storage for the site currently opened in the browser.
    /// </summary>
    public interface IWebStorage
    {
        /// <summary>
        ///     Gets the local storage for the site currently opened in the browser.
        /// </summary>
        Task<ILocalStorage> LocalStorage { get; }

        /// <summary>
        ///     Gets the session storage for the site currently opened in the browser.
        /// </summary>
        Task<ISessionStorage> SessionStorage { get; }
    }
}