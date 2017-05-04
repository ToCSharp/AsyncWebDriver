using MyCommunicationLib.Communication.MarionetteComands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser;
using Zu.WebBrowser.Communication;

namespace Zu.Firefox
{
    public class AsyncFirefoxDriver : IAsyncWebBrowserClient
    {
        public int port;
        public int port2;
        public IDebuggerClient clientMarionette;
        private DebuggerConnectionMarionette _connectionM;
        private Uri _debuggerEndpointUri;
        private DebuggerConnectionMarionette _connectionM2;
        private Uri _debuggerEndpointUri2;
        public DebuggerClientMarionette clientMarionette2;

        public event EventHandler<string> EventMessage;

        public AsyncFirefoxDriver(string profileName)
            : this(FirefoxProfilesWorker.GetMarionettePort(profileName))
        {
        }

        public AsyncFirefoxDriver(int port, int port2 = 0)
        {
            CurrentContext = Contexts.Chrome;
            this.port = port;
            this.port2 = port2;
            if (this.port2 == 0) this.port2 = this.port + 10000;
        }

        public int ConnectTimeoutMs { get; set; } = 10000;
        public async Task<string> Connect(CancellationToken cancellationToken = new CancellationToken())
        {
            if (port == 0) return "Error: MarionettePort not set";
            _connectionM = new DebuggerConnectionMarionette(new NetworkClientFactory());
            _debuggerEndpointUri = new UriBuilder { Scheme = "tcp", Host = "127.0.0.1", Port = port }.Uri;
            //_connectionM.OutputMessage += _connection_OutputMessage;
            var r = "connected";
            await Task.Factory.StartNew(async () =>
            {
                var timeoutAt = DateTime.Now.AddMilliseconds(ConnectTimeoutMs);
                while (DateTime.Now < timeoutAt)
                {

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

            }, cancellationToken);
            if (r != "connected") return r;

            clientMarionette = new DebuggerClientMarionette(_connectionM);
            var comm1 = new NewSessionCommand();
            try
            {
                await clientMarionette.SendRequestAsync(comm1, cancellationToken);
            }
            catch
            {
                await Task.Delay(200, cancellationToken);
                try
                {
                    await clientMarionette.SendRequestAsync(comm1, cancellationToken);
                }
                catch (Exception e)
                {
                    return e.ToString();
                }
            }
            var res = comm1.Result;
            if (DoConnect2)
            {
                var res2 = await Connect2();
            }
            await SetContextChrome(cancellationToken);
            return "Marionette " + port +  (_connectionM2?.Connected == true ? (" Marionette2 " + port2) : "");
        }



        public async Task<string> Connect2()
        {
            await SetContextChrome();
            if (await ObjectExists("top.zuMServer"))
            {
                var p = await GetString("top.zuMServer.port");
                if (int.TryParse(p, out int p1))
                {
                    port2 = p1;
                }
            }
            else
            {
                if (!await ObjectExists("MarionetteServer2"))
                {
                    await EvalFile("mymarionetteserver.js");
                }
                var res = await ExecuteScript($@"
            var s;
            try {{
                s = new MarionetteServer2({port2}, true);
                s.start();
            }} catch (e) {{
                return e.toString();
            }} finally {{
            	if (s) {{
            		top.zuMServer = s;
            	}}
            }}
            return ""ok""",
              $@"D:\scripts\script{scriptInd++}.js");
            }
            _connectionM2 = new DebuggerConnectionMarionette(new NetworkClientFactory());
            _connectionM2.OutputMessage += _connectionM2_OutputMessage;
            _debuggerEndpointUri2 = new UriBuilder { Scheme = "tcp", Host = "127.0.0.1", Port = port2 }.Uri;
            //_connectionM.OutputMessage += _connection_OutputMessage;
            await Task.Factory.StartNew(() => { try { _connectionM2.Connect(_debuggerEndpointUri2); } catch { } });
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
            if (clientMarionette == null)
            {
                command.Error = "error: no clientMarionette";
            }
            else
            {
                await clientMarionette.SendRequestAsync(command, cancellationToken);
            }
            return command;
        }
        public async Task<string> GetUrl(string url, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetCommand(url);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> AcceptDialog(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new AcceptDialogCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> ClearElement(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new ClearElementCommand(elementId);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> ClearImportedScripts(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new ClearImportedScriptsCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> ClickElement(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new ClickElementCommand(elementId);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> Close(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new CloseCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> DismissDialog(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new DismissDialogCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> CloseChromeWindow(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new CloseChromeWindowCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }

        public async Task<JToken> FindElement(string strategy, string expr, string startNode = null, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new FindElementCommand(strategy, expr, startNode);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error;
            return comm1.Result;
        }

        public async Task<JToken> FindElements(string strategy, string expr, string startNode = null, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new FindElementsCommand(strategy, expr, startNode);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error;
            return comm1.Result;
        }

        public async Task<string> GetActiveElement(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetActiveElementCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }
        public async Task<string> GetActiveFrame(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetActiveFrameCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<string> GetChromeWindowHandle(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetChromeWindowHandleCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }
        public async Task<JToken> GetChromeWindowHandles(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetChromeWindowHandlesCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result;
        }

        public async Task<string> GetCurrentUrl(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetCurrentUrlCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<string> GetElementAttribute(string elementId, string attrName, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetElementAttributeCommand(elementId, attrName);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<string> GetElementProperty(string elementId, string propName, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetElementPropertyCommand(elementId, propName);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<JToken> GetElementRect(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetElementRectCommand(elementId);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result;
        }

        public async Task<string> GetElementTagName(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetElementTagNameCommand(elementId);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<string> GetElementText(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetElementTextCommand(elementId);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<string> GetElementValueOfCssProperty(string elementId, string propName, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetElementValueOfCssPropertyCommand(elementId, propName);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }
        public async Task<string> GetPageSource(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetPageSourceCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<string> GetTextFromDialog(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetTextFromDialogCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<string> GetTitle(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetTitleCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }
        public async Task<string> GetWindowHandle(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetWindowHandleCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }
        public async Task<JToken> GetWindowHandles(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetWindowHandlesCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error;
            return comm1.Result;
        }
        public async Task<JToken> GetWindowPosition(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetWindowPositionCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error;
            return comm1.Result;
        }
        public async Task<JToken> GetWindowSize(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetWindowSizeCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error;
            return comm1.Result;
        }
        public async Task<string> GetWindowType(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetWindowTypeCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }
        public async Task<string> GoBack(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GoBackCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> GoForward(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GoForwardCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> ImportScript(string script, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new ImportScriptCommand(script);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> IsElementDisplayed(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new IsElementDisplayedCommand(elementId);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }
        public async Task<string> IsElementEnabled(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new IsElementEnabledCommand(elementId);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }
        public async Task<string> IsElementSelected(string elementId, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new IsElementSelectedCommand(elementId);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }
        public async Task<string> MaximizeWindow(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new MaximizeWindowCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> Refresh(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new RefreshCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> SendKeysToDialog(string value, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new SendKeysToDialogCommand(value);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> SendKeysToElement(string elementId, string value, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new SendKeysToElementCommand(elementId, value);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> SessionTearDown(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new SessionTearDownCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> SetTimeouts(TimeoutType elementId, int ms, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new SetTimeoutsCommand(elementId, ms);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> SetWindowPosition(int x, int y, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new SetWindowPositionCommand(x, y);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> SetWindowSize(int width, int height, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new SetWindowSizeCommand(width, height);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> SwitchToFrame(string frameId, string element = null, bool doFocus = true, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new SwitchToFrameCommand(frameId, element, doFocus);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> SwitchToParentFrame(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new SwitchToParentFrameCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> SwitchToWindow(string name, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new SwitchToWindowCommand(name);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }
        public async Task<string> TakeScreenshot(string elementId, string highlights, string full, string hash, CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new TakeScreenshotCommand(elementId, highlights, full, hash);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error.ToString();
            return "ok";
        }


        public async Task<JToken> ExecuteScript(string script, string filename = null, string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new ExecuteScriptCommand(script) { filename = filename, sandbox = sandbox };
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error;
            return comm1.Result;
        }
        public async Task<JToken> GetClientContext(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new GetContextCommand();
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error;
            return comm1.Result;
        }

        public async Task<string> GetString(string path, CancellationToken cancellationToken = new CancellationToken())
        {
            //string s = (await Eval(path))?["value"]?.ToString();
            //return s;
            var res = await ExecuteScript($"return {path}.toString()", null, "defaultSandbox", cancellationToken);
            return res?["value"].ToString();
        }

        public async Task<JToken> SetContextChrome(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new SetContextCommand(SetContextCommand.Contexts.chrome);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error;
            CurrentContext = Contexts.Chrome;
            return comm1.Result;
        }
        public async Task<JToken> SetContextContent(CancellationToken cancellationToken = new CancellationToken())
        {
            if (clientMarionette == null) return "error: no clientMarionette";
            var comm1 = new SetContextCommand(SetContextCommand.Contexts.content);
            await clientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) return comm1.Error;
            CurrentContext = Contexts.Content;
            return comm1.Result;
        }
        int scriptInd = 0;

        public string FilesBasePath { get; set; } = "js\\";

        //        public async Task<string> CreateConnection2(int port)
        //        {
        //            await SetContextChrome();

        //            var scriptFile = @"D:\jsfiles\mymarionetteserver.js";
        //            var script = File.ReadAllText(scriptFile);
        //            var res1 = await ExecuteScript(script, scriptFile);
        //            var res = await ExecuteScript($@"
        //try {{
        //    //Cu.import(""chrome://marionette/content/server.js"");
        //    //MarionetteServer.prototype.sendEvent = function(packet) {{
        //    //    for (let c in this.conns) {{
        //    //        this.conns[c].sendRaw(packet);
        //    //    }}
        //    //}};

        //                s = new MarionetteServer2({port}, true);
        //                s.start();
        //            }} catch (e) {{
        //                return e.toString();
        //            }} finally {{
        //            	if (s) {{
        //            		//this.server = s;
        //                    zuConn2 = s;
        //            	}}
        //            }}
        //            return ""ok""", $@"D:\scripts\script{scriptInd++}.js");
        //            //if (res?["value"].ToString() != "ok")
        //            //{
        //            //    return res?.ToString();
        //            //}
        //            _connectionM2 = new DebuggerConnectionMarionette(new NetworkClientFactory());
        //            _connectionM2.OutputMessage += _connectionM2_OutputMessage;
        //            _debuggerEndpointUri2 = new UriBuilder { Scheme = "tcp", Host = "127.0.0.1", Port = port }.Uri;
        //            //_connectionM.OutputMessage += _connection_OutputMessage;
        //            await Task.Factory.StartNew(() => _connectionM2.Connect(_debuggerEndpointUri2));
        //            clientMarionette2 = new DebuggerClientMarionette(_connectionM2);
        //            //var comm1 = new newSessionCommand(1);
        //            //await clientMarionette2.SendRequestAsync(comm1, cancellationToken);
        //            return "ok"; // comm1.Result.ToString();
        //        }
        //public async Task<string> CreateConnection2b(int port)
        //{
        //    //await SetContextChrome();

        //    //var scriptFile = @"D:\jsfiles\mymarionetteserver.js";
        //    //var script = File.ReadAllText(scriptFile);
        //    //var res1 = await ExecuteScript(script, scriptFile);
        //    //var res = await ExecuteScript($@"
        //    //try {{
        //    //    s = new MarionetteServer2({port}, true);
        //    //    s.start();
        //    //}} catch (e) {{
        //    //    return e.toString();
        //    //}} finally {{
        //    //	if (s) {{
        //    //		this.server = s;
        //    //	}}
        //    //}}
        //    //return ""ok""", $@"D:\scripts\script{scriptInd++}.js");
        //    ////if (res?["value"].ToString() != "ok")
        //    ////{
        //    ////    return res?.ToString();
        //    //}
        //    _connectionM2 = new DebuggerConnectionMarionette(new NetworkClientFactory());
        //    _connectionM2.OutputMessage += _connectionM2_OutputMessage;
        //    _debuggerEndpointUri2 = new UriBuilder { Scheme = "tcp", Host = "127.0.0.1", Port = port }.Uri;
        //    //_connectionM.OutputMessage += _connection_OutputMessage;
        //    await Task.Factory.StartNew(() => _connectionM2.Connect(_debuggerEndpointUri2));
        //    clientMarionette2 = new DebuggerClientMarionette(_connectionM2);
        //    var comm1 = new newSessionCommand(1);
        //    await clientMarionette2.SendRequestAsync(comm1, cancellationToken);
        //    return comm1.Result.ToString();
        //}
        object lock1 = new object();
        private void _connectionM2_OutputMessage(object sender, MessageEventArgs e)
        {
            var mess = e.Message;
            EventMessage?.Invoke(this, mess);
            var json = JToken.Parse(mess);
            var to = json?["event"]?["value"]?["to"]?["value"]?.ToString();
            if (to != null)
            {
                //List<Tuple<string, TaskCompletionSource<JToken>>> evs;
                //lock (lock1)
                //{
                //    evs = whenEventAsyncTasks.Where(v => v.Item1 == to).ToList();
                //    whenEventAsyncTasks = whenEventAsyncTasks.Except(evs).ToList();
                //}
                //foreach (var ev in evs)
                //{
                //    ev.Item2.TrySetResult(e);
                //}
                List<Tuple<string, Action<JToken>>> acts;
                lock (lock2)
                {
                    acts = eventAsyncActions.Where(v => v.Item1 == to).ToList();
                }
                foreach (var act in acts)
                {
                    act?.Item2?.Invoke(json?["event"]?["value"]);
                }
            }

        }
        public async Task<JToken> AddSendEventFunc(string name = "top.zuSendEvent", CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await ExecuteScript($@"{name} = function(mess) {{
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
        public async Task<JToken> AddSendEventFuncIfNo(string name = "top.zuSendEvent", CancellationToken cancellationToken = new CancellationToken())
        {
            if (CurrentContext == Contexts.Content)
            {
                await SetContext(Contexts.Chrome, cancellationToken);
            }
            if (!await ObjectExists("top.zuSendEvent", cancellationToken: cancellationToken))
            {
                var res = await ExecuteScript($@"{name} = function(mess) {{
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

        public async Task<JToken> SendEvent(string to, string mess, string funcname = "top.zuSendEvent", CancellationToken cancellationToken = new CancellationToken())
        {

            var res = await EvalInChrome($@"{funcname}({{ ""to"": ""{to}"", ""value"": {mess}}})", null, cancellationToken);
            return res;
        }

        public async Task<JToken> TestMessage(string mess = "TestMessage", CancellationToken cancellationToken = new CancellationToken())
        {
            //mess = mess.Replace("\"", "\"\"");
            var res = await ExecuteScript($@"
try {{
    if(typeof(top.zuConn2) !== ""undefined"" && typeof(top.zuConn2.sendEvent) === ""function"") top.zuConn2.sendEvent({mess});
    if(typeof(top.zuMServer) !== ""undefined"" && typeof(top.zuMServer.sendEvent) === ""function"") top.zuMServer.sendEvent({mess});
}} catch (e) {{
    return e.toString();
}}
return ""ok""", $@"D:\scripts\script{scriptInd++}.js", "defaultSandbox", cancellationToken);
            return res;

        }

        public async Task<string> Disconnect(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                await Task.Run(() =>
                {
                    _connectionM?.Close();
                    _connectionM2?.Close();
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "ok"; 
        }

        public async Task<JToken> Eval(string expression, string fileName = null, string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await ExecuteScript(expression, fileName, sandbox, cancellationToken);
            return res;
        }
        public async Task<JToken> EvalInContent(string expression, string fileName = null, string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken()) // null)
        {
            if (CurrentContext == Contexts.Chrome)
            {
                await SetContext(Contexts.Content, cancellationToken);
            }
            var res = await ExecuteScript(expression, fileName, sandbox, cancellationToken);
            return res;
        }
        public async Task<JToken> EvalInChrome(string expression, string fileName = null, CancellationToken cancellationToken = new CancellationToken())
        {
            if (CurrentContext == Contexts.Content)
            {
                await SetContext(Contexts.Chrome, cancellationToken);
            }
            var res = await ExecuteScript(expression, fileName, "defaultSandbox", cancellationToken);
            return res;
        }
        public async Task<string> Eval2(string expression, string fileName = null, string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await ExecuteScript(expression, fileName, sandbox, cancellationToken);
            return res?.ToString();
        }

        public async Task<JToken> EvalFile(string fileName, string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken())
        {
            if (!File.Exists(fileName) && !string.IsNullOrWhiteSpace(FilesBasePath)) fileName = Path.Combine(FilesBasePath, fileName);
            if (!File.Exists(fileName)) return null;
            var code = File.ReadAllText(fileName);
            var res = await ExecuteScript(code, Path.GetFullPath(fileName), sandbox, cancellationToken);
            return res;
        }

        public async Task<bool> ObjectExists(string path, string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await Eval($"let res = 'undefined'; try {{ res = typeof ({path}); }} catch (ex) {{}} return res", null, sandbox, cancellationToken);
            var resStr = res?["value"].ToString();
            if (resStr == null || resStr == "undefined") return false;
            return true;
        }



        List<Tuple<string, Action<JToken>>> eventAsyncActions = new List<Tuple<string, Action<JToken>>>();
        object lock2 = new object();


        public void AddEventListener(string to, Action<JToken> action)
        {
            lock (lock2)
            {
                foreach (var item in eventAsyncActions)
                {
                    if (item.Item1 == to && item.Item2 == action) return;
                }
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

        public Contexts CurrentContext { get; set; }
        public bool DoConnect2 { get; set; } = true;

        public async Task<Contexts> GetContext(CancellationToken cancellationToken = new CancellationToken())
        {
            var res = await GetClientContext(cancellationToken);
            var s = res?["value"].ToString();
            if (s == "content") return Contexts.Content;
            else if (s == "chrome") return Contexts.Chrome;
            return Contexts.None;
        }

        public Task<JToken> SetContext(Contexts context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (context == Contexts.Chrome)
            {
                CurrentContext = Contexts.Chrome;
                return SetContextChrome(cancellationToken);
            }
            else if (context == Contexts.Content)
            {
                CurrentContext = Contexts.Content;
                return SetContextContent(cancellationToken);
            }
            else
            {
                CurrentContext = Contexts.None;
                return null;
            }
        }
    }
}
