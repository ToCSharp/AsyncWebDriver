// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Internal;

namespace Zu.AsyncWebDriver
{
    /// <summary>
    ///     Provides a mechanism by which to find elements within a document.
    /// </summary>
    /// <remarks>
    ///     It is possible to create your own locating mechanisms for finding documents.
    ///     In order to do this,subclass this class and override the protected methods. However,
    ///     it is expected that that all subclasses rely on the basic finding mechanisms provided
    ///     through static methods of this class.
    /// </remarks>
    [Serializable]
    public class By
    {
        private Func<ISearchContext, CancellationToken, Task<IWebElement>> _findElementMethod;
        private Func<ISearchContext, CancellationToken, Task<ReadOnlyCollection<IWebElement>>> _findElementsMethod;
        private string description = "By";

        /// <summary>
        ///     Initializes a new instance of the <see cref="By" /> class.
        /// </summary>
        protected By()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="By" /> class using the given functions to find elements.
        /// </summary>
        /// <param name="findElementMethod">
        ///     A function that takes an object implementing <see cref="ISearchContext" />
        ///     and returns the found <see cref="IWebElement" />.
        /// </param>
        /// <param name="findElementsMethod">
        ///     A function that takes an object implementing <see cref="ISearchContext" />
        ///     and returns a <see cref="ReadOnlyCollection{T}" /> of the found<see cref="IWebElement">IWebElements</see>.
        ///     <see cref="IWebElement">IWebElements</see>/>.
        /// </param>
        protected By(Func<ISearchContext, CancellationToken, Task<IWebElement>> findElementMethod,
            Func<ISearchContext, CancellationToken, Task<ReadOnlyCollection<IWebElement>>> findElementsMethod)
        {
            _findElementMethod = findElementMethod;
            _findElementsMethod = findElementsMethod;
        }

        /// <summary>
        ///     Gets or sets the value of the description for this <see cref="By" /> class instance.
        /// </summary>
        protected string Description
        {
            get => description;
            set => description = value;
        }

        /// <summary>
        ///     Gets or sets the method used to find a single element matching specified criteria.
        /// </summary>
        protected Func<ISearchContext, CancellationToken, Task<IWebElement>> FindElementMethod
        {
            get => _findElementMethod;
            set => _findElementMethod = value;
        }

        /// <summary>
        ///     Gets or sets the method used to find all elements matching specified criteria.
        /// </summary>
        protected Func<ISearchContext, CancellationToken, Task<ReadOnlyCollection<IWebElement>>> FindElementsMethod
        {
            get => _findElementsMethod;
            set => _findElementsMethod = value;
        }

        /// <summary>
        ///     Determines if two <see cref="By" /> instances are equal.
        /// </summary>
        /// <param name="one">One instance to compare.</param>
        /// <param name="two">The other instance to compare.</param>
        /// <returns><see langword="true" /> if the two instances are equal; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(By one, By two)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(one, two))
                return true;
            // If one is null, but not both, return false.
            if ((object) one == null || (object) two == null)
                return false;
            return one.Equals(two);
        }

        /// <summary>
        ///     Determines if two <see cref="By" /> instances are unequal.
        /// </summary>
        /// s
        /// <param name="one">One instance to compare.</param>
        /// <param name="two">The other instance to compare.</param>
        /// <returns><see langword="true" /> if the two instances are not equal; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(By one, By two)
        {
            return !(one == two);
        }

        /// <summary>
        ///     Gets a mechanism to find elements by their ID.
        /// </summary>
        /// <param name="idToFind">The ID to find.</param>
        /// <returns>A <see cref="By" /> object the driver can use to find the elements.</returns>
        public static By Id(string idToFind)
        {
            if (idToFind == null)
                throw new ArgumentNullException("idToFind", "Cannot find elements with a null id attribute.");
            var by = new By();
            by._findElementMethod =
                (context, cancellationToken) => ((IFindsById) context).FindElementById(idToFind, cancellationToken);
            by._findElementsMethod =
                (context, cancellationToken) => ((IFindsById) context).FindElementsById(idToFind, cancellationToken);
            by.description = "By.Id: " + idToFind;
            return by;
        }

