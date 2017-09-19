// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver
{
    /// <summary>
    ///     Defines the interface through which the user can locate a given frame or window.
    /// </summary>
    public interface IWebDriverTargetLocator
    {
        /// <summary>
        ///     Select a frame by its (zero-based) index.
        /// </summary>
        /// <param name="frameIndex">The zero-based index of the frame to select.</param>
        /// <returns>An <see cref="IWebDriver" /> instance focused on the specified frame.</returns>
        /// <exception cref="NoSuchFrameException">If the frame cannot be found.</exception>
        Task<IWebDriver> Frame(int frameIndex, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Select a frame by its name or ID.
        /// </summary>
        /// <param name="frameName">The name of the frame to select.</param>
        /// <returns>An <see cref="IWebDriver" /> instance focused on the specified frame.</returns>
        /// <exception cref="NoSuchFrameException">If the frame cannot be found.</exception>
        Task<IWebDriver> Frame(string frameName, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Select a frame using its previously located <see cref="IWebElement" />
        /// </summary>
        /// <param name="frameElement">The frame element to switch to.</param>
        /// <returns>An <see cref="IWebDriver" /> instance focused on the specified frame.</returns>
        /// <exception cref="NoSuchFrameException">If the element is neither a FRAME nor an IFRAME element.</exception>
        /// <exception cref="StaleElementReferenceException">If the element is no longer valid.</exception>
        Task<IWebDriver> Frame(IWebElement frameElement, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Select the parent frame of the currently selected frame.
        /// </summary>
        /// <returns>An <see cref="IWebDriver" /> instance focused on the specified frame.</returns>
        Task<IWebDriver> ParentFrame(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Switches the focus of future commands for this driver to the window with the given name.
        /// </summary>
        /// <param name="windowName">The name of the window to select.</param>
        /// <returns>An <see cref="IWebDriver" /> instance focused on the given window.</returns>
        /// <exception cref="NoSuchWindowException">If the window cannot be found.</exception>
        Task<IWebDriver> Window(string windowName, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Selects either the first frame on the page or the main document when a page contains iFrames.
        /// </summary>
        /// <returns>An <see cref="IWebDriver" /> instance focused on the default frame.</returns>
        Task<IWebDriver> DefaultContent(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Switches to the element that currently has the focus, or the body element
        ///     if no element with focus can be detected.
        /// </summary>
        /// <returns>
        ///     An <see cref="IWebElement" /> instance representing the element
        ///     with the focus, or the body element if no element with focus can be detected.
        /// </returns>
        Task<IWebElement> ActiveElement(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///     Switches to the currently active modal dialog for this particular driver instance.
        /// </summary>
        /// <returns>A handle to the dialog.</returns>
        Task<IAlert> Alert(CancellationToken cancellationToken = new CancellationToken());
    }
}