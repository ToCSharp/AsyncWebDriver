// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver
{
    /// <summary>
    ///     Interface allowing handling of driver logs.
    /// </summary>
    public interface ILogs
    {
        /// <summary>
        ///     Gets the list of available log types for this driver.
        /// </summary>
        Task<ReadOnlyCollection<string>> AvailableLogTypes { get; }

        /// <summary>
        ///     Gets the set of <see cref="LogEntry" /> objects for a specified log.
        /// </summary>
        /// <param name="logKind">
        ///     The log for which to retrieve the log entries.
        ///     Log types can be found in the <see cref="LogType" /> class.
        /// </param>
        /// <returns>The list of <see cref="LogEntry" /> objects for the specified log.</returns>
        Task<ReadOnlyCollection<LogEntry>> GetLog(string logKind,
            CancellationToken cancellationToken = new CancellationToken());
    }
}