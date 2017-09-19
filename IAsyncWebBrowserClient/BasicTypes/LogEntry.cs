// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Zu.WebBrowser.BasicTypes
{
    /// <summary>
    ///     Represents an entry in a log from a driver instance.
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LogEntry" /> class.
        /// </summary>
        private LogEntry()
        {
        }

        /// <summary>
        ///     Gets the timestamp value of the log entry.
        /// </summary>
        public DateTime Timestamp { get; private set; }

            = DateTime.MinValue;

        /// <summary>
        ///     Gets the logging level of the log entry.
        /// </summary>
        public LogLevel Level { get; private set; }

            = LogLevel.All;

        /// <summary>
        ///     Gets the message of the log entry.
        /// </summary>
        public string Message { get; private set; }

            = string.Empty;

        /// <summary>
        ///     Returns a string that represents the current <see cref="LogEntry" />.
        /// </summary>
        /// <returns>A string that represents the current <see cref="LogEntry" />.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0:yyyy-MM-ddTHH:mm:ssZ}] [{1}] {2}", Timestamp, Level,
                Message);
        }

        /// <summary>
        ///     Creates a <see cref="LogEntry" /> from a dictionary as deserialized from JSON.
        /// </summary>
        /// <param name="entryDictionary">
        ///     The <see cref="Dictionary{TKey, TValue}" /> from
        ///     which to create the <see cref="LogEntry" />.
        /// </param>
        /// <returns>A <see cref="LogEntry" /> with the values in the dictionary.</returns>
        public static LogEntry FromDictionary(Dictionary<string, object> entryDictionary)
        {
            var entry = new LogEntry();
            if (entryDictionary.ContainsKey("message"))
                entry.Message = entryDictionary["message"].ToString();
            if (entryDictionary.ContainsKey("timestamp"))
            {
                var zeroDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var timestampValue = Convert.ToDouble(entryDictionary["timestamp"], CultureInfo.InvariantCulture);
                entry.Timestamp = zeroDate.AddMilliseconds(timestampValue);
            }

            if (entryDictionary.ContainsKey("level"))
            {
                var levelValue = entryDictionary["level"].ToString();
                try
                {
                    entry.Level = (LogLevel) Enum.Parse(typeof(LogEntry), levelValue, true);
                }
                catch (ArgumentException)
                {
                    // If the requested log level string is not a valid log level,
                    // ignore it and use LogLevel.All.
                }
            }

            return entry;
        }
    }
}