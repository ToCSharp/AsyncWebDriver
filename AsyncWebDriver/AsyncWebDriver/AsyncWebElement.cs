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
    /// <seealso cref="IWebElement" />
    /// <seealso cref="ILocatable" />
    public class AsyncWebElement : IWebElement, IFindsByLinkText, IFindsById, IFindsByName, IFindsByTagName,
        IFindsByClassName, IFindsByXPath, IFindsByPartialLinkText, IFindsByCssSelector, IWrapsDriver, ITakesScreenshot,
        ILocatable, IWebElementReference
    {
        private readonly WebDriver driver;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncWebElement" /> class.
        /// </summary>
        /// <param name="parentDriver">The <see cref="WebDriver" /> instance hosting this element.</param>
        /// <param name="id">The ID assigned to the element.</param>
        public AsyncWebElement(WebDriver parentDriver, string id)
        {
            driver = parentDriver;
            InternalElementId = id;
        }

        public int SimpleCommandsTimeoutMs { get; set; } = 10000;

        /// <summary>
        ///     Gets the ID of the element.
        /// </summary>
        /// <remarks>
        ///     This property is internal to the WebDriver instance, and is
        ///     not intended to be used in your code. The element's ID has no meaning
        ///     outside of internal WebDriver usage, so it would be improper to scope
        ///     it as public. However, both subclasses of <see cref="AsyncWebElement" />
        ///     and the parent driver hosting the element have a need to access the
        ///     internal element ID. Therefore, we have two properties returning the
        ///     same value, one scoped as internal, the other as protected.
        /// </remarks>
        internal string InternalElementId { get; }

        /// <summary>
        ///     Gets the ID of the element
        /// </summary>
        /// <remarks>
        ///     This property is internal to the WebDriver instance, and is
        ///     not intended to be used in your code. The element's ID has no meaning
        ///     outside of internal WebDriver usage, so it would be improper to scope
        ///     it as public. However, both subclasses of <see cref="AsyncWebElement" />
        ///     and the parent driver hosting the element have a need to access the
        ///     internal element ID. Therefore, we have two properties returning the
        ///     same value, one scoped as internal, the other as protected.
        /// </remarks>
        public string Id => InternalElementId;

        public Task<string> OuterHTML => GetProperty("outerHTML");
        public Task<string> InnerHTML => GetProperty("innerHTML");

        /// <summary>
        ///     Finds the first element in the page that matches the CSS Class supplied
        /// </summary>
        /// <param name="className">CSS class name of the element on the page</param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// IWebElement elem = driver.FindElementByClassName("classname")
        /// </code>
        /// </example>
        public async Task<IWebElement> FindElementByClassName(string className,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // Element finding mechanism is not allowed by the W3C WebDriver
            // specification, but rather should be implemented as a function
            // of other finder mechanisms as documented in the spec.
            // Implementation after spec reaches recommendation should be as
            // follows:
            // return this.FindElement("css selector", "." + className);
            if (driver.IsSpecificationCompliant)
                return await FindElement("css selector", "." + WebDriver.EscapeCssSelector(className),
                    cancellationToken);

            return await FindElement("class name", className, cancellationToken);
        }

        /// <summary>
        ///     Finds a list of elements that match the class name supplied
        /// </summary>
        /// <param name="className">CSS class name of the elements on the page</param>
        /// <returns>ReadOnlyCollection of IWebElement object so that you can interact with those objects</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> elem = driver.FindElementsByClassName("classname")
        /// </code>
        /// </example>
        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByClassName(string className,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // Element finding mechanism is not allowed by the W3C WebDriver
            // specification, but rather should be implemented as a function
            // of other finder mechanisms as documented in the spec.
            // Implementation after spec reaches recommendation should be as
            // follows:
            // return this.FindElements("css selector", "." + className);
            if (driver.IsSpecificationCompliant)
                return await FindElements("css selector", "." + WebDriver.EscapeCssSelector(className),
                    cancellationToken);

            return await FindElements("class name", className, cancellationToken);
        }

        /// <summary>
        ///     Finds the first element matching the specified CSS selector.
        /// </summary>
        /// <param name="cssSelector">The id to match.</param>
        /// <returns>The first <see cref="IWebElement" /> matching the criteria.</returns>
        public async Task<IWebElement> FindElementByCssSelector(string cssSelector,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await FindElement("css selector", cssSelector, cancellationToken);
        }

        /// <summary>
        ///     Finds all elements matching the specified CSS selector.
        /// </summary>
        /// <param name="cssSelector">The CSS selector to match.</param>
        /// <returns>
        ///     A <see cref="ReadOnlyCollection{T}" /> containing all
        ///     <see cref="IWebElement">IWebElements</see> matching the criteria.
        /// </returns>
        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByCssSelector(string cssSelector,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await FindElements("css selector", cssSelector, cancellationToken);
        }

        /// <summary>
        ///     Finds the first element in the page that matches the ID supplied
        /// </summary>
        /// <param name="id">ID of the element</param>
        /// <returns>IWebElement object so that you can interact with that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// IWebElement elem = driver.FindElementById("id")
        /// </code>
        /// </example>
        public async Task<IWebElement> FindElementById(string id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.IsSpecificationCompliant)
                return await FindElement("css selector", "#" + WebDriver.EscapeCssSelector(id), cancellationToken);

            return await FindElement("id", id, cancellationToken);
        }

        /// <summary>
        ///     Finds the first element in the page that matches the ID supplied
        /// </summary>
        /// <param name="id">ID of the Element</param>
        /// <returns>ReadOnlyCollection of Elements that match the object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> elem = driver.FindElementsById("id")
        /// </code>
        /// </example>
        public async Task<ReadOnlyCollection<IWebElement>> FindElementsById(string id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.IsSpecificationCompliant)
                return await FindElements("css selector", "#" + WebDriver.EscapeCssSelector(id), cancellationToken);

            return await FindElements("id", id, cancellationToken);
        }

        /// <summary>
        ///     Finds the first of elements that match the link text supplied
        /// </summary>
        /// <param name="linkText">Link text of element </param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// IWebElement elem = driver.FindElementByLinkText("linktext")
        /// </code>
        /// </example>
        public async Task<IWebElement> FindElementByLinkText(string linkText,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await FindElement("link text", linkText, cancellationToken);
        }

        /// <summary>
        ///     Finds the first of elements that match the link text supplied
        /// </summary>
        /// <param name="linkText">Link text of element </param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> elem = driver.FindElementsByLinkText("linktext")
        /// </code>
        /// </example>
        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByLinkText(string linkText,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await FindElements("link text", linkText, cancellationToken);
        }

        /// <summary>
        ///     Finds the first of elements that match the name supplied
        /// </summary>
        /// <param name="name">Name of the element</param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// elem = driver.FindElementsByName("name")
        /// </code>
        /// </example>
        public async Task<IWebElement> FindElementByName(string name,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // Element finding mechanism is not allowed by the W3C WebDriver
            // specification, but rather should be implemented as a function
            // of other finder mechanisms as documented in the spec.
            // Implementation after spec reaches recommendation should be as
            // follows:
            // return this.FindElement("css selector", "*[name=\"" + name + "\"]");
            if (driver.IsSpecificationCompliant)
                return await FindElement("css selector", "*[name=\"" + name + "\"]", cancellationToken);

            return await FindElement("name", name, cancellationToken);
        }

        /// <summary>
        ///     Finds a list of elements that match the name supplied
        /// </summary>
        /// <param name="name">Name of element</param>
        /// <returns>ReadOnlyCollect of IWebElement objects so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> elem = driver.FindElementsByName("name")
        /// </code>
        /// </example>
        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByName(string name,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // Element finding mechanism is not allowed by the W3C WebDriver
            // specification, but rather should be implemented as a function
            // of other finder mechanisms as documented in the spec.
            // Implementation after spec reaches recommendation should be as
            // follows:
            // return this.FindElements("css selector", "*[name=\"" + name + "\"]");
            if (driver.IsSpecificationCompliant)
                return await FindElements("css selector", "*[name=\"" + name + "\"]", cancellationToken);

            return await FindElements("name", name, cancellationToken);
        }

        /// <summary>
        ///     Finds the first of elements that match the part of the link text supplied
        /// </summary>
        /// <param name="partialLinkText">part of the link text</param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// IWebElement elem = driver.FindElementsByPartialLinkText("partOfLink")
        /// </code>
        /// </example>
        public async Task<IWebElement> FindElementByPartialLinkText(string partialLinkText,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await FindElement("partial link text", partialLinkText, cancellationToken);
        }

        /// <summary>
        ///     Finds a list of elements that match the link text supplied
        /// </summary>
        /// <param name="partialLinkText">part of the link text</param>
        /// <returns>
        ///     ReadOnlyCollection<![CDATA[<IWebElement>]]> objects so that you can interact that object
        /// </returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> elem = driver.FindElementsByPartialLinkText("partOfTheLink")
        /// </code>
        /// </example>
        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByPartialLinkText(string partialLinkText,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await FindElements("partial link text", partialLinkText, cancellationToken);
        }

        /// <summary>
        ///     Finds the first of elements that match the DOM Tag supplied
        /// </summary>
        /// <param name="tagName">tag name of the element</param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// IWebElement elem = driver.FindElementsByTagName("tag")
        /// </code>
        /// </example>
        public async Task<IWebElement> FindElementByTagName(string tagName,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // Element finding mechanism is not allowed by the W3C WebDriver
            // specification, but rather should be implemented as a function
            // of other finder mechanisms as documented in the spec.
            // Implementation after spec reaches recommendation should be as
            // follows:
            // return this.FindElement("css selector", tagName);
            if (driver.IsSpecificationCompliant)
                return await FindElement("css selector", tagName, cancellationToken);

            return await FindElement("tag name", tagName, cancellationToken);
        }

        /// <summary>
        ///     Finds a list of elements that match the DOM Tag supplied
        /// </summary>
        /// <param name="tagName">DOM Tag of the element on the page</param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> elem = driver.FindElementsByTagName("tag")
        /// </code>
        /// </example>
        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByTagName(string tagName,
            CancellationToken cancellationToken = new CancellationToken())
        {
            // Element finding mechanism is not allowed by the W3C WebDriver
            // specification, but rather should be implemented as a function
            // of other finder mechanisms as documented in the spec.
            // Implementation after spec reaches recommendation should be as
            // follows:
            // return this.FindElements("css selector", tagName);
            if (driver.IsSpecificationCompliant)
                return await FindElements("css selector", tagName, cancellationToken);

            return await FindElements("tag name", tagName, cancellationToken);
        }

        /// <summary>
        ///     Finds the first of elements that match the XPath supplied
        /// </summary>
        /// <param name="xpath">xpath to the element</param>
        /// <returns>IWebElement object so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// IWebElement elem = driver.FindElementsByXPath("//table/tbody/tr/td/a");
        /// </code>
        /// </example>
        public async Task<IWebElement> FindElementByXPath(string xpath,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await FindElement("xpath", xpath, cancellationToken);
        }

        /// <summary>
        ///     Finds a list of elements that match the XPath supplied
        /// </summary>
        /// <param name="xpath">xpath to element on the page</param>
        /// <returns>ReadOnlyCollection of IWebElement objects so that you can interact that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> elem = driver.FindElementsByXpath("//tr/td/a")
        /// </code>
        /// </example>
        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByXPath(string xpath,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await FindElements("xpath", xpath, cancellationToken);
        }

        /// <summary>
        ///     Gets the point where the element would be when scrolled into view.
        /// </summary>
        public Task<WebPoint> LocationOnScreenOnceScrolledIntoView(
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Gets the coordinates identifying the location of this element using
        ///     various frames of reference.
        /// </summary>
        public ICoordinates Coordinates => new RemoteCoordinates(this);

        string IWebElementReference.ElementReferenceId
        {
            get { return this.InternalElementId; }
        }
        /// <summary>
        /// Converts an object into an object that represents an element for the wire protocol.
        /// </summary>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that represents an element in the wire protocol.</returns>
        Dictionary<string, object> IWebElementReference.ToDictionary()
        {
            Dictionary<string, object> elementDictionary = new Dictionary<string, object>();
            elementDictionary.Add("element-6066-11e4-a52e-4f735466cecf", this.InternalElementId);
            return elementDictionary;
        }

        /// <summary>
        ///     Gets a <see cref="Screenshot" /> object representing the image of this element on the screen.
        /// </summary>
        /// <returns>A <see cref="Screenshot" /> object containing the image.</returns>
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
                res = await driver.browserClient.GetElementText(InternalElementId, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
            }
            catch (Exception ex)
            {
                return null;
            }

            var base64 = res; // screenshotResponse.Value.ToString();
            // ... and convert it.
            return new Screenshot(base64);
        }

        /// <summary>
        ///     Gets the tag name of this element.
        /// </summary>
        /// <remarks>
        ///     The <see cref="TagName" /> property returns the tag name of the
        ///     element, not the value of the name attribute. For example, it will return
        ///     "input" for an element specified by the HTML markup &lt;input name="foo" /&gt;.
        /// </remarks>
        public async Task<string> TagName(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var res = await driver.browserClient.GetElementTagName(InternalElementId, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
                return res;
            }
            catch (Exception ex)
            {
                return ex.ToString();
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
                var res = await driver.browserClient.GetElementText(InternalElementId, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
                return res;
            }
            catch (Exception ex)
            {
                return ex.ToString();
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
                var res = await driver.browserClient.IsElementEnabled(InternalElementId, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
                return res;
            }
            catch (Exception ex)
            {
                return false;
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
                var res = await driver.browserClient.IsElementSelected(InternalElementId, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        ///     Gets a <see cref="Point" /> object containing the coordinates of the upper-left corner
        ///     of this element relative to the upper-left corner of the page.
        /// </summary>
        public async Task<WebPoint> Location(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            //try
            //{
                var res = await driver.browserClient.GetElementLocation(InternalElementId, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
                return res;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }

        /// <summary>
        ///     Gets a <see cref="Size" /> object containing the height and width of this element.
        /// </summary>
        public async Task<WebSize> Size(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            //try
            //{
                var res = await driver.browserClient.GetElementSize(InternalElementId, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
                return res;
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }

        /// <summary>
        ///     Gets a value indicating whether or not this element is displayed.
        /// </summary>
        /// <remarks>
        ///     The <see cref="Displayed" /> property avoids the problem
        ///     of having to parse an element's "style" attribute to determine
        ///     visibility of an element.
        /// </remarks>
        public async Task<bool> Displayed(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var res = await driver.browserClient.IsElementDisplayed(InternalElementId, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
                return res;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        ///     Clears the content of this element.
        /// </summary>
        /// <remarks>
        ///     If this element is a text entry element, the <see cref="Clear" />
        ///     method will clear the value. It has no effect on other elements. Text entry elements
        ///     are defined as elements with INPUT or TEXTAREA tags.
        /// </remarks>
        public async Task Clear(CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var res = await driver.browserClient.ClearElement(InternalElementId, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///     Simulates typing text into the element.
        /// </summary>
        /// <param name="text">The text to type into the element.</param>
        /// <remarks>
        ///     The text to be typed may include special characters like arrow keys,
        ///     backspaces, function keys, and so on. Valid special keys are defined in
        ///     <see cref="Keys" />.
        /// </remarks>
        /// <seealso cref="Keys" />
        public async Task SendKeys(string text, CancellationToken cancellationToken = new CancellationToken())
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text), "text cannot be null");

            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var res = await driver.browserClient.SendKeysToElement(InternalElementId, text, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
            }
            catch (Exception ex)
            {
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
                var form = await FindElement(By.XPath("./ancestor-or-self::form"));
                await driver.ExecuteScript(
                    "var e = arguments[0].ownerDocument.createEvent('Event');" + "e.initEvent('submit', true, true);" +
                    "if (arguments[0].dispatchEvent(e)) { arguments[0].submit(); }", cancellationToken, form);
            }
            else
            {
                if (driver.browserClient == null)
                    throw new WebDriverException("no browserClient");

                try
                {
                    var res = await driver.browserClient.IsElementEnabled(InternalElementId, cancellationToken)
                        .TimeoutAfter(SimpleCommandsTimeoutMs);
                    var res2 = await driver.browserClient.SubmitElement(InternalElementId, cancellationToken: cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs);
                }
                catch (Exception ex)
                {
                }
            }
        }

        /// <summary>
        ///     Clicks this element.
        /// </summary>
        /// <remarks>
        ///     Click this element. If the click causes a new page to load, the <see cref="Click" />
        ///     method will attempt to block until the page has loaded. After calling the
        ///     <see cref="Click" /> method, you should discard all references to this
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
                var res = await driver.browserClient.ClickElement(InternalElementId, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///     Gets the value of the specified attribute for this element.
        /// </summary>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>
        ///     The attribute's current value. Returns a <see langword="null" /> if the
        ///     value is not set.
        /// </returns>
        /// <remarks>
        ///     The <see cref="GetAttribute" /> method will return the current value
        ///     of the attribute, even if the value has been modified after the page has been
        ///     loaded. Note that the value of the following attributes will be returned even if
        ///     there is no explicit attribute on the element:
        ///     <list type="table">
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
        /// <exception cref="StaleElementReferenceException">Thrown when the target element is no longer valid in the document DOM.</exception>
        public async Task<string> GetAttribute(string attributeName,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var res = await driver.browserClient
                    .GetElementAttribute(InternalElementId, attributeName, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
                return res;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        ///     Gets the value of a CSS property of this element.
        /// </summary>
        /// <param name="propertyName">The name of the CSS property to get the value of.</param>
        /// <returns>The value of the specified CSS property.</returns>
        /// <remarks>
        ///     The value returned by the <see cref="GetCssValue" />
        ///     method is likely to be unpredictable in a cross-browser environment.
        ///     Color values should be returned as hex strings. For example, a
        ///     "background-color" property set as "green" in the HTML source, will
        ///     return "#008000" for its value.
        /// </remarks>
        public async Task<string> GetCssValue(string propertyName,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var res = await driver.browserClient
                    .GetElementValueOfCssProperty(InternalElementId, propertyName, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
                return res;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// <summary>
        ///     Finds all <see cref="IWebElement">IWebElements</see> within the current context
        ///     using the given mechanism.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>
        ///     A <see cref="ReadOnlyCollection{T}" /> of all <see cref="IWebElement">WebElements</see>
        ///     matching the current criteria, or an empty list if nothing matches.
        /// </returns>
        public async Task<ReadOnlyCollection<IWebElement>> FindElements(By by,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (by == null)
                throw new ArgumentNullException("by", "by cannot be null");

            return await by.FindElements(this, cancellationToken);
        }

        /// <summary>
        ///     Finds the first <see cref="IWebElement" /> using the given method.
        /// </summary>
        /// <param name="by">The locating mechanism to use.</param>
        /// <returns>The first matching <see cref="IWebElement" /> on the current context.</returns>
        /// <exception cref="NoSuchElementException">If no element matches the criteria.</exception>
        public async Task<IWebElement> FindElement(By by, CancellationToken cancellationToken = new CancellationToken())
        {
            if (by == null)
                throw new ArgumentNullException("by", "by cannot be null");

            return await by.FindElement(this, cancellationToken);
        }

        /// <summary>
        ///     Gets the <see cref="IWebDriver" /> used to find this element.
        /// </summary>
        public IWebDriver WrappedDriver => driver;

        public override string ToString()
        {
            return "RemoteWebElement: " + InternalElementId;
        }

        public async Task<string> GetProperty(string propertyName,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var res = await driver.browserClient
                    .GetElementProperty(InternalElementId, propertyName, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
                return res;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<IWebElement> FindElementByIdStartsWith(string id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var selector = EscapeCssSelector(id);
            return await FindElement("css selector", $"[id^={selector}]", cancellationToken);
        }

        public async Task<IWebElement> FindElementByIdEndsWith(string id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var selector = EscapeCssSelector(id);
            return await FindElement("css selector", $"[id$={selector}]", cancellationToken);
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByIdStartsWith(string id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var selector = EscapeCssSelector(id);
            if (string.IsNullOrEmpty(selector))
                return new List<IWebElement>().AsReadOnly();

            return await FindElements("css selector", $"[id^={selector}]", cancellationToken);
        }

        public async Task<ReadOnlyCollection<IWebElement>> FindElementsByIdEndsWith(string id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var selector = EscapeCssSelector(id);
            if (string.IsNullOrEmpty(selector))
                return new List<IWebElement>().AsReadOnly();

            return await FindElements("css selector", $"[id$={selector}]", cancellationToken);
        }

        public static string EscapeCssSelector(string selector)
        {
            var escaped = Regex.Replace(selector, @"(['""\\#.:;,!?+<>=~*^$|%&@`{}\-/\[\]\(\)])", @"\$1");
            if (selector.Length > 0 && char.IsDigit(selector[0]))
                escaped =
                    @"\" +
                    (30 + int.Parse(selector.Substring(0, 1), CultureInfo.InvariantCulture)).ToString(CultureInfo
                        .InvariantCulture) + " " + selector.Substring(1);

            return escaped;
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
        /// <param name="obj">Object to compare against</param>
        /// <returns>A boolean if it is equal or not</returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return InternalElementId == (obj as AsyncWebElement).InternalElementId;
        }

        /// <summary>
        ///     Finds a child element matching the given mechanism and value.
        /// </summary>
        /// <param name="mechanism">The mechanism by which to find the element.</param>
        /// <param name="value">The value to use to search for the element.</param>
        /// <returns>The first <see cref="IWebElement" /> matching the given criteria.</returns>
        public async Task<IWebElement> FindElement(string mechanism, string value,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            JToken res = null;
            try
            {
                res = await driver.browserClient.FindElement(mechanism, value, InternalElementId, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
            }
            catch (Exception ex)
            {
                return null;
            }

            return driver.GetElementFromResponse(res);
        }

        public async Task<ReadOnlyCollection<IWebElement>> Children(
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await FindElementsByXPath("child::*", cancellationToken);
        }

        /// <summary>
        ///     Finds all child elements matching the given mechanism and value.
        /// </summary>
        /// <param name="mechanism">The mechanism by which to find the elements.</param>
        /// <param name="value">The value to use to search for the elements.</param>
        /// <returns>A collection of all of the <see cref="IWebElement">IWebElements</see> matching the given criteria.</returns>
        public async Task<ReadOnlyCollection<IWebElement>> FindElements(string mechanism, string value,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (driver.browserClient == null)
                throw new WebDriverException("no browserClient");

            JToken res = null;
            try
            {
                res = await driver.browserClient.FindElements(mechanism, value, InternalElementId, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
            }
            catch (Exception ex)
            {
                return null;
            }

            return driver.GetElementsFromResponse(res);
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