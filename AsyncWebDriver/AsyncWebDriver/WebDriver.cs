// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Zu.AsyncWebDriver.Internal;
using Zu.WebBrowser;
using Zu.WebBrowser.BasicTypes;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BrowserOptions;

namespace Zu.AsyncWebDriver.Remote
{
    public class WebDriver : IWebDriver, ISearchContext, IJavaScriptExecutor, IFindsById, IFindsByClassName,
        IFindsByLinkText, IFindsByName, IFindsByTagName, IFindsByXPath, IFindsByPartialLinkText, IFindsByCssSelector,
        ITakesScreenshot, IHasInputDevices, IHasWebStorage, IHasLocationContext, IHasApplicationCache
    {
        /// <summary>
        ///     The default command timeout for HTTP requests in a RemoteWebDriver instance.
        /// </summary>
        public static readonly TimeSpan DefaultCommandTimeout = TimeSpan.FromSeconds(60);

        private string appPath;
        public IAsyncWebBrowserClient browserClient;

        //private ICommandExecutor executor;
        //private ICapabilities capabilities;

        //private SessionId sessionId;
        private IWebStorage storage;

        public WebDriver(IAsyncWebBrowserClient client)
        {
            browserClient = client;
            Mouse = browserClient.Mouse;
            Keyboard = browserClient.Keyboard;
            appPath = Path.GetDirectoryName(typeof(WebDriver).Assembly.Location);
        }

        public bool IsSpecificationCompliant { get; set; }

        public int SimpleCommandsTimeoutMs { get; set; } = 5000;

        public int GoToUrlTimeoutMs { get; set; } = 30000;

        /// <summary>
        ///     Finds the first element in the page that matches the CSS Class supplied
        /// </summary>
        /// <param name="className">className of the</param>
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
            if (IsSpecificationCompliant)
            {
                var selector = EscapeCssSelector(className);
                if (selector.Contains(" "))
                    throw new InvalidSelectorException(
                        "Compound class names not allowed. Cannot have whitespace in class name. Use CSS selectors instead.");

                return await FindElement("css selector", "." + selector, cancellationToken);
            }

            return await FindElement("class name", className, cancellationToken);
        }

        /// <summary>
        ///     Finds a list of elements that match the class name supplied
        /// </summary>
        /// <param name="className">CSS class Name on the element</param>
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
            if (IsSpecificationCompliant)
            {
                var selector = EscapeCssSelector(className);
                if (selector.Contains(" "))
                    throw new InvalidSelectorException(
                        "Compound class names not allowed. Cannot have whitespace in class name. Use CSS selectors instead.");

                return await FindElements("css selector", "." + selector, cancellationToken);
            }