        /// <summary>
        ///     Gets a mechanism to find elements by their link text.
        /// </summary>
        /// <param name="linkTextToFind">The link text to find.</param>
        /// <returns>A <see cref="By" /> object the driver can use to find the elements.</returns>
        public static By LinkText(string linkTextToFind)
        {
            if (linkTextToFind == null)
                throw new ArgumentNullException("linkTextToFind", "Cannot find elements when link text is null.");
            var by = new By();
            by._findElementMethod =
                (context, cancellationToken) => ((IFindsByLinkText) context).FindElementByLinkText(linkTextToFind,
                    cancellationToken);
            by._findElementsMethod =
                (context, cancellationToken) => ((IFindsByLinkText) context).FindElementsByLinkText(linkTextToFind,
                    cancellationToken);
            by.description = "By.LinkText: " + linkTextToFind;
            return by;
        }

        /// <summary>
        ///     Gets a mechanism to find elements by their name.
        /// </summary>
        /// <param name="nameToFind">The name to find.</param>
        /// <returns>A <see cref="By" /> object the driver can use to find the elements.</returns>
        public static By Name(string nameToFind)
        {
            if (nameToFind == null)
                throw new ArgumentNullException("nameToFind", "Cannot find elements when name text is null.");
            var by = new By();
            by._findElementMethod =
                (context, cancellationToken) => ((IFindsByName) context).FindElementByName(nameToFind,
                    cancellationToken);
            by._findElementsMethod =
                (context, cancellationToken) => ((IFindsByName) context).FindElementsByName(nameToFind,
                    cancellationToken);
            by.description = "By.Name: " + nameToFind;
            return by;
        }

        /// <summary>
        ///     Gets a mechanism to find elements by an XPath query.
        ///     When searching within a WebElement using xpath be aware that WebDriver follows standard conventions:
        ///     a search prefixed with "//" will search the entire document, not just the children of this current node.
        ///     Use ".//" to limit your search to the children of this WebElement.
        /// </summary>
        /// <param name="xpathToFind">The XPath query to use.</param>
        /// <returns>A <see cref="By" /> object the driver can use to find the elements.</returns>
        public static By XPath(string xpathToFind)
        {
            if (xpathToFind == null)
                throw new ArgumentNullException("xpathToFind",
                    "Cannot find elements when the XPath expression is null.");
            var by = new By();
            by._findElementMethod =
                (context, cancellationToken) => ((IFindsByXPath) context).FindElementByXPath(xpathToFind,
                    cancellationToken);
            by._findElementsMethod =
                (context, cancellationToken) => ((IFindsByXPath) context).FindElementsByXPath(xpathToFind,
                    cancellationToken);
            by.description = "By.XPath: " + xpathToFind;
            return by;
        }

        /// <summary>
        ///     Gets a mechanism to find elements by their CSS class.
        /// </summary>
        /// <param name="classNameToFind">The CSS class to find.</param>
        /// <returns>A <see cref="By" /> object the driver can use to find the elements.</returns>
        /// <remarks>
        ///     If an element has many classes then this will match against each of them.
        ///     For example if the value is "one two onone", then the following values for the
        ///     className parameter will match: "one" and "two".
        /// </remarks>
        public static By ClassName(string classNameToFind)
        {
            if (classNameToFind == null)
                throw new ArgumentNullException("classNameToFind",
                    "Cannot find elements when the class name expression is null.");
            var by = new By();
            by._findElementMethod =
                (context, cancellationToken) => ((IFindsByClassName) context).FindElementByClassName(classNameToFind,
                    cancellationToken);
            by._findElementsMethod =
                (context, cancellationToken) => ((IFindsByClassName) context).FindElementsByClassName(classNameToFind,
                    cancellationToken);
            by.description = "By.ClassName[Contains]: " + classNameToFind;
            return by;
        }

