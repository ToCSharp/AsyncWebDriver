// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Firefox
{
    internal class ResultValueConverter
    {
        internal static bool ToBool(JToken result)
        {
            return result is JValue ? (result.ToString().ToLower() == "true") : (result?["value"]?.ToString().ToLower() == "true");
        }

        internal static WebPoint ToWebPoint(JToken value)
        {
            var res = (value as JObject)?["value"];
            if (res != null)
            {
                var x = res["x"]?.Value<int>();
                var y = res["y"]?.Value<int>();
                if (x != null && y != null) return new WebPoint((int)x, (int)y);
            }
            return null;
        }

 
        internal static WebSize ToWebSize(JToken value)
        {
            var res = (value as JObject)?["value"];
            if (res != null)
            {
                var width = res["width"]?.Value<int>();
                var height = res["height"]?.Value<int>();
                if (width != null && height != null) return new WebSize((int)width, (int)height);
            }
            return null;
        }

        internal static WebRect ToWebRect(JToken value)
        {
            var res = (value as JObject)?["value"];
            if (res != null)
            {
                var x = res["x"]?.Value<int>() ?? res["left"]?.Value<int>();
                var y = res["y"]?.Value<int>() ?? res["top"]?.Value<int>();
                var width = res["width"]?.Value<int>();
                var height = res["height"]?.Value<int>();
                if (x != null && y != null && width != null && height != null) return new WebRect((int)x, (int)y, (int)width, (int)height);
            }
            return null;
        }

        internal static List<string> WindowHandles(JToken result)
        {
            var res = new List<string>();
            var arr = result as JArray;
            if(arr != null)
            {
                foreach (var item in arr)
                {
                    res.Add(item.ToString());
                }
            }
            return res;
        }

        internal static bool ValueIsNull(JToken res)
        {
            if (res == null) return true;
            if (res?["value"] is JValue && (res?["value"] as JValue)?.Value == null) return true;
            return false;
        }
    }
}