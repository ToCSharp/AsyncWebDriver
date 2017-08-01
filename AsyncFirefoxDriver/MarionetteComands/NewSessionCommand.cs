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
    public class NewSessionCommand : MarionetteDebuggerCommand
    {
        public NewSessionCommand(int id = 0, string commandName = "") : base(id, commandName)
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
                   "newSession",
                   new {
                        capabilities = new
                        {
                            desiredCapabilities = new
                            {
                                browserName = "firefox",
                                marionette = true,
                                platform = "ANY",
                                version = ""
                            },
                            requiredCapabilities = new
                            {

                            }
                        },
                        //sessionId = null
                   }

                });

        }
    }
}
