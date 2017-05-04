
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCommunicationLib.Communication.MarionetteComands
{
    public class GetContextCommand : MarionetteDebuggerCommand
    {
        public GetContextCommand(int id = 0, string commandName = "getContext") : base(id, commandName)
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
                   //new {
                   //     capabilities = new
                   //     {
                   //         desiredCapabilities = new
                   //         {
                   //             browserName = "firefox",
                   //             marionette = true,
                   //             platform = "ANY",
                   //             version = ""
                   //         },
                   //         requiredCapabilities = new
                   //         {

                   //         }
                   //     },
                   //     //sessionId = null
                   //}

                });

        }
    }
}
