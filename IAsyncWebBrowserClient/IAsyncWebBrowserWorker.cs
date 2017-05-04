// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Zu.WebBrowser
{
    public enum Contexts
    {
        None = 0,
        Chrome = 1,
        Content = 2
    }

    public interface IAsyncWebBrowserWorker
    {
        string FilesBasePath { get; set; }
        Contexts CurrentContext { get; set; }
        Task<string> Connect(CancellationToken cancellationToken = new CancellationToken());
        Task<string> Disconnect(CancellationToken cancellationToken = new CancellationToken());

        Task<JToken> Eval(string expression, string fileName = null, string sandbox = "defaultSandbox",
            CancellationToken cancellationToken = new CancellationToken());

        Task<string> Eval2(string expression, string fileName = null, string sandbox = "defaultSandbox",
            CancellationToken cancellationToken = new CancellationToken());

        Task<JToken> EvalInContent(string expression, string fileName = null, string sandbox = "defaultSandbox",
            CancellationToken cancellationToken = new CancellationToken());

        Task<JToken> EvalInChrome(string expression, string fileName = null,
            CancellationToken cancellationToken = new CancellationToken());

        Task<JToken> EvalFile(string fileName, string sandbox = "defaultSandbox",
            CancellationToken cancellationToken = new CancellationToken());

        Task<bool> ObjectExists(string path, string sandbox = "defaultSandbox",
            CancellationToken cancellationToken = new CancellationToken());

        void AddEventListener(string to, Action<JToken> action);
        void RemoveEventListener(Action<JToken> action);

        Task<JToken> AddSendEventFunc(string name = "top.zuSendEvent",
            CancellationToken cancellationToken = new CancellationToken());

        Task<JToken> AddSendEventFuncIfNo(string name = "top.zuSendEvent",
            CancellationToken cancellationToken = new CancellationToken());

        Task<JToken> SendEvent(string to, string mess, string funcname = "top.zuSendEvent",
            CancellationToken cancellationToken = new CancellationToken());

        Task<Contexts> GetContext(CancellationToken cancellationToken = new CancellationToken());
        Task<JToken> SetContext(Contexts context, CancellationToken cancellationToken = new CancellationToken());
    }
}