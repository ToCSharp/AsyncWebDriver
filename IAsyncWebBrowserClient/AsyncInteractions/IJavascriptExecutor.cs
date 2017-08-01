// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.WebBrowser.AsyncInteractions
{
    /// <summary>
    ///     Defines the interface through which the user can execute JavaScript.
    /// </summary>
    public interface IJavaScriptExecutor
    {
        /// <summary>
        ///     Executes JavaScript in the context of the currently selected frame or window.
        /// </summary>
        /// <param name="script">The JavaScript code to execute.</param>
        /// <param name="args">The arguments to the script.</param>
        /// <returns>The value returned by the script.</returns>
        /// <remarks>
        ///     <para>
        ///         The <see cref="ExecuteScript" />method executes JavaScript in the context of
        ///         the currently selected frame or window. This means that "document" will refer
        ///         to the current document. If the script has a return value, then the following
        ///         steps will be taken:
        ///     </para>
        ///     <para>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>For an HTML element, this method returns a <see cref="IWebElement" /></description>
        ///             </item>
        ///             <item>
        ///                 <description>For a number, a <see cref="long " /> is returned</description>
        ///             </item>
        ///             <item>
        ///                 <description>For a boolean, a <see cref="bool " /> is returned</description>
        ///             </item>
        ///             <item>
        ///                 <description>For all other cases a <see cref="string " /> is returned.</description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     For an array,we check the first element, and attempt to return a
        ///                     <see cref="List{T}" /> of that type, following the rules above. Nested lists are not
        ///                     supported.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     If the value is null or there is no return value,
        ///                     <see langword="null" /> is returned.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        ///     <para>
        ///         Arguments must be a number (which will be converted to a <see cref="long " />),
        ///         a <see cref="bool " />, a <see cref="string " /> or a <see cref="IWebElement" />.
        ///         An exception will be thrown if the arguments do not meet these criteria.
        ///         The arguments will be made available to the JavaScript via the "arguments" magic
        ///         variable, as if the function were called via "Function.apply"
        ///     </para>
        /// </remarks>
        Task<object> ExecuteScript(string script, CancellationToken cancellationToken = new CancellationToken(),
            params object[] args);

        /// <summary>
        ///     Executes JavaScript asynchronously in the context of the currently selected frame or window.
        /// </summary>
        /// <param name="script">The JavaScript code to execute.</param>
        /// <param name="args">The arguments to the script.</param>
        /// <returns>The value returned by the script.</returns>
        Task<object> ExecuteAsyncScript(string script, CancellationToken cancellationToken = new CancellationToken(),
            params object[] args);
    }
}