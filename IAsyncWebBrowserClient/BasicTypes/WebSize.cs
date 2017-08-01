// Copyright (c) Oleg Zudov. All Rights Reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Zu.WebBrowser.BasicTypes
{
    public class WebSize
    {
        public WebSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public override string ToString()
        {
            return $"WebSize: {Width}, {Height}";
        }
    }
}
