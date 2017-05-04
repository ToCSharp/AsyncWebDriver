using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCommunicationLib.Communication.MarionetteComands
{
    public class MarionetteDebuggerCommandString : MarionetteDebuggerCommand
    {
        public MarionetteDebuggerCommandString(string commandStr, int id) : base(id, "")
        {
            CommandStr = commandStr;
        }

        public string CommandStr { get; set; }

        public override string ToString()
        {
            return CommandStr;
        }
    }
}
