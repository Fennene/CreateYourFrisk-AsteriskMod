namespace AsteriskMod.ExtendedUtil
{
    public class LuaStringUtil
    {
        public static string[] Split(string text, params char[] separator) { return text.Split(separator); }

        public static bool StartsWith(string text, string value) { return text.StartsWith(value); }
        public static bool EndsWith(string text, string value) { return text.EndsWith(value); }

        public static string Trim(string text, params char[] trimChars) { return text.Trim(trimChars); }
        public static string TrimStart(string text, params char[] trimChars) { return text.TrimStart(trimChars); }
        public static string TrimEnd(string text, params char[] trimChars) { return text.TrimEnd(trimChars); }

        public static char[] ToCharArray(string text) { return text.ToCharArray(); }
    }
}
