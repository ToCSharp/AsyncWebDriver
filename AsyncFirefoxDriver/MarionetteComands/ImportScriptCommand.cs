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
    public class ImportScriptCommand : MarionetteDebuggerCommand
    {
        /**
         * Import script to the JS evaluation runtime.
         *
         * Imported scripts are exposed in the contexts of all subsequent
         * calls to {@code executeScript}, {@code executeAsyncScript}, and
         * {@code executeJSScript} by prepending them to the evaluated script.
         *
         * Scripts can be cleared with the {@code clearImportedScripts} command.
         *
         * @param {string} script
         *     Script to include.  If the script is byte-by-byte equal to an
         *     existing imported script, it is not imported.
         */
        public ImportScriptCommand(string script, int id = 0, string commandName = "importScript") : base(id, commandName)
        {
            Script = script;
        }

        public string Script { get; set; }

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
                       script = Script,
                   }

  });

            //return $"[0, {Id}, \"{CommandName}\", {{\"script\": \"{Script}\"}} ]";

        }
    }
}
