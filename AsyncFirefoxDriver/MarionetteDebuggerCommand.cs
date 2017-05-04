using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
