// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver
{
    public static class TaskCollectionsExtensions
    {
        public static async Task<int> Count<T>(this Task<ReadOnlyCollection<T>> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.Count;
        }
        public static async Task<bool> Contains<T>(this Task<ReadOnlyCollection<T>> task, T value)
        {
            var el = await task.ConfigureAwait(false);
            return el.Contains(value);
        }
        public static async Task<IEnumerator<T>> GetEnumerator<T>(this Task<ReadOnlyCollection<T>> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.GetEnumerator();
        }
        public static async Task<int> Count<T>(this Task<List<T>> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.Count;
        }
        public static async Task<IEnumerator<T>> GetEnumerator<T>(this Task<List<T>> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.GetEnumerator();
        }
        public static async Task<ReadOnlyCollection<T>> AsReadOnly<T>(this Task<List<T>> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.AsReadOnly();
        }
        public static async Task<int> BinarySearch<T>(this Task<List<T>> task, int index, int count, T item, IComparer<T> comparer)
        {
            var el = await task.ConfigureAwait(false);
            return el.BinarySearch(index, count, item, comparer);
        }
        public static async Task<int> BinarySearch<T>(this Task<List<T>> task, T item)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOf(item);
        }
        public static async Task<int> BinarySearch<T>(this Task<List<T>> task, T item, IComparer<T> comparer)
        {
            var el = await task.ConfigureAwait(false);
            return el.BinarySearch(item, comparer);
        }
        public static async Task<bool> Contains<T>(this Task<List<T>> task, T item)
        {
            var el = await task.ConfigureAwait(false);
            return el.Contains(item);
        }
        public static async Task<List<TOutput>> ConvertAll<T, TOutput>(this Task<List<T>> task, Converter<T, TOutput> converter)
        {
            var el = await task.ConfigureAwait(false);
            return el.ConvertAll(converter);
        }
        public static async Task<T[]> CopyTo<T>(this Task<List<T>> task, T[] array, int arrayIndex)
        {
            var el = await task.ConfigureAwait(false);
            el.CopyTo(array, arrayIndex);
            return array;
        }
        public static async Task<T[]> CopyTo<T>(this Task<List<T>> task, int index, T[] array, int arrayIndex, int count)
        {
            var el = await task.ConfigureAwait(false);
            el.CopyTo(index, array, arrayIndex, count);
            return array;
        }
        public static async Task<T[]> CopyTo<T>(this Task<List<T>> task, T[] array)
        {
            var el = await task.ConfigureAwait(false);
            el.CopyTo(array);
            return array;
        }
        public static async Task<bool> Exists<T>(this Task<List<T>> task, Predicate<T> match)
        {
            var el = await task.ConfigureAwait(false);
            return el.Exists(match);
        }
        public static async Task<T> Find<T>(this Task<List<T>> task, Predicate<T> match)
        {
            var el = await task.ConfigureAwait(false);
            return el.Find(match);
        }
        public static async Task<List<T>> FindAll<T>(this Task<List<T>> task, Predicate<T> match)
        {
            var el = await task.ConfigureAwait(false);
            return el.FindAll(match);
        }
        public static async Task<int> FindIndex<T>(this Task<List<T>> task, Predicate<T> match)
        {
            var el = await task.ConfigureAwait(false);
            return el.FindIndex(match);
        }
        public static async Task<int> FindIndex<T>(this Task<List<T>> task, int startIndex, Predicate<T> match)
        {
            var el = await task.ConfigureAwait(false);
            return el.FindIndex(startIndex, match);
        }
        public static async Task<int> FindIndex<T>(this Task<List<T>> task, int startIndex, int count, Predicate<T> match)
        {
            var el = await task.ConfigureAwait(false);
            return el.FindIndex(startIndex, count, match);
        }
        public static async Task<T> FindLast<T>(this Task<List<T>> task, Predicate<T> match)
        {
            var el = await task.ConfigureAwait(false);
            return el.FindLast(match);
        }
        public static async Task<int> FindLastIndex<T>(this Task<List<T>> task, Predicate<T> match)
        {
            var el = await task.ConfigureAwait(false);
            return el.FindLastIndex(match);
        }
        public static async Task<int> FindLastIndex<T>(this Task<List<T>> task, int startIndex, Predicate<T> match)
        {
            var el = await task.ConfigureAwait(false);
            return el.FindLastIndex(startIndex, match);
        }
        public static async Task<int> FindLastIndex<T>(this Task<List<T>> task, int startIndex, int count, Predicate<T> match)
        {
            var el = await task.ConfigureAwait(false);
            return el.FindLastIndex(startIndex, count, match);
        }
        public static async Task<List<T>> ForEach<T>(this Task<List<T>> task, Action<T> action)
        {
            var el = await task.ConfigureAwait(false);
            el.ForEach(action);
            return el;
        }
        public static async Task<List<T>> GetRange<T>(this Task<List<T>> task, int index, int count)
        {
            var el = await task.ConfigureAwait(false);
            return el.GetRange(index, count);
        }
        public static async Task<int> IndexOf<T>(this Task<List<T>> task, T item, int index, int count)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOf(item, index, count);
        }
        public static async Task<int> IndexOf<T>(this Task<List<T>> task, T item, int index)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOf(item, index);
        }
        public static async Task<int> IndexOf<T>(this Task<List<T>> task, T item)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOf(item);
        }
        public static async Task<List<T>> Insert<T>(this Task<List<T>> task, int index, T item)
        {
            var el = await task.ConfigureAwait(false);
            el.Insert(index, item);
            return el;
        }
        public static async Task<List<T>> InsertRange<T>(this Task<List<T>> task, int index, IEnumerable<T> collection)
        {
            var el = await task.ConfigureAwait(false);
            el.InsertRange(index, collection);
            return el;
        }
        public static async Task<int> LastIndexOf<T>(this Task<List<T>> task, T item)
        {
            var el = await task.ConfigureAwait(false);
            return el.LastIndexOf(item);
        }
        public static async Task<int> LastIndexOf<T>(this Task<List<T>> task, T item, int index)
        {
            var el = await task.ConfigureAwait(false);
            return el.LastIndexOf(item, index);
        }
        public static async Task<int> LastIndexOf<T>(this Task<List<T>> task, T item, int index, int count)
        {
            var el = await task.ConfigureAwait(false);
            return el.LastIndexOf(item, index, count);
        }
        public static async Task<bool> Remove<T>(this Task<List<T>> task, T item)
        {
            var el = await task.ConfigureAwait(false);
            return el.Remove(item);
        }
        public static async Task<int> RemoveAll<T>(this Task<List<T>> task, Predicate<T> match)
        {
            var el = await task.ConfigureAwait(false);
            return el.RemoveAll(match);
        }
        public static async Task<List<T>> RemoveAt<T>(this Task<List<T>> task, int index)
        {
            var el = await task.ConfigureAwait(false);
            el.RemoveAt(index);
            return el;
        }
        public static async Task<List<T>> RemoveRange<T>(this Task<List<T>> task, int index, int count)
        {
            var el = await task.ConfigureAwait(false);
            el.RemoveRange(index, count);
            return el;
        }
        public static async Task<List<T>> Reverse<T>(this Task<List<T>> task, int index, int count)
        {
            var el = await task.ConfigureAwait(false);
            el.Reverse(index, count);
            return el;
        }
        public static async Task<List<T>> Reverse<T>(this Task<List<T>> task)
        {
            var el = await task.ConfigureAwait(false);
            el.Reverse();
            return el;
        }
        public static async Task<List<T>> Sort<T>(this Task<List<T>> task, int index, int count, IComparer<T> comparer)
        {
            var el = await task.ConfigureAwait(false);
            el.Sort(index, count, comparer);
            return el;
        }
        public static async Task<List<T>> Sort<T>(this Task<List<T>> task, Comparison<T> comparison)
        {
            var el = await task.ConfigureAwait(false);
            el.Sort(comparison);
            return el;
        }
        public static async Task<List<T>> Sort<T>(this Task<List<T>> task)
        {
            var el = await task.ConfigureAwait(false);
            el.Sort();
            return el;
        }
        public static async Task<List<T>> Sort<T>(this Task<List<T>> task, IComparer<T> comparer)
        {
            var el = await task.ConfigureAwait(false);
            el.Sort(comparer);
            return el;
        }
        public static async Task<T[]> ToArray<T>(this Task<List<T>> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.ToArray();
        }
        public static async Task<List<T>> TrimExcess<T>(this Task<List<T>> task)
        {
            var el = await task.ConfigureAwait(false);
            el.TrimExcess();
            return el;
        }
        public static async Task<bool> TrueForAll<T>(this Task<List<T>> task, Predicate<T> match)
        {
            var el = await task.ConfigureAwait(false);
            return el.TrueForAll(match);
        }
    }

}