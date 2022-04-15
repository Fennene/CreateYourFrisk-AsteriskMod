using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.Lua
{
    public class Engine
    {
        public static void SetTargetFrameRate(int frameRate = 60)
        {
            Asterisk.RequireExperimentalFeature("Engine.SetTargetFrameRate");
            if (frameRate > 0)
                Application.targetFrameRate = frameRate;
        }

        public static bool ModExists(string modFolderName)
        {
            if (!Asterisk.RequireExperimentalFeature("Engine.ModExists")) return false;
            return Directory.Exists(Path.Combine(FileLoader.DataRoot, "Mods/" + modFolderName));
        }

        public static void SetBulletPoolSize(int newSize = 100)
        {
            Asterisk.RequireExperimentalFeature("Engine.SetBulletPoolSize");
            if (newSize > 0)
                BulletPool.POOLSIZE = newSize;
        }
    }
}
