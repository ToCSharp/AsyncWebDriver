// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Globalization;

namespace Zu.AsyncWebDriver.Html5
{
    /// <summary>
    ///     Represents the physical location of the browser.
    /// </summary>
    public class Location
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Location" /> class.
        /// </summary>
        /// <param name="latitude">latitude for current location</param>
        /// <param name="longitude">longitude for current location</param>
        /// <param name="altitude">altitude for current location</param>
        public Location(double latitude, double longitude, double altitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }

        /// <summary>
        ///     Gets the latitude of the current location.
        /// </summary>
        public double Latitude { get; }

        /// <summary>
        ///     Gets the longitude of the current location.
        /// </summary>
        public double Longitude { get; }

        /// <summary>
        ///     Gets the altitude of the current location.
        /// </summary>
        public double Altitude { get; }

        /// <summary>
        ///     Retuns string represenation for current location.
        /// </summary>
        /// <returns>Returns <see cref="string ">string</see> reprsentation for current location.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "Latitude: {0}, Longitude: {1}, Altitude: {2}", Latitude,
                Longitude, Altitude);
        }
    }
}