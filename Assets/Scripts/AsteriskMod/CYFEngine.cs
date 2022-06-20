using MoonSharp.Interpreter;
using System;
using System.IO;
using UnityEngine;

namespace AsteriskMod
{
    public class CYFEngine
    {
        [MoonSharpHidden]
        internal static void Initialize()
        {
            Application.targetFrameRate = 60;
            BulletPool.POOLSIZE = 100;
        }

        [MoonSharpHidden]
        internal static void Reset()
        {
            Application.targetFrameRate = 60;
        }

        public static void SetTargetFrameRate(int frameRate = 60)
        {
            if (frameRate < 30 || frameRate > 120)
            {
                if (!Asterisk.RequireExperimentalFeature("Engine.SetTargetFrameRate", false))
                {
                    throw new CYFException("Engine.SetTargetFrameRate: frame rate should be between 30 and 120, or you need to enable \"Experimental Features\" in AsteriskMod's option.");
                }
                if (frameRate <= 0) throw new CYFException("Engine.SetTargetFrameRate: frame rate should be positive.");
            }
            Application.targetFrameRate = frameRate;
        }

        public static void SetBulletPoolSize(int newSize = 100)
        {
            Asterisk.RequireExperimentalFeature("Engine.SetBulletPoolSize");
            if (newSize <= 0) throw new CYFException("Engine.SetBulletPoolSize: the size of bullet pool should be positive.");
            BulletPool.POOLSIZE = newSize;
        }

        public static bool ModExists(string modFolderName)
        {
            if (modFolderName == null) throw new CYFException("Engine.ModExists: folder's name can not be nil.");
            if (modFolderName.Contains("..")) throw new CYFException("You cannot check file or directory outside of a mod folder. The use of \"..\" is forbidden.");
            return Directory.Exists(Path.Combine(FileLoader.DataRoot, "Mods/" + modFolderName));
        }

        public static void SetProjectilesAutoRemovingActive(bool active = true)
        {
            Asterisk.RequireExperimentalFeature("Engine.SetProjectilesAutoRemovingActive");
            AsteriskEngine.AutoRemoveProjectiles = active;
        }

        public static void RemoveAllProjectiles()
        {
            UIController.instance.encounter.RemoveAllProjectiles();
        }
        public static void RemoveAllBullets() { RemoveAllProjectiles(); }

        private static string ConvertToFullPath(string path)
        {
            if (path.Contains("\\") || path.Contains("/")) throw new CYFException("You cannot check for a folder inside of a AppData folder. The use of \"/\"(\"\\\") is forbidden.");
            return Path.Combine(Application.persistentDataPath + "/Mods", StaticInits.MODFOLDER + "/" + path);
        }

        private static void CheckDirectory()
        {
            string appDataRoot = Path.Combine(Application.persistentDataPath + "/Mods", StaticInits.MODFOLDER).Replace('\\', '/');
            if (!Directory.Exists(appDataRoot))
            {
                try { Directory.CreateDirectory(appDataRoot); }
                catch (IOException e) { throw new CYFException(e.GetType() + " error:\n\n" + e.Message); }
            }
        }

        public static LuaFile OpenAppDataFile(string path, string mode = "rw")
        {
            CheckDirectory();
            return new LuaFile(ConvertToFullPath(path), mode, true);
        }
        public static LuaFile OpenFile(string path, string mode = "rw") { return OpenAppDataFile(path, mode); }

        public bool AppDataFileExists(string path)
        {
            CheckDirectory();
            if (path.Contains("..")) throw new CYFException("You cannot check for a file outside of a AppData folder. The use of \"..\" is forbidden.");
            return File.Exists(ConvertToFullPath(path).Replace('\\', '/'));
        }
        public bool FileExists(string path) { return AppDataFileExists(path); }

        /*
        public static void RegistSprite(string filename)
        {
            Sprite sprite = SpriteUtil.FromFile(FileLoader.pathToModFile(filename + ".png"));
            if (sprite == null)
                throw new CYFException("The sprite " + filename + ".png doesn't exist.");
            SpriteRegistry.Set(filename, sprite);
        }
        */

        public static void SetSpriteFilterMode(string filename, string filtermode = "POINT")
        {
            Asterisk.RequireExperimentalFeature("Engine.SetSpriteFilterMode");
            Sprite sprite = SpriteRegistry.Get(filename);
            if (sprite == null)
            {
                if (filename.Length == 0)
                {
                    Debug.LogError("Engine.SetSpriteFilterMode: Filename is empty!");
                    return;
                }
                sprite = SpriteUtil.FromFile(FileLoader.pathToModFile("Sprites/" + filename + ".png"));
                if (sprite == null)
                    throw new CYFException("The sprite Sprites/" + filename + ".png doesn't exist.");
                SpriteRegistry.Set(filename, sprite);
            }
            try
            {
                sprite.texture.filterMode = (FilterMode)Enum.Parse(typeof(FilterMode), filtermode);
            }
            catch
            {
                if (filtermode != null) throw new CYFException("Engine.SetSpriteFilterMode: filtermode can only have either \"POINT\", \"BILINEAR\" or \"TRILINEAR\", but you entered \"" + filtermode.ToUpper() + "\".");
                                        throw new CYFException("Engine.SetSpriteFilterMode: filtermode can only have either \"POINT\", \"BILINEAR\" or \"TRILINEAR\", but you set it to a nil value.");
            }
        }

        /*
        public static void UnregistSprite(string filename)
        {
            Asterisk.RequireExperimentalFeature("Engine.UnregistSprite");
            if (SpriteRegistry.Get(filename)) throw new CYFException("The sprite Sprites/" + filename + ".png doesn't exist.");
            System.Type type = typeof(SpriteRegistry);
            System.Reflection.FieldInfo field = type.GetField("dict", System.Reflection.BindingFlags.InvokeMethod | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            System.Collections.Generic.Dictionary<string, Sprite> dict = (System.Collections.Generic.Dictionary<string, Sprite>)(field.GetValue(typeof(SpriteRegistry)));
            filename = filename.ToLower();
            string dictKey = (UnitaleUtil.IsOverworld ? "ow" : "b") + filename;
            dict.Remove(dictKey);
        }
        */

        /*
        public static void RegistAudioAsMusic(string filename)
        {
            Asterisk.RequireExperimentalFeature("Engine.RegistAudioAsMusic");
            AudioClip clip = FileLoader.getAudioClip(currentPath, dictMod[key].FullName);
        }
        */
    }
}
