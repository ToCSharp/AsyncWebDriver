
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCommunicationLib.Communication.MarionetteComands
{
    public class FindElementsCommand : MarionetteDebuggerCommand
    {
        public FindElementsCommand(string strategy, string expr, string startNode = null, int id = 0, string commandName = "findElements") : base(id, commandName)
        {
            Strategy = strategy;
            Expr = expr;
            StartNode = startNode;
        }

        public string Strategy { get; set; }
        public string Expr { get; set; }
        public string StartNode { get; set; }

        public override void ProcessResponse(JToken response)
        {
            base.ProcessResponse(response);
        }

        public override string ToString()
        {
            if(StartNode == null) return $"[0, {Id}, \"{CommandName}\", {{\"using\" : \"{Strategy}\", \"value\": \"{Expr?.Replace("\"", "\\\"")}\"}} ]";
            return $"[0, {Id}, \"{CommandName}\", {{\"using\" : \"{Strategy}\", \"value\": \"{Expr?.Replace("\"", "\\\"")}\", \"element\": \"{StartNode.Replace("\"", "\\\"")}\"}} ]";
          
        }
    }
}
