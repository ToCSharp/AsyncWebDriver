// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MyCommunicationLib.Communication.MarionetteComands;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.Firefox
{
    public class FirefoxDriverJavaScriptExecutor: IJavaScriptExecutor
    {
        private IAsyncFirefoxDriver asyncFirefoxDriver;

        public FirefoxDriverJavaScriptExecutor(IAsyncFirefoxDriver asyncFirefoxDriver)
        {
            this.asyncFirefoxDriver = asyncFirefoxDriver;
        }

        public Task<object> ExecuteAsyncScript(string script, CancellationToken cancellationToken = default(CancellationToken), params object[] args)
        {
            throw new System.NotImplementedException();
        }

        public async Task<object> ExecuteScript(string script, CancellationToken cancellationToken = default(CancellationToken), params object[] args)
        {
            var res = await ExecuteScript(script, null, "defaultSandbox", cancellationToken, args);
            return res;
        }

        public async Task<JToken> ExecuteScript(string script, string filename = null,
    string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken(), params object[] args)
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new ExecuteScriptCommand(script) { filename = filename, sandbox = sandbox };
            if (args.Length > 0) comm1.Args = args.Select(v => v.ToString()).ToArray();
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error;
            return comm1.Result;
        }

    }
}