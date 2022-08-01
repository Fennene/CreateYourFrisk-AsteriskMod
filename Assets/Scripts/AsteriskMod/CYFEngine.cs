using MoonSharp.Interpreter;
using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AsteriskMod
{
    public class CYFEngine
    {
        internal static void Initialize()
        {
            Application.targetFrameRate = 60;
            BulletPool.POOLSIZE = 100;
        }

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

        public bool ModExists(string modFolderName)
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

        [ToDo("Translate Error / Deletes when GMS is Implemented.")]
        public static void SetLayerActive(string layerName, bool active)
        {
            string canvas = UnitaleUtil.IsOverworld ? "Canvas Two/" : "Canvas/";
            if (string.IsNullOrEmpty(layerName)) throw new CYFException("Engine.SetLayerActive: Layer's name shouldn't be nil or empty string.");
            GameObject layer = GameObject.Find(canvas + layerName + "Layer");
            if (layer == null) throw new CYFException("Engine.SetLayerActive: Layer \"" + layerName + "\" is not found.");
            layer.SetActive(active);
        }

        [ToDo("Translate Error / Deletes when GMS is Implemented.")]
        public static bool GetLayerActive(string layerName)
        {
            string canvas = UnitaleUtil.IsOverworld ? "Canvas Two/" : "Canvas/";
            if (string.IsNullOrEmpty(layerName)) throw new CYFException("Engine.GetLayerActive: Layer's name shouldn't be nil or empty string.");
            GameObject layer = GameObject.Find(canvas + layerName + "Layer");
            if (layer == null) throw new CYFException("Engine.GetLayerActive: Layer \"" + layerName + "\" is not found.");
            return layer.activeSelf;
        }

        public static void GCCollect()
        {
            Asterisk.RequireExperimentalFeature("Engine.GCCollect");
            System.GC.Collect();
        }
        public static void GCCollect(int generation)
        {
            Asterisk.RequireExperimentalFeature("Engine.GCCollect");
            System.GC.Collect(generation);
        }
        public static void GCCollect(int generation, string mode)
        {
            Asterisk.RequireExperimentalFeature("Engine.GCCollect");
            GCCollectionMode gcCollectionMode;
            try
            {
                gcCollectionMode = (GCCollectionMode)Enum.Parse(typeof(GCCollectionMode), mode);
            }
            catch
            {
                throw new CYFException("Engine.GCCollect: mode should be \"Default\", \"Forced\", or \"Optimized\"");
            }            
            System.GC.Collect(generation, gcCollectionMode);
        }


        internal static string ConvertToAppDataFullPath(string path)
        {
            return Path.Combine(Application.persistentDataPath + "/Mods", StaticInits.MODFOLDER + "/" + path);
        }

        private void CheckDirectory()
        {
            string appDataRoot = Path.Combine(Application.persistentDataPath + "/Mods", StaticInits.MODFOLDER).Replace('\\', '/');
            if (!Directory.Exists(appDataRoot))
            {
                try { Directory.CreateDirectory(appDataRoot); }
                catch (IOException e) { throw new CYFException(e.GetType() + " error:\n\n" + e.Message); }
            }
        }

        private string GetAppDataErrorMessage(string keyName)
        {
            return EngineLang.Get("Exception", keyName).Replace("mod", "AppData");
        }

        public LuaFile OpenAppDataFile(string path, string mode = "rw")
        {
            if (AsteriskEngine.ModTarget_AsteriskVersion >= Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("CYFEngine", "OpenAppDataFile");
            CheckDirectory();
            return new LuaFile(ConvertToAppDataFullPath(path), mode, true);
        }
        public LuaFile OpenFile(string path, string mode = "rw") { return OpenAppDataFile(path, mode); }

        public bool AppDataFileExists(string path)
        {
            if (AsteriskEngine.ModTarget_AsteriskVersion >= Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("CYFEngine", "AppDataFileExists");
            CheckDirectory();
            if (path.Contains("..")) throw new CYFException(GetAppDataErrorMessage("MiscCheckFileOutside"));
            return File.Exists(ConvertToAppDataFullPath(path).Replace('\\', '/'));
        }
        public bool FileExists(string path) { return AppDataFileExists(path); }

        public bool AppDataDirExists(string path)
        {
            if (AsteriskEngine.ModTarget_AsteriskVersion >= Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("CYFEngine", "AppDataDirExists");
            CheckDirectory();
            if (path.Contains("..")) throw new CYFException(GetAppDataErrorMessage("MiscCheckDirOutside"));
            return Directory.Exists(ConvertToAppDataFullPath(path).Replace('\\', '/'));
        }
        public bool DirExists(string path) { return AppDataDirExists(path); }

        public bool CreateAppDataDir(string path)
        {
            if (AsteriskEngine.ModTarget_AsteriskVersion >= Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("CYFEngine", "CreateAppDataDir");
            if (path.Contains("..")) throw new CYFException(GetAppDataErrorMessage("MiscCreateDirOutside"));

            if (Directory.Exists(ConvertToAppDataFullPath(path).Replace('\\', '/'))) return false;
            Directory.CreateDirectory(ConvertToAppDataFullPath(path));
            return true;
        }
        public bool CreateDir(string path) { return CreateAppDataDir(path); }

        private static bool PathValid(string path) { return path != " " && path != "" && path != "/" && path != "\\" && path != "." && path != "./" && path != ".\\"; }

        public bool MoveAppDataDir(string path, string newPath)
        {
            if (AsteriskEngine.ModTarget_AsteriskVersion >= Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("CYFEngine", "MoveAppDataDir");
            if (path.Contains("..") || newPath.Contains("..")) throw new CYFException(GetAppDataErrorMessage("MiscMoveDirOutside"));

            if (!AppDataDirExists(path) || AppDataDirExists(newPath) || !PathValid(path)) return false;
            Directory.Move(ConvertToAppDataFullPath(path), ConvertToAppDataFullPath(newPath));
            return true;
        }
        public bool MoveDir(string path, string newPath) { return MoveAppDataDir(path, newPath); }

        public bool RemoveAppDataDir(string path, bool force = false)
        {
            if (AsteriskEngine.ModTarget_AsteriskVersion >= Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("CYFEngine", "RemoveAppDataDir");
            if (path.Contains("..")) throw new CYFException(GetAppDataErrorMessage("MiscRemoveDirOutside"));

            if (!Directory.Exists(ConvertToAppDataFullPath(path).Replace('\\', '/'))) return false;
            try { Directory.Delete(ConvertToAppDataFullPath(path), force); }
            catch { /* ignored */ }

            return false;
        }
        public bool RemoveDir(string path, bool force = false) { return RemoveAppDataDir(path, force); }

        public string[] ListAppDataDir(string path, bool getFolders = false)
        {
            if (AsteriskEngine.ModTarget_AsteriskVersion >= Asterisk.Versions.BeAddedShaderAndAppData) Asterisk.FakeNotFoundError("CYFEngine", "ListAppDataDir");
            if (path == null) throw new CYFException(EngineLang.Get("Exception", "MiscNullListDir"));
            if (path.Contains("..")) throw new CYFException(GetAppDataErrorMessage("MiscListDirOutside"));

            path = ConvertToAppDataFullPath(path).Replace('\\', '/');
            if (!Directory.Exists(path)) throw new CYFException("Invalid path:\n\n\"" + path + "\"");

            DirectoryInfo d = new DirectoryInfo(path);
            System.Collections.Generic.List<string> retval = new System.Collections.Generic.List<string>();
            retval.AddRange(!getFolders ? d.GetFiles().Select(fi => Path.GetFileName(fi.ToString()))
                                        : d.GetDirectories().Select(di => di.Name));
            return retval.ToArray();
        }
        public string[] ListDir(string path, bool getFolders = false) { return ListAppDataDir(path, getFolders); }



        /*
        public static void RegistSprite(string filename)
        {
            Sprite sprite = SpriteUtil.FromFile(FileLoader.pathToModFile(filename + ".png"));
            if (sprite == null)
                throw new CYFException("The sprite " + filename + ".png doesn't exist.");
            SpriteRegistry.Set(filename, sprite);
        }
        */

        public void SetSpriteFilterMode(string filename, string filtermode = "POINT")
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
