// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Zu.Browser
{
    public class ZuRequestInfo
    {
        public string Body { get; set; }
        public string Url { get; set; }
        public string Code { get; set; }

        public override string ToString()
        {
            return Url;
        }
    }
}