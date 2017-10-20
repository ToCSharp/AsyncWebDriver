// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using MyCommunicationLib.Communication.MarionetteComands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;

namespace Zu.Firefox
{
    public class FirefoxDriverJavaScriptExecutor: IJavaScriptExecutor
    {
        private IAsyncFirefoxDriver asyncFirefoxDriver;

        public string Sandbox { get; set; } = null;

        public void DoExecuteInDefaultSandbox() => Sandbox = "defaultSandbox";

        public FirefoxDriverJavaScriptExecutor(IAsyncFirefoxDriver asyncFirefoxDriver)
        {
            this.asyncFirefoxDriver = asyncFirefoxDriver;
        }

        public async Task<object> ExecuteScript(string script, CancellationToken cancellationToken = default(CancellationToken), params object[] args)
        {
            var res = await ExecuteScript(script, null, Sandbox/*"defaultSandbox"*/, cancellationToken, args);
            return res;
        }

        public async Task<object> ExecuteScript(string script, string filename = null,
    string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken(), params object[] args)
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new ExecuteScriptCommand(script) { filename = filename, sandbox = sandbox };
            if (args.Length > 0) comm1.Args = args; //.Select(v => v.ToString()).ToArray();
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null)
            {
                var err = comm1.Error["error"]?.ToString();
                if (err == "javascript error") throw new InvalidOperationException(comm1.Error["message"].ToString() ?? comm1.Error.ToString());
                /*if (err == "stale element reference") */throw new WebBrowserException(comm1.Error);
            }
            return ParseExecuteScriptReturnValue((comm1.Result as JObject)?["value"]); //comm1.Result;
        }

        public async Task<object> ExecuteAsyncScript(string script, CancellationToken cancellationToken = default(CancellationToken), params object[] args)
        {
            var res = await ExecuteAsyncScript(script, null, null/*"defaultSandbox"*/, cancellationToken, args);
            return res;
        }

        public async Task<object> ExecuteAsyncScript(string script, string filename = null,
    string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken(), params object[] args)
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new ExecuteAsyncScriptCommand(script) { filename = filename, sandbox = sandbox };
            if (args.Length > 0) comm1.Args = args; //.Select(v => v.ToString()).ToArray();
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null)
            {
                var err = comm1.Error["error"]?.ToString();
                if (err == "javascript error") throw new InvalidOperationException(comm1.Error["message"].ToString() ?? comm1.Error.ToString());
                /*if (err == "stale element reference") */
                throw new WebBrowserException(comm1.Error);
            }
            return ParseExecuteScriptReturnValue((comm1.Result as JObject)?["value"]); //comm1.Result;
        }

        internal List<string> ArgsToStringList(object[] args)
        {
            return args.Select(v => ArgToString(v)).ToList();
        }
        internal string ArgToString(object arg)
        {
            if (arg == null) return "null";
            if (arg is bool) return (bool)arg ? "true" : "false";
            if (arg is string) return $"'{(string)arg}'";
            IDictionary dictionaryArg = arg as IDictionary;
            if (dictionaryArg != null)
            {
                List<string> stringList = new List<string>();
                foreach (DictionaryEntry kv in dictionaryArg)
                {
                    stringList.Add($"'{kv.Key}': {ArgToString(kv.Value)}");
                }
                return $"{{ {string.Join(", ", stringList)} }}";
            }
            if (arg is IDictionary<string, object>) return $"{{ {string.Join(", ", ((IDictionary<string, object>)arg).Select(v => ArgToString(v)))} }}";
            if (arg is KeyValuePair<string, object>)
            {
                var kv = (KeyValuePair<string, object>)arg;
                return $"{{ '{kv.Key}': {ArgToString(kv.Value)} }}";
            }
            IEnumerable enumerableArg = arg as IEnumerable;
            if (enumerableArg != null)
            {
                List<object> objectList = new List<object>();
                foreach (object item in enumerableArg)
                {
                    objectList.Add(item);
                }
                return $"[ {string.Join(", ", ArgsToStringList((objectList.ToArray())))} ]";
            }
            return arg.ToString();

        }

        private object ParseExecuteScriptReturnValue(JToken responseValue)
        {
            if (responseValue is JValue) return ((JValue)responseValue).Value;
            if (responseValue is JArray)
            {
                var res = new List<object>();
                foreach (var item in (JArray)responseValue)
                {
                    res.Add(ParseExecuteScriptReturnValue(item));
                }
                return res.ToArray();
            }
            else if (responseValue is JObject)
            {
                var res = new Dictionary<string, object>();
                foreach (var item in (JObject)responseValue)
                {
                    res.Add(item.Key, ParseExecuteScriptReturnValue(item.Value));
                }
                return res;
            }
            return responseValue;
        }
    }
}