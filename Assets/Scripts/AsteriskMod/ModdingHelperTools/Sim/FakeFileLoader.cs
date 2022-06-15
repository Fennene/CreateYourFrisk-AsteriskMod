using System;
using System.IO;

namespace AsteriskMod.ModdingHelperTools
{
    internal class FakeFileLoader
    {
        public static string DataRoot { get { return FileLoader.DataRoot; } }
        public static string ModDataPath { get { return Path.Combine(DataRoot, "Mods/" + FakeStaticInits.MODFOLDER); } }
        public static string DefaultDataPath { get { return FileLoader.DefaultDataPath; } }

        public static byte[] getBytesFrom(string filename) { return File.ReadAllBytes(requireFile(filename)); }

        public static string pathToModFile(string filename) { return Path.Combine(ModDataPath, filename); }
        public static string pathToDefaultFile(string filename) { return Path.Combine(DefaultDataPath, filename); }

        public static string requireFile(string filename, bool errorOnFailure = true)
        {
            FileInfo fi = new FileInfo(pathToModFile(filename));
            if (!fi.Exists)
                fi = new FileInfo(pathToDefaultFile(filename));

            if (fi.Exists) return fi.FullName;
            if (!errorOnFailure) return null;
            if (filename.Length != 0)
                throw new CYFException("Attempted to load " + filename + " from either a mod or default directory, but it was missing in both.");
            return null;
        }

        public static string getRelativePathWithoutExtension(string fullPath)
        {
            fullPath = fullPath.Replace('\\', '/');
            if (fullPath.Contains(ModDataPath.Replace('\\', '/')))
                fullPath = fullPath.Substring(ModDataPath.Length + 9, fullPath.Length - ModDataPath.Length - 13);
            if (fullPath.Contains(DefaultDataPath.Replace('\\', '/')))
                fullPath = fullPath.Substring(DefaultDataPath.Length + 9, fullPath.Length - DefaultDataPath.Length - 13);
            return fullPath;
        }

        public static string getRelativePathWithoutExtension(string rootPath, string fullPath) { return FileLoader.getRelativePathWithoutExtension(rootPath, fullPath); }
    }
}
