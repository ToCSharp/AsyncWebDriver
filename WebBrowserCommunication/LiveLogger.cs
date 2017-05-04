using System;

namespace Zu.WebBrowser.Logging
{
    public class LiveLogger
    {
        public static event EventHandler<string> OnMessage;

        public static void WriteLine(string v, object obj = null, Exception ex = null)
        {
            OnMessage?.Invoke(null, v);
        }
    }
}