// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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