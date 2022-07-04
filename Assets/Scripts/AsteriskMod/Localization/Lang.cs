using AsteriskMod.FakeIniLoader;
using MoonSharp.Interpreter;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AsteriskMod
{
    public class Lang
    {
        private static string _target;
        private static Dictionary<string, FakeIni> _modLangs;

        private const string ENGLISH_TAG = "*en";
        private const string JAPANESE_TAG = "*ja";

        private const string LANG_DIR = "Localization";
        [MoonSharpHidden] internal const string ENGLISH_LANG_FILE = "Localization/en.lang";
        [MoonSharpHidden] internal const string JAPANESE_LANG_FILE = "Localization/ja.lang";

        [MoonSharpHidden]
        internal static void Initialize()
        {
            _target = ENGLISH_TAG;
            _modLangs = new Dictionary<string, FakeIni>();
        }

        [MoonSharpHidden]
        internal static void PrepareMod(string modName)
        {
            _target = (Asterisk.language == Languages.Japanese) ? JAPANESE_TAG : ENGLISH_TAG;
            _modLangs = new Dictionary<string, FakeIni>();

            string path = Path.Combine(FileLoader.DataRoot, "Mods/" + modName);
            if (File.Exists(path + "/" + ENGLISH_LANG_FILE))
            {
                Debug.Log("File \"" + path + " / " + ENGLISH_LANG_FILE + "\" is found!");
                _modLangs[ENGLISH_TAG] = FakeIniFileLoader.Load(path + "/" + ENGLISH_LANG_FILE);
            }
            else
            {
                _modLangs[ENGLISH_TAG] = new FakeIni();
            }
            if (!Directory.Exists(path + "/" + LANG_DIR)) return;
            if (File.Exists(path + "/" + JAPANESE_LANG_FILE))
            {
                Debug.Log("File \"" + path + " / " + JAPANESE_LANG_FILE + "\" is found!");
                _modLangs[JAPANESE_TAG] = _modLangs[ENGLISH_TAG].Clone();
                _modLangs[JAPANESE_TAG].Join(FakeIniFileLoader.Load(path + "/" + JAPANESE_LANG_FILE), true);
            }
            foreach (string filePath in Directory.GetFiles(path + "/" + LANG_DIR, "*.lang", SearchOption.AllDirectories))
            {
                if (!filePath.EndsWith(".lang")) continue;
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                if (fileName == "en" || fileName == "ja") continue;
                _modLangs[fileName.ToLower()] = _modLangs[ENGLISH_TAG].Clone();
                _modLangs[fileName.ToLower()].Join(FakeIniFileLoader.Load(filePath), true);
            }
        }

        [MoonSharpHidden]
        internal static void Reset()
        {
            _modLangs = new Dictionary<string, FakeIni>();
        }

        [MoonSharpHidden]
        internal static void Exist(string modName, out bool english, out bool japanese)
        {
            string path = Path.Combine(FileLoader.DataRoot, "Mods/" + modName);
            english  = File.Exists(path + "/" + ENGLISH_LANG_FILE );
            japanese = File.Exists(path + "/" + JAPANESE_LANG_FILE);
        }

        public static void SetTargetLanguage(string langName)
        {
            if (_modLangs.ContainsKey(langName.ToLower()))
            {
                _target = langName.ToLower();
                return;
            }
            Languages _ = AsteriskUtil.ConvertToLanguage(langName, true);
            if (_ == Languages.Japanese) _target = JAPANESE_TAG;
            else                         _target = ENGLISH_TAG;
        }

        public static string Get(string key) { return _modLangs[_target].Main[key].String; }
        public static string Get(string sectionName, string parameterName) { return _modLangs[_target][sectionName][parameterName].String; }

        public static string[] GetRaw(string key) { return _modLangs[_target].Main[key].Array; }
        public static string[] GetRaw(string sectionName, string parameterName) { return _modLangs[_target][sectionName][parameterName].Array; }

        public string this[string key] { get { return Get(key); } }

        public static void Set(string key, string value) { _modLangs[_target].Main[key] = new FakeIniParameter(value); }
        public static void Set(string sectionName, string parameterName, string parameter) { _modLangs[_target][sectionName][parameterName] = new FakeIniParameter(parameter); }

        public static void SetRaw(string key, string[] values) { _modLangs[_target].Main[key] = new FakeIniParameter(values); }
        public static void SetRaw(string sectionName, string parameterName, string[] parameters) { _modLangs[_target][sectionName][parameterName] = new FakeIniParameter(parameters); }
    }
}
