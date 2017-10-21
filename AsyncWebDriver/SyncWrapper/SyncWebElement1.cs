// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zu.AsyncWebDriver.Interactions.Internal;

namespace Zu.AsyncWebDriver.Remote
{
    public class SyncWebElement
    {
        public SyncWebElement(AsyncWebElement element)
        {
            AsyncElement = element;
        }

        public AsyncWebElement AsyncElement
        {
            get;
        }

        public string Id => AsyncElement?.Id;
        public string TagName => GetTagName();
        public string Text => GetText();
        public bool Enabled => GetEnabled();
        public bool Selected => GetSelected();
        public Point Location => GetLocation();
        public Size Size => GetSize();
        public bool Displayed => GetDisplayed();
        public Point LocationOnScreenOnceScrolledIntoView => GetLocationOnScreenOnceScrolledIntoView();
        public ICoordinates Coordinates => AsyncElement.Coordinates;
        public string OuterHTML => GetProperty("outerHTML");
        public string InnerHTML => GetProperty("innerHTML");
        public SyncWebElement First => GetChildren()?.FirstOrDefault();
        public SyncWebElement Last => GetChildren()?.LastOrDefault();
        public List<SyncWebElement> Children => GetChildren();
        public override string ToString()
        {
            return AsyncElement?.ToString() ?? "AsyncElement = null";
        }

        public string GetTagName()
        {
            return AsyncElement.TagName().DoSync();
        }

        public string GetText()
        {
            return AsyncElement.Text().DoSync();
        }

        public bool GetEnabled()
        {
            return AsyncElement.Enabled().DoSync();
        }

        public bool GetSelected()
        {
            return AsyncElement.Selected().DoSync();
        }

        public Point GetLocation()
        {
            return AsyncElement.Location().DoSync();
        }

        public Size GetSize()
        {
            var res = new Size(0, 0);
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Task.Run(async () =>
            {
                res = await AsyncElement.Size().ConfigureAwait(false);
                mRes.Set();
            }

            );
            mRes.Wait();
            return res;
        }

        public bool GetDisplayed()
        {
            return AsyncElement.Displayed().DoSync();
        }

        public Point GetLocationOnScreenOnceScrolledIntoView()
        {
            return AsyncElement.LocationOnScreenOnceScrolledIntoView().DoSync();
        }

        public void Clear()
        {
            AsyncElement.Clear().DoSync();
        }

        public void SendKeys(string text)
        {
            AsyncElement.SendKeys(text).DoSync();
        }

        public void Submit()
        {
            AsyncElement.Submit().DoSync();
        }

        public void Click()
        {
            AsyncElement.Click().DoSync();
        }

        public string GetAttribute(string attributeName)
        {
            return AsyncElement.GetAttribute(attributeName).DoSync();
        }

        public string GetProperty(string propertyName)
        {
            return AsyncElement.GetProperty(propertyName).DoSync();
        }

        public string GetCssValue(string propertyName)
        {
            return AsyncElement.GetCssValue(propertyName).DoSync();
        }

