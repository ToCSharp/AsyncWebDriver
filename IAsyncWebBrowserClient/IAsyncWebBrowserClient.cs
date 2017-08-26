// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Zu.WebBrowser.BasicTypes;
using Zu.WebBrowser.AsyncInteractions;

namespace Zu.WebBrowser
{
    public enum TimeoutType
    {
        @implicit = 0,
        script = 1,
        page_load = 2
    }

    public interface IAsyncWebBrowserClient : IAsyncWebBrowserWorker
    {
        IMouse Mouse { get; }
        IKeyboard Keyboard { get; }

        Task<string> AcceptDialog(CancellationToken cancellationToken = new CancellationToken());
        Task<string> ClearElement(string elementId, CancellationToken cancellationToken = new CancellationToken());
        Task<string> ClearImportedScripts(CancellationToken cancellationToken = new CancellationToken());
        Task<string> ClickElement(string elementId, CancellationToken cancellationToken = new CancellationToken());
        Task<string> Close(CancellationToken cancellationToken = new CancellationToken());
        void CloseSync();
        Task<string> DismissDialog(CancellationToken cancellationToken = new CancellationToken());
        Task<string> CloseChromeWindow(CancellationToken cancellationToken = new CancellationToken());

        Task<JToken> FindElement(string strategy, string expr, string startNode = null,
            CancellationToken cancellationToken = new CancellationToken());

        Task<JToken> FindElements(string strategy, string expr, string startNode = null,
            CancellationToken cancellationToken = new CancellationToken());

        Task<string> GetActiveElement(CancellationToken cancellationToken = new CancellationToken());
        Task<string> GetActiveFrame(CancellationToken cancellationToken = new CancellationToken());
        Task<string> GetChromeWindowHandle(CancellationToken cancellationToken = new CancellationToken());
        Task<JToken> GetChromeWindowHandles(CancellationToken cancellationToken = new CancellationToken());
        Task<JToken> GetClientContext(CancellationToken cancellationToken = new CancellationToken());
        Task<string> GetUrl(CancellationToken cancellationToken = new CancellationToken());

        Task<string> GetElementAttribute(string elementId, string attrName,
            CancellationToken cancellationToken = new CancellationToken());

        Task<string> GetElementProperty(string elementId, string propName,
            CancellationToken cancellationToken = new CancellationToken());

        Task<WebRect> GetElementRect(string elementId, CancellationToken cancellationToken = new CancellationToken());

        Task<string> GetElementTagName(string elementId, CancellationToken cancellationToken = new CancellationToken());

        Task<string> GetElementText(string elementId, CancellationToken cancellationToken = new CancellationToken());

        Task<string> GetElementValueOfCssProperty(string elementId, string propName,
            CancellationToken cancellationToken = new CancellationToken());

        Task<string> GetPageSource(CancellationToken cancellationToken = new CancellationToken());

        Task<string> GetString(string path, CancellationToken cancellationToken = new CancellationToken());

        Task<string> GetTextFromDialog(CancellationToken cancellationToken = new CancellationToken());

        Task<string> GetTitle(CancellationToken cancellationToken = new CancellationToken());

        Task<string> GoToUrl(string url, CancellationToken cancellationToken = new CancellationToken());

        Task<string> GetWindowHandle(CancellationToken cancellationToken = new CancellationToken());

        Task<JToken> GetWindowHandles(CancellationToken cancellationToken = new CancellationToken());

        Task<JToken> GetWindowPosition(CancellationToken cancellationToken = new CancellationToken());

        Task<JToken> GetWindowSize(CancellationToken cancellationToken = new CancellationToken());

        Task<string> GetWindowType(CancellationToken cancellationToken = new CancellationToken());

        Task<string> GoBack(CancellationToken cancellationToken = new CancellationToken());

        Task<string> GoForward(CancellationToken cancellationToken = new CancellationToken());

        Task<string> ImportScript(string script, CancellationToken cancellationToken = new CancellationToken());

        Task<bool> IsElementDisplayed(string elementId, CancellationToken cancellationToken = new CancellationToken());

        Task<bool> IsElementEnabled(string elementId, CancellationToken cancellationToken = new CancellationToken());

        Task<bool> IsElementSelected(string elementId, CancellationToken cancellationToken = new CancellationToken());

        Task<string> MaximizeWindow(CancellationToken cancellationToken = new CancellationToken());

        Task<string> Refresh(CancellationToken cancellationToken = new CancellationToken());

        Task<string> SendKeysToDialog(string value, CancellationToken cancellationToken = new CancellationToken());

        Task<string> SendKeysToElement(string elementId, string value,
            CancellationToken cancellationToken = new CancellationToken());

        Task<string> SessionTearDown(CancellationToken cancellationToken = new CancellationToken());

        Task<JToken> SetContextChrome(CancellationToken cancellationToken = new CancellationToken());

        Task<JToken> SetContextContent(CancellationToken cancellationToken = new CancellationToken());

        Task<string> SetTimeouts(TimeoutType elementId, int ms,
            CancellationToken cancellationToken = new CancellationToken());

        Task<string> SetWindowPosition(int x, int y, CancellationToken cancellationToken = new CancellationToken());

        Task<string> SetWindowSize(int width, int height,
            CancellationToken cancellationToken = new CancellationToken());

        Task<string> SwitchToFrame(string frameId, string element = null, bool doFocus = true,
            CancellationToken cancellationToken = new CancellationToken());

        Task<string> SwitchToParentFrame(CancellationToken cancellationToken = new CancellationToken());

        Task<string> SwitchToWindow(string name, CancellationToken cancellationToken = new CancellationToken());

        Task<Screenshot> TakeScreenshot(string elementId, string highlights, string full, string hash,
            CancellationToken cancellationToken = new CancellationToken());

        Task<JToken> ExecuteScript(string script, string filename = null,
            string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken(), params object[] args);

        Task<JToken> ExecuteAsyncScript(string script, string filename = null,
            string sandbox = "defaultSandbox", CancellationToken cancellationToken = new CancellationToken(), params object[] args);

        Task<WebPoint> GetElementLocation(string elementId, CancellationToken cancellationToken = new CancellationToken());

        Task<WebSize> GetElementSize(string elementId, CancellationToken cancellationToken = new CancellationToken());

        Task<string> SubmitElement(string elementId, CancellationToken cancellationToken = new CancellationToken());
    }
}