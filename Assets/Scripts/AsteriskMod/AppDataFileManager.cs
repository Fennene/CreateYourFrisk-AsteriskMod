using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AsteriskMod
{
    [ShouldAddToDocument]
    public class AppDataFileManager
    {
        private static string _appDataDirPath;
        private static bool _dirCreated;

        internal static void Initialize()
        {
            _appDataDirPath = Path.Combine(Application.persistentDataPath, "Mods/" + StaticInits.MODFOLDER).Replace('\\', '/');
            _dirCreated = false;
        }

        private static string AppDataDirPath { get { return _appDataDirPath; } }

        internal static string GetFullPath(string path) { return (AppDataDirPath + "/" + path).Replace('\\', '/'); }

        private static void CheckDirectory()
        {
            if (_dirCreated) return;
            if (Directory.Exists(AppDataDirPath))
            {
                _dirCreated = true;
                return;
            }
            try
            {
                Directory.CreateDirectory(AppDataDirPath);
            }
            /*
            catch (IOException ex)
            {
            }
            */
            catch (Exception ex)
            {
                throw new CYFException("An error has occurred while creating AppData's directory.\n\nException: " + ex.GetType() + "\nerror: " + ex.Message);
            }
        }

        private string GetAppDataErrorMessage(string keyName) { return EngineLang.Get("Exception", keyName).Replace("mod", "AppData"); }

        public LuaFile OpenFile(string path, string mode = "rw")
        {
            CheckDirectory();
            return new LuaFile(GetFullPath(path), mode, true);
        }

        public bool FileExists(string path)
        {
            CheckDirectory();
            if (path.Contains("..")) throw new CYFException(GetAppDataErrorMessage("MiscCheckFileOutside"));
            return File.Exists(GetFullPath(path));
        }

        public bool DirExists(string path)
        {
            CheckDirectory();
            if (path.Contains("..")) throw new CYFException(GetAppDataErrorMessage("MiscCheckDirOutside"));
            return Directory.Exists(GetFullPath(path));
        }

        public bool CreateDir(string path)
        {
            if (path.Contains("..")) throw new CYFException(GetAppDataErrorMessage("MiscCreateDirOutside"));

            if (Directory.Exists(GetFullPath(path))) return false;
            Directory.CreateDirectory(GetFullPath(path));
            return true;
        }

        private static bool PathValid(string path) { return path != " " && path != "" && path != "/" && path != "\\" && path != "." && path != "./" && path != ".\\"; }

        public bool MoveDir(string path, string newPath)
        {
            if (path.Contains("..") || newPath.Contains("..")) throw new CYFException(GetAppDataErrorMessage("MiscMoveDirOutside"));

            if (!DirExists(path) || DirExists(newPath) || !PathValid(path)) return false;
            Directory.Move(GetFullPath(path), GetFullPath(newPath));
            return true;
        }

        public bool RemoveDir(string path, bool force = false)
        {
            if (path.Contains("..")) throw new CYFException(GetAppDataErrorMessage("MiscRemoveDirOutside"));

            if (!Directory.Exists(GetFullPath(path))) return false;
            try { Directory.Delete(GetFullPath(path), force); }
            catch { /* ignored */ }

            return false;
        }

        public string[] ListDir(string path, bool getFolders = false)
        {
            if (path == null) throw new CYFException(EngineLang.Get("Exception", "MiscNullListDir"));
            if (path.Contains("..")) throw new CYFException(GetAppDataErrorMessage("MiscListDirOutside"));

            path = GetFullPath(path);
            if (!Directory.Exists(path)) throw new CYFException("Invalid path:\n\n\"" + path + "\"");

            DirectoryInfo d = new DirectoryInfo(path);
            System.Collections.Generic.List<string> retval = new System.Collections.Generic.List<string>();
            retval.AddRange(!getFolders ? d.GetFiles().Select(fi => Path.GetFileName(fi.ToString()))
                                        : d.GetDirectories().Select(di => di.Name));
            return retval.ToArray();
        }

        [ShouldAddToDocument]
        public bool RemoveAll()
        {
            _dirCreated = false;
            try   { Directory.Delete(_appDataDirPath, true); }
            catch { return false; }
            return true;
        }

        [ShouldAddToDocument]
        public UTini OpenIniFile(string path)
        {
            CheckDirectory();
            return new UTini(GetFullPath(path));
        }
        public UTini OpenFileAsIni(string path) { return OpenIniFile(path); }
    }
}
