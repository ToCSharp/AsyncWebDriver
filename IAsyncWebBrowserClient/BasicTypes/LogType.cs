// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

namespace Zu.WebBrowser.BasicTypes
{
    /// <summary>
    ///     Class containing names of common log types.
    /// </summary>
    public static class LogType
    {
        /// <summary>
        ///     Log messages from the client language bindings.
        /// </summary>
        public static readonly string Client = "client";

        /// <summary>
        ///     Logs from the current WebDriver instance.
        /// </summary>
        public static readonly string Driver = "driver";

        /// <summary>
        ///     Logs from the browser.
        /// </summary>
        public static readonly string Browser = "browser";

        /// <summary>
        ///     Logs from the server.
        /// </summary>
        public static readonly string Server = "server";

        /// <summary>
        ///     Profiling logs.
        /// </summary>
        public static readonly string Profiler = "profiler";
    }
}