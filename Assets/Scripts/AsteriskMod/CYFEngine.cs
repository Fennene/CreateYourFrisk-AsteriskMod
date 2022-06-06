using MoonSharp.Interpreter;
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

        // wow, I can use ../ to CreateSprite. I didn't know that.

        /*
        public static void RegistSprite(string filename)
        {
            //Asterisk.RequireExperimentalFeature("Engine.RegistSprite");
            Sprite sprite = SpriteUtil.FromFile(FileLoader.pathToModFile(filename + ".png"));
            if (sprite == null)
                throw new CYFException("The sprite " + filename + ".png doesn't exist.");
            SpriteRegistry.Set("*" + filename, sprite);
        }
        */

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