        /// <summary>
        ///     Gets a mechanism to find elements by a partial match on their link text.
        /// </summary>
        /// <param name="partialLinkTextToFind">The partial link text to find.</param>
        /// <returns>A <see cref="By" /> object the driver can use to find the elements.</returns>
        public static By PartialLinkText(string partialLinkTextToFind)
        {
            var by = new By();
            by._findElementMethod =
                (context, cancellationToken) => ((IFindsByPartialLinkText) context).FindElementByPartialLinkText(
                    partialLinkTextToFind, cancellationToken);
            by._findElementsMethod =
                (context, cancellationToken) => ((IFindsByPartialLinkText) context).FindElementsByPartialLinkText(
                    partialLinkTextToFind, cancellationToken);
            by.description = "By.PartialLinkText: " + partialLinkTextToFind;
            return by;
        }

        /// <summary>
        ///     Gets a mechanism to find elements by their tag name.
        /// </summary>
        /// <param name="tagNameToFind">The tag name to find.</param>
        /// <returns>A <see cref="By" /> object the driver can use to find the elements.</returns>
        public static By TagName(string tagNameToFind)
        {
            if (tagNameToFind == null)
                throw new ArgumentNullException("tagNameToFind", "Cannot find elements when name tag name is null.");
            var by = new By();
            by._findElementMethod =
                (context, cancellationToken) => ((IFindsByTagName) context).FindElementByTagName(tagNameToFind,
                    cancellationToken);
            by._findElementsMethod =
                (context, cancellationToken) => ((IFindsByTagName) context).FindElementsByTagName(tagNameToFind,
                    cancellationToken);
            by.description = "By.TagName: " + tagNameToFind;
            return by;
        }

        /// <summary>
        ///     Gets a mechanism to find elements by their cascading style sheet (CSS) selector.
        /// </summary>
        /// <param name="cssSelectorToFind">The CSS selector to find.</param>
        /// <returns>A <see cref="By" /> object the driver can use to find the elements.</returns>
        public static By CssSelector(string cssSelectorToFind)
        {
            if (cssSelectorToFind == null)
                throw new ArgumentNullException("cssSelectorToFind",
                    "Cannot find elements when name CSS selector is null.");
            var by = new By();
            by._findElementMethod =
                (context, cancellationToken) => ((IFindsByCssSelector) context).FindElementByCssSelector(
                    cssSelectorToFind, cancellationToken);
            by._findElementsMethod =
                (context, cancellationToken) => ((IFindsByCssSelector) context).FindElementsByCssSelector(
                    cssSelectorToFind, cancellationToken);
            by.description = "By.CssSelector: " + cssSelectorToFind;
            return by;
        }

        /// <summary>
        ///     Finds the first element matching the criteria.
        /// </summary>
        /// <param name="context">An <see cref="ISearchContext" /> object to use to search for the elements.</param>
        /// <returns>The first matching <see cref="IWebElement" /> on the current context.</returns>
        public virtual Task<IWebElement> FindElement(ISearchContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _findElementMethod(context, cancellationToken);
        }

        /// <summary>
        ///     Finds all elements matching the criteria.
        /// </summary>
        /// <param name="context">An <see cref="ISearchContext" /> object to use to search for the elements.</param>
        /// <returns>
        ///     A <see cref="ReadOnlyCollection{T}" /> of all <see cref="IWebElement">WebElements</see>
        ///     matching the current criteria, or an empty list if nothing matches.
        /// </returns>
        public virtual Task<ReadOnlyCollection<IWebElement>> FindElements(ISearchContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _findElementsMethod(context, cancellationToken);
        }

        /// <summary>
        ///     Gets a string representation of the finder.
        /// </summary>
        /// <returns>The string displaying the finder content.</returns>
        public override string ToString()
        {
            return description;
        }

        /// <summary>
        ///     Determines whether the specified <see cref="object ">Object</see> is equal
        ///     to the current <see cref="object ">Object</see>.
        /// </summary>
        /// <param name="obj">
        ///     The <see cref="object ">Object</see> to compare with the
        ///     current <see cref="object ">Object</see>.
        /// </param>
        /// <returns>
        ///     <see langword="true" /> if the specified <see cref="object ">Object</see>
        ///     is equal to the current <see cref="object ">Object</see>; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as By;
            // TODO(dawagner): This isn't ideal
            return other != null && description.Equals(other.description);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="object ">Object</see>.</returns>
        public override int GetHashCode()
        {
            return description.GetHashCode();
        }
    }
}