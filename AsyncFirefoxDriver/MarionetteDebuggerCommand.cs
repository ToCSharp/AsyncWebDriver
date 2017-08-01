// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json.Linq;
using System.Linq;
using Zu.WebBrowser.Commands;

namespace MyCommunicationLib.Communication.MarionetteComands
{
    public class MarionetteDebuggerCommand : DebuggerCommand
    {
        public MarionetteDebuggerCommand(int id, string commandName) : base(id, commandName)
        {
        }
        public JToken Result { get; set; }
        public JToken Error { get; set; }

        public override void ProcessResponse(JToken response)
        {
            base.ProcessResponse(response);
            Result = response;
            Error = (response?.Parent as JArray)?.ElementAtOrDefault(2);
            //if (Error?.HasValues != true) Error = null;
            if (Error is JValue && (Error as JValue).Value == null ) Error = null;
        }


    }
}
