// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


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
