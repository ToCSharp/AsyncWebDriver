using System.Collections.Generic;

namespace Zu.Firefox
{
    public class KeyValuePairVM
    {
        private KeyValuePair<string, string> v;

        public KeyValuePairVM(KeyValuePair<string, string> v)
        {
            this.v = v;
        }

        public string Name => v.Key;
        public string Val => v.Value;

        public override string ToString()
        {
            return $"{v.Key} ({v.Value})";
        }
    }
}