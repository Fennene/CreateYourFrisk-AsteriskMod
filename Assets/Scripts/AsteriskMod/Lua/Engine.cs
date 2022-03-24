using MoonSharp.Interpreter;
using System;
using System.IO;
using UnityEngine;

namespace AsteriskMod.Lua
{
    public class Engine
    {
        public static bool ModExists(string modFolderName)
        {
            return Directory.Exists(Path.Combine(FileLoader.DataRoot, "Mods/" + modFolderName));
        }

        [MoonSharpHidden]
        public static void SetScreenResolution(int width, int height, bool fullscreen)
        {
            if (!Asterisk.experimentMode)
            {
                throw new CYFException("Engine.SetScreenResolution() is experimental feature. You should enable \"Experimental Feature\" in AsteriskMod's option.");
            }
            Screen.SetResolution(width, height, fullscreen);
        }

        private static string GetLocalAppDataPath(string path)
        {
            if (path.Contains(".."))
                throw new CYFException("You cannot check for a file outside of AppData/Local/CreateYourFrisk/AsteriskMod/" + StaticInits.MODFOLDER + " folder. The use of \"..\" is forbidden.");
            string targetFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/CreateYourFrisk/AsteriskMod/" + StaticInits.MODFOLDER;
            if (!Directory.Exists(targetFilePath))
            {
                Directory.CreateDirectory(targetFilePath);
            }
            return (targetFilePath + "/" + path).Replace("\\", "/");
        }

        [MoonSharpHidden]
        public static bool AppDataFileExists(string path)
        {
            if (!Asterisk.experimentMode)
            {
                throw new CYFException("Engine.AppDataFileExists() is experimental feature. You should enable \"Experimental Feature\" in AsteriskMod's option.");
            }
            return File.Exists(GetLocalAppDataPath(path));
        }

        [MoonSharpHidden]
        public static bool AppDataDirExists(string path)
        {
            if (!Asterisk.experimentMode)
            {
                throw new CYFException("Engine.AppDataDirExists() is experimental feature. You should enable \"Experimental Feature\" in AsteriskMod's option.");
            }
            return Directory.Exists(GetLocalAppDataPath(path));
        }

        [MoonSharpHidden]
        public static bool AppDataCreateDir(string path)
        {
            if (!Asterisk.experimentMode)
            {
                throw new CYFException("Engine.AppDataCreateDir() is experimental feature. You should enable \"Experimental Feature\" in AsteriskMod's option.");
            }
            path = GetLocalAppDataPath(path);
            if (!Directory.Exists(path)) return false;
            Directory.CreateDirectory(path);
            return true;
        }

        [MoonSharpHidden]
        public static LuaFile OpenAppDataFile(string path, string mode = "rw")
        {
            if (!Asterisk.experimentMode)
            {
                throw new CYFException("Engine.OpenAppDataFile() is experimental feature. You should enable \"Experimental Feature\" in AsteriskMod's option.");
            }
            return new LuaFile(GetLocalAppDataPath(path), mode, true);
        }
    }
}
