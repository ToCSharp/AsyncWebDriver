// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Zu.AsyncWebDriver
{
    /// <summary>
    ///     Represents an image of the page currently loaded in the browser.
    /// </summary>
    [Serializable]
    public class Screenshot
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Screenshot" /> class.
        /// </summary>
        /// <param name="base64EncodedScreenshot">The image of the page as a Base64-encoded string.</param>
        public Screenshot(string base64EncodedScreenshot)
        {
            AsBase64EncodedString = base64EncodedScreenshot;
            AsByteArray = Convert.FromBase64String(AsBase64EncodedString);
        }

        /// <summary>
        ///     Gets the value of the screenshot image as a Base64-encoded string.
        /// </summary>
        public string AsBase64EncodedString { get; }

        /// <summary>
        ///     Gets the value of the screenshot image as an array of bytes.
        /// </summary>
        public byte[] AsByteArray { get; }

        /// <summary>
        ///     Saves the screenshot to a file, overwriting the file if it already exists.
        /// </summary>
        /// <param name="fileName">The full path and file name to save the screenshot to.</param>
        /// <param name="format">
        ///     A <see cref="System.Drawing.Imaging.ImageFormat" /> object indicating the format
        ///     to save the image to.
        /// </param>
        public void SaveAsFile(string fileName, ImageFormat format)
        {
            using (var imageStream = new MemoryStream(AsByteArray))
            {
                var screenshotImage = Image.FromStream(imageStream);
                screenshotImage.Save(fileName, format);
            }
        }

        /// <summary>
        ///     Returns a <see cref="string ">String</see> that represents the current <see cref="object ">Object</see>.
        /// </summary>
        /// <returns>A <see cref="string ">String</see> that represents the current <see cref="object ">Object</see>.</returns>
        public override string ToString() => AsBase64EncodedString;
    }
}