            return await FindElements("class name", className, cancellationToken);
        }

        /// <summary>
        ///     Finds the first element matching the specified CSS selector.
        /// </summary>
        /// <param name="cssSelector">The CSS selector to match.</param>
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
            if (IsSpecificationCompliant)
                return await FindElement("css selector", "#" + EscapeCssSelector(id), cancellationToken);

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
            if (IsSpecificationCompliant)
            {
                var selector = EscapeCssSelector(id);
                if (string.IsNullOrEmpty(selector))
                    return new List<IWebElement>().AsReadOnly();

                return await FindElements("css selector", "#" + selector, cancellationToken);
            }

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
        /// IWebElement elem = driver.FindElementsByLinkText("linktext")
        /// </code>
        /// </example>
        public async Task<IWebElement> FindElementByLinkText(string linkText,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await FindElement("link text", linkText, cancellationToken);
        }

        /// <summary>
        ///     Finds a list of elements that match the link text supplied
        /// </summary>
        /// <param name="linkText">Link text of element</param>
        /// <returns>
        ///     ReadOnlyCollection<![CDATA[<IWebElement>]]> object so that you can interact with those objects
        /// </returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new RemoteWebDriver(DesiredCapabilities.Firefox());
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> elem = driver.FindElementsByClassName("classname")
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
        /// <param name="name">Name of the element on the page</param>
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
            if (IsSpecificationCompliant)
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
            if (IsSpecificationCompliant)
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
        ///     Finds a list of elements that match the class name supplied
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
        /// <param name="tagName">DOM tag Name of the element being searched</param>
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
            if (IsSpecificationCompliant)
                return await FindElement("css selector", tagName, cancellationToken);

            return await FindElement("tag name", tagName, cancellationToken);
        }

        /// <summary>
        ///     Finds a list of elements that match the DOM Tag supplied
        /// </summary>
        /// <param name="tagName">DOM tag Name of element being searched</param>
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
            if (IsSpecificationCompliant)
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
        /// <param name="xpath">xpath to the element</param>
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

        public bool HasApplicationCache => ApplicationCache != null;

        public IApplicationCache ApplicationCache { get; }

        //private IFileDetector fileDetector = new DefaultFileDetector();
        /// <summary>
        ///     Gets an <see cref="IKeyboard" /> object for sending keystrokes to the browser.
        /// </summary>
        public IKeyboard Keyboard { get; }

        /// <summary>
        ///     Gets an <see cref="IMouse" /> object for sending mouse commands to the browser.
        /// </summary>
        public IMouse Mouse { get; }

        public bool HasLocationContext => LocationContext != null;
        public ILocationContext LocationContext { get; }

        /// <summary>
        ///     Gets a value indicating whether web storage is supported for this driver.
        /// </summary>
        public bool HasWebStorage => storage != null;

        /// <summary>
        ///     Gets an <see cref="IWebStorage" /> object for managing web storage.
        /// </summary>
        public IWebStorage WebStorage
        {
            get
            {
                if (storage == null)
                    throw new InvalidOperationException(
                        "Driver does not support manipulating HTML5 web storage. Use the HasWebStorage property to test for the driver capability");

                return storage;
            }
        }

        public async Task<object> ExecuteScript(string script, CancellationToken cancellationToken = new CancellationToken(),
            params object[] args)
        {
            if (browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                //var res = await browserClient.JavaScriptExecutor.ExecuteScript(script, null, "defaultSandbox", cancellationToken, args).TimeoutAfter(SimpleCommandsTimeoutMs);
                var res = await browserClient.JavaScriptExecutor.ExecuteScript(script, cancellationToken, args).TimeoutAfter(SimpleCommandsTimeoutMs);
                return res; // (string)res?["value"];
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<object> ExecuteAsyncScript(string script,
            CancellationToken cancellationToken = new CancellationToken(), params object[] args)
        {
            if (browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                //var res = await browserClient.JavaScriptExecutor.ExecuteAsyncScript(script, null, "defaultSandbox", cancellationToken, args).TimeoutAfter(SimpleCommandsTimeoutMs);
                var res = await browserClient.JavaScriptExecutor.ExecuteAsyncScript(script, cancellationToken, args).TimeoutAfter(SimpleCommandsTimeoutMs);
                return res; // (string)res?["value"];
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<Screenshot> GetScreenshot(CancellationToken cancellationToken = new CancellationToken())
        {
            if (browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var res = await browserClient.Screenshot.GetScreenshot(/*null, null, null, null, */cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs);
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        ///     Finds the first element in the page that matches the <see cref="By" /> object
        /// </summary>
        /// <param name="by">By mechanism to find the object</param>
        /// <returns>IWebElement object so that you can interact with that object</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new InternetExplorerDriver();
        /// IWebElement elem = driver.FindElement(By.Name("q"));
        /// </code>
        /// </example>
        public async Task<IWebElement> FindElement(By by, CancellationToken cancellationToken = new CancellationToken())
        {
            if (by == null)
                throw new ArgumentNullException(nameof(by), "by cannot be null");

            return await by.FindElement(this, cancellationToken);
        }

        /// <summary>
        ///     Finds the elements on the page by using the <see cref="By" /> object and returns a ReadOnlyCollection of the
        ///     Elements on the page
        /// </summary>
        /// <param name="by">By mechanism to find the element</param>
        /// <returns>ReadOnlyCollection of IWebElement</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new InternetExplorerDriver();
        /// ReadOnlyCollection<![CDATA[<IWebElement>]]> classList = driver.FindElements(By.ClassName("class"));
        /// </code>
        /// </example>
        public async Task<ReadOnlyCollection<IWebElement>> FindElements(By by,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (by == null)
                throw new ArgumentNullException(nameof(by), "by cannot be null");

            return await by.FindElements(this, cancellationToken);
        }

        /// <summary>
        ///     Method For getting an object to set the Speed
        /// </summary>
        /// <returns>Returns an IOptions object that allows the driver to set the speed and cookies and getting cookies</returns>
        /// <seealso cref="IOptions" />
        /// <example>
        ///     <code>
        /// IWebDriver driver = new InternetExplorerDriver();
        /// driver.Manage().GetCookies();
        /// </code>
        /// </example>
        public IOptions Manage()
        {
            return browserClient.Options; // new RemoteOptions(this);
        }

        /// <summary>
        ///     Method to allow you to Navigate with WebDriver
        /// </summary>
        /// <returns>Returns an INavigation Object that allows the driver to navigate in the browser</returns>
        /// <example>
        ///     <code>
        ///     IWebDriver driver = new InternetExplorerDriver();
        ///     driver.Navigate().GoToUrl("http://www.google.co.uk");
        /// </code>
        /// </example>
        public INavigation Navigate()
        {
            return browserClient.Navigation; // new RemoteNavigator(this);
        }

        /// <summary>
        ///     Method to give you access to switch frames and windows
        /// </summary>
        /// <returns>Returns an Object that allows you to Switch Frames and Windows</returns>
        /// <example>
        ///     <code>
        /// IWebDriver driver = new InternetExplorerDriver();
        /// driver.SwitchTo().Frame("FrameName");
        /// </code>
        /// </example>
        public RemoteTargetLocator SwitchTo()
        {
            return new RemoteTargetLocator(this);
        }


        /// <summary>
        ///     Dispose the RemoteWebDriver Instance
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<string> GetUrl(CancellationToken cancellationToken = new CancellationToken())
        {
            if (browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var res = await browserClient.Navigation.GetUrl(cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs);
                return res;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> GoToUrl(string url, CancellationToken cancellationToken = new CancellationToken())
        {
            if (browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                await browserClient.Navigation.GoToUrl(url, cancellationToken).TimeoutAfter(GoToUrlTimeoutMs);
                return "";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> Title(CancellationToken cancellationToken = new CancellationToken())
        {
            if (browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var res = await browserClient.GetTitle(cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs);
                return res;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> PageSource(CancellationToken cancellationToken = new CancellationToken())
        {
            if (browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var res = await browserClient.GetPageSource(cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs);
                return res;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<string> CurrentWindowHandle(CancellationToken cancellationToken = new CancellationToken())
        {
            if (browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var res = await browserClient.TargetLocator.GetWindowHandle(cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs);
                return res;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public async Task<List<string>> WindowHandles(
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var res = await browserClient.TargetLocator.GetWindowHandles(cancellationToken).TimeoutAfter(SimpleCommandsTimeoutMs);
                //var res2 = new ReadOnlyCollection<string>((res as JArray)?.Select(v => v.ToString())?.ToList());
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task Close(CancellationToken cancellationToken = new CancellationToken())
        {
            if (browserClient != null)
                await browserClient.Close(cancellationToken);
        }

        public void CloseSync()
        {
            if (browserClient != null)
                browserClient.CloseSync();
        }

        public async Task Quit(CancellationToken cancellationToken = new CancellationToken())
        {
            if (browserClient != null)
                await browserClient.Disconnect(cancellationToken);
        }

        public async Task<string> GetContext(CancellationToken cancellationToken = new CancellationToken())
        {
            var bc = browserClient as WebBrowser.Firefox.IAsyncWebBrowserClientFirefox;
            return bc == null
                ? "None"
                : (await bc?.GetContext(cancellationToken) == Zu.WebBrowser.Firefox.Contexts.Chrome ? "Chrome" : "Content");
        }

        public async Task<JToken> SetContextChrome(CancellationToken cancellationToken = new CancellationToken())
        {
            var bc = browserClient as WebBrowser.Firefox.IAsyncWebBrowserClientFirefox;
            return bc == null ? null : await bc.SetContextChrome(cancellationToken);
        }

        public async Task<JToken> SetContextContent(CancellationToken cancellationToken = new CancellationToken())
        {
            var bc = browserClient as WebBrowser.Firefox.IAsyncWebBrowserClientFirefox;
            return bc == null ? null : await bc.SetContextContent(cancellationToken);
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

        public async Task<ReadOnlyCollection<IWebElement>> Children(
            CancellationToken cancellationToken = new CancellationToken())
        {
            return await FindElementsByXPath("child::*", cancellationToken);
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

        /// <summary>
        ///     Escapes invalid characters in a CSS selector.
        /// </summary>
        /// <param name="selector">The selector to escape.</param>
        /// <returns>The selector with invalid characters escaped.</returns>
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
        ///     Find the element in the response
        /// </summary>
        /// <param name="response">Response from the browser</param>
        /// <returns>Element from the page</returns>
        public IWebElement GetElementFromResponse(JToken response)
        {
            if (response == null)
                throw new NoSuchElementException();

            string id = null;
            try
            {
                var json = response is JValue ? JToken.Parse(response.Value<string>()) : response["value"];
                id = json?["element-6066-11e4-a52e-4f735466cecf"]?.ToString();
                if (id == null)
                    id = json?["ELEMENT"]?.ToString();
            }
            catch
            {
            }

            var element = CreateElement(id);
            //}
            return element;
        }
        public IWebElement GetElementFromResponse(string id)
        {
            var element = CreateElement(id);
            return element;
        }

        /// <summary>
        ///     Finds the elements that are in the response
        /// </summary>
        /// <param name="response">Response from the browser</param>
        /// <returns>Collection of elements</returns>
        public ReadOnlyCollection<IWebElement> GetElementsFromResponse(JToken response)
        {
            var toReturn = new List<IWebElement>();
            if (response is JArray)
                foreach (var item in response)
                {
                    string id = null;
                    try
                    {
                        var json = item is JValue ? JToken.Parse(item.Value<string>()) : item;
                        id = json?["element-6066-11e4-a52e-4f735466cecf"]?.ToString();
                        if (id == null)
                            id = json?["ELEMENT"]?.ToString();
                    }
                    catch
                    {
                    }

                    var element = CreateElement(id);
                    if (element != null)
                        toReturn.Add(element);
                }

            return toReturn.AsReadOnly();
        }

        /// <summary>
        ///     Stops the client from running
        /// </summary>
        /// <param name="disposing">if its in the process of disposing</param>
        public virtual void Dispose(bool disposing)
        {
            try
            {
                //Quit();
            }
            catch (NotImplementedException)
            {
            }
            catch (InvalidOperationException)
            {
            }
            catch (WebDriverException)
            {
            }
            finally
            {
                StopClient();
            }
        }

        /// <summary>
        ///     Stops the command executor, ending further communication with the browser.
        /// </summary>
        public virtual void StopClient()
        {
        }

        /// <summary>
        ///     Finds an element matching the given mechanism and value.
        /// </summary>
        /// <param name="mechanism">The mechanism by which to find the element.</param>
        /// <param name="value">The value to use to search for the element.</param>
        /// <returns>The first <see cref="IWebElement" /> matching the given criteria.</returns>
        public async Task<IWebElement> FindElement(string mechanism, string value,
            CancellationToken cancellationToken = new CancellationToken())
        {
            //Dictionary<string, object> parameters = new Dictionary<string, object>();
            //parameters.Add("using", mechanism);
            //parameters.Add("value", value);
            //Response commandResponse = this.Execute(DriverCommand.FindElement, parameters);
            if (browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var commandResponse = await browserClient.Elements
                    .FindElement(mechanism, value, cancellationToken: cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
                return GetElementFromResponse(commandResponse);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task ClickElement(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                await browserClient.Elements.Click(elementId, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task ClearElement(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var commandResponse = await browserClient.Elements.ClearElement(elementId, cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        ///     Finds all elements matching the given mechanism and value.
        /// </summary>
        /// <param name="mechanism">The mechanism by which to find the elements.</param>
        /// <param name="value">The value to use to search for the elements.</param>
        /// <returns>A collection of all of the <see cref="IWebElement">IWebElements</see> matching the given criteria.</returns>
        public async Task<ReadOnlyCollection<IWebElement>> FindElements(string mechanism, string value,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (browserClient == null)
                throw new WebDriverException("no browserClient");

            try
            {
                var commandResponse = await browserClient.Elements
                    .FindElements(mechanism, value, cancellationToken: cancellationToken)
                    .TimeoutAfter(SimpleCommandsTimeoutMs);
                return GetElementsFromResponse(commandResponse);
            }
            catch (Exception ex)
            {
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        /// <summary>
        ///     Creates a <see cref="AsyncWebElement" /> with the specified ID.
        /// </summary>
        /// <param name="elementId">The ID of this element.</param>
        /// <returns>A <see cref="AsyncWebElement" /> with the specified ID.</returns>
        public virtual AsyncWebElement CreateElement(string elementId)
        {
            if (string.IsNullOrWhiteSpace(elementId))
                return null;
            var toReturn = new AsyncWebElement(this, elementId);
            return toReturn;
        }

        private static object ConvertObjectToJavaScriptObject(object arg)
        {
            var argAsWrapsElement = arg as IWrapsElement;
            var argAsElement = arg as AsyncWebElement;
            var argAsEnumerable = arg as IEnumerable;
            var argAsDictionary = arg as IDictionary;
            if (argAsElement == null && argAsWrapsElement != null)
                argAsElement = argAsWrapsElement.WrappedElement as AsyncWebElement;

            object converted = null;
            if (arg is string || arg is float || arg is double || arg is int || arg is long || arg is bool ||
                arg == null)
            {
                converted = arg;
            }
            else if (argAsElement != null)
            {
                // TODO: Remove addition of 'id' key when spec is changed.
                var elementDictionary = new Dictionary<string, object>();
                elementDictionary.Add("ELEMENT", argAsElement.InternalElementId);
                elementDictionary.Add("element-6066-11e4-a52e-4f735466cecf", argAsElement.InternalElementId);
                converted = elementDictionary;
            }
            else if (argAsDictionary != null)
            {
                // Note that we must check for the argument being a dictionary before
                // checking for IEnumerable, since dictionaries also implement IEnumerable.
                // Additionally, JavaScript objects have property names as strings, so all
                // keys will be converted to strings.
                var dictionary = new Dictionary<string, object>();
                foreach (var key in argAsDictionary.Keys)
                    dictionary.Add(key.ToString(), ConvertObjectToJavaScriptObject(argAsDictionary[key]));

                converted = dictionary;
            }
            else if (argAsEnumerable != null)
            {
                var objectList = new List<object>();
                foreach (var item in argAsEnumerable)
                    objectList.Add(ConvertObjectToJavaScriptObject(item));

                converted = objectList.ToArray();
            }
            else
            {
                throw new ArgumentException("Argument is of an illegal type" + arg, "arg");
            }

            return converted;
        }

        /// <summary>
        ///     Converts the arguments to JavaScript objects.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>The list of the arguments converted to JavaScript objects.</returns>
        private static object[] ConvertArgumentsToJavaScriptObjects(object[] args)
        {
            if (args == null)
                return new object[] { null };

            for (var i = 0; i < args.Length; i++)
                args[i] = ConvertObjectToJavaScriptObject(args[i]);

            return args;
        }

        public Task<IWebElement> WaitForElementWithId(string id, string notWebElementId = null, int attemptsCount = 20, int delayMs = 500,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (notWebElementId != null) return WaitForWebElement(async () => await FindElementById(id), notWebElementId, attemptsCount, delayMs, cancellationToken);
            else return WaitForWebElement(async () => await FindElementById(id), attemptsCount, delayMs, cancellationToken);
        }

        public Task<IWebElement> WaitForElementWithName(string name, string notWebElementId = null, int attemptsCount = 20, int delayMs = 500,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (notWebElementId != null) return WaitForWebElement(async () => await FindElementByName(name), notWebElementId, attemptsCount, delayMs, cancellationToken);
            else return WaitForWebElement(async () => await FindElementByName(name), attemptsCount, delayMs, cancellationToken);
        }

        public Task<IWebElement> WaitForElementWithCssSelector(string cssSelector, string notWebElementId = null, int attemptsCount = 20, int delayMs = 500,
            CancellationToken cancellationToken = new CancellationToken())
        {
            if (notWebElementId != null) return WaitForWebElement(async () => await FindElementByCssSelector(cssSelector), notWebElementId, attemptsCount, delayMs, cancellationToken);
            else return WaitForWebElement(async () => await FindElementByCssSelector(cssSelector), attemptsCount, delayMs, cancellationToken);
        }

        public async Task<IWebElement> WaitForWebElement(Func<Task<IWebElement>> func, int attemptsCount = 20, int delayMs = 500,
            CancellationToken cancellationToken = new CancellationToken())
        {
            for (int i = 0; i < attemptsCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(delayMs, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var el = await func();
                if (!string.IsNullOrWhiteSpace(el?.Id)) return el;
            }
            return null;
        }

        public Task<IWebElement> WaitForWebElement(Func<Task<IWebElement>> func, IWebElement notWebElement, int attemptsCount = 20, int delayMs = 500,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return WaitForWebElement(func, notWebElement?.Id, attemptsCount, delayMs, cancellationToken);
        }

        public async Task<IWebElement> WaitForWebElement(Func<Task<IWebElement>> func, string notWebElementId, int attemptsCount = 20, int delayMs = 500,
            CancellationToken cancellationToken = new CancellationToken())
        {
            for (int i = 0; i < attemptsCount; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(delayMs, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var el = await func();
                if (notWebElementId != null && el?.Id == notWebElementId) continue;
                if (!string.IsNullOrWhiteSpace(el?.Id)) return el;
            }
            return null;
        }

    }
}