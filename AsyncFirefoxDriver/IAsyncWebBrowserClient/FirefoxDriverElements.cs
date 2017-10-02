// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Zu.WebBrowser.AsyncInteractions;
using Zu.WebBrowser.BasicTypes;
using MyCommunicationLib.Communication.MarionetteComands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zu.Firefox
{
    public class FirefoxDriverElements : IElements
    {
        private IAsyncFirefoxDriver asyncFirefoxDriver;

        public FirefoxDriverElements(IAsyncFirefoxDriver asyncFirefoxDriver)
        {
            this.asyncFirefoxDriver = asyncFirefoxDriver;
        }

        public async Task<string> ClearElement(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new ClearElementCommand(elementId);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return "ok";
        }

        public async Task Click(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new ClickElementCommand(elementId);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
        }

        public async Task<JToken> FindElement(string strategy, string expr, string startNode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                var res = await FindElementNotWait(strategy, expr, startNode, cancellationToken);
                if (ResultValueConverter.ValueIsNull(res))
                {
                    var implicitWait = await asyncFirefoxDriver.Options.Timeouts.GetImplicitWait();
                    if (implicitWait != default(TimeSpan))
                    {
                        var waitEnd = DateTime.Now + implicitWait;
                        while (ResultValueConverter.ValueIsNull(res) && DateTime.Now < waitEnd)
                        {
                            Thread.Sleep(50);
                            res = await FindElementNotWait(strategy, expr, startNode, cancellationToken = default(CancellationToken));
                        }
                    }
                }
                if (ResultValueConverter.ValueIsNull(res)) throw new WebBrowserException($"Element not found by {strategy} = {expr}", "no such element");
                return res;
            }
            catch { throw; }
        }
        public async Task<JToken> FindElementNotWait(string strategy, string expr, string startNode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new FindElementCommand(strategy, expr, startNode);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return comm1.Result;
        }

        public async Task<JToken> FindElements(string strategy, string expr, string startNode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await FindElementsNotWait(strategy, expr, startNode, cancellationToken = default(CancellationToken));
            if ((res as JArray)?.Any() != true)
            {
                var implicitWait = await asyncFirefoxDriver.Options.Timeouts.GetImplicitWait();
                if (implicitWait != default(TimeSpan))
                {
                    var waitEnd = DateTime.Now + implicitWait;
                    while (((res as JArray)?.Any() != true) && DateTime.Now < waitEnd)
                    {
                        Thread.Sleep(50);
                        res = await FindElementsNotWait(strategy, expr, startNode, cancellationToken = default(CancellationToken));
                    }
                }
            }
            if (res == null) throw new WebBrowserException($"Elements not found by {strategy} = {expr}", "no such element");
            return res;
            //return asyncChromeDriver.WindowCommands.FindElements(strategy, expr, startNode, cancellationToken);
        }

        public async Task<JToken> FindElementsNotWait(string strategy, string expr, string startNode = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new FindElementsCommand(strategy, expr, startNode);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return comm1.Result;
        }

        public async Task<string> GetActiveElement(CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new GetActiveElementCommand();
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return (string)comm1.Result["value"]; // comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<string> GetElementAttribute(string elementId, string attrName, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new GetElementAttributeCommand(elementId, attrName);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return (string)comm1.Result["value"]; // comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public Task<WebPoint> GetElementLocation(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> GetElementProperty(string elementId, string propertyName, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new GetElementPropertyCommand(elementId, propertyName);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return (string)comm1.Result["value"]; // comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<WebRect> GetElementRect(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new GetElementRectCommand(elementId);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return ResultValueConverter.ToWebRect(comm1.Result);
        }

        public Task<WebSize> GetElementSize(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new System.NotImplementedException();
        }

        public async Task<string> GetElementTagName(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new GetElementTagNameCommand(elementId);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return (string)comm1.Result["value"]; // comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<string> GetElementText(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new GetElementTextCommand(elementId);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return (string)comm1.Result["value"]; // comm1.Result is JValue ? (JValue)comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<string> GetElementValueOfCssProperty(string elementId, string propertyName, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new GetElementValueOfCssPropertyCommand(elementId, propertyName);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return (string)comm1.Result["value"]; // comm1.Result is JValue ? comm1.Result.ToString() : comm1.Result?["value"]?.ToString();
        }

        public async Task<bool> IsElementDisplayed(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new IsElementDisplayedCommand(elementId);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return ResultValueConverter.ToBool(comm1.Result);
        }

        public async Task<bool> IsElementEnabled(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new IsElementEnabledCommand(elementId);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return ResultValueConverter.ToBool(comm1.Result);
        }

        public async Task<bool> IsElementSelected(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new IsElementSelectedCommand(elementId);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return ResultValueConverter.ToBool(comm1.Result);
        }

        public async Task<string> SendKeysToElement(string elementId, string value, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            var comm1 = new SendKeysToElementCommand(elementId, value);
            await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return "ok";
        }

        public async Task<string> SubmitElement(string elementId, CancellationToken cancellationToken = default(CancellationToken))
        {
            await asyncFirefoxDriver.CheckConnected(cancellationToken);
            if (asyncFirefoxDriver.ClientMarionette == null) throw new Exception("error: no clientMarionette");
            string elementType = await GetElementProperty(elementId, "type", cancellationToken);
            if (elementType != null && elementType == "submit")
            {
                await this.Click(elementId, cancellationToken);
            }
            else
            {
                var json = await asyncFirefoxDriver.Elements.FindElement("xpath", "./ancestor-or-self::form", elementId);
                var form = GetElementFromResponse(json);
                var elementDictionary = new Dictionary<string, object>();
                elementDictionary.Add("ELEMENT", form);
                elementDictionary.Add("element-6066-11e4-a52e-4f735466cecf", form);

                await asyncFirefoxDriver.JavaScriptExecutor.ExecuteScript(
                    "var e = arguments[0].ownerDocument.createEvent('Event');" +
                    "e.initEvent('submit', true, true);" +
                    "if (arguments[0].dispatchEvent(e)) { arguments[0].submit(); }", cancellationToken, elementDictionary);// json.ToString());// "{ \"ELEMENT\": \"" + form + "\"}" );
            }


            //var comm1 = new SubmitElementCommand(elementId);
            //await asyncFirefoxDriver.ClientMarionette?.SendRequestAsync(comm1, cancellationToken);
            //if (comm1.Error != null) throw new WebBrowserException(comm1.Error);
            return "ok";
        }

        public static string GetElementFromResponse(JToken response)
        {
            string id = null;
            var json = response is JValue ? JToken.Parse(response.Value<string>()) : response["value"];
            id = json?["element-6066-11e4-a52e-4f735466cecf"]?.ToString();
            if (id == null)
                id = json?["ELEMENT"]?.ToString();
            return id;
        }
        public static List<string> GetElementsFromResponse(JToken response)
        {
            var toReturn = new List<string>();
            if (response is JArray)
                foreach (var item in response)
                {
                    string id = null;
                    try
                    {
                        var json = item is JValue ? JToken.Parse(item.Value<string>()) : item;
                        id = json?["element-6066-11e4-a52e-4f735466cecf"]?.ToString();
                        if (id == null)
                            id = json?["ELEMENT"]?.ToString();
                    }
                    catch
                    {
                    }

                    toReturn.Add(id);
                }

            return toReturn;
        }
    }
}