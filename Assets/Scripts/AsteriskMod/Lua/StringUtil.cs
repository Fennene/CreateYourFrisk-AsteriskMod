using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteriskMod.Lua
{
    public class StringUtil
    {
        public static bool StartsWith(string text, string value)
        {
            return text.StartsWith(value);
        }

        public static bool EndsWith(string text, string value)
        {
            return text.EndsWith(value);
        }

        public static bool Contains(string text, string value)
        {
            return text.Contains(value);
        }
    }
}
