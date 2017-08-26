// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Zu.WebBrowser
{
    public class DriverProcessInfo
    {
        public Process Proc { get; set; }
        public ProcessWithJobObject ProcWithJobObject { get; set; }
        public string UserDir { get; set; }
        public int Port { get; set; }

        public override string ToString()
        {
            return UserDir + " " + Port;
        }

        public async Task CloseAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (Proc != null && !Proc.HasExited)
            {
                try
                {
                    Proc.CloseMainWindow();
                }
                catch
                {
                    try
                    {
                        Proc.Kill();
                    }
                    catch
                    {

                    }
                }
                while (!Proc.HasExited)
                {
                    await Task.Delay(250);
                }
            }
            Proc?.Dispose();
            if (ProcWithJobObject != null)
            {
                ProcWithJobObject.TerminateProc();
            }

        }
        public void Close()
        {
            if (Proc != null && !Proc.HasExited)
            {
                try
                {
                    Proc.CloseMainWindow();
                }
                catch
                {
                    try
                    {
                        Proc.Kill();
                    }
                    catch
                    {

                    }
                }
                while (Proc.HasExited)
                {
                    Thread.Sleep(250);
                }
            }
            Proc?.Dispose();
            if (ProcWithJobObject != null)
            {
                ProcWithJobObject.TerminateProc();
            }

        }
    }
}