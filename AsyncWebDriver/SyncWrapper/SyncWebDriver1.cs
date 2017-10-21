// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.AsyncWebDriver.Remote
{
    public class SyncWebDriver
    {
        public SyncWebDriver(WebDriver driver)
        {
            Driver = driver;
        }

        public WebDriver Driver
        {
            get;
        }

        public string Context => GetContext();
        public string CurrentWindowHandle => GetCurrentWindowHandle();
        public string Url => GetUrl();
        public void Close()
        {
            Driver.Close().DoSync();
        }

        public string GetContext()
        {
            string res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Task.Run(async () =>
            {
                res = await Driver.GetContext().ConfigureAwait(false);
                mRes.Set();
            }

            );
            mRes.Wait();
            return res;
        }

        public void SetContextChrome()
        {
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Task.Run(async () =>
            {
                await Driver.SetContextChrome().ConfigureAwait(false);
                mRes.Set();
            }

            );
            mRes.Wait();
        }

        public void SetContextContent()
        {
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Task.Run(async () =>
            {
                await Driver.SetContextContent().ConfigureAwait(false);
                mRes.Set();
            }

            );
            mRes.Wait();
        }

        public string GetCurrentWindowHandle()
        {
            return Driver.CurrentWindowHandle().DoSync();
        }

        public void Dispose()
        {
            Driver.Dispose();
        }

        public object ExecuteAsyncScript(string script, params object[] args)
        {
            return Driver.ExecuteAsyncScript(script, default (CancellationToken), args).DoSync();
        }

        public object ExecuteScript(string script, params object[] args)
        {
            return Driver.ExecuteScript(script, default (CancellationToken), args).DoSync();
        }

        public void ClickElement(string elementId)
        {
            Driver.ClickElement(elementId).DoSync();
        }

        public void ClearElement(string elementId)
        {
            Driver.ClearElement(elementId).DoSync();
        }

        public SyncWebElement FindElement(By by)
        {
            var r = Driver.FindElement(by).DoSync();
            if (r is AsyncWebElement)
                return new SyncWebElement(r as AsyncWebElement);
            return null;
        }

        public SyncWebElement FindElementByClassName(string className)
        {
            var r = Driver.FindElementByClassName(className).DoSync();
            return r is AsyncWebElement ? new SyncWebElement(r as AsyncWebElement) : null;
        }

        public SyncWebElement FindElementByCssSelector(string cssSelector)
        {
            var r = Driver.FindElementByCssSelector(cssSelector).DoSync();
            return r is AsyncWebElement ? new SyncWebElement(r as AsyncWebElement) : null;
        }

        public SyncWebElement FindElementById(string id)
        {
            var r = Driver.FindElementById(id).DoSync();
            return r is AsyncWebElement ? new SyncWebElement(r as AsyncWebElement) : null;
        }

        public SyncWebElement FindElementByIdStartsWith(string id)
        {
            var r = Driver.FindElementByIdStartsWith(id).DoSync();
            return r is AsyncWebElement ? new SyncWebElement(r as AsyncWebElement) : null;
        }

        public SyncWebElement FindElementByIdEndsWith(string id)
        {
            var r = Driver.FindElementByIdEndsWith(id).DoSync();
            return r is AsyncWebElement ? new SyncWebElement(r as AsyncWebElement) : null;
        }

        public SyncWebElement FindElementByLinkText(string linkText)
        {
            var r = Driver.FindElementByLinkText(linkText).DoSync();
            return r is AsyncWebElement ? new SyncWebElement(r as AsyncWebElement) : null;
        }

        public SyncWebElement FindElementByName(string name)
        {
            var r = Driver.FindElementByName(name).DoSync();
            return r is AsyncWebElement ? new SyncWebElement(r as AsyncWebElement) : null;
        }

        public SyncWebElement FindElementByPartialLinkText(string partialLinkText)
        {
            var r = Driver.FindElementByPartialLinkText(partialLinkText).DoSync();
            return r is AsyncWebElement ? new SyncWebElement(r as AsyncWebElement) : null;
        }

        public SyncWebElement FindElementByTagName(string tagName)
        {
            var r = Driver.FindElementByTagName(tagName).DoSync();
            return r is AsyncWebElement ? new SyncWebElement(r as AsyncWebElement) : null;
        }

        public SyncWebElement FindElementByXPath(string xpath)
        {
            var r = Driver.FindElementByXPath(xpath).DoSync();
            return r is AsyncWebElement ? new SyncWebElement(r as AsyncWebElement) : null;
        }

        public List<SyncWebElement> FindElements(By by)
        {
            return Driver.FindElements(by).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public List<SyncWebElement> Children()
        {
            return Driver.Children().DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public List<SyncWebElement> FindElementsByClassName(string className)
        {
            return Driver.FindElementsByClassName(className).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public List<SyncWebElement> FindElementsByCssSelector(string cssSelector)
        {
            return Driver.FindElementsByCssSelector(cssSelector).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public List<SyncWebElement> FindElementsById(string id)
        {
            return Driver.FindElementsById(id).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public List<SyncWebElement> FindElementsByIdStartsWith(string id)
        {
            return Driver.FindElementsByIdStartsWith(id).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public List<SyncWebElement> FindElementsByIdEndsWith(string id)
        {
            return Driver.FindElementsByIdEndsWith(id).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public List<SyncWebElement> FindElementsByLinkText(string linkText)
        {
            return Driver.FindElementsByLinkText(linkText).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public List<SyncWebElement> FindElementsByName(string name)
        {
            return Driver.FindElementsByName(name).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public List<SyncWebElement> FindElementsByPartialLinkText(string partialLinkText)
        {
            return Driver.FindElementsByPartialLinkText(partialLinkText).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public List<SyncWebElement> FindElementsByTagName(string tagName)
        {
            return Driver.FindElementsByTagName(tagName).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public List<SyncWebElement> FindElementsByXPath(string xpath)
        {
            return Driver.FindElementsByXPath(xpath).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public string GetUrl()
        {
            return Driver.GetUrl().DoSync();
        }

        public string GoToUrl(string url)
        {
            return Driver.GoToUrl(url).DoSync();
        }

        public IOptions Manage()
        {
            return Driver.Manage();
        }

        public INavigation Navigate()
        {
            return Driver.Navigate();
        }

        public string PageSource()
        {
            return Driver.PageSource().DoSync();
        }

        public void Quit()
        {
            Driver.Quit().DoSync();
        }

        public ITargetLocator SwitchTo()
        {
            return Driver.SwitchTo();
        }

        public string Title()
        {
            return Driver.Title().DoSync();
        }

        public ReadOnlyCollection<string> WindowHandles()
        {
            return Driver.WindowHandles().DoSync();
        }

        public bool WaitForElementWithId(string id, string notWebElementId = null, int attemptsCount = 20, int delayMs = 500)
        {
            if (notWebElementId != null)
                return WaitForWebElement(() => FindElementById(id), attemptsCount, delayMs);
            return WaitForWebElement(() => FindElementById(id), notWebElementId, attemptsCount, delayMs);
        }

        private bool WaitForWebElement(Func<SyncWebElement> func, int attemptsCount = 20, int delayMs = 500)
        {
            for (var i = 0; i < attemptsCount; i++)
            {
                Thread.Sleep(delayMs);
                var el = func.Invoke();
                if (!string.IsNullOrWhiteSpace(el?.AsyncElement?.Id))
                    return true;
            }

            return false;
        }

        private bool WaitForWebElement(Func<SyncWebElement> func, string notWebElementId, int attemptsCount = 20, int delayMs = 500)
        {
            for (var i = 0; i < attemptsCount; i++)
            {
                Thread.Sleep(delayMs);
                var el = func.Invoke();
                if (notWebElementId != null && el?.Id == notWebElementId)
                    continue;
                if (!string.IsNullOrWhiteSpace(el?.AsyncElement?.Id))
                    return true;
            }

            return false;
        }
    }
}