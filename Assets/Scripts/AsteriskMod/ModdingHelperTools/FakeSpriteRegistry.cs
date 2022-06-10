using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace AsteriskMod.ModdingHelperTools
{
    public static class FakeSpriteRegistry
    {
        private static readonly Dictionary<string, Sprite> dict = new Dictionary<string, Sprite>();
        public static Image GENERIC_SPRITE_PREFAB;
        public static Sprite EMPTY_SPRITE;
        private static readonly Dictionary<string, FileInfo> dictDefault = new Dictionary<string, FileInfo>();
        private static readonly Dictionary<string, FileInfo> dictMod = new Dictionary<string, FileInfo>();

        public static void Init()
        {
            //dict.Clear();
            GENERIC_SPRITE_PREFAB = Resources.Load<Image>("Prefabs/generic_sprite");
            Texture2D tex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            tex.SetPixel(0, 0, new Color(0, 0, 0, 0));
            tex.Apply();
            EMPTY_SPRITE = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
            EMPTY_SPRITE.name = "blank";
            string modPath = FakeFileLoader.pathToModFile("Sprites");
            //string defaultPath = FakeFileLoader.pathToDefaultFile("Sprites");
            //loadAllFrom(defaultPath);
            prepareMod(modPath);
        }

        private static void prepareMod(string directoryPath)
        {
            dict.Clear();

            loadAllFrom(directoryPath, true);
        }

        private static void loadAllFrom(string directoryPath, bool mod = false)
        {
            DirectoryInfo dInfo = new DirectoryInfo(directoryPath);

            if (!dInfo.Exists)
            {
                UnitaleUtil.DisplayLuaError("mod loading", "You tried to load the mod \"" + FakeStaticInits.MODFOLDER + "\" but it can't be found, or at least its \"Sprites\" folder can't be found.\nAre you sure it exists?");
                throw new CYFException("mod loading");
            }

            FileInfo[] fInfoTest = dInfo.GetFiles("*.png", SearchOption.AllDirectories);

            if (mod)
            {
                dictMod.Clear();
                foreach (FileInfo file in fInfoTest)
                    dictMod[FakeFileLoader.getRelativePathWithoutExtension(directoryPath, file.FullName).ToLower()] = file;
            }
            else
            {
                dictDefault.Clear();
                foreach (FileInfo file in fInfoTest)
                    dictDefault[FakeFileLoader.getRelativePathWithoutExtension(directoryPath, file.FullName).ToLower()] = file;
            }
        }

        public static Sprite Get(string key)
        {
            key = key.ToLower();
            string dictKey = "sim" + key;
            return dict.ContainsKey(dictKey) ? dict[dictKey] : tryLoad(key);
        }

        private static Sprite tryLoad(string key)
        {
            string dictKey = "sim" + key;
            if (dictMod.ContainsKey(key))          dict[dictKey] = FromFile(dictMod[key].FullName);
            else if (dictDefault.ContainsKey(key)) dict[dictKey] = FromFile(dictDefault[key].FullName);
            else                                   return null;
            return dict[dictKey];
        }

        public static Sprite FromFile(string filename)
        {
            Texture2D SpriteTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            SpriteTexture.LoadImage(FakeFileLoader.getBytesFrom(filename));
            SpriteTexture.filterMode = FilterMode.Point;
            SpriteTexture.wrapMode = TextureWrapMode.Clamp;

            Sprite newSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0.5f, 0.5f), SpriteUtil.PIXELS_PER_UNIT);
            filename = filename.Contains("File at ") ? filename.Substring(8) : filename;
            newSprite.name = FileLoader.getRelativePathWithoutExtension(filename);
            //optional XML loading
            FileInfo fi = new FileInfo(Path.ChangeExtension(filename, "xml"));
            if (!fi.Exists) return newSprite;
            XmlDocument xmld = new XmlDocument();
            xmld.Load(fi.FullName);
            if (xmld["spritesheet"] != null && "single".Equals(xmld["spritesheet"].GetAttribute("type")))
                return SpriteUtil.SpriteWithXml(xmld["spritesheet"].GetElementsByTagName("sprite")[0], newSprite);
            return newSprite;
        }
    }
}