        public List<SyncWebElement> GetChildren()
        {
            return AsyncElement.Children().DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public List<SyncWebElement> FindElements(By by)
        {
            return AsyncElement.FindElements(by).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public SyncWebElement FindElement(By by)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Task.Run(async () =>
            {
                var r = await AsyncElement.FindElement(by).ConfigureAwait(false);
                if (r is AsyncWebElement)
                    res = new SyncWebElement(r as AsyncWebElement);
                mRes.Set();
            }

            );
            mRes.Wait();
            return res;
        }

        public SyncWebElement FindElementByLinkText(string linkText)
        {
            var r = AsyncElement.FindElementByLinkText(linkText).DoSync() as AsyncWebElement;
            return r != null ? new SyncWebElement(r) : null;
        }

        public List<SyncWebElement> FindElementsByLinkText(string linkText)
        {
            return AsyncElement.FindElementsByLinkText(linkText).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public SyncWebElement FindElementById(string id)
        {
            var r = AsyncElement.FindElementById(id).DoSync() as AsyncWebElement;
            return r != null ? new SyncWebElement(r) : null;
        }

        public SyncWebElement FindElementByIdStartsWith(string id)
        {
            var r = AsyncElement.FindElementByIdStartsWith(id).DoSync() as AsyncWebElement;
            return r != null ? new SyncWebElement(r) : null;
        }

        public SyncWebElement FindElementByIdEndsWith(string id)
        {
            var r = AsyncElement.FindElementByIdEndsWith(id).DoSync() as AsyncWebElement;
            return r != null ? new SyncWebElement(r) : null;
        }

        public List<SyncWebElement> FindElementsById(string id)
        {
            return AsyncElement.FindElementsById(id).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public List<SyncWebElement> FindElementsByIdStartsWith(string id)
        {
            return AsyncElement.FindElementsByIdStartsWith(id).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public List<SyncWebElement> FindElementsByIdEndsWith(string id)
        {
            return AsyncElement.FindElementsByIdEndsWith(id).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public SyncWebElement FindElementByName(string name)
        {
            var r = AsyncElement.FindElementByName(name).DoSync() as AsyncWebElement;
            return r != null ? new SyncWebElement(r) : null;
        }

        public List<SyncWebElement> FindElementsByName(string name)
        {
            return AsyncElement.FindElementsByName(name).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public SyncWebElement FindElementByTagName(string tagName)
        {
            var r = AsyncElement.FindElementByTagName(tagName).DoSync() as AsyncWebElement;
            return r != null ? new SyncWebElement(r) : null;
        }

        public List<SyncWebElement> FindElementsByTagName(string tagName)
        {
            return AsyncElement.FindElementsByTagName(tagName).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public SyncWebElement FindElementByClassName(string className)
        {
            var r = AsyncElement.FindElementByClassName(className).DoSync() as AsyncWebElement;
            return r != null ? new SyncWebElement(r) : null;
        }

        public List<SyncWebElement> FindElementsByClassName(string className)
        {
            return AsyncElement.FindElementsByClassName(className).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public SyncWebElement FindElementByXPath(string xpath)
        {
            var r = AsyncElement.FindElementByXPath(xpath).DoSync() as AsyncWebElement;
            return r != null ? new SyncWebElement(r) : null;
        }

        public List<SyncWebElement> FindElementsByXPath(string xpath)
        {
            return AsyncElement.FindElementsByXPath(xpath).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public SyncWebElement FindElementByPartialLinkText(string partialLinkText)
        {
            var r = AsyncElement.FindElementByPartialLinkText(partialLinkText).DoSync() as AsyncWebElement;
            return r != null ? new SyncWebElement(r) : null;
        }

        public List<SyncWebElement> FindElementsByPartialLinkText(string partialLinkText)
        {
            return AsyncElement.FindElementsByPartialLinkText(partialLinkText).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public SyncWebElement FindElementByCssSelector(string cssSelector)
        {
            var r = AsyncElement.FindElementByCssSelector(cssSelector).DoSync() as AsyncWebElement;
            return r != null ? new SyncWebElement(r) : null;
        }

        public List<SyncWebElement> FindElementsByCssSelector(string cssSelector)
        {
            return AsyncElement.FindElementsByCssSelector(cssSelector).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public Screenshot GetScreenshot()
        {
            return AsyncElement.GetScreenshot().DoSync();
        }

        protected SyncWebElement FindElement(string mechanism, string value)
        {
            var r = AsyncElement.FindElement(mechanism, value).DoSync() as AsyncWebElement;
            return r != null ? new SyncWebElement(r) : null;
        }

        protected List<SyncWebElement> FindElements(string mechanism, string value)
        {
            return AsyncElement.FindElements(mechanism, value).DoSync()?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
        }

        public string UploadFile(string localFile)
        {
            return AsyncElement.UploadFile(localFile).DoSync();
        }

        public Dictionary<string, object> ToElementReference()
        {
            return AsyncElement.ToElementReference();
        }
    }
}