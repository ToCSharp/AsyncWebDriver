// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyCommunicationLib.Communication.MarionetteComands
{
    public class AcceptDialogCommand : MarionetteDebuggerCommand
    {
        public AcceptDialogCommand(int id = 0, string commandName = "acceptDialog") : base(id, commandName)
        {
        }
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

                });

        }

    }
}
