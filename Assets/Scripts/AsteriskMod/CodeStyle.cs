using AsteriskMod.FakeIniLoader;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AsteriskMod
{
    public class CodeStyle
    {
        public bool gms;
        public string[] libPathes;
        public bool stringUtil;
        public bool arrayUtil;
        public bool cyfUtil;

        public CodeStyle()
        {
            gms = false;
            libPathes = new string[0];
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

            FakeIni ini = FakeIniFileLoader.Load(path, true);
            string[] rawLibPathes = new string[0];
            foreach (string realKey in ini.Main.ParameterNames)
            {
                string keyName = realKey.ToLower().Replace("_", "-");
                if (realKey != keyName && ini.Main.ParameterExists(keyName)) continue;
                switch (keyName)
                {
                    case "environment-pathes":
                    case "env-pathes":
                    case "library-pathes":
                    case "lib-pathes":
                        rawLibPathes = ini.Main[realKey].Array;
                        break;
                    case "gameobjectmodifyingsystem":
                    case "gameobject-modifying-system":
                    case "gms":
                        style.gms = ConvertToBoolean(ini.Main[realKey].String);
                        break;
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
                }
            }
            if (rawLibPathes.Length == 0) return style;

            List<string> libPathes = new List<string>();
            foreach (string libPath in rawLibPathes)
            {
                if (libPath.Contains("..")) continue;
                /*
                string tempPath = libPath;
                if (tempPath.EndsWith("/"))
                {
                    tempPath += "?.lua";
                }
                else if (tempPath.EndsWith("?"))
                {
                    tempPath += ".lua";
                }
                else if (tempPath.Contains(".") && !tempPath.EndsWith(".lua"))
                {
                    continue;
                }
                else if (!tempPath.EndsWith("?.lua"))
                {
                    tempPath += "/?.lua";
                }
                libPathes.Add(tempPath);
                */
                string tempPath = libPath;
                if (tempPath.EndsWith("/"))
                {
                    tempPath += "?.lua";
                }
                else if (!tempPath.EndsWith("/?.lua"))
                {
                    tempPath += "/?.lua";
                }
                libPathes.Add(tempPath);
            }
            style.libPathes = libPathes.ToArray();
            return style;
        }
    }
}
