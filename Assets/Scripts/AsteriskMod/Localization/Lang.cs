using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteriskMod
{
    public static class Lang
    {
        private static Dictionary<string, TransText> Texts;

        public static string Get(string key)
        {
            return Texts[key].Get();
        }

        internal static void Initialize()
        {
            Load();
        }

        internal static void Load()
        {
            if (Asterisk.language == Languages.Japanese)
            {
                Texts = PreInstalledLanguagePack.Japanese;
            }
            Texts = PreInstalledLanguagePack.English;
        }
    }
}
