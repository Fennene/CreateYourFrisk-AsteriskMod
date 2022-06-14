using AsteriskMod.FakeIniLoader;
using System;
using System.IO;
using UnityEngine;

namespace AsteriskMod
{
    public class CodeStyle
    {
        public bool stringUtil;
        public bool arrayUtil;
        public bool cyfUtil;

        public CodeStyle()
        {
            stringUtil = false;
            arrayUtil = false;
            cyfUtil = false;
        }

        public const string CODESTYLE_FILE_NAME = "Lua/codestyle.cyfmod";

        private static bool IgnoreFile(string fullpath)
        {
            return !File.Exists(fullpath) || new FileInfo(fullpath).Length > 1024 * 1024; // 1MB
        }

        private static bool ConvertToBoolean(string text)
        {
            text = text.ToLower();
            if (text == "true") return true;
            if (text == "false") return false;
            return false;
        }

        public static CodeStyle Get(string modDirName)
        {
            CodeStyle style = new CodeStyle();
            string path = Path.Combine(FileLoader.DataRoot, "Mods/" + modDirName + "/" + CODESTYLE_FILE_NAME);
            if (IgnoreFile(path)) return style;

            FakeIni ini = FakeIniFileLoader.Load(path, false);

            foreach (string realKey in ini.Main.ParameterNames)
            {
                string keyName = realKey.ToLower().Replace("_", "-");
                if (realKey != keyName && ini.Main.ParameterExists(keyName)) continue;
                switch (keyName)
                {
                    //case "environment-pathes":
                    //case "env-pathes":
                    //style.environmentPathes = ini.Main[realKey].Array;
                    //break;
                    /*
                    case "extended-util":
                    case "exutil":
                    case "extendedutil":
                        style.extendedUtil = ConvertToBoolean(ini.Main[realKey].String);
                        break;
                    */
                    case "stringutil":
                    case "string-util":
                        style.stringUtil = ConvertToBoolean(ini.Main[realKey].String);
                        break;
                    case "arrayutil":
                    case "array-util":
                        style.arrayUtil = ConvertToBoolean(ini.Main[realKey].String);
                        break;
                    case "cyf-util":
                    case "cyfutil":
                        style.cyfUtil = ConvertToBoolean(ini.Main[realKey].String);
                        break;
                    /*
                    case "faster-util":
                    case "faster-tools":
                    case "faster-table":
                    case "faster-array":
                    case "fasterutil":
                    case "fastertools":
                    case "fastertable":
                    case "fasterarray":
                        style.fasterTable = ConvertToBoolean(ini.Main[realKey].String);
                        break;
                    */
                }
            }
            return style;
        }
    }
}
