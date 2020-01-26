// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Internal;
using Zu.AsyncWebDriver.Remote;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BrowserOptions;

namespace Zu.AsyncWebDriver
{
    /// <summary>
    ///     Defines the interface through which the user controls the browser.
    /// </summary>
    /// <remarks>
    ///     The <see cref="IWebDriver" /> interface is the main interface to use for testing, which
    ///     represents an idealized web browser. The methods in this class fall into three categories:
    ///     <list type="bullet">
    ///         <item>
    ///             <description>Control of the browser itself</description>
    ///         </item>
    ///         <item>
    ///             <description>Selection of <see cref="IWebElement">IWebElements</see></description>
    ///         </item>
    ///         <item>
    ///             <description>Debugging aids</description>
    ///         </item>
    ///     </list>
    ///     <para>
    ///         Key properties and methods are <see cref="GoToUrl" />, which is used to
    ///         load a new web page by setting the property, and the various methods similar
    ///         to <see cref="ISearchContext.FindElement" />, which is used to find <see cref="IWebElement">IWebElements</see>.
    ///     </para>
    ///     <para>
    ///         You use the interface by instantiate drivers that implement of this interface.
    ///         You should write your tests against this interface so that you may "swap in" a
    ///         more fully featured browser when there is a requirement for one.
    ///     </para>
    /// </remarks>
    public interface IWebDriver : IDisposable, ISearchContext, IJavaScriptExecutor, IFindsById, IFindsByClassName, IFindsByLinkText, IFindsByName, IFindsByTagName, IFindsByXPath, IFindsByPartialLinkText, IFindsByCssSelector, ITakesScreenshot, IHasInputDevices, IHasWebStorage, IHasLocationContext, IHasApplicationCache, IActionExecutor
    {
        int GoToUrlTimeoutMs { get; set; }
        Task<string> GetUrl(CancellationToken cancellationToken = new CancellationToken());
        Task<string> GoToUrl(string url, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Gets the title of the current browser window.
        /// </summary>
        Task<string> Title(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Gets the source of the page last loaded by the browser.
        /// </summary>
        /// <remarks>
        ///     If the page has been modified after loading (for example, by JavaScript)
        ///     there is no guarantee that the returned text is that of the modified page.
        ///     Please consult the documentation of the particular driver being used to
        ///     determine whether the returned text reflects the current state of the page
        ///     or the text last sent by the web server. The page source returned is a
        ///     representation of the underlying DOM: do not expect it to be formatted
        ///     or escaped in the same way as the response sent from the web server.
        /// </remarks>
        Task<string> PageSource(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Gets the current window handle, which is an opaque handle to this
        ///     window that uniquely identifies it within this driver instance.
        /// </summary>
        Task<string> CurrentWindowHandle(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Gets the window handles of open browser windows.
        /// </summary>
        Task<List<string>> WindowHandles(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Openes browser if the last window currently not open.
        /// </summary>
        Task Open(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Close the current window, quitting the browser if it is the last window currently open.
        /// </summary>
        Task Close(CancellationToken cancellationToken = new CancellationToken());

        void CloseSync();

        /// <summary>
        ///     Quits this driver, closing every associated window.
        /// </summary>
        Task Quit(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Instructs the driver to change its settings.
        /// </summary>
        /// <returns>
        ///     An <see cref="IOptions" /> object allowing the user to change
        ///     the settings of the driver.
        /// </returns>
        IOptions Options();

        /// <summary>
        ///     Instructs the driver to navigate the browser to another location.
        /// </summary>
        /// <returns>
        ///     An <see cref="INavigation" /> object allowing the user to access
        ///     the browser's history and to navigate to a given URL.
        /// </returns>
        INavigation Navigate();

        /// <summary>
        ///     Instructs the driver to send future commands to a different frame or window.
        /// </summary>
        /// <returns>
        ///     An <see cref="RemoteTargetLocator" /> object which can be used to select
        ///     a frame or window.
        /// </returns>
        RemoteTargetLocator SwitchTo();

        Task ClearElement(string elementId, CancellationToken cancellationToken = default(CancellationToken));
        Task ClickElement(string elementId, CancellationToken cancellationToken = default(CancellationToken));
    }
}