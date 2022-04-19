namespace AsteriskMod
{
    public class InstantTextMessage
    {
        public InstantTextMessage(string text, bool actualText = true)
        {
            text = Unescape(text); // compensate for unity inspector autoescaping control characters
            text = text.Replace("[name]", PlayerCharacter.instance.Name);
            Text = text;
            ActualText = actualText;
        }

        public string Text { get; set; }
        public bool Decorated { get { return false; } }
        public bool ShowImmediate { get { return true; } }
        public bool ActualText { get; private set; }

        public void SetText(string text) { Text = text; }

        private static string Unescape(string str)
        {
            try { return str.Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\t", "\t"); }
            catch { return str; }
        }

        public TextMessage Convert()
        {
            return new TextMessage(Text, false, true, ActualText, null);
        }
    }
}
