using System;
using System.IO;
using UnityEngine;

namespace AsteriskMod.Lua
{
    // ! => ?
    public class Engine_old
    {
        // ! => nil
        public static void SetScreenResolution(int width, int height, bool fullscreen = false)
        {
            Asterisk.RequireExperimentalFeature("Engine.SetScreenResolution");
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

        // ! => false
        public static bool AppDataFileExists(string path)
        {
            Asterisk.RequireExperimentalFeature("Engine.AppDataFileExists");
            return File.Exists(GetLocalAppDataPath(path));
        }

        // ! => false
        public static bool AppDataDirExists(string path)
        {
            Asterisk.RequireExperimentalFeature("Engine.AppDataDirExists");
            return Directory.Exists(GetLocalAppDataPath(path));
        }

        // ! => false
        public static bool AppDataCreateDir(string path)
        {
            Asterisk.RequireExperimentalFeature("Engine.AppDataCreateDir");
            path = GetLocalAppDataPath(path);
            if (!Directory.Exists(path)) return false;
            Directory.CreateDirectory(path);
            return true;
        }

        // ! => nil
        public static LuaFile OpenAppDataFile(string path, string mode = "rw")
        {
            Asterisk.RequireExperimentalFeature("Engine.AppDataCreateDir");
            if (!Asterisk.experimentMode)
            {
                throw new CYFException("Engine.OpenAppDataFile() is experimental feature. You should enable \"Experimental Feature\" in AsteriskMod's option.");
            }
            return new LuaFile(GetLocalAppDataPath(path), mode, true);
        }
    }
}
