using System.IO;
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

        public static void SetTargetFrameRate(int frameRate = 60)
        {
            Asterisk.RequireExperimentalFeature("Engine.SetTargetFrameRate");
            if (frameRate <= 0) throw new CYFException("Engine.SetTargetFrameRate: frame rate should be positive.");
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
    }
}
