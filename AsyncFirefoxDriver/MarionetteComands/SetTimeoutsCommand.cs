// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zu.WebBrowser;

namespace MyCommunicationLib.Communication.MarionetteComands
{
    public class SetTimeoutsCommand : MarionetteDebuggerCommand
    {
        public enum TimeoutType { implicitWait, script, page_load };
        public SetTimeoutsCommand(TimeoutType timeoutType, int ms, int id = 0, string commandName = "setTimeouts") : base(id, commandName)
        {
            Type = timeoutType;
            Ms = ms;
        }
        public TimeoutType Type { get; set; }
        public int Ms { get; set; }

        public override void ProcessResponse(JToken response)
        {
            base.ProcessResponse(response);
        }

        public override string ToString()
        {
            return $"[0, {Id}, \"{CommandName}\", {{\"{GetTimeoutTypeStr(Type)}\": {Ms}}} ]";

        }

        public static string GetTimeoutTypeStr(TimeoutType t)
        {
            switch(t)
            {
                case TimeoutType.implicitWait: return "implicit";
                case TimeoutType.script: return "script";
                case TimeoutType.page_load: return "pageLoad"; //"page load"; //
            }
            return null;
        }
    }
}
