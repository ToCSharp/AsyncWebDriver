using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zu.WebBrowser;

namespace Zu.Browser
{
    public class ZuRequestListener
    {
        private IAsyncWebBrowserWorker browserClient;
        public event EventHandler<JToken> StartRequest;
        public event EventHandler<JToken> StopRequest;

        public bool IsListeningFileLoaded { get; set; } = false;
        public event EventHandler<ZuRequestInfo> FileLoaded;

        public bool DoSendBinary { get; set; } = false;
        public string SaveAllFilesToFolder { get; set; } = null;
        public ZuRequestListener(IAsyncWebBrowserWorker browserClient)
        {
            this.browserClient = browserClient;
        }

        public async Task DoNotRecordFilesWithExt(string str)
        {
            await browserClient?.SetContext(Contexts.Chrome);
            if (!(await browserClient?.ObjectExists("httpResponseObserver")))
            {
                var res = await browserClient?.EvalFile("TracingListener.js");
            }
            //await browserClient?.Eval($"httpResponseObserver.filesExtNotRecord.push('{str}');");
            await browserClient?.Eval($@"if (httpResponseObserver.filesExtNotRecord.indexOf('{str}') === -1) {{
    httpResponseObserver.filesExtNotRecord.push('{str}');
        }}");

        }

        public async Task DoNotRecordContentType(string str)
        {
            await browserClient?.SetContext(Contexts.Chrome);
            if (!(await browserClient?.ObjectExists("httpResponseObserver")))
            {
                var res = await browserClient?.EvalFile("TracingListener.js");
            }
            await browserClient?.Eval($@"if (httpResponseObserver.contentTypesNotRecord.indexOf('{str}') === -1) {{
    httpResponseObserver.contentTypesNotRecord.push('{str}');
        }}");
        }
        public async Task DoNotRecordZip()
        {
            await DoNotRecordContentType("application/zip");
        }
            //        public async Task StartListening()
            //        {
            //            browserClient?.AddEventListener("JsRequestListener", OnEventFromConnection);
            //            var res = await browserClient?.EvalFile("TracingListenerSaverReplacer.js");
            //            res = await browserClient?.Eval(@"
            //this.httpResponseObserver.doSaveAll = true;
            //function onFileSaved(url, path, hashC){
            //    server.sendEvent({""to"": ""JsRequestListener"", ""type"": 1, ""url"": url, ""path"": path, ""hashC"": hashC}); 
            //} 
            //this.httpResponseObserver.addListener(this);
            //");
            //        }

            //        public async Task StartListeningFileLoaded()
            //        {
            //            IsListeningFileLoaded = true;
            //            browserClient?.AddEventListener("JsRequestListener", OnEventFromConnection);
            //            try
            //            {
            //                var res = await browserClient?.EvalFile("TracingListener.js");
            //                //res = await browserClient?.Eval("this.TracingListenerActivate(); return 'ok';");
            //                res = await browserClient?.Eval(@"
            //this.onFileLoaded = function (url, body){
            //    server.sendEvent({""to"": ""JsRequestListener"", ""type"": 2, ""url"": url, ""body"": body}); 
            //} 
            //this.httpResponseObserver.addListener(this);
            //");
            //            }catch(Exception ex)
            //            {

            //            }
            //        }

            public async Task StartListeningFileLoaded()
        {
            IsListeningFileLoaded = true;
            browserClient?.AddEventListener("JsRequestListener", OnEventFromConnection);
            browserClient?.AddEventListener("RequestListener", OnRequestListenerEvent);
            try
            {
                await browserClient?.SetContext(Contexts.Chrome);
                if (!(await browserClient?.ObjectExists("httpResponseObserver")))
                {
                    var res1 = await browserClient?.EvalFile("TracingListener.js");
                }
                if (DoSendBinary)
                {
                    var res1 = await browserClient?.EvalFile("base64.js");
                    res1 = await browserClient?.Eval($"httpResponseObserver.doSendBinary = true; return 'ok'");
                }
                if (SaveAllFilesToFolder != null)
                {
                    var res1 = await browserClient?.Eval("require = this._zuAddon.require;");
                    res1 = await browserClient?.Eval($"httpResponseObserver.pathToSave = \"{SaveAllFilesToFolder.Replace("\\", "\\\\")}\"");
                    res1 = await browserClient?.Eval($"httpResponseObserver.doSaveAll = true; return 'ok'");
                }
                var res = await browserClient?.AddSendEventFuncIfNo();
                //res = await browserClient?.Eval("this.TracingListenerActivate(); return 'ok';");
                res = await browserClient?.EvalInChrome(@"try {
this.onFileLoaded = function (url, body, code){
    top.zuSendEvent({""to"": ""JsRequestListener"", ""type"": 2, ""url"": url, ""body"": body, ""code"": code}); 
} 
this.httpResponseObserver.addListener(this);
} catch(ex) {
return ex.toString();
}
return ""ok"";
");
            }
            catch (Exception ex)
            {

            }
        }

        private void OnRequestListenerEvent(JToken obj)
        {
            var t = obj?["fname"]?["value"]?.ToString();
            if (t == "RequestListener.onStartRequest")
            {
                StartRequest?.Invoke(this, obj);
            }
            else if (t == "RequestListener.onStopRequest")
            {
                StopRequest?.Invoke(this, obj);
            }
        }

        private void OnEventFromConnection(JToken json)
        {
            var type = json?["type"]?["value"]?.ToString();
            if (type == "2")
            {
                var url = json?["url"]?["value"]?.ToString();
                var body = json?["body"]?["value"]?.ToString();
                var code = json?["code"]?["value"]?.ToString();
                FileLoaded?.Invoke(this, new ZuRequestInfo { Url = url, Body = body, Code = code });
            }
        }
    }
}
