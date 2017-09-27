// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCommunicationLib.Communication.MarionetteComands
{
    public class SwitchToFrameCommand : MarionetteDebuggerCommand
    {
        public SwitchToFrameCommand(object frameId, string element = null, bool doFocus = true, int id = 0, string commandName = "switchToFrame") : base(id, commandName)
        {
            FrameId = frameId;
            Element = element;
            DoFocus = doFocus;
        }
        public object FrameId { get; set; }
        public string Element { get; set; }
        public bool DoFocus { get; set; }

        public override void ProcessResponse(JToken response)
        {
            base.ProcessResponse(response);
        }

        public override string ToString()
        {
            if(FrameId != null && Element != null) return JsonConvert.SerializeObject(
                new object[]
                {
                   0,
                   base.Id,
                   CommandName,
                   new {
                        id = FrameId,
                        element = Element,
                        focus = DoFocus
                   }

                });
            if (FrameId != null) return JsonConvert.SerializeObject(
                 new object[]
                 {
                   0,
                   base.Id,
                   CommandName,
                   new {
                        id = FrameId,
                        focus = DoFocus
                   }

                 });
            if (Element != null) return JsonConvert.SerializeObject(
                 new object[]
                 {
                   0,
                   base.Id,
                   CommandName,
                   new {
                        element = Element,
                        focus = DoFocus
                   }

                 });
            return JsonConvert.SerializeObject(
                 new object[]
                 {
                   0,
                   base.Id,
                   CommandName,
                   new {
                       id = FrameId,
                        //focus = DoFocus
                   }

                 });

        }
    }
}
