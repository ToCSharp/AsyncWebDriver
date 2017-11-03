// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MyCommunicationLib.Communication.MarionetteComands;
using Newtonsoft.Json.Linq;
using Zu.WebBrowser;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;
using Zu.WebBrowser.BrowserOptions;
using Zu.WebBrowser.Communication;
using Zu.WebBrowser.Firefox;

namespace Zu.Firefox
{
    public class AsyncFirefoxDriver : IAsyncFirefoxDriver
    {
        #region Connection
        protected IDebuggerConnection _connectionM;
        protected IDebuggerConnection _connectionM2;
        private Uri _debuggerEndpointUri;
        private Uri _debuggerEndpointUri2;
        public IDebuggerClient ClientMarionette
        {
            get;
            set;
        }

        public IDebuggerClient clientMarionette2;
        public FirefoxDriverConfig Config
        {
            get;
            set;
        }

        protected List<Tuple<string, Action<JToken>>> eventAsyncActions = new List<Tuple<string, Action<JToken>>>();
        private object lock1 = new object ();
        private readonly object lock2 = new object ();
        public int Port
        {
            get => Config.Port;
            set => Config.SetPort(value);
        }

        public int Port2;
        private int scriptInd;
        public AsyncFirefoxDriver(DriverConfig config): this (new FirefoxDriverConfig(config))
        {
        }

        public AsyncFirefoxDriver(FirefoxDriverConfig config = null)
        //: this(FirefoxProfilesWorker.GetMarionettePort(profileName))
        {
            if (config == null)
                Config = new FirefoxDriverConfig().SetIsTempProfile();
            else
                Config = config;
            if (Config.IsDefaultProfile)
                Config.SetProfileName("default");
            if (Config.Port == 0)
            {
                Config.Port = 11000 + rnd.Next(2000);
            }

            Port2 = Port + 10000;
        }

        public AsyncFirefoxDriver(string profileName): this (FirefoxProfilesWorker.GetMarionettePort(profileName))
        {
            Config.ProfileName = profileName;
        }

        public AsyncFirefoxDriver(int port, int port2 = 0)
        {
            Config = new FirefoxDriverConfig();
            CurrentContext = Contexts.Chrome;
            Port = port;
            Port2 = port2;
            if (Port2 == 0)
                Port2 = Port + 10000;
        }

        public int ConnectTimeoutMs
        {
            get;
            set;
        } = 10000;

        public bool DoConnect2
        {
            get;
            set;
        } = true;

        public bool DoConnectWhenCheckConnected
        {
            get;
            set;
        } = true; 

        private bool isConnected = false;
        public async Task CheckConnected(CancellationToken cancellationToken = default (CancellationToken))
        {
            if (!DoConnectWhenCheckConnected)
                return;
            DoConnectWhenCheckConnected = false;
            if (!isConnected)
            {
                isConnected = true;
                await Connect(cancellationToken).ConfigureAwait(false);
            }
        }

        public virtual async Task<DriverProcessInfo> OpenFirefoxProfile(FirefoxDriverConfig config)
        {
            if (config.DoOpenBrowserDevTools)
            {
                config.SetIsMultiprocessFalse().SetDoSetDebuggerRemoteEnabled();
            }

            DriverProcessInfo res = null;
            await Task.Run(async () => res = await FirefoxProfilesWorker.OpenFirefoxProfile(config)).ConfigureAwait(false); // userDir, Port, isHeadless));
            return res;
        }

        public Task<string> Open(CancellationToken cancellationToken = new CancellationToken())
        {
            return Connect(cancellationToken);
        }

