using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace AsteriskMod.ModdingHelperTools
{
    public class FakeSpriteFontRegistry
    {
        internal GameObject LETTER_OBJECT;
        internal GameObject BUBBLE_OBJECT;
        private readonly Dictionary<string, FileInfo> dictDefault = new Dictionary<string, FileInfo>();
        private readonly Dictionary<string, FileInfo> dictMod = new Dictionary<string, FileInfo>();

        private readonly Dictionary<string, UnderFont> dict = new Dictionary<string, UnderFont>();

        public void Start() { LoadAllFrom(SimInstance.FakeFileLoader.pathToDefaultFile("Sprites/UI/Fonts")); }

        public void Init()
        {
            dict.Clear();
            LETTER_OBJECT = Resources.Load<GameObject>("Prefabs/letter");
            BUBBLE_OBJECT = Resources.Load<GameObject>("Prefabs/DialogBubble");

            //string modPath = FakeFileLoader.pathToModFile("Sprites/UI/Fonts");
            //string defaultPath = FakeFileLoader.pathToDefaultFile("Sprites/UI/Fonts");
            //loadAllFrom(defaultPath);
            LoadAllFrom(SimInstance.FakeFileLoader.pathToModFile("Sprites/UI/Fonts"), true);
        }

        private void LoadAllFrom(string directoryPath, bool mod = false)
        {
            DirectoryInfo dInfo = new DirectoryInfo(directoryPath);

            if (!dInfo.Exists)
                return;

            FileInfo[] fInfo = dInfo.GetFiles("*.png", SearchOption.TopDirectoryOnly);

            if (mod)
            {
                dictMod.Clear();
                foreach (FileInfo file in fInfo)
                    dictMod[Path.GetFileNameWithoutExtension(file.FullName).ToLower()] = file;
            }
            else
            {
                dictDefault.Clear();
                foreach (FileInfo file in fInfo)
                    dictDefault[Path.GetFileNameWithoutExtension(file.FullName).ToLower()] = file;
            }
        }

        public UnderFont Get(string key)
        {
            string k = key;
            key = key.ToLower();
            return dict.ContainsKey(key) ? dict[key] : TryLoad(k);
        }

        public UnderFont TryLoad(string key)
        {
            string k = key;
            key = key.ToLower();
            if (dictMod.ContainsKey(key) || dictDefault.ContainsKey(key))
            {
                UnderFont underfont = GetUnderFont(k);
                //if (underfont != null)
                dict[key] = underfont;
            }
            else
                return null;
            return dict[key];
        }

        private UnderFont GetUnderFont(string fontName)
        {
            XmlDocument xml = new XmlDocument();
            string fontPath = SimInstance.FakeFileLoader.requireFile("Sprites/UI/Fonts/" + fontName + ".png");
            string xmlPath = SimInstance.FakeFileLoader.requireFile("Sprites/UI/Fonts/" + fontName + ".xml", false);
            if (xmlPath == null)
                return null;
            try { xml.Load(xmlPath); }
            catch (XmlException ex)
            {
                UnitaleUtil.DisplayLuaError("Instanciating a font", "An error was encountered while loading the font \"" + fontName + "\":\n\n" + ex.Message);
                return null;
            }
            if (xml["font"] == null)
            {
                UnitaleUtil.DisplayLuaError("Instanciating a font", "The font '" + fontName + "' doesn't have a font element at its root.");
                return null;
            }
            Dictionary<char, Sprite> fontMap = LoadBuiltInFont(xml["font"]["spritesheet"], fontPath);

            UnderFont underfont;
            try { underfont = new UnderFont(fontMap, fontName); }
            catch
            {
                UnitaleUtil.DisplayLuaError("Instanciating a font", "The fonts need a space character to compute their line height, and the font '" + fontName + "' doesn't have one.");
                return null;
            }

            if (xml["font"]["voice"] != null) underfont.Sound = AudioClipRegistry.GetVoice(xml["font"]["voice"].InnerText);
            if (xml["font"]["linespacing"] != null) underfont.LineSpacing = ParseUtil.GetFloat(xml["font"]["linespacing"].InnerText);
            if (xml["font"]["charspacing"] != null) underfont.CharSpacing = ParseUtil.GetFloat(xml["font"]["charspacing"].InnerText);
            if (xml["font"]["color"] != null) underfont.DefaultColor = ParseUtil.GetColor(xml["font"]["color"].InnerText);

            return underfont;
        }

        private Dictionary<char, Sprite> LoadBuiltInFont(XmlNode sheetNode, string fontPath)
        {
            Sprite[] letterSprites = SpriteUtil.AtlasFromXml(sheetNode, SimInstance.FakeSpriteRegistry.FromFile(fontPath));
            Dictionary<char, Sprite> letters = new Dictionary<char, Sprite>();
            foreach (Sprite s in letterSprites)
            {
                string name = s.name;
                if (name.Length == 1)
                    letters.Add(name[0], s);
                else
                    switch (name)
                    {
                        case "slash": letters.Add('/', s); break;
                        case "dot": letters.Add('.', s); break;
                        case "pipe": letters.Add('|', s); break;
                        case "backslash": letters.Add('\\', s); break;
                        case "colon": letters.Add(':', s); break;
                        case "questionmark": letters.Add('?', s); break;
                        case "doublequote": letters.Add('"', s); break;
                        case "asterisk": letters.Add('*', s); break;
                        case "space": letters.Add(' ', s); break;
                        case "lt": letters.Add('<', s); break;
                        case "rt": letters.Add('>', s); break;
                        case "ampersand": letters.Add('&', s); break;
                    }
            }
            return letters;
        }
    }
}
