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
    public class ExecuteScriptCommand : MarionetteDebuggerCommand
    {
        public ExecuteScriptCommand(string code, int id = 0) : base(id, "executeScript")
        {
            Code = code;
        }

        public string Code { get; set; }


        public string[] Args { get; set; } = new string[0];
        public bool newSandbox { get; set; } = false;
        public string sandbox { get; set; }
        public object scriptTimeout { get; set; }
        public bool specialPowers { get; set; } = false;
        public string filename { get; set; } = null;

        public override void ProcessResponse(JToken response)
        {
            base.ProcessResponse(response);
        }

        public override string ToString()
        {
            if (sandbox != null)
            {
                return JsonConvert.SerializeObject(
                    new object[]
                        {
                   0,
                   Id,
                   "executeScript",
                   new {
                       args = Args,
                       sandbox = sandbox,
                       newSandbox = newSandbox,
                       script = Code,
                       scriptTimeout = scriptTimeout,
                       specialPowers = specialPowers,
                       filename = filename
                   }

                });

            }
            return JsonConvert.SerializeObject(
                new object[]
                {
                   0,
                   Id,
                   "executeScript",
                   new {
                       args = Args,
                       newSandbox = newSandbox,
                       script = Code,
                       scriptTimeout = scriptTimeout,
                       specialPowers = specialPowers,
                       filename = filename
                   }

                });

        }
    }
}
