//* using MoonSharp.Interpreter;
//* using System;
using System.Collections.Generic;
using System.IO;
//* using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
//* using Object = UnityEngine.Object;

namespace AsteriskMod.ModdingHelperTools
{
    public static class FakeSpriteRegistry
    {
        private static readonly Dictionary<string, Sprite> dict = new Dictionary<string, Sprite>();
        public static Image GENERIC_SPRITE_PREFAB;
        public static Sprite EMPTY_SPRITE;
        private static readonly Dictionary<string, FileInfo> dictDefault = new Dictionary<string, FileInfo>();
        private static readonly Dictionary<string, FileInfo> dictMod = new Dictionary<string, FileInfo>();

        public static void Start() { loadAllFrom(FakeFileLoader.pathToDefaultFile("Sprites")); }

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

        public static void Set(string key, Sprite value) { dict["sim" + key.ToLower()] = value; }

        public static void SwapSpriteFromFile(Component target, string filename, int bubbleID = -1)
        {
            /**
            try
            {
                if (bubbleID != -1)
                {
                    FileInfo fi = new FileInfo(Path.ChangeExtension(FakeFileLoader.pathToModFile("Sprites/" + filename + ".png"), "xml"));
                    if (!fi.Exists)
                        fi = new FileInfo(Path.ChangeExtension(FakeFileLoader.pathToDefaultFile("Sprites/" + filename + ".png"), "xml"));
                    if (fi.Exists)
                    {
                        XmlDocument xmld = new XmlDocument();
                        xmld.Load(fi.FullName);
                        if (xmld["spritesheet"] != null && "single".Equals(xmld["spritesheet"].GetAttribute("type")))
                            if (!UnitaleUtil.IsOverworld)
                                UIController.instance.encounter.EnabledEnemies[bubbleID].bubbleWidth = ParseUtil.GetFloat(xmld["spritesheet"].GetElementsByTagName("width").Count > 0
                                    ? xmld["spritesheet"].GetElementsByTagName("width")[0].InnerText
                                    : xmld["spritesheet"].GetElementsByTagName("wideness")[0].InnerText);
                    }
                    else
                        UIController.instance.encounter.EnabledEnemies[bubbleID].bubbleWidth = 0;
                }
            }
            catch (Exception)
            {
                UIController.instance.encounter.EnabledEnemies[bubbleID].bubbleWidth = 0;
            }
            */
            Sprite newSprite = FakeSpriteRegistry.Get(filename);
            if (newSprite == null)
            {
                if (filename.Length == 0)
                {
                    Debug.LogError("SwapSprite: Filename is empty!");
                    return;
                }
                newSprite = FromFile(FakeFileLoader.pathToModFile("Sprites/" + filename + ".png"));
                if (newSprite == null)
                    throw new CYFException("The sprite Sprites/" + filename + ".png doesn't exist.");
                FakeSpriteRegistry.Set(filename, newSprite);
            }

            Image img = target.GetComponent<Image>();
            if (!img)
            {
                SpriteRenderer img2 = target.GetComponent<SpriteRenderer>();
                Vector2 pivot = img2.GetComponent<RectTransform>().pivot;
                img2.sprite = newSprite;
                img2.GetComponent<RectTransform>().sizeDelta = new Vector2(newSprite.texture.width, newSprite.texture.height);
                img2.GetComponent<RectTransform>().pivot = pivot;
            }
            else
            {
                Vector2 pivot = img.rectTransform.pivot;
                img.sprite = newSprite;
                //enemyImg.SetNativeSize();
                img.rectTransform.sizeDelta = new Vector2(newSprite.texture.width, newSprite.texture.height);
                img.rectTransform.pivot = pivot;
            }
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
