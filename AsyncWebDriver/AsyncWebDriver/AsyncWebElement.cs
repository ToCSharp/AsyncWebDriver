// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Zu.AsyncWebDriver.Internal;
using Zu.WebBrowser.BasicTypes;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.AsyncWebDriver.Remote
{
    /// <summary>
    ///     RemoteWebElement allows you to have access to specific items that are found on the page
    /// </summary>
    /// <seealso cref = "IWebElement"/>
    /// <seealso cref = "ILocatable"/>
    public class AsyncWebElement : IWebElement, IFindsByLinkText, IFindsById, IFindsByName, IFindsByTagName, IFindsByClassName, IFindsByXPath, IFindsByPartialLinkText, IFindsByCssSelector, IWrapsDriver, ITakesScreenshot, ILocatable, IWebElementReference
    {
        private readonly WebDriver driver;
        /// <summary>
        ///     Initializes a new instance of the <see cref = "AsyncWebElement"/> class.
        /// </summary>
        /// <param name = "parentDriver">The <see cref = "WebDriver"/> instance hosting this element.</param>
        /// <param name = "id">The ID assigned to the element.</param>
        public AsyncWebElement(WebDriver parentDriver, string id)
        {
            driver = parentDriver;
            InternalElementId = id;
        }

        public int SimpleCommandsTimeoutMs
        {
            get;
            set;
        }

        = 10000;
        /// <summary>
        ///     Gets the ID of the element.
        /// </summary>
        /// <remarks>
        ///     This property is internal to the WebDriver instance, and is
        ///     not intended to be used in your code. The element's ID has no meaning
        ///     outside of internal WebDriver usage, so it would be improper to scope
        ///     it as public. However, both subclasses of <see cref = "AsyncWebElement"/>
        ///     and the parent driver hosting the element have a need to access the
        ///     internal element ID. Therefore, we have two properties returning the
        ///     same value, one scoped as internal, the other as protected.
        /// </remarks>
        internal string InternalElementId
        {
            get;
        }

        /// <summary>
        ///     Gets the ID of the element
        /// </summary>
        /// <remarks>
        ///     This property is internal to the WebDriver instance, and is
        ///     not intended to be used in your code. The element's ID has no meaning
        ///     outside of internal WebDriver usage, so it would be improper to scope
        ///     it as public. However, both subclasses of <see cref = "AsyncWebElement"/>
        ///     and the parent driver hosting the element have a need to access the
        ///     internal element ID. Therefore, we have two properties returning the
        ///     same value, one scoped as internal, the other as protected.
        /// </remarks>
        public string Id => InternalElementId;
        public Task<string> OuterHTML => GetProperty("outerHTML");
        public Task<string> InnerHTML => GetProperty("innerHTML");
#region FindElement
        public Task<IWebElement> FindElementByClassName(string className, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            // Element finding mechanism is not allowed by the W3C WebDriver
            // specification, but rather should be implemented as a function
            // of other finder mechanisms as documented in the spec.
            // Implementation after spec reaches recommendation should be as
            // follows:
            // return this.FindElement("css selector", "." + className);
            if (driver.IsSpecificationCompliant)
                return FindElement("css selector", "." + WebDriver.EscapeCssSelector(className), notElementId, timeout, cancellationToken);
            return FindElement("class name", className, notElementId, timeout, cancellationToken);
        }

        /// <summary>
        ///     Finds the first element in the page that matches the CSS Class supplied
        /// </summary>
        /// <param name = "className">CSS class name of the element on the page</param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// IWebElement elem = driver.FindElementByClassName("classname")
        /// </code>
        /// </example>
        public Task<IWebElement> FindElementByClassName(string className, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByClassName(className, null, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementByClassName(string className, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByClassName(className, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementByClassName(string className, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByClassName(className, null, timeout, cancellationToken);
        }

        public Task<IWebElement> FindElementByClassName(string className, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByClassName(className, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<IWebElement> FindElementByClassNameOrDefault(string className, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByClassName(className, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByClassNameOrDefault(string className, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByClassName(className, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByClassNameOrDefault(string className, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByClassName(className, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByClassNameOrDefault(string className, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByClassName(className, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByClassName(string className, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.IsSpecificationCompliant)
            {
                var selector = EscapeCssSelector(className);
                if (selector.Contains(" "))
                    throw new InvalidSelectorException("Compound class names not allowed. Cannot have whitespace in class name. Use CSS selectors instead.");
                return FindElements("css selector", "." + selector, notElementId, timeout, cancellationToken);
            }

            return FindElements("class name", className, notElementId, timeout, cancellationToken);
        }

        /// <summary>
        ///     Finds a list of elements that match the class name supplied
        /// </summary>
        /// <param name = "className">CSS class name of the elements on the page</param>
        /// <returns>ReadOnlyCollection of IWebElement object so that you can interact with those objects</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> elem = driver.FindElementsByClassName("classname")
        /// </code>
        /// </example>
        public Task<ReadOnlyCollection<IWebElement>> FindElementsByClassName(string className, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByClassName(className, null, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByClassName(string className, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByClassName(className, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByClassName(string className, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByClassName(className, null, timeout, cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByClassName(string className, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByClassName(className, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByClassNameOrDefault(string className, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByClassName(className, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByClassNameOrDefault(string className, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByClassName(className, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByClassNameOrDefault(string className, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByClassName(className, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByClassNameOrDefault(string className, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByClassName(className, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public Task<IWebElement> FindElementByCssSelector(string cssSelector, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElement("css selector", cssSelector, notElementId, timeout, cancellationToken);
        }

        /// <summary>
        ///     Finds the first element matching the specified CSS selector.
        /// </summary>
        /// <param name = "cssSelector">The id to match.</param>
        /// <returns>The first <see cref = "IWebElement"/> matching the criteria.</returns>
        public Task<IWebElement> FindElementByCssSelector(string cssSelector, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByCssSelector(cssSelector, null, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementByCssSelector(string cssSelector, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByCssSelector(cssSelector, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementByCssSelector(string cssSelector, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByCssSelector(cssSelector, null, timeout, cancellationToken);
        }

        public Task<IWebElement> FindElementByCssSelector(string cssSelector, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByCssSelector(cssSelector, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<IWebElement> FindElementByCssSelectorOrDefault(string cssSelector, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByCssSelector(cssSelector, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByCssSelectorOrDefault(string cssSelector, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByCssSelector(cssSelector, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByCssSelectorOrDefault(string cssSelector, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByCssSelector(cssSelector, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByCssSelectorOrDefault(string cssSelector, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByCssSelector(cssSelector, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelector(string cssSelector, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElements("css selector", cssSelector, notElementId, timeout, cancellationToken);
        }

        /// <summary>
        ///     Finds all elements matching the specified CSS selector.
        /// </summary>
        /// <param name = "cssSelector">The CSS selector to match.</param>
        /// <returns>
        ///     A <see cref = "ReadOnlyCollection{T}"/> containing all
        ///     <see cref = "IWebElement">IWebElements</see> matching the criteria.
        /// </returns>
        public Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelector(string cssSelector, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByCssSelector(cssSelector, null, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelector(string cssSelector, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByCssSelector(cssSelector, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelector(string cssSelector, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByCssSelector(cssSelector, null, timeout, cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelector(string cssSelector, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByCssSelector(cssSelector, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelectorOrDefault(string cssSelector, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByCssSelector(cssSelector, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelectorOrDefault(string cssSelector, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByCssSelector(cssSelector, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelectorOrDefault(string cssSelector, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByCssSelector(cssSelector, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelectorOrDefault(string cssSelector, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByCssSelector(cssSelector, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public Task<IWebElement> FindElementById(string id, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.IsSpecificationCompliant)
                return FindElement("css selector", "#" + EscapeCssSelector(id), notElementId, timeout, cancellationToken);
            return FindElement("id", id, notElementId, timeout, cancellationToken);
        }

        /// <summary>
        ///     Finds the first element in the page that matches the ID supplied
        /// </summary>
        /// <param name = "id">ID of the element</param>
        /// <returns>IWebElement object so that you can interact with that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// IWebElement elem = driver.FindElementById("id")
        /// </code>
        /// </example>
        public Task<IWebElement> FindElementById(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementById(id, null, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementById(string id, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementById(id, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementById(string id, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementById(id, null, timeout, cancellationToken);
        }

        public Task<IWebElement> FindElementById(string id, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementById(id, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<IWebElement> FindElementByIdOrDefault(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementById(id, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByIdOrDefault(string id, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementById(id, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByIdOrDefault(string id, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementById(id, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByIdOrDefault(string id, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementById(id, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsById(string id, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.IsSpecificationCompliant)
            {
                var selector = EscapeCssSelector(id);
                if (string.IsNullOrEmpty(selector))
                    return Task.FromResult(new List<IWebElement>().AsReadOnly());
                return FindElements("css selector", "#" + selector, notElementId, timeout, cancellationToken);
            }

            return FindElements("id", id, notElementId, timeout, cancellationToken);
        }

        /// <summary>
        ///     Finds the first element in the page that matches the ID supplied
        /// </summary>
        /// <param name = "id">ID of the Element</param>
        /// <returns>ReadOnlyCollection of Elements that match the object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> elem = driver.FindElementsById("id")
        /// </code>
        /// </example>
        public Task<ReadOnlyCollection<IWebElement>> FindElementsById(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsById(id, null, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsById(string id, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsById(id, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsById(string id, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsById(id, null, timeout, cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsById(string id, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsById(id, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByIdOrDefault(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsById(id, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByIdOrDefault(string id, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsById(id, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByIdOrDefault(string id, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsById(id, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByIdOrDefault(string id, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsById(id, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public Task<IWebElement> FindElementByLinkText(string linkText, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElement("link text", linkText, notElementId, timeout, cancellationToken);
        }

        /// <summary>
        ///     Finds the first of elements that match the link text supplied
        /// </summary>
        /// <param name = "linkText">Link text of element </param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// IWebElement elem = driver.FindElementByLinkText("linktext")
        /// </code>
        /// </example>
        public Task<IWebElement> FindElementByLinkText(string linkText, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByLinkText(linkText, null, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementByLinkText(string linkText, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByLinkText(linkText, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementByLinkText(string linkText, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByLinkText(linkText, null, timeout, cancellationToken);
        }

        public Task<IWebElement> FindElementByLinkText(string linkText, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByLinkText(linkText, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<IWebElement> FindElementByLinkTextOrDefault(string linkText, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByLinkText(linkText, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByLinkTextOrDefault(string linkText, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByLinkText(linkText, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByLinkTextOrDefault(string linkText, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByLinkText(linkText, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByLinkTextOrDefault(string linkText, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByLinkText(linkText, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkText(string linkText, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElements("link text", linkText, notElementId, timeout, cancellationToken);
        }

        /// <summary>
        ///     Finds the first of elements that match the link text supplied
        /// </summary>
        /// <param name = "linkText">Link text of element </param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> elem = driver.FindElementsByLinkText("linktext")
        /// </code>
        /// </example>
        public Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkText(string linkText, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByLinkText(linkText, null, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkText(string linkText, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByLinkText(linkText, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkText(string linkText, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByLinkText(linkText, null, timeout, cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkText(string linkText, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByLinkText(linkText, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkTextOrDefault(string linkText, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByLinkText(linkText, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkTextOrDefault(string linkText, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByLinkText(linkText, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkTextOrDefault(string linkText, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByLinkText(linkText, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkTextOrDefault(string linkText, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByLinkText(linkText, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public Task<IWebElement> FindElementByName(string name, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.IsSpecificationCompliant)
                return FindElement("css selector", "*[name=\"" + name + "\"]", notElementId, timeout, cancellationToken);
            return FindElement("name", name, notElementId, timeout, cancellationToken);
        }

        /// <summary>
        ///     Finds the first of elements that match the name supplied
        /// </summary>
        /// <param name = "name">Name of the element</param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// elem = driver.FindElementsByName("name")
        /// </code>
        /// </example>
        public Task<IWebElement> FindElementByName(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByName(name, null, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementByName(string name, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByName(name, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementByName(string name, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByName(name, null, timeout, cancellationToken);
        }

        public Task<IWebElement> FindElementByName(string name, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByName(name, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<IWebElement> FindElementByNameOrDefault(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByName(name, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByNameOrDefault(string name, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByName(name, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByNameOrDefault(string name, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByName(name, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByNameOrDefault(string name, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByName(name, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByName(string name, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.IsSpecificationCompliant)
                return FindElements("css selector", "*[name=\"" + name + "\"]", notElementId, timeout, cancellationToken);
            return FindElements("name", name, cancellationToken);
        }

        /// <summary>
        ///     Finds a list of elements that match the name supplied
        /// </summary>
        /// <param name = "name">Name of element</param>
        /// <returns>ReadOnlyCollect of IWebElement objects so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> elem = driver.FindElementsByName("name")
        /// </code>
        /// </example>
        public Task<ReadOnlyCollection<IWebElement>> FindElementsByName(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByName(name, null, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByName(string name, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByName(name, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByName(string name, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByName(name, null, timeout, cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByName(string name, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByName(name, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByNameOrDefault(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByName(name, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByNameOrDefault(string name, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByName(name, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByNameOrDefault(string name, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByName(name, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByNameOrDefault(string name, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByName(name, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public Task<IWebElement> FindElementByPartialLinkText(string partialLinkText, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElement("partial link text", partialLinkText, notElementId, timeout, cancellationToken);
        }

        /// <summary>
        ///     Finds the first of elements that match the part of the link text supplied
        /// </summary>
        /// <param name = "partialLinkText">part of the link text</param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// IWebElement elem = driver.FindElementsByPartialLinkText("partOfLink")
        /// </code>
        /// </example>
        public Task<IWebElement> FindElementByPartialLinkText(string partialLinkText, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByPartialLinkText(partialLinkText, null, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementByPartialLinkText(string partialLinkText, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByPartialLinkText(partialLinkText, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementByPartialLinkText(string partialLinkText, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByPartialLinkText(partialLinkText, null, timeout, cancellationToken);
        }

        public Task<IWebElement> FindElementByPartialLinkText(string partialLinkText, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByPartialLinkText(partialLinkText, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<IWebElement> FindElementByPartialLinkTextOrDefault(string partialLinkText, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByPartialLinkText(partialLinkText, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByPartialLinkTextOrDefault(string partialLinkText, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByPartialLinkText(partialLinkText, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByPartialLinkTextOrDefault(string partialLinkText, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByPartialLinkText(partialLinkText, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByPartialLinkTextOrDefault(string partialLinkText, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByPartialLinkText(partialLinkText, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkText(string partialLinkText, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElements("partial link text", partialLinkText, notElementId, timeout, cancellationToken);
        }

        /// <summary>
        ///     Finds a list of elements that match the link text supplied
        /// </summary>
        /// <param name = "partialLinkText">part of the link text</param>
        /// <returns>
        ///     ReadOnlyCollection<![CDATA[<IWebElement>]]> objects so that you can interact that object
        /// </returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> elem = driver.FindElementsByPartialLinkText("partOfTheLink")
        /// </code>
        /// </example>
        public Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkText(string partialLinkText, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByPartialLinkText(partialLinkText, null, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkText(string partialLinkText, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByPartialLinkText(partialLinkText, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkText(string partialLinkText, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByPartialLinkText(partialLinkText, null, timeout, cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkText(string partialLinkText, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByPartialLinkText(partialLinkText, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkTextOrDefault(string partialLinkText, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByPartialLinkText(partialLinkText, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkTextOrDefault(string partialLinkText, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByPartialLinkText(partialLinkText, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkTextOrDefault(string partialLinkText, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByPartialLinkText(partialLinkText, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkTextOrDefault(string partialLinkText, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByPartialLinkText(partialLinkText, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public Task<IWebElement> FindElementByTagName(string tagName, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.IsSpecificationCompliant)
                return FindElement("css selector", tagName, notElementId, timeout, cancellationToken);
            return FindElement("tag name", tagName, notElementId, timeout, cancellationToken);
        }

        /// <summary>
        ///     Finds the first of elements that match the DOM Tag supplied
        /// </summary>
        /// <param name = "tagName">tag name of the element</param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// IWebElement elem = driver.FindElementsByTagName("tag")
        /// </code>
        /// </example>
        public Task<IWebElement> FindElementByTagName(string tagName, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByTagName(tagName, null, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementByTagName(string tagName, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByTagName(tagName, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementByTagName(string tagName, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByTagName(tagName, null, timeout, cancellationToken);
        }

        public Task<IWebElement> FindElementByTagName(string tagName, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByTagName(tagName, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<IWebElement> FindElementByTagNameOrDefault(string tagName, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByTagName(tagName, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByTagNameOrDefault(string tagName, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByTagName(tagName, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByTagNameOrDefault(string tagName, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByTagName(tagName, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByTagNameOrDefault(string tagName, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByTagName(tagName, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByTagName(string tagName, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.IsSpecificationCompliant)
                return FindElements("css selector", tagName, notElementId, timeout, cancellationToken);
            return FindElements("tag name", tagName, cancellationToken);
        }

        /// <summary>
        ///     Finds a list of elements that match the DOM Tag supplied
        /// </summary>
        /// <param name = "tagName">DOM Tag of the element on the page</param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> elem = driver.FindElementsByTagName("tag")
        /// </code>
        /// </example>
        public Task<ReadOnlyCollection<IWebElement>> FindElementsByTagName(string tagName, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByTagName(tagName, null, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByTagName(string tagName, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByTagName(tagName, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByTagName(string tagName, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByTagName(tagName, null, timeout, cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByTagName(string tagName, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByTagName(tagName, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByTagNameOrDefault(string tagName, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByTagName(tagName, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByTagNameOrDefault(string tagName, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByTagName(tagName, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByTagNameOrDefault(string tagName, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByTagName(tagName, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByTagNameOrDefault(string tagName, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByTagName(tagName, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public Task<IWebElement> FindElementByXPath(string xpath, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElement("xpath", xpath, notElementId, timeout, cancellationToken);
        }

        /// <summary>
        ///     Finds the first of elements that match the XPath supplied
        /// </summary>
        /// <param name = "xpath">xpath to the element</param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// IWebElement elem = driver.FindElementsByXPath("//table/tbody/tr/td/a");
        /// </code>
        /// </example>
        public Task<IWebElement> FindElementByXPath(string xpath, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByXPath(xpath, null, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementByXPath(string xpath, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByXPath(xpath, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<IWebElement> FindElementByXPath(string xpath, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByXPath(xpath, null, timeout, cancellationToken);
        }

        public Task<IWebElement> FindElementByXPath(string xpath, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementByXPath(xpath, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<IWebElement> FindElementByXPathOrDefault(string xpath, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByXPath(xpath, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByXPathOrDefault(string xpath, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByXPath(xpath, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByXPathOrDefault(string xpath, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByXPath(xpath, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IWebElement> FindElementByXPathOrDefault(string xpath, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementByXPath(xpath, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByXPath(string xpath, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElements("xpath", xpath, notElementId, timeout, cancellationToken);
        }

        /// <summary>
        ///     Finds a list of elements that match the XPath supplied
        /// </summary>
        /// <param name = "xpath">xpath to element on the page</param>
        /// <returns>ReadOnlyCollection of IWebElement objects so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> elem = driver.FindElementsByXpath("//tr/td/a")
        /// </code>
        /// </example>
        public Task<ReadOnlyCollection<IWebElement>> FindElementsByXPath(string xpath, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByXPath(xpath, null, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByXPath(string xpath, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByXPath(xpath, notElementId, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByXPath(string xpath, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByXPath(xpath, null, timeout, cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByXPath(string xpath, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByXPath(xpath, null, TimeSpan.FromMilliseconds(timeoutMs), cancellationToken);
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByXPathOrDefault(string xpath, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByXPath(xpath, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByXPathOrDefault(string xpath, string notElementId, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByXPath(xpath, notElementId, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByXPathOrDefault(string xpath, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByXPath(xpath, timeout, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByXPathOrDefault(string xpath, int timeoutMs, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                return await FindElementsByXPath(xpath, timeoutMs, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        /// <summary>
        ///     Finds all <see cref = "IWebElement">IWebElements</see> within the current context
        ///     using the given mechanism.
        /// </summary>
        /// <param name = "by">The locating mechanism to use.</param>
        /// <returns>
        ///     A <see cref = "ReadOnlyCollection{T}"/> of all <see cref = "IWebElement">WebElements</see>
        ///     matching the current criteria, or an empty list if nothing matches.
        /// </returns>
        public Task<ReadOnlyCollection<IWebElement>> FindElements(By by, CancellationToken cancellationToken = new CancellationToken())
        {
            if (by == null)
                throw new ArgumentNullException("by", "by cannot be null");
            return by.FindElements(this, cancellationToken);
        }

        /// <summary>
        ///     Finds the first <see cref = "IWebElement"/> using the given method.
        /// </summary>
        /// <param name = "by">The locating mechanism to use.</param>
        /// <returns>The first matching <see cref = "IWebElement"/> on the current context.</returns>
        /// <exception cref = "NoSuchElementException">If no element matches the criteria.</exception>
        public Task<IWebElement> FindElement(By by, CancellationToken cancellationToken = new CancellationToken())
        {
            if (by == null)
                throw new ArgumentNullException("by", "by cannot be null");
            return by.FindElement(this, cancellationToken);
        }

        public Task<IWebElement> FindElementByIdStartsWith(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            var selector = EscapeCssSelector(id);
            return FindElement("css selector", $"[id^={selector}]", cancellationToken);
        }

        public Task<IWebElement> FindElementByIdStartsWithOrDefault(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var selector = EscapeCssSelector(id);
                return FindElement("css selector", $"[id^={selector}]", cancellationToken);
            }
            catch
            {
                return null;
            }
        }

        public Task<IWebElement> FindElementByIdEndsWith(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            var selector = EscapeCssSelector(id);
            return FindElement("css selector", $"[id$={selector}]", cancellationToken);
        }

        public Task<IWebElement> FindElementByIdEndsWithOrDefault(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var selector = EscapeCssSelector(id);
                return FindElement("css selector", $"[id$={selector}]", cancellationToken);
            }
            catch
            {
                return null;
            }
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByIdStartsWith(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            var selector = EscapeCssSelector(id);
            if (string.IsNullOrEmpty(selector))
                return Task.FromResult(new List<IWebElement>().AsReadOnly());
            return FindElements("css selector", $"[id^={selector}]", cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> FindElementsByIdEndsWith(string id, CancellationToken cancellationToken = new CancellationToken())
        {
            var selector = EscapeCssSelector(id);
            if (string.IsNullOrEmpty(selector))
                return Task.FromResult(new List<IWebElement>().AsReadOnly());
            return FindElements("css selector", $"[id$={selector}]", cancellationToken);
        }

        public static string EscapeCssSelector(string selector)
        {
            var escaped = Regex.Replace(selector, @"(['""\\#.:;,!?+<>=~*^$|%&@`{}\-/\[\]\(\)])", @"\$1");
            if (selector.Length > 0 && char.IsDigit(selector[0]))
                escaped = @"\" + (30 + int.Parse(selector.Substring(0, 1), CultureInfo.InvariantCulture)).ToString(CultureInfo.InvariantCulture) + " " + selector.Substring(1);
            return escaped;
        }

        public async Task<IWebElement> FindElement(string mechanism, string value, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            JToken res = null;
            try
            {
                res = await driver.browserClient.Elements.FindElement(mechanism, value, InternalElementId, notElementId, timeout, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }

            return driver.GetElementFromResponse(res);
        }

        /// <summary>
        ///     Finds a child element matching the given mechanism and value.
        /// </summary>
        /// <param name = "mechanism">The mechanism by which to find the element.</param>
        /// <param name = "value">The value to use to search for the element.</param>
        /// <returns>The first <see cref = "IWebElement"/> matching the given criteria.</returns>
        public Task<IWebElement> FindElement(string mechanism, string value, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElement(mechanism, value, null, default (TimeSpan), cancellationToken);
        }

        public Task<ReadOnlyCollection<IWebElement>> Children(CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElementsByXPath("child::*", cancellationToken);
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElements(string mechanism, string value, string notElementId, TimeSpan timeout, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            JToken res = null;
            try
            {
                res = await driver.browserClient.Elements.FindElements(mechanism, value, InternalElementId, notElementId, timeout, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }

            return driver.GetElementsFromResponse(res);
        }

        /// <summary>
        ///     Finds all child elements matching the given mechanism and value.
        /// </summary>
        /// <param name = "mechanism">The mechanism by which to find the elements.</param>
        /// <param name = "value">The value to use to search for the elements.</param>
        /// <returns>A collection of all of the <see cref = "IWebElement">IWebElements</see> matching the given criteria.</returns>
        public Task<ReadOnlyCollection<IWebElement>> FindElements(string mechanism, string value, CancellationToken cancellationToken = new CancellationToken())
        {
            return FindElements(mechanism, value, null, default (TimeSpan), cancellationToken);
        }

#endregion
        /// <summary>
        ///     Gets the point where the element would be when scrolled into view.
        /// </summary>
        public Task<WebPoint> LocationOnScreenOnceScrolledIntoView(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets the coordinates identifying the location of this element using
        ///     various frames of reference.
        /// </summary>
        public ICoordinates Coordinates => driver.browserClient.Coordinates; // new RemoteCoordinates(this);
        string IWebElementReference.ElementReferenceId
        {
            get
            {
                return this.InternalElementId;
            }
        }

        /// <summary>
        /// Converts an object into an object that represents an element for the wire protocol.
        /// </summary>
        /// <returns>A <see cref = "Dictionary{TKey, TValue}"/> that represents an element in the wire protocol.</returns>
        Dictionary<string, object> IWebElementReference.ToDictionary()
        {
            Dictionary<string, object> elementDictionary = new Dictionary<string, object>();
            elementDictionary.Add("element-6066-11e4-a52e-4f735466cecf", this.InternalElementId);
            return elementDictionary;
        }

        /// <summary>
        ///     Gets a <see cref = "Screenshot"/> object representing the image of this element on the screen.
        /// </summary>
        /// <returns>A <see cref = "Screenshot"/> object containing the image.</returns>
        public async Task<Screenshot> GetScreenshot(CancellationToken cancellationToken = new CancellationToken())
        {
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("id", this.elementId);
            //// Get the screenshot as base64.
            //Response screenshotResponse = this.Execute(DriverCommand.ElementScreenshot, parameters);
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            var res = "";
            try
            {
                res = await driver.browserClient.Elements.GetElementText(InternalElementId, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }

            var base64 = res; // screenshotResponse.Value.ToString();
            // ... and convert it.
            return new Screenshot(base64);
        }

        /// <summary>
        ///     Gets the tag name of this element.
        /// </summary>
        /// <remarks>
        ///     The <see cref = "TagName"/> property returns the tag name of the
        ///     element, not the value of the name attribute. For example, it will return
        ///     "input" for an element specified by the HTML markup &lt;input name="foo" /&gt;.
        /// </remarks>
        public async Task<string> TagName(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            try
            {
                var res = await driver.browserClient.Elements.GetElementTagName(InternalElementId, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
                return res;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Gets the innerText of this element, without any leading or trailing whitespace,
        ///     and with other whitespace collapsed.
        /// </summary>
        public async Task<string> Text(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            try
            {
                var res = await driver.browserClient.Elements.GetElementText(InternalElementId, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
                return res;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether or not this element is enabled.
        /// </summary>
        public async Task<bool> Enabled(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            try
            {
                var res = await driver.browserClient.Elements.IsElementEnabled(InternalElementId, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
                return res;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether or not this element is selected.
        /// </summary>
        public async Task<bool> Selected(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            try
            {
                var res = await driver.browserClient.Elements.IsElementSelected(InternalElementId, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
                return res;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Gets a <see cref = "Point"/> object containing the coordinates of the upper-left corner
        ///     of this element relative to the upper-left corner of the page.
        /// </summary>
        public async Task<WebPoint> Location(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            try
            {
                var res = await driver.browserClient.Elements.GetElementLocation(InternalElementId, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
                return res;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Gets a <see cref = "Size"/> object containing the height and width of this element.
        /// </summary>
        public async Task<WebSize> Size(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            try
            {
                var res = await driver.browserClient.Elements.GetElementSize(InternalElementId, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
                return res;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether or not this element is displayed.
        /// </summary>
        /// <remarks>
        ///     The <see cref = "Displayed"/> property avoids the problem
        ///     of having to parse an element's "style" attribute to determine
        ///     visibility of an element.
        /// </remarks>
        public async Task<bool> Displayed(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            try
            {
                var res = await driver.browserClient.Elements.IsElementDisplayed(InternalElementId, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
                return res;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Clears the content of this element.
        /// </summary>
        /// <remarks>
        ///     If this element is a text entry element, the <see cref = "Clear"/>
        ///     method will clear the value. It has no effect on other elements. Text entry elements
        ///     are defined as elements with INPUT or TEXTAREA tags.
        /// </remarks>
        public async Task Clear(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            try
            {
                var res = await driver.browserClient.Elements.ClearElement(InternalElementId, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Simulates typing text into the element.
        /// </summary>
        /// <param name = "text">The text to type into the element.</param>
        /// <remarks>
        ///     The text to be typed may include special characters like arrow keys,
        ///     backspaces, function keys, and so on. Valid special keys are defined in
        ///     <see cref = "Keys"/>.
        /// </remarks>
        /// <seealso cref = "Keys"/>
        public async Task SendKeys(string text, CancellationToken cancellationToken = new CancellationToken())
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text), "text cannot be null");
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            try
            {
                var res = await driver.browserClient.Elements.SendKeysToElement(InternalElementId, text, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Submits this element to the web server.
        /// </summary>
        /// <remarks>
        ///     If this current element is a form, or an element within a form,
        ///     then this will be submitted to the web server. If this causes the current
        ///     page to change, then this method will attempt to block until the new page
        ///     is loaded.
        /// </remarks>
        public async Task Submit(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.IsSpecificationCompliant)
            {
                try
                {
                    var form = await FindElement(By.XPath("./ancestor-or-self::form")).ConfigureAwait(false);
                    await driver.ExecuteScript("var e = arguments[0].ownerDocument.createEvent('Event');" + "e.initEvent('submit', true, true);" + "if (arguments[0].dispatchEvent(e)) { arguments[0].submit(); }", cancellationToken, form).ConfigureAwait(false);
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                if (driver.browserClient == null)
                    throw new WebDriverException("no browserClient");
                try
                {
                    var res = await driver.browserClient.Elements.IsElementEnabled(InternalElementId, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
                    var res2 = await driver.browserClient.Elements.SubmitElement(InternalElementId, cancellationToken: cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        ///     Clicks this element.
        /// </summary>
        /// <remarks>
        ///     Click this element. If the click causes a new page to load, the <see cref = "Click"/>
        ///     method will attempt to block until the page has loaded. After calling the
        ///     <see cref = "Click"/> method, you should discard all references to this
        ///     element unless you know that the element and the page will still be present.
        ///     Otherwise, any further operations performed on this element will have an undefined
        ///     behavior.
        /// </remarks>
        public async Task Click(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            try
            {
                await driver.browserClient.Elements.Click(InternalElementId, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Gets the value of the specified attribute for this element.
        /// </summary>
        /// <param name = "attributeName">The name of the attribute.</param>
        /// <returns>
        ///     The attribute's current value. Returns a <see langword = "null"/> if the
        ///     value is not set.
        /// </returns>
        /// <remarks>
        ///     The <see cref = "GetAttribute"/> method will return the current value
        ///     of the attribute, even if the value has been modified after the page has been
        ///     loaded. Note that the value of the following attributes will be returned even if
        ///     there is no explicit attribute on the element:
        ///     <list type = "table">
        ///         <listheader>
        ///             <term>Attribute name</term>
        ///             <term>Value returned if not explicitly specified</term>
        ///             <term>Valid element types</term>
        ///         </listheader>
        ///         <item>
        ///             <description>checked</description>
        ///             <description>checked</description>
        ///             <description>Check Box</description>
        ///         </item>
        ///         <item>
        ///             <description>selected</description>
        ///             <description>selected</description>
        ///             <description>Options in Select elements</description>
        ///         </item>
        ///         <item>
        ///             <description>disabled</description>
        ///             <description>disabled</description>
        ///             <description>Input and other UI elements</description>
        ///         </item>
        ///     </list>
        /// </remarks>
        /// <exception cref = "StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public async Task<string> GetAttribute(string attributeName, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            try
            {
                var res = await driver.browserClient.Elements.GetElementAttribute(InternalElementId, attributeName, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
                return res;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Gets the value of a CSS property of this element.
        /// </summary>
        /// <param name = "propertyName">The name of the CSS property to get the value of.</param>
        /// <returns>The value of the specified CSS property.</returns>
        /// <remarks>
        ///     The value returned by the <see cref = "GetCssValue"/>
        ///     method is likely to be unpredictable in a cross-browser environment.
        ///     Color values should be returned as hex strings. For example, a
        ///     "background-color" property set as "green" in the HTML source, will
        ///     return "#008000" for its value.
        /// </remarks>
        public async Task<string> GetCssValue(string propertyName, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            try
            {
                var res = await driver.browserClient.Elements.GetElementValueOfCssProperty(InternalElementId, propertyName, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
                return res;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Gets the <see cref = "IWebDriver"/> used to find this element.
        /// </summary>
        public IWebDriver WrappedDriver => driver;
        public override string ToString()
        {
            return "RemoteWebElement: " + InternalElementId;
        }

        public async Task<string> GetProperty(string propertyName, CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");
            try
            {
                var res = await driver.browserClient.Elements.GetElementProperty(InternalElementId, propertyName, cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs).ConfigureAwait(false);
                return res;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///     Method to get the hash code of the element
        /// </summary>
        /// <returns>Integer of the hash code for the element</returns>
        public override int GetHashCode()
        {
            return InternalElementId.GetHashCode();
        }

        /// <summary>
        ///     Compares if two elements are equal
        /// </summary>
        /// <param name = "obj">Object to compare against</param>
        /// <returns>A boolean if it is equal or not</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return InternalElementId == (obj as AsyncWebElement).InternalElementId;
        }

        public Task<string> UploadFile(string localFile, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> ToElementReference()
        {
            var elementDictionary = new Dictionary<string, object>();
            elementDictionary["element-6066-11e4-a52e-4f735466cecf"] = InternalElementId;
            return elementDictionary;
        }
    }
}