// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections.ObjectModel;
using System.Threading;
using Zu.AsyncWebDriver;
using Zu.AsyncWebDriver.Remote;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser;
using Zu.WebBrowser.BrowserOptions;
using Zu.WebBrowser.BasicTypes;
using System.Collections;

namespace Zu.AsyncWebDriver.Remote
{
    public class SyncWebDriver
    {
        public SyncWebDriver(WebDriver asyncDriver)
        {
            AsyncDriver = asyncDriver;
        }

        public WebDriver AsyncDriver { get; private set; }

        public void Close()
        {
            AsyncDriver.CloseSync();
        }
        public void Open()
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await AsyncDriver.Open();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
        public string Context { get => GetContext(); }
        public string GetContext()
        {
            string res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncDriver.GetContext();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }
        public void SetContextChrome()
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await AsyncDriver.SetContextChrome();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
        public void SetContextContent()
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await AsyncDriver.SetContextContent();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }

        public string CurrentWindowHandle { get => GetCurrentWindowHandle(); }
        public string GetCurrentWindowHandle()
        {
            string res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncDriver.CurrentWindowHandle();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public void Dispose() => AsyncDriver.Dispose();

        public object ExecuteAsyncScript(string script, params object[] args)
        {
            object res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncDriver.ExecuteAsyncScript(script, default(CancellationToken), ReplaceWebElementsInArgs(args));
                    res = ReplaceWebElements(res);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public object ExecuteScript(string script, params object[] args)
        {
            object res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncDriver.ExecuteScript(script, default(CancellationToken), ReplaceWebElementsInArgs(args));
                    res = ReplaceWebElements(res);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        private object[] ReplaceWebElementsInArgs(object[] args)
        {
            if (args == null) return new object[] { null };
            var list = new List<object>();
            foreach (var item in args)
            {
                list.Add(ReplaceWebElementsInArg(item));
            }
            return list.ToArray();
        }

        private object ReplaceWebElementsInArg(object obj)
        {
            if (obj is string || obj is float || obj is double || obj is int || obj is long || obj is bool || obj == null) return obj;
            if (obj is SyncWebElement) return (obj as SyncWebElement).AsyncElement;
            var dic = obj as IDictionary;
            var col = obj as IEnumerable;
            if (dic != null)
            {
                var resDic = new Dictionary<string, object>();
                foreach (DictionaryEntry item in dic)
                {
                    resDic.Add(item.Key.ToString(), ReplaceWebElementsInArg(item.Value));
                }
                return resDic;
            }
            else if (col != null)
            {
                var list = new List<object>();
                foreach (var item in col)
                {
                    list.Add(ReplaceWebElementsInArg(item));
                }
                return list;
            }

            else return obj;

        }


        private object ReplaceWebElements(object obj)
        {
            if (obj is string || obj is float || obj is double || obj is int || obj is long || obj is bool || obj == null) return obj;
            if (obj is AsyncWebElement) return new SyncWebElement(obj as AsyncWebElement);

            var dic = obj as IDictionary;
            var col = obj as IEnumerable;
            if (dic != null)
            {
                var resDic = new Dictionary<string, object>();
                foreach (DictionaryEntry item in dic)
                {
                    resDic.Add(item.Key.ToString(), ReplaceWebElements(item.Value));
                }
                return resDic;
            }
            else if (col != null)
            {
                var list = new List<object>();
                foreach (var item in col)
                {
                    list.Add(ReplaceWebElements(item));
                }
                return new ReadOnlyCollection<object>(list);
            }
            else return obj;
        }

        public void ClickElement(string elementId)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await AsyncDriver.ClickElement(elementId);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
        public void ClearElement(string elementId)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await AsyncDriver.ClearElement(elementId);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }

        public SyncWebElement FindElement(By by)
        {
            SyncWebElement res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElement(by);
                    if ((r as AsyncWebElement) != null) res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public SyncWebElement FindElementByClassName(string className)
        {
            SyncWebElement res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementByClassName(className);
                    if ((r as AsyncWebElement) != null) res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public SyncWebElement FindElementByCssSelector(string cssSelector)
        {
            SyncWebElement res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementByCssSelector(cssSelector);
                    if ((r as AsyncWebElement) != null) res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public SyncWebElement FindElementById(string id)
        {
            SyncWebElement res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementById(id);
                    if ((r as AsyncWebElement) != null) res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }
        public SyncWebElement FindElementByIdStartsWith(string id)
        {
            SyncWebElement res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementByIdStartsWith(id);
                    if ((r as AsyncWebElement) != null) res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }
        public SyncWebElement FindElementByIdEndsWith(string id)
        {
            SyncWebElement res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementByIdEndsWith(id);
                    if ((r as AsyncWebElement) != null) res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public SyncWebElement FindElementByLinkText(string linkText)
        {
            SyncWebElement res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementByLinkText(linkText);
                    if ((r as AsyncWebElement) != null) res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public SyncWebElement FindElementByName(string name)
        {
            SyncWebElement res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementByName(name);
                    if ((r as AsyncWebElement) != null) res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public SyncWebElement FindElementByPartialLinkText(string partialLinkText)
        {
            SyncWebElement res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementByPartialLinkText(partialLinkText);
                    if ((r as AsyncWebElement) != null) res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public SyncWebElement FindElementByTagName(string tagName)
        {
            SyncWebElement res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementByTagName(tagName);
                    if ((r as AsyncWebElement) != null) res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public SyncWebElement FindElementByXPath(string xpath)
        {
            SyncWebElement res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementByXPath(xpath);
                    if ((r as AsyncWebElement) != null) res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }
        public List<SyncWebElement> FindElements(By by)
        {
            List<SyncWebElement> res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElements(by);
                    res = r?.Where(a => (a as AsyncWebElement) != null).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public List<SyncWebElement> Children()
        {
            List<SyncWebElement> res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.Children();
                    res = r?.Where(a => (a as AsyncWebElement) != null).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByClassName(string className)
        {
            List<SyncWebElement> res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementsByClassName(className);
                    res = r?.Where(a => (a as AsyncWebElement) != null).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByCssSelector(string cssSelector)
        {
            List<SyncWebElement> res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementsByCssSelector(cssSelector);
                    res = r?.Where(a => (a as AsyncWebElement) != null).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsById(string id)
        {
            List<SyncWebElement> res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementsById(id);
                    res = r?.Where(a => (a as AsyncWebElement) != null).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }
        public List<SyncWebElement> FindElementsByIdStartsWith(string id)
        {
            List<SyncWebElement> res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementsByIdStartsWith(id);
                    res = r?.Where(a => (a as AsyncWebElement) != null).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }
        public List<SyncWebElement> FindElementsByIdEndsWith(string id)
        {
            List<SyncWebElement> res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementsByIdEndsWith(id);
                    res = r?.Where(a => (a as AsyncWebElement) != null).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByLinkText(string linkText)
        {
            List<SyncWebElement> res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementsByLinkText(linkText);
                    res = r?.Where(a => (a as AsyncWebElement) != null).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByName(string name)
        {
            List<SyncWebElement> res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementsByName(name);
                    res = r?.Where(a => (a as AsyncWebElement) != null).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByPartialLinkText(string partialLinkText)
        {
            List<SyncWebElement> res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementsByPartialLinkText(partialLinkText);
                    res = r?.Where(a => (a as AsyncWebElement) != null).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByTagName(string tagName)
        {
            List<SyncWebElement> res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementsByTagName(tagName);
                    res = r?.Where(a => (a as AsyncWebElement) != null).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByXPath(string xpath)
        {
            List<SyncWebElement> res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncDriver.FindElementsByXPath(xpath);
                    res = r?.Where(a => (a as AsyncWebElement) != null).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public string Url { get => GetUrl(); }
        public string GetUrl()
        {
            string res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncDriver.GetUrl();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public Screenshot GetScreenshot()
        {
            Screenshot res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncDriver.GetScreenshot();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public string GoToUrl(string url)
        {
            string res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncDriver.GoToUrl(url);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public SyncOptions Options() => new SyncOptions(AsyncDriver.Options());
        public SyncNavigation Navigate() => new SyncNavigation(AsyncDriver.Navigate());
        public SyncKeyboard Keyboard => new SyncKeyboard(AsyncDriver.Keyboard);
        public SyncMouse Mouse => new SyncMouse(AsyncDriver.Mouse);

        public string PageSource()
        {
            string res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncDriver.PageSource();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public void Quit()
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await AsyncDriver.Quit();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }

        public SyncRemoteTargetLocator SwitchTo() => new SyncRemoteTargetLocator(AsyncDriver.SwitchTo());

        public string Title()
        {
            string res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncDriver.Title();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public List<string> WindowHandles()
        {
            List<string> res = null;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = (await AsyncDriver.WindowHandles()).ToList();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public bool WaitForElementWithId(string id, string notWebElementId = null, int attemptsCount = 20, int delayMs = 500)
        {
            if (notWebElementId != null) return WaitForWebElement(() => FindElementById(id), attemptsCount, delayMs);
            else return WaitForWebElement(() => FindElementById(id), notWebElementId, attemptsCount, delayMs);
        }

        private bool WaitForWebElement(Func<SyncWebElement> func, int attemptsCount = 20, int delayMs = 500)
        {
            for (int i = 0; i < attemptsCount; i++)
            {
                Thread.Sleep(delayMs);
                var el = func.Invoke();
                if (!string.IsNullOrWhiteSpace(el?.AsyncElement?.Id)) return true;
            }
            return false;
        }
        private bool WaitForWebElement(Func<SyncWebElement> func, string notWebElementId, int attemptsCount = 20, int delayMs = 500)
        {
            for (int i = 0; i < attemptsCount; i++)
            {
                Thread.Sleep(delayMs);
                var el = func.Invoke();
                if (notWebElementId != null && el?.Id == notWebElementId) continue;
                if (!string.IsNullOrWhiteSpace(el?.AsyncElement?.Id)) return true;
            }
            return false;
        }

        public bool IsActionExecutor()
        {
            bool res = false;
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncDriver.IsActionExecutor();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
            return res;
        }

        public void PerformActions(IList<ActionSequence> actionSequenceList)
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await AsyncDriver.PerformActions(actionSequenceList);
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }

        public void ResetInputState()
        {
            var MRes = new ManualResetEventSlim(true);
            MRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await AsyncDriver.ResetInputState();
                }
                catch (Exception ex) { exception = ex; }
                MRes.Set();
            });
            MRes.Wait();
            if (exception != null) throw exception;
        }
    }
}
