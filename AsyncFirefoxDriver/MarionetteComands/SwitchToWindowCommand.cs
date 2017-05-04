
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCommunicationLib.Communication.MarionetteComands
{
    public class SwitchToWindowCommand : MarionetteDebuggerCommand
    {
        public SwitchToWindowCommand(string name, int id = 0, string commandName = "switchToWindow") : base(id, commandName)
        {
            Name = name;
        }
        public string Name { get; set; }
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
                        name = Name
                   }

                });

        }
    }
}
