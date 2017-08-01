// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Globalization;

namespace Zu.WebBrowser.BasicTypes
{
    /// <summary>
    ///     Represents a cookie returned to the driver by the browser.
    /// </summary>
    public class ReturnedCookie : Cookie
    {
        private readonly bool isHttpOnly;
        private readonly bool isSecure;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReturnedCookie" /> class with a specific name,
        ///     value, domain, path and expiration date.
        /// </summary>
        /// <param name="name">The name of the cookie.</param>
        /// <param name="value">The value of the cookie.</param>
        /// <param name="domain">The domain of the cookie.</param>
        /// <param name="path">The path of the cookie.</param>
        /// <param name="expiry">The expiration date of the cookie.</param>
        /// <param name="isSecure"><see langword="true" /> if the cookie is secure; otherwise <see langword="false" /></param>
        /// <param name="isHttpOnly">
        ///     <see langword="true" /> if the cookie is an HTTP-only cookie; otherwise
        ///     <see langword="false" />
        /// </param>
        /// <exception cref="ArgumentException">
        ///     If the name is <see langword="null" /> or an empty string,
        ///     or if it contains a semi-colon.
        /// </exception>
        /// <exception cref="ArgumentNullException">If the value or currentUrl is <see langword="null" />.</exception>
        public ReturnedCookie(string name, string value, string domain, string path, DateTime? expiry, bool isSecure,
            bool isHttpOnly) : base(name, value, domain, path, expiry)
        {
            this.isSecure = isSecure;
            this.isHttpOnly = isHttpOnly;
        }

        /// <summary>
        ///     Gets a value indicating whether the cookie is secure.
        /// </summary>
        public override bool Secure => isSecure;

        /// <summary>
        ///     Gets a value indicating whether the cookie is an HTTP-only cookie.
        /// </summary>
        public override bool IsHttpOnly => isHttpOnly;

        /// <summary>
        ///     Creates and returns a string representation of the current cookie.
        /// </summary>
        /// <returns>A string representation of the current cookie.</returns>
        public override string ToString()
        {
            return Name + "=" + Value +
                   (Expiry == null
                       ? string.Empty
                       : "; expires=" + Expiry.Value.ToUniversalTime()
                             .ToString("ddd MM/dd/yyyy HH:mm:ss UTC", CultureInfo.InvariantCulture)) +
                   (string.IsNullOrEmpty(Path) ? string.Empty : "; path=" + Path) +
                   (string.IsNullOrEmpty(Domain) ? string.Empty : "; domain=" + Domain) +
                   (isSecure ? "; secure" : string.Empty) + (isHttpOnly ? "; httpOnly" : string.Empty);
        }
    }
}