// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zu.WebBrowser.BasicTypes;
using System;

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
        public WebPoint Location => GetLocation();
        public WebSize Size => GetSize();
        public bool Displayed => GetDisplayed();
        public WebPoint LocationOnScreenOnceScrolledIntoView => GetLocationOnScreenOnceScrolledIntoView();
        public SyncCoordinates Coordinates => new SyncCoordinates(AsyncElement.Coordinates);
        public string OuterHtml => GetProperty("outerHTML");
        public string InnerHtml => GetProperty("innerHTML");
        public SyncWebElement First => GetChildren()?.FirstOrDefault();
        public SyncWebElement Last => GetChildren()?.FirstOrDefault();
        public List<SyncWebElement> Children => GetChildren();
        public override string ToString()
        {
            return AsyncElement?.ToString() ?? "AsyncElement = null";
        }

        public string GetTagName()
        {
            string res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncElement.TagName().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public string GetText()
        {
            string res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncElement.Text().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public bool GetEnabled()
        {
            var res = false;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncElement.Enabled().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public bool GetSelected()
        {
            var res = false;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncElement.Selected().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public WebPoint GetLocation()
        {
            WebPoint res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncElement.Location().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public WebSize GetSize()
        {
            WebSize res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncElement.Size().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public bool GetDisplayed()
        {
            var res = false;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncElement.Displayed().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public WebPoint GetLocationOnScreenOnceScrolledIntoView()
        {
            WebPoint res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncElement.LocationOnScreenOnceScrolledIntoView().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public void Clear()
        {
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await AsyncElement.Clear().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
        }

        public void SendKeys(string text)
        {
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await AsyncElement.SendKeys(text).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
        }

        public void Submit()
        {
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await AsyncElement.Submit().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
        }

        public void Click()
        {
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    await AsyncElement.Click().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
        }

        public string GetAttribute(string attributeName)
        {
            string res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncElement.GetAttribute(attributeName).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public string GetProperty(string propertyName)
        {
            string res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncElement.GetProperty(propertyName).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public string GetCssValue(string propertyName)
        {
            string res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncElement.GetCssValue(propertyName).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public List<SyncWebElement> GetChildren()
        {
            List<SyncWebElement> res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.Children().ConfigureAwait(false);
                    res = r?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public List<SyncWebElement> FindElements(By by)
        {
            List<SyncWebElement> res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElements(by).ConfigureAwait(false);
                    res = r?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElement(By by)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElement(by).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByLinkText(string linkText)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByLinkText(linkText).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByLinkTextOrDefault(string linkText)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByLinkTextOrDefault(linkText).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByLinkText(string linkText)
        {
            List<SyncWebElement> res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementsByLinkText(linkText).ConfigureAwait(false);
                    res = r?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementById(string id)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementById(id).ConfigureAwait(false);
                    if (r as AsyncWebElement != null)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByIdOrDefault(string id)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByIdOrDefault(id).ConfigureAwait(false);
                    if (r as AsyncWebElement != null)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByIdStartsWith(string id)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByIdStartsWith(id).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByIdStartsWithOrDefault(string id)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByIdStartsWithOrDefault(id).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByIdEndsWith(string id)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByIdEndsWith(id).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByIdEndsWithOrDefault(string id)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByIdEndsWithOrDefault(id).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsById(string id)
        {
            List<SyncWebElement> res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementsById(id).ConfigureAwait(false);
                    res = r?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByIdStartsWith(string id)
        {
            List<SyncWebElement> res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementsByIdStartsWith(id).ConfigureAwait(false);
                    res = r?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByIdEndsWith(string id)
        {
            List<SyncWebElement> res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementsByIdEndsWith(id).ConfigureAwait(false);
                    res = r?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByName(string name)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByName(name).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByNameOrDefault(string name)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByNameOrDefault(name).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByName(string name)
        {
            List<SyncWebElement> res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementsByName(name).ConfigureAwait(false);
                    res = r?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByTagName(string tagName)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByTagName(tagName).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByTagNameOrDefault(string tagName)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByTagNameOrDefault(tagName).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByTagName(string tagName)
        {
            List<SyncWebElement> res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementsByTagName(tagName).ConfigureAwait(false);
                    res = r?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByClassName(string className)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByClassName(className).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByClassNameOrDefault(string className)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByClassNameOrDefault(className).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByClassName(string className)
        {
            List<SyncWebElement> res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementsByClassName(className).ConfigureAwait(false);
                    res = r?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByXPath(string xpath)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByXPath(xpath).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByXPathOrDefault(string xpath)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByXPathOrDefault(xpath).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByXPath(string xpath)
        {
            List<SyncWebElement> res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementsByXPath(xpath).ConfigureAwait(false);
                    res = r?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByPartialLinkText(string partialLinkText)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByPartialLinkText(partialLinkText).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByPartialLinkTextOrDefault(string partialLinkText)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByPartialLinkTextOrDefault(partialLinkText).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByPartialLinkText(string partialLinkText)
        {
            List<SyncWebElement> res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementsByPartialLinkText(partialLinkText).ConfigureAwait(false);
                    res = r?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByCssSelector(string cssSelector)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByCssSelector(cssSelector).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public SyncWebElement FindElementByCssSelectorOrDefault(string cssSelector)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementByCssSelectorOrDefault(cssSelector).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public List<SyncWebElement> FindElementsByCssSelector(string cssSelector)
        {
            List<SyncWebElement> res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElementsByCssSelector(cssSelector).ConfigureAwait(false);
                    res = r?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public Screenshot GetScreenshot()
        {
            Screenshot res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncElement.GetScreenshot().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        protected SyncWebElement FindElement(string mechanism, string value)
        {
            SyncWebElement res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElement(mechanism, value).ConfigureAwait(false);
                    if (r is AsyncWebElement)
                        res = new SyncWebElement(r as AsyncWebElement);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        protected List<SyncWebElement> FindElements(string mechanism, string value)
        {
            List<SyncWebElement> res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    var r = await AsyncElement.FindElements(mechanism, value).ConfigureAwait(false);
                    res = r?.Where(a => a is AsyncWebElement).Select(v => new SyncWebElement(v as AsyncWebElement)).ToList();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public string UploadFile(string localFile) //=> AsyncElement.UploadFile(localFile);
        {
            string res = null;
            var mRes = new ManualResetEventSlim(true);
            mRes.Reset();
            Exception exception = null;
            Task.Run(async () =>
            {
                try
                {
                    res = await AsyncElement.UploadFile(localFile).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                mRes.Set();
            }

            );
            mRes.Wait();
            if (exception != null)
                throw exception;
            return res;
        }

        public Dictionary<string, object> ToElementReference()
        {
            return AsyncElement.ToElementReference();
        }
    }
}