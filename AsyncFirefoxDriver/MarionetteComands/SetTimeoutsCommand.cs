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
        //public enum TimeoutType { @implicit, script, page_load };
        public SetTimeoutsCommand(TimeoutType timeoutType, int ms, int id = 0, string commandName = "timeouts") : base(id, commandName)
        {
            TimeoutTyp = timeoutType;
            Ms = ms;
        }
        public TimeoutType TimeoutTyp { get; set; }
        public int Ms { get; set; }

        public override void ProcessResponse(JToken response)
        {
            base.ProcessResponse(response);
        }

        public override string ToString()
        {
            return $"[0, {Id}, \"{CommandName}\", {{{GetTimeoutTypeStr(TimeoutTyp)}: {Ms}}} ]";

        }

        public static string GetTimeoutTypeStr(TimeoutType t)
        {
            switch(t)
            {
                case TimeoutType.@implicit: return "implicit";
                case TimeoutType.script: return "script";
                case TimeoutType.page_load: return "page load";
            }
            return null;
        }
    }
}
