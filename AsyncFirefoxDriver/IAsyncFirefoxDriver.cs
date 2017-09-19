// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.Communication;
using Zu.WebBrowser.Firefox;

namespace Zu.Firefox
{
    public interface IAsyncFirefoxDriver: IAsyncWebBrowserClientFirefox
    {
        IDebuggerClient ClientMarionette { get; set; }

    }
}