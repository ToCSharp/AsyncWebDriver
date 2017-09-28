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
    public class GetElementRectCommand : MarionetteDebuggerCommand
    {
        public GetElementRectCommand(string elementId, int id = 0, string commandName = "getElementRect") : base(id, commandName)
        {
            ElementId = elementId;
        }

        public string ElementId { get; set; }

        public override void ProcessResponse(JToken response)
        {
            base.ProcessResponse(response);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(
        new object[]
        {
                   0,
                   Id,
                   CommandName,
                   new {
                       id = ElementId,
                   }

        });
            //return $"[0, {Id}, \"{CommandName}\", {{\"id\": \"{ElementId}\"}} ]";
          
        }
    }
}
