// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Remote;

namespace Zu.AsyncWebDriver
{
    public static class TaskStringExtensions
    {
        public static async Task<int> CompareTo(this Task<string> task, object value)
        {
            var el = await task.ConfigureAwait(false);
            return el.CompareTo(value);
        }
        public static async Task<int> CompareTo(this Task<string> task, String strB)
        {
            var el = await task.ConfigureAwait(false);
            return el.CompareTo(strB);
        }
        public static async Task<bool> Contains(this Task<string> task, String value)
        {
            var el = await task.ConfigureAwait(false);
            return el.Contains(value);
        }
        public static async Task CopyTo(this Task<string> task, int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            var el = await task.ConfigureAwait(false);
            el.CopyTo(sourceIndex, destination, destinationIndex, count);
        }
        public static async Task<bool> EndsWith(this Task<string> task, String value)
        {
            var el = await task.ConfigureAwait(false);
            return el.EndsWith(value);
        }
        public static async Task<bool> EndsWith(this Task<string> task, String value, StringComparison comparisonType)
        {
            var el = await task.ConfigureAwait(false);
            return el.EndsWith(value, comparisonType);
        }
        public static async Task<bool> EndsWith(this Task<string> task, String value, bool ignoreCase, CultureInfo culture)
        {
            var el = await task.ConfigureAwait(false);
            return el.EndsWith(value, ignoreCase, culture);
        }
        public static async Task<CharEnumerator> GetEnumerator(this Task<string> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.GetEnumerator();
        }
        public static async Task<TypeCode> GetTypeCode(this Task<string> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.GetTypeCode();
        }
        public static async Task<int> IndexOf(this Task<string> task, char value, int startIndex, int count)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOf(value, startIndex, count);
        }
        public static async Task<int> IndexOf(this Task<string> task, char value, int startIndex)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOf(value, startIndex);
        }
        public static async Task<int> IndexOf(this Task<string> task, String value)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOf(value);
        }
        public static async Task<int> IndexOf(this Task<string> task, String value, int startIndex)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOf(value, startIndex);
        }
        public static async Task<int> IndexOf(this Task<string> task, String value, int startIndex, int count)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOf(value, startIndex, count);
        }
        public static async Task<int> IndexOf(this Task<string> task, String value, StringComparison comparisonType)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOf(value, comparisonType);
        }
        public static async Task<int> IndexOf(this Task<string> task, String value, int startIndex, StringComparison comparisonType)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOf(value, startIndex, comparisonType);
        }
        public static async Task<int> IndexOf(this Task<string> task, char value)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOf(value);
        }
        public static async Task<int> IndexOf(this Task<string> task, String value, int startIndex, int count, StringComparison comparisonType)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOf(value, startIndex, count, comparisonType);
        }
        public static async Task<int> IndexOfAny(this Task<string> task, char[] anyOf, int startIndex, int count)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOfAny(anyOf, startIndex, count);
        }
        public static async Task<int> IndexOfAny(this Task<string> task, char[] anyOf, int startIndex)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOfAny(anyOf, startIndex);
        }
        public static async Task<int> IndexOfAny(this Task<string> task, char[] anyOf)
        {
            var el = await task.ConfigureAwait(false);
            return el.IndexOfAny(anyOf);
        }
        public static async Task<String> Insert(this Task<string> task, int startIndex, String value)
        {
            var el = await task.ConfigureAwait(false);
            return el.Insert(startIndex, value);
        }
        public static async Task<bool> IsNormalized(this Task<string> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.IsNormalized();
        }
        public static async Task<bool> IsNormalized(this Task<string> task, NormalizationForm normalizationForm)
        {
            var el = await task.ConfigureAwait(false);
            return el.IsNormalized(normalizationForm);
        }
        public static async Task<int> LastIndexOf(this Task<string> task, String value, int startIndex, int count, StringComparison comparisonType)
        {
            var el = await task.ConfigureAwait(false);
            return el.LastIndexOf(value, startIndex, count, comparisonType);
        }
        public static async Task<int> LastIndexOf(this Task<string> task, char value, int startIndex, int count)
        {
            var el = await task.ConfigureAwait(false);
            return el.LastIndexOf(value, startIndex, count);
        }
        public static async Task<int> LastIndexOf(this Task<string> task, char value, int startIndex)
        {
            var el = await task.ConfigureAwait(false);
            return el.LastIndexOf(value, startIndex);
        }
        public static async Task<int> LastIndexOf(this Task<string> task, char value)
        {
            var el = await task.ConfigureAwait(false);
            return el.LastIndexOf(value);
        }
        public static async Task<int> LastIndexOf(this Task<string> task, String value, int startIndex)
        {
            var el = await task.ConfigureAwait(false);
            return el.LastIndexOf(value, startIndex);
        }
        public static async Task<int> LastIndexOf(this Task<string> task, String value, int startIndex, StringComparison comparisonType)
        {
            var el = await task.ConfigureAwait(false);
            return el.LastIndexOf(value, startIndex, comparisonType);
        }
        public static async Task<int> LastIndexOf(this Task<string> task, String value, int startIndex, int count)
        {
            var el = await task.ConfigureAwait(false);
            return el.LastIndexOf(value, startIndex, count);
        }
        public static async Task<int> LastIndexOf(this Task<string> task, String value, StringComparison comparisonType)
        {
            var el = await task.ConfigureAwait(false);
            return el.LastIndexOf(value);
        }
        public static async Task<int> LastIndexOf(this Task<string> task, String value)
        {
            var el = await task.ConfigureAwait(false);
            return el.LastIndexOf(value);
        }
        public static async Task<int> LastIndexOfAny(this Task<string> task, char[] anyOf)
        {
            var el = await task.ConfigureAwait(false);
            return el.LastIndexOfAny(anyOf);
        }
        public static async Task<int> LastIndexOfAny(this Task<string> task, char[] anyOf, int startIndex)
        {
            var el = await task.ConfigureAwait(false);
            return el.LastIndexOfAny(anyOf, startIndex);
        }
        public static async Task<int> LastIndexOfAny(this Task<string> task, char[] anyOf, int startIndex, int count)
        {
            var el = await task.ConfigureAwait(false);
            return el.LastIndexOfAny(anyOf, startIndex, count);
        }
        public static async Task<String> Normalize(this Task<string> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.Normalize();
        }
        public static async Task<String> Normalize(this Task<string> task, NormalizationForm normalizationForm)
        {
            var el = await task.ConfigureAwait(false);
            return el.Normalize(normalizationForm);
        }
        public static async Task<String> PadLeft(this Task<string> task, int totalWidth, char paddingChar)
        {
            var el = await task.ConfigureAwait(false);
            return el.PadLeft(totalWidth, paddingChar);
        }
        public static async Task<String> PadLeft(this Task<string> task, int totalWidth)
        {
            var el = await task.ConfigureAwait(false);
            return el.PadLeft(totalWidth);
        }
        public static async Task<String> PadRight(this Task<string> task, int totalWidth, char paddingChar)
        {
            var el = await task.ConfigureAwait(false);
            return el.PadRight(totalWidth, paddingChar);
        }
        public static async Task<String> PadRight(this Task<string> task, int totalWidth)
        {
            var el = await task.ConfigureAwait(false);
            return el.PadRight(totalWidth);
        }
        public static async Task<String> Remove(this Task<string> task, int startIndex)
        {
            var el = await task.ConfigureAwait(false);
            return el.Remove(startIndex);
        }
        public static async Task<String> Remove(this Task<string> task, int startIndex, int count)
        {
            var el = await task.ConfigureAwait(false);
            return el.Remove(startIndex, count);
        }
        public static async Task<String> Replace(this Task<string> task, String oldValue, String newValue)
        {
            var el = await task.ConfigureAwait(false);
            return el.Replace(oldValue, newValue);
        }
        public static async Task<String> Replace(this Task<string> task, char oldChar, char newChar)
        {
            var el = await task.ConfigureAwait(false);
            return el.Replace(oldChar, newChar);
        }
        public static async Task<String[]> Split(this Task<string> task, params char[] separator)
        {
            var el = await task.ConfigureAwait(false);
            return el.Split(separator);
        }
        public static async Task<String[]> Split(this Task<string> task, char[] separator, int count)
        {
            var el = await task.ConfigureAwait(false);
            return el.Split(separator, count);
        }
        public static async Task<String[]> Split(this Task<string> task, char[] separator, StringSplitOptions options)
        {
            var el = await task.ConfigureAwait(false);
            return el.Split(separator, options);
        }
        public static async Task<String[]> Split(this Task<string> task, char[] separator, int count, StringSplitOptions options)
        {
            var el = await task.ConfigureAwait(false);
            return el.Split(separator, count, options);
        }
        public static async Task<String[]> Split(this Task<string> task, String[] separator, StringSplitOptions options)
        {
            var el = await task.ConfigureAwait(false);
            return el.Split(separator, options);
        }
        public static async Task<String[]> Split(this Task<string> task, String[] separator, int count, StringSplitOptions options)
        {
            var el = await task.ConfigureAwait(false);
            return el.Split(separator, count, options);
        }
        public static async Task<bool> StartsWith(this Task<string> task, String value, StringComparison comparisonType)
        {
            var el = await task.ConfigureAwait(false);
            return el.StartsWith(value, comparisonType);
        }
        public static async Task<bool> StartsWith(this Task<string> task, String value, bool ignoreCase, CultureInfo culture)
        {
            var el = await task.ConfigureAwait(false);
            return el.StartsWith(value, ignoreCase, culture);
        }
        public static async Task<bool> StartsWith(this Task<string> task, String value)
        {
            var el = await task.ConfigureAwait(false);
            return el.StartsWith(value);
        }
        public static async Task<String> Substring(this Task<string> task, int startIndex, int length)
        {
            var el = await task.ConfigureAwait(false);
            return el.Substring(startIndex, length);
        }
        public static async Task<String> Substring(this Task<string> task, int startIndex)
        {
            var el = await task.ConfigureAwait(false);
            return el.Substring(startIndex);
        }
        public static async Task<char[]> ToCharArray(this Task<string> task, int startIndex, int length)
        {
            var el = await task.ConfigureAwait(false);
            return el.ToCharArray(startIndex, length);
        }
        public static async Task<char[]> ToCharArray(this Task<string> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.ToCharArray();
        }
        public static async Task<String> ToLower(this Task<string> task, CultureInfo culture)
        {
            var el = await task.ConfigureAwait(false);
            return el.ToLower(culture);
        }
        public static async Task<String> ToLower(this Task<string> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.ToLower();
        }
        public static async Task<String> ToLowerInvariant(this Task<string> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.ToLowerInvariant();
        }
        public static async Task<String> ToUpper(this Task<string> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.ToUpper();
        }
        public static async Task<String> ToUpper(this Task<string> task, CultureInfo culture)
        {
            var el = await task.ConfigureAwait(false);
            return el.ToUpper(culture);
        }
        public static async Task<String> ToUpperInvariant(this Task<string> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.ToUpperInvariant();
        }
        public static async Task<String> Trim(this Task<string> task, params char[] trimChars)
        {
            var el = await task.ConfigureAwait(false);
            return el.Trim(trimChars);
        }
        public static async Task<String> Trim(this Task<string> task)
        {
            var el = await task.ConfigureAwait(false);
            return el.Trim();
        }
        public static async Task<String> TrimEnd(this Task<string> task, params char[] trimChars)
        {
            var el = await task.ConfigureAwait(false);
            return el.TrimEnd(trimChars);
        }
        public static async Task<String> TrimStart(this Task<string> task, params char[] trimChars)
        {
            var el = await task.ConfigureAwait(false);
            return el.TrimStart(trimChars);
        }
    }

    public class TaskStringExtensionsTest
    {
        public async Task Method()
        {
            var parent = new AsyncWebElement(null, null);
            var count = await parent.FindElement(By.TagName("div")).Text().ToLower().ConfigureAwait(false);
        }
    }
}