        public virtual async Task<string> Connect(CancellationToken cancellationToken = new CancellationToken())
        {
            if (Port == 0)
                return "Error: MarionettePort not set";
            DoConnectWhenCheckConnected = false;
            if (!Config.DoNotOpenChromeProfile)
            {
                DriverProcess = await OpenFirefoxProfile(Config).ConfigureAwait(false);
                if (Config.IsTempProfile)
                    await Task.Delay(Config.TempDirCreateDelay).ConfigureAwait(false);
            }

            _connectionM = new DebuggerConnectionMarionette(new NetworkClientFactory());
            _debuggerEndpointUri = new UriBuilder{Scheme = "tcp", Host = "127.0.0.1", Port = Port}.Uri;
            //_connectionM.OutputMessage += _connection_OutputMessage;
            var r = "connected";
            await Task.Factory.StartNew(async () =>
            {
                var timeoutAt = DateTime.Now.AddMilliseconds(ConnectTimeoutMs);
                while (DateTime.Now < timeoutAt)
                    try
                    {
                        _connectionM.Connect(_debuggerEndpointUri);
                        r = "connected";
                        break;
                    }
                    catch (SocketException se)
                    {
                        r = se.Message;
                        await Task.Delay(500, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        r = ex.Message;
                        break;
                    }
            }
            , cancellationToken).ConfigureAwait(false);

            if (r != "connected")
                return r;
            ClientMarionette = new DebuggerClientMarionette(_connectionM);
            var comm1 = new NewSessionCommand();
            try
            {
                await ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                await Task.Delay(200, cancellationToken).ConfigureAwait(false);
                try
                {
                    await ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    return e.ToString();
                }
            }

            var res = comm1.Result;
            if (DoConnect2)
            {
                var res2 = await Connect2().ConfigureAwait(false);
            }

            await SetContextChrome(cancellationToken).ConfigureAwait(false);
            if (Config.DoOpenBrowserDevTools)
                await OpenBrowserDevTools().ConfigureAwait(false);
            return "Marionette " + Port + (_connectionM2?.Connected == true ? " Marionette2 " + Port2 : "");
        }

        #endregion
        public Contexts CurrentContext
        {
            get;
            set;
        }

        #region IAsyncWebBrowserClient
        public IMouse Mouse => mouse ?? (mouse = new FirefoxDriverMouse(this));
        public IKeyboard Keyboard => keyboard ?? (keyboard = new FirefoxDriverKeyboard(this));
        public INavigation Navigation => navigation ?? (navigation = new FirefoxDriverNavigation(this)); // { get { if (navigation == null) navigation = new FirefoxDriverNavigation(this); return navigation; } }
        public IJavaScriptExecutor JavaScriptExecutor => firefoxJavaScriptExecutor ?? (firefoxJavaScriptExecutor = new FirefoxDriverJavaScriptExecutor(this)); // { get { if (firefoxJavaScriptExecutor == null) firefoxJavaScriptExecutor = new FirefoxDriverJavaScriptExecutor(this); return firefoxJavaScriptExecutor; } }
        public IOptions Options => options ?? (options = new FirefoxDriverOptions(this));
        public ITargetLocator TargetLocator => targetLocator ?? (targetLocator = new FirefoxDriverTargetLocator(this));
        public IElements Elements => elements ?? (elements = new FirefoxDriverElements(this));
        public IAlert Alert => alert ?? (alert = new FirefoxDriverAlert(this));
        public ICoordinates Coordinates => coordinates ?? (coordinates = new FirefoxDriverCoordinates(this));
        public ITakesScreenshot Screenshot => screenshot ?? (screenshot = new FirefoxDriverScreenshot(this));
        public ITouchScreen TouchScreen => touchScreen ?? (touchScreen = new FirefoxDriverTouchScreen(this));
        public IActionExecutor ActionExecutor => actionExecutor ?? (actionExecutor = new FirefoxDriverActionExecutor(this));
        private IMouse mouse;
        private IKeyboard keyboard;
        private FirefoxDriverNavigation navigation;
        public FirefoxDriverJavaScriptExecutor firefoxJavaScriptExecutor;
        public FirefoxDriverJavaScriptExecutor FirefoxJavaScriptExecutor => firefoxJavaScriptExecutor ?? (firefoxJavaScriptExecutor = new FirefoxDriverJavaScriptExecutor(this));
        private FirefoxDriverOptions options;
        private FirefoxDriverTargetLocator targetLocator;
        private FirefoxDriverElements elements;
        private FirefoxDriverAlert alert;
        private FirefoxDriverCoordinates coordinates;
        private FirefoxDriverScreenshot screenshot;
        private FirefoxDriverTouchScreen touchScreen;
        private FirefoxDriverActionExecutor actionExecutor;
        #endregion

        public async Task<string> GetPageSource(CancellationToken cancellationToken = new CancellationToken())
        {
            await CheckConnected(cancellationToken).ConfigureAwait(false);
            if (ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new GetPageSourceCommand();
            await ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<string> GetTitle(CancellationToken cancellationToken = new CancellationToken())
        {
            await CheckConnected(cancellationToken).ConfigureAwait(false);
            if (ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new GetTitleCommand();
            await ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public virtual async Task<string> Close(CancellationToken cancellationToken = new CancellationToken())
        {
            //await CheckConnected(cancellationToken);
            //if (clientMarionette == null) throw new Exception("error: no clientMarionette");
            //var comm1 = new CloseCommand();
            //await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            //if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            if (DriverProcess != null)
                await DriverProcess.CloseAsync(cancellationToken).ConfigureAwait(false);
            DriverProcess = null;
            if (Config.IsTempProfile)
                await Task.Run(() => FirefoxProfilesWorker.RemoveProfile(Config.ProfileName)).ConfigureAwait(false);
            return "ok";
        }

        public virtual void CloseSync()
        {
            BrowserDevTools?.CloseSync();
            DriverProcess?.Close();
            DriverProcess = null;
            if (Config.IsTempProfile)
                FirefoxProfilesWorker.RemoveProfile(Config.ProfileName);
        }

        public async Task<string> CloseChromeWindow(CancellationToken cancellationToken = new CancellationToken())
        {
            await CheckConnected(cancellationToken).ConfigureAwait(false);
            if (ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new CloseChromeWindowCommand();
            await ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
            return "ok";
        }

        public async Task<string> SessionTearDown(CancellationToken cancellationToken = new CancellationToken())
        {
            await CheckConnected(cancellationToken).ConfigureAwait(false);
            if (ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new SessionTearDownCommand();
            await ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
            return "ok";
        }

        public async Task<string> GetActiveFrame(CancellationToken cancellationToken = new CancellationToken())
        {
            await CheckConnected(cancellationToken).ConfigureAwait(false);
            if (ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new GetActiveFrameCommand();
            await ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<string> GetChromeWindowHandle(CancellationToken cancellationToken = new CancellationToken())
        {
            await CheckConnected(cancellationToken).ConfigureAwait(false);
            if (ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new GetChromeWindowHandleCommand();
            await ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<JToken> GetChromeWindowHandles(CancellationToken cancellationToken = new CancellationToken())
        {
            await CheckConnected(cancellationToken).ConfigureAwait(false);
            if (ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new GetChromeWindowHandlesCommand();
            await ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
            return comm1.Result;
        }

        public async Task<string> GetWindowType(CancellationToken cancellationToken = new CancellationToken())
        {
            await CheckConnected(cancellationToken).ConfigureAwait(false);
            if (ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new GetWindowTypeCommand();
            await ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<string> ImportScript(string script, CancellationToken cancellationToken = new CancellationToken())
        {
            await CheckConnected(cancellationToken).ConfigureAwait(false);
            if (ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new ImportScriptCommand(script);
            await ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
            return "ok";
        }

        public async Task<string> ClearImportedScripts(CancellationToken cancellationToken = new CancellationToken())
        {
            await CheckConnected(cancellationToken).ConfigureAwait(false);
            if (ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new ClearImportedScriptsCommand();
            await ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
            return "ok";
        }

        public async Task<JToken> GetClientContext(CancellationToken cancellationToken = new CancellationToken())
        {
            await CheckConnected(cancellationToken).ConfigureAwait(false);
            if (ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new GetContextCommand();
            await ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
            return comm1.Result;
        }

        public async Task<string> GetString(string path, CancellationToken cancellationToken = new CancellationToken())
        {
            //string s = (await Eval(path))?["value"]?.ToString();
            //return s;
            var res = await ((FirefoxDriverJavaScriptExecutor)JavaScriptExecutor).ExecuteScript($"return {path}.toString()", null, "defaultSandbox", cancellationToken);
            return res.ToString(); //?["value"].ToString();
        }

        public async Task<JToken> SetContextChrome(CancellationToken cancellationToken = new CancellationToken())
        {
            await CheckConnected(cancellationToken).ConfigureAwait(false);
            if (ClientMarionette == null)
                throw new Exception("error: no clientMarionette");
            var comm1 = new SetContextCommand(SetContextCommand.Contexts.chrome);
            await ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
            if (comm1.Error != null)
                throw new WebBrowserException(comm1.Error);
            CurrentContext = Contexts.Chrome;
            return comm1.Result;
        }

        public async Task<JToken> SetContextContent(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                await CheckConnected(cancellationToken).ConfigureAwait(false);
                if (ClientMarionette == null)
                    throw new Exception("error: no clientMarionette");
                var comm1 = new SetContextCommand(SetContextCommand.Contexts.content);
                await ClientMarionette.SendRequestAsync(comm1, cancellationToken).ConfigureAwait(false);
                if (comm1.Error != null)
                    throw new WebBrowserException(comm1.Error);
                CurrentContext = Contexts.Content;
                return comm1.Result;
            }
            catch
            {
                throw;
            }
        }

        public string FilesBasePath
        {
            get;
            set;
        } = "js\\";

        public DriverProcessInfo DriverProcess
        {
            get;
            set;
        }

        public AsyncFirefoxDriver BrowserDevTools
        {
            get;
            private set;
        }

        public async Task<object> AddSendEventFunc(string name = "top.zuSendEvent", CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await ((FirefoxDriverJavaScriptExecutor)JavaScriptExecutor).ExecuteScript($@"{name} = function(mess) {{
try {{
    if(typeof(top.zuConn2) !== ""undefined"" && typeof(top.zuConn2.sendEvent) === ""function"") top.zuConn2.sendEvent(mess);
    if(typeof(top.zuMServer) !== ""undefined"" && typeof(top.zuMServer.sendEvent) === ""function"") top.zuMServer.sendEvent(mess);
}} catch (e) {{
    return e.toString();
}}
return ""ok""
}}", null, "defaultSandbox", cancellationToken); //, $@"D:\scripts\script{scriptInd++}.js");
            return res;
        }

        public async Task<object> AddSendEventFuncIfNo(string name = "top.zuSendEvent", CancellationToken cancellationToken = new CancellationToken())
        {
            if (CurrentContext == Contexts.Content)
                await SetContext(Contexts.Chrome, cancellationToken).ConfigureAwait(false);
            if (!await ObjectExists("top.zuSendEvent", cancellationToken: cancellationToken).ConfigureAwait(false))
            {
                var res = await ((FirefoxDriverJavaScriptExecutor)JavaScriptExecutor).ExecuteScript($@"{name} = function(mess) {{
try {{
    if(typeof(top.zuConn2) !== ""undefined"" && typeof(top.zuConn2.sendEvent) === ""function"") top.zuConn2.sendEvent(mess);
    if(typeof(top.zuMServer) !== ""undefined"" && typeof(top.zuMServer.sendEvent) === ""function"") top.zuMServer.sendEvent(mess);
}} catch (e) {{
    return e.toString();
}}
return ""ok""
}}
return ""ok"";", $@"D:\scripts\script{scriptInd++}.js", cancellationToken: cancellationToken); //, cancellationToken: cancellationToken); //
                return res;
            }

            return null;
        }

        public async Task<object> SendEvent(string to, string mess, string funcname = "top.zuSendEvent", CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await EvalInChrome($@"{funcname}({{ ""to"": ""{to}"", ""value"": {mess}}})", null, cancellationToken).ConfigureAwait(false);
            return res;
        }

        public async Task Disconnect(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                await Task.Run(() =>
                {
                    _connectionM?.Close();
                    _connectionM2?.Close();
                }

                , cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<object> Eval(string expression, string fileName = null, string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await ((FirefoxDriverJavaScriptExecutor)JavaScriptExecutor).ExecuteScript(expression, fileName, sandbox, cancellationToken);
            return res;
        }

        public async Task<object> EvalInContent(string expression, string fileName = null, string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken()) // null)
        {
            if (CurrentContext == Contexts.Chrome)
                await SetContext(Contexts.Content, cancellationToken).ConfigureAwait(false);
            var res = await ((FirefoxDriverJavaScriptExecutor)JavaScriptExecutor).ExecuteScript(expression, fileName, sandbox, cancellationToken);
            return res;
        }

        public async Task<object> EvalInChrome(string expression, string fileName = null, CancellationToken cancellationToken = new CancellationToken())
        {
            if (CurrentContext == Contexts.Content)
                await SetContext(Contexts.Chrome, cancellationToken).ConfigureAwait(false);
            var res = await ((FirefoxDriverJavaScriptExecutor)JavaScriptExecutor).ExecuteScript(expression, fileName, "defaultSandbox", cancellationToken);
            return res;
        }

        public async Task<string> Eval2(string expression, string fileName = null, string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await ((FirefoxDriverJavaScriptExecutor)JavaScriptExecutor).ExecuteScript(expression, fileName, sandbox, cancellationToken);
            return res?.ToString();
        }

        public async Task<object> EvalFile(string fileName, string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken())
        {
            if (!File.Exists(fileName) && !string.IsNullOrWhiteSpace(FilesBasePath))
                fileName = Path.Combine(FilesBasePath, fileName);
            if (!File.Exists(fileName))
                return null;
            var code = File.ReadAllText(fileName);
            var res = await ((FirefoxDriverJavaScriptExecutor)JavaScriptExecutor).ExecuteScript(code, Path.GetFullPath(fileName), sandbox, cancellationToken);
            return res;
        }

        public async Task<bool> ObjectExists(string path, string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await Eval($"let res = 'undefined'; try {{ res = typeof ({path}); }} catch (ex) {{}} return res", null, sandbox, cancellationToken).ConfigureAwait(false);
            var resStr = res?.ToString(); //?["value"].ToString();
            if (resStr == null || resStr == "undefined")
                return false;
            return true;
        }

        public void AddEventListener(string to, Action<JToken> action)
        {
            lock (lock2)
            {
                foreach (var item in eventAsyncActions)
                    if (item.Item1 == to && item.Item2 == action)
                        return;
                eventAsyncActions.Add(Tuple.Create(to, action));
            }
        }

        public void RemoveEventListener(Action<JToken> action)
        {
            lock (lock2)
            {
                eventAsyncActions = eventAsyncActions.Where(v => v.Item2 != action).ToList();
            }
        }

        public async Task<Contexts> GetContext(CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await GetClientContext(cancellationToken).ConfigureAwait(false);
            var s = res?["value"].ToString();
            if (s == "content")
                return Contexts.Content;
            if (s == "chrome")
                return Contexts.Chrome;
            return Contexts.None;
        }

        public Task<JToken> SetContext(Contexts context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (context == Contexts.Chrome)
            {
                CurrentContext = Contexts.Chrome;
                return SetContextChrome(cancellationToken);
            }

            if (context == Contexts.Content)
            {
                CurrentContext = Contexts.Content;
                return SetContextContent(cancellationToken);
            }

            CurrentContext = Contexts.None;
            return null;
        }

        public event EventHandler<string> EventMessage;
        public virtual async Task<string> Connect2()
        {
            await SetContextChrome().ConfigureAwait(false);
            if (await ObjectExists("top.zuMServer").ConfigureAwait(false))
            {
                var p = await GetString("top.zuMServer.port").ConfigureAwait(false);
                if (int.TryParse(p, out int p1))
                    Port2 = p1;
            }
            else
            {
                if (!await ObjectExists("MarionetteServer2").ConfigureAwait(false))
                {
                    var assem = this.GetType().Assembly;
                    var resPath = $"{assem.GetName().Name}.js.mymarionetteserver.js";
                    var code = "";
                    if (assem.GetManifestResourceNames().Contains(resPath))
                    {
                        using (Stream stream = assem.GetManifestResourceStream(resPath))
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                code = reader.ReadToEnd();
                            }
                        }

                        await ((FirefoxDriverJavaScriptExecutor)JavaScriptExecutor).ExecuteScript(code, Path.GetFullPath("mymarionetteserver.js"));
                    }
                    else
                    {
                        await EvalFile("mymarionetteserver.js").ConfigureAwait(false);
                    }
                }

                var res = await ((FirefoxDriverJavaScriptExecutor)JavaScriptExecutor).ExecuteScript($@"
            var s;
            try {{
                s = new MarionetteServer2({Port2}, true);
                s.start();
            }} catch (e) {{
                return e.toString();
            }} finally {{
            	if (s) {{
            		top.zuMServer = s;
            	}}
            }}
            return ""ok""", $@"D:\scripts\script{scriptInd++}.js");
            }

            _connectionM2 = new DebuggerConnectionMarionette(new NetworkClientFactory());
            _connectionM2.OutputMessage += _connectionM2_OutputMessage;
            _debuggerEndpointUri2 = new UriBuilder{Scheme = "tcp", Host = "127.0.0.1", Port = Port2}.Uri;
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    _connectionM2.Connect(_debuggerEndpointUri2);
                }
                catch
                {
                }
            }

            ).ConfigureAwait(false);
            clientMarionette2 = new DebuggerClientMarionette(_connectionM2);
            return "ok";
        }

        public Task<MarionetteDebuggerCommand> SendCommandString(string command)
        {
            try
            {
                var id = JArray.Parse(command)[1].Value<int>();
                return SendCommand(new MarionetteDebuggerCommandString(command, id));
            }
            catch
            {
                return null;
            }
        }

        public async Task<MarionetteDebuggerCommand> SendCommand(MarionetteDebuggerCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            if (ClientMarionette == null)
                command.Error = "error: no clientMarionette";
            else
                await ClientMarionette.SendRequestAsync(command, cancellationToken).ConfigureAwait(false);
            return command;
        }

        private void _connectionM2_OutputMessage(object sender, MessageEventArgs e)
        {
            var mess = e.Message;
            EventMessage?.Invoke(this, mess);
            var json = JToken.Parse(mess);
            var to = json?["event"]?["value"]?["to"]?["value"]?.ToString();
            if (to != null)
            {
                List<Tuple<string, Action<JToken>>> acts;
                lock (lock2)
                {
                    acts = eventAsyncActions.Where(v => v.Item1 == to).ToList();
                }

                foreach (var act in acts)
                    act?.Item2?.Invoke(json?["event"]?["value"]);
            }
        }

        public async Task<object> TestMessage(string mess = "TestMessage", CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await ((FirefoxDriverJavaScriptExecutor)JavaScriptExecutor).ExecuteScript($@"
try {{
    if(typeof(top.zuConn2) !== ""undefined"" && typeof(top.zuConn2.sendEvent) === ""function"") top.zuConn2.sendEvent({mess});
    if(typeof(top.zuMServer) !== ""undefined"" && typeof(top.zuMServer.sendEvent) === ""function"") top.zuMServer.sendEvent({mess});
}} catch (e) {{
    return e.toString();
}}
return ""ok""", $@"D:\scripts\script{scriptInd++}.js", "defaultSandbox", cancellationToken);
            return res;
        }

        string DBG_XUL = "chrome://devtools/content/framework/toolbox-process-window.xul";
        string CHROME_DEBUGGER_PROFILE_NAME = "chrome_debugger_profile";
        public virtual async Task StartDebuggerServer(int port = 9876, bool webSocket = false, CancellationToken cancellationToken = default (CancellationToken))
        {
            //http://searchfox.org/mozilla-central/source/devtools/shared/gcli/commands/listen.js
            var script = @"
(function () {
    var { require } = Cu.import('resource://devtools/shared/Loader.jsm', {});
    const { XPCOMUtils } = require('resource://gre/modules/XPCOMUtils.jsm');
    XPCOMUtils.defineLazyModuleGetter(this, 'DevToolsLoader',
    'resource://devtools/shared/Loader.jsm');

    var preferences = require('sdk/preferences/service');
    //preferences.set('browser.tabs.remote.autostart.2', false);
    preferences.set('devtools.debugger.prompt-connection', false);
    preferences.set('devtools.debugger.remote-port', " + port + @");
    preferences.set('devtools.chrome.enabled', true);
    preferences.set('devtools.debugger.remote-enabled', true);

    // Create a separate loader instance, so that we can be sure to receive
    // a separate instance of the DebuggingServer from the rest of the
    // devtools.  This allows us to safely use the tools against even the
    // actors and DebuggingServer itself, especially since we can mark
    // serverLoader as invisible to the debugger (unlike the usual loader
    // settings).
    let serverLoader = new DevToolsLoader();
    serverLoader.invisibleToDebugger = true;
    let { DebuggerServer: debuggerServer } = serverLoader.require('devtools/server/main');
    debuggerServer.init();
    debuggerServer.addBrowserActors();
    //debuggerServer.registerActors({ root: true, browser: true, tab: true });
    debuggerServer.allowChromeProcess = true; //!l10n.hiddenByChromePref();
    let listener = debuggerServer.createListener();
    let webSocket = " + webSocket.ToString().ToLower() + @";
      //if (args.protocol === 'websocket') {
      //  webSocket = true;
      //} else if (args.protocol === 'mozilla-rdp') {
      //  webSocket = false;
      //}
    listener.portOrPath = " + port + @";
    listener.webSocket = webSocket;
    listener.open();
}());
";
            await SetContextChrome(cancellationToken).ConfigureAwait(false);
            await JavaScriptExecutor.ExecuteScript(script, cancellationToken).ConfigureAwait(false);
            await SetContextContent(cancellationToken).ConfigureAwait(false);
        }

        public async Task ReloadIfMultiprocess(CancellationToken cancellationToken = default (CancellationToken))
        {
            await SetContextChrome(cancellationToken).ConfigureAwait(false);
            var path = "browser.tabs.remote.autostart.2";
            var res = await JavaScriptExecutor.ExecuteScript($@"try {{
var {{ require }} = Cu.import('resource://devtools/shared/Loader.jsm', {{}});
var preferences = require('sdk/preferences/service');
return preferences.get('{path}');
}} catch(ex) {{
return ex.toString();
}}
").ConfigureAwait(false);
            if (res.ToString().ToLower() != "false")
            {
                //res = await JavaScriptExecutor.ExecuteScript(@"try {
                //        var { require } = Cu.import('resource://devtools/shared/Loader.jsm', {});
                //        var preferences = require('sdk/preferences/service');
                //        preferences.set('browser.tabs.remote.autostart.2', false);
                //    } catch(ex) {
                //        return ex.toString();
                //    }
                //");
                if (Config.IsTempProfile)
                {
                    Config.IsTempProfile = false;
                    await Close().ConfigureAwait(false);
                    await Task.Delay(1000).ConfigureAwait(false);
                    FirefoxProfilesWorker.AddWriteUserPreference(Config.UserDir, "browser.tabs.remote.autostart.2", "false");
                    await Task.Delay(1000).ConfigureAwait(false);
                    await Connect().ConfigureAwait(false);
                    Config.IsTempProfile = true;
                }
                else
                {
                    await Close().ConfigureAwait(false);
                    await Task.Delay(1000).ConfigureAwait(false);
                    FirefoxProfilesWorker.AddWriteUserPreference(Config.UserDir, "browser.tabs.remote.autostart.2", "false");
                    await Task.Delay(1000).ConfigureAwait(false);
                    await Connect().ConfigureAwait(false);
                }
            }
        }

        static Random rnd = new Random();
        public virtual async Task<AsyncFirefoxDriver> OpenBrowserDevTools(int port = 9876, bool openInBrowserWindow = true, CancellationToken cancellationToken = default (CancellationToken))
        {
            await StartDebuggerServer(port, false, cancellationToken).ConfigureAwait(false);
            var devToolsPrefs = new Dictionary<string, string>{{"browser.tabs.remote.autostart.2", "false"}, {"devtools.debugger.prompt-connection", "false"}, {"devtools.debugger.remote-enabled", "true"}, //{ "devtools.debugger.remote-port", "9888" },
            {"devtools.debugger.chrome-debugging-port", port.ToString()}, {"devtools.chrome.enabled", "true"}};
            var profileDir = Config.UserDir;
            var devToolsProfileDir = Path.Combine(profileDir, CHROME_DEBUGGER_PROFILE_NAME);
            var devToolsProfileName = CHROME_DEBUGGER_PROFILE_NAME + rnd.Next(1000).ToString();
            if (Directory.Exists(devToolsProfileDir))
                Directory.Delete(devToolsProfileDir, true);
            await FirefoxProfilesWorker.CreateFirefoxProfile(devToolsProfileDir, devToolsProfileName).ConfigureAwait(false);
            //var prefsFile = Path.Combine(profileDir, "prefs.js");
            //if (File.Exists(prefsFile)) File.Copy(prefsFile, Path.Combine(devToolsProfileDir, "prefs.js"), true);
            //var userPrefsFile = Path.Combine(profileDir, "user.js");
            //if (File.Exists(userPrefsFile)) File.Copy(userPrefsFile, Path.Combine(devToolsProfileDir, "user.js"), true);
            FirefoxProfilesWorker.AddWriteUserPreferences(devToolsProfileDir, devToolsPrefs);
            var xulURI = DBG_XUL;
            var args = new string[]{//"-no-remote",
            "-foreground", //"-profile", this._dbgProfilePath,
            "-chrome", xulURI};
            var argsStr = string.Join(" ", args);
            var configDevTools = new FirefoxDriverConfig().SetProfileName(devToolsProfileName);
            if (!openInBrowserWindow)
                configDevTools.SetCommandLineArgumets(argsStr);
            BrowserDevTools = new AsyncFirefoxDriver(configDevTools);
            await BrowserDevTools.Connect().ConfigureAwait(false);
            if (openInBrowserWindow)
                await BrowserDevTools.Navigation.GoToUrl(DBG_XUL).ConfigureAwait(false);
            return BrowserDevTools;
        }

        public async Task<AsyncFirefoxDriver> OpenBrowserDevTools2(int port = 9876, bool openInBrowserWindow = true, CancellationToken cancellationToken = default (CancellationToken))
        {
            //await ReloadIfMultiprocess(cancellationToken);
            //http://searchfox.org/mozilla-central/source/devtools/shared/gcli/commands/listen.js
            var script = @"
(function () {
    var { require } = Cu.import('resource://devtools/shared/Loader.jsm', {});
    var { BrowserToolboxProcess } = require('resource://devtools/client/framework/ToolboxProcess.jsm');
 
    var preferences = require('sdk/preferences/service');
    preferences.set('devtools.debugger.prompt-connection', false);
    preferences.set('devtools.debugger.remote-port', " + port + @");
    preferences.set('devtools.chrome.enabled', true);
    preferences.set('devtools.debugger.remote-enabled', true);

    BrowserToolboxProcess.init();
}());
";
            await SetContextChrome().ConfigureAwait(false);
            await JavaScriptExecutor.ExecuteScript(script).ConfigureAwait(false); //evalInTop);//
            await SetContextContent().ConfigureAwait(false);
            return null;
        }
    }
}