// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// This file is based on or incorporates material from the project Selenium, licensed under the Apache License, Version 2.0. More info in THIRD-PARTY-NOTICES file.

using System.Threading.Tasks;
using Zu.WebBrowser.BasicTypes;

namespace Zu.AsyncWebDriver
{
    public static class TaskWebSizeWebPointExtensions
    {
        public static async Task<int> Width(this Task<WebSize> elementTask)
        {
            var el = await elementTask.ConfigureAwait(false);
            return el.Width;
        }
        public static async Task<int> Height(this Task<WebSize> elementTask)
        {
            var el = await elementTask.ConfigureAwait(false);
            return el.Height;
        }

        public static async Task<int> X(this Task<WebPoint> elementTask)
        {
            var el = await elementTask.ConfigureAwait(false);
            return el.X;
        }
        public static async Task<int> Y(this Task<WebPoint> elementTask)
        {
            var el = await elementTask.ConfigureAwait(false);
            return el.Y;
        }
        public static async Task<WebPoint> Offset(this Task<WebPoint> elementTask, int x, int y)
        {
            var el = await elementTask.ConfigureAwait(false);
            return el.Offset(x, y);
        }
    }
}