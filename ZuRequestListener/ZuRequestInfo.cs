namespace Zu.Browser
{
    public class ZuRequestInfo
    {
        public string Body { get; set; }
        public string Url { get; set; }
        public string Code { get; set; }

        public override string ToString()
        {
            return Url;
        }
    }
}