// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver
{
    public static class TaskCollectionsExtensions
    {
        public static async Task<int> Count<T>(this Task<ReadOnlyCollection<T>> task, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await task.ConfigureAwait(false);
            return el.Count;
        }
        public static async Task<bool> Contains<T>(this Task<ReadOnlyCollection<T>> task, T value, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await task.ConfigureAwait(false);
            return el.Contains(value);
        }
        public static async Task<IEnumerator<T>> GetEnumerator<T>(this Task<ReadOnlyCollection<T>> task, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await task.ConfigureAwait(false);
            return el.GetEnumerator();
        }
        public static async Task<int> IndexOf<T>(this Task<ReadOnlyCollection<T>> task, T value, CancellationToken cancellationToken = new CancellationToken())
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOf(value);
        }

    }

}