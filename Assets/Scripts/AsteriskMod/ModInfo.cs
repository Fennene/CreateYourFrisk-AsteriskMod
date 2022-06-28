using AsteriskMod.FakeIniLoader;
using System;
using System.IO;
using UnityEngine;

namespace AsteriskMod
{
    public enum DisplayFont
    {
        PixelOperator,
        EightBitoperator,
        JFDotShinonome14
    }

    public class ModInfo
    {
        public Asterisk.Versions targetVersion;
        public bool[] supportedLanguages;
        public string[] showEncounters;
        public string[] hideEncounters;
        public bool? retroMode;

        public string title;
        public string subtitle;
        public string description;
        public TextAnchor descAnchor;
        public bool richText;
        public DisplayFont font;
        public Color bgColor;
        public Color launchBGColor;

        public const string MODINFO_FILE_NAME = "info.cyfmod";

        public ModInfo()
        {
            targetVersion = Asterisk.Versions.Unknwon;
            supportedLanguages = new bool[0];
            showEncounters = new string[0];
            hideEncounters = new string[0];
            retroMode = null;

            title = "";
            subtitle = "";
            description = "";
            descAnchor = TextAnchor.UpperLeft;
            richText = false;
            font = DisplayFont.PixelOperator;
            bgColor = new Color32(255, 255, 255, 64);
            launchBGColor = new Color(1f, 1f, 1f, 0.1875f);
        }

        private static bool IgnoreFile(string fullpath)
        {
            return !File.Exists(fullpath) || new FileInfo(fullpath).Length > 1024 * 1024; // 1MB
        }

        private static bool? ConvertToNullableBoolean(string text)
        {
            text = text.ToLower();
            if (text == "true") return true;
            if (text == "false") return false;
            return null;
        }

        private static Color TryConvertToColor(Color origin, string[] array)
        {
            int len = array.Length;
            if (len != 3 && len != 4) return origin;
            float[] c = new float[4] { 1f, 1f, 1f, 1f };
            float value;
            for (var i = 0; i < len; i++)
            {
                if (!float.TryParse(array[i], out value)) return origin;
                if (value < 0.0f) value = 0.0f;
                if (value > 1.0f) value = 1.0f;
                c[i] = value;
            }
            Color color = new Color();
            color.r = c[0];
            color.g = c[1];
            color.b = c[2];
            if (len == 4) color.a = c[3];
            return color;
        }

        private static Color32 TryConvertToColor32(Color origin, string[] array)
        {
            int len = array.Length;
            if (len != 3 && len != 4) return origin;
            byte[] c = new byte[4] { 255, 255, 255, 255 };
            byte value;
            for (var i = 0; i < len; i++)
            {
                if (!byte.TryParse(array[i], out value)) return origin;
                c[i] = value;
            }
            Color32 color = new Color32();
            color.r = c[0];
            color.g = c[1];
            color.b = c[2];
            if (len == 4) color.a = c[3];
            return color;
        }

        public static ModInfo Get(string modDirName)
        {
            ModInfo info = new ModInfo();
            string path = Path.Combine(FileLoader.DataRoot, "Mods/" + modDirName + "/" + MODINFO_FILE_NAME);
            if (IgnoreFile(path)) return info;

            FakeIni ini = FakeIniFileLoader.Load(path, true);
            string descFilePath = "";
            foreach (string realKey in ini.Main.ParameterNames)
            {
                string keyName = realKey.ToLower().Replace("_", "-");
                if (realKey != keyName && ini.Main.ParameterExists(keyName)) continue;
                switch (keyName)
                {
                    case "target-version":
                    case "target":
                    case "asterisk-version":
                        info.targetVersion = Asterisk.ConvertToModVersion(ini.Main[realKey].String);
                        break;

                    case "supported-languages":
                    //case "supported-language":
                    case "languages":
                    //case "language":
                    case "localization":
                        info.supportedLanguages = new bool[2];
                        foreach (string langText in ini.Main[realKey].Array)
                        {
                            Languages lang = AsteriskUtil.ConvertToLanguage(langText, false);
                            if (lang == Languages.Unknown) continue;
                            if (lang == Languages.English) info.supportedLanguages[0] = true;
                            if (lang == Languages.Japanese) info.supportedLanguages[1] = true;
                        }
                        break;

                    case "show":
                    case "show-encounters":
                    //case "show-encounter":
                        if (info.hideEncounters.Length > 0) info.hideEncounters = new string[0];
                        info.showEncounters = ini.Main[realKey].Array;
                        break;

                    case "hide":
                    case "hide-encounters":
                    //case "hide-encounter":
                        if (info.showEncounters.Length > 0) continue; // Showキーの優先
                        info.hideEncounters = ini.Main[realKey].Array;
                        break;

                    case "retro-mode":
                    case "retro":
                        info.retroMode = ConvertToNullableBoolean(ini.Main[realKey].String);
                        break;


                    case "title":
                    case "mod-title":
                    //case "name":
                    case "mod-name":
                        info.title = ini.Main[realKey].String;
                        break;

                    case "subtitle":
                    case "sub-title":
                        info.subtitle = ini.Main[realKey].String;
                        break;
                    case "author":
                        info.subtitle = (ini.Main[realKey].String.StartsWith("by ") ? "" : "by ") + ini.Main[realKey].String;
                        break;

                    case "description":
                    case "desc":
                        info.description = ini.Main[realKey].String;
                        break;

                    case "description-file":
                    case "desc-file":
                    case "descfile":
                        descFilePath = ini.Main[realKey].String;
                        break;

                    case "description-align":
                    case "align":
                    case "description-anchor":
                    case "anchor":
                        try   { info.descAnchor = (TextAnchor)Enum.Parse(typeof(TextAnchor), ini.Main[realKey].String); }
                        catch { /* ignore */ }
                        break;

                    case "rich-text":
                        bool? _ = ConvertToNullableBoolean(ini.Main[realKey].String);
                        if (_.HasValue) info.richText = _.Value;
                        break;

                    case "font":
                        string fontName = ini.Main[realKey].String.ToLower().Replace("-", " ").Replace("_", " ");
                        if (fontName == "8bit" || fontName == "8bitoperator" || fontName == "8bit operator" || fontName == "8bitoperator jve" || fontName == "8bit operator jve")
                        {
                            info.font = DisplayFont.EightBitoperator;
                        }
                        break;


                    case "bg-color":
                    case "background-color":
                        info.bgColor = TryConvertToColor(info.bgColor, ini.Main[realKey].Array);
                        break;
                    case "bg-color32":
                    case "background-color32":
                        info.bgColor = TryConvertToColor32(info.bgColor, ini.Main[realKey].Array);
                        break;

                    case "bg-alpha":
                    case "background-alpha":
                        float alpha;
                        if (float.TryParse(ini.Main[realKey].String, out alpha))
                            info.bgColor = new Color(1f, 1f, 1f, alpha);
                        break;
                    case "bg-alpha32":
                    case "background-alpha32":
                        byte alpha32;
                        if (byte.TryParse(ini.Main[realKey].String, out alpha32))
                            info.bgColor = new Color32(255, 255, 255, alpha32);
                        break;
                }
            }
            if (descFilePath.IsNullOrWhiteSpace() || descFilePath.Contains("..")) return info;
            descFilePath = Path.Combine(FileLoader.DataRoot, "Mods/" + modDirName + "/" + descFilePath);
            if (IgnoreFile(descFilePath)) return info;
            try   { info.description = File.ReadAllText(descFilePath); }
            catch { /* ignore */ }
            return info;
        }

        public static bool Exists(string modDirName)
        {
            return File.Exists(Path.Combine(FileLoader.DataRoot, "Mods/" + modDirName + "/" + MODINFO_FILE_NAME));
        }

        public static bool CreateFile(string modDirName)
        {
            string path = Path.Combine(FileLoader.DataRoot, "Mods/" + modDirName + "/" + MODINFO_FILE_NAME);
            try { File.WriteAllText(path, "target-version=\"v0.5.3\""); }
            catch { return false; }
            return true;
        }

        public ModInfo Clone()
        {
            ModInfo clone = new ModInfo();

            clone.targetVersion = targetVersion;
            clone.supportedLanguages = supportedLanguages.Copy();
            clone.showEncounters = showEncounters.Copy();
            clone.hideEncounters = hideEncounters.Copy();
            clone.retroMode = retroMode;

            clone.title = title;
            clone.subtitle = subtitle;
            clone.description = description;
            clone.descAnchor = descAnchor;
            clone.richText = richText;
            clone.font = font;
            clone.bgColor = bgColor;
            clone.launchBGColor = launchBGColor;

            return clone;
        }

        public bool Write(string modDirName)
        {
            string text;
            text = "target-version=\"" + Asterisk.ConvertFromModVersion(targetVersion) + "\"\n";
            if (showEncounters.Length > 0)
            {
                text += "show-encounters={";
                foreach (string encounterName in showEncounters)
                {
                    if (!text.EndsWith("{")) text += ",";
                    text += "\"" + encounterName + "\"";
                }
                text += "}\n";
            }
            if (hideEncounters.Length > 0)
            {
                text += "hide-encounters={";
                foreach (string encounterName in hideEncounters)
                {
                    if (!text.EndsWith("{")) text += ",";
                    text += "\"" + encounterName + "\"";
                }
                text += "}\n";
            }
            if (supportedLanguages.Length > 0)
            {
                text += "supported-languages={";
                if (supportedLanguages[0]) text += "\"en\"";
                if (supportedLanguages[1]) text += (!text.EndsWith("{") ? "," : "") + "\"ja\"";
                text += "}\n";
            }
            if (!string.IsNullOrEmpty(title))    text += "title=\"" + title + "\"\n";
            if (!string.IsNullOrEmpty(subtitle)) text += "subtitle=\"" + subtitle + "\"\n";
            if (!string.IsNullOrEmpty(description)) text += "description-file=\"" + description + "\"\n";
            if (descAnchor != TextAnchor.UpperLeft) text += "description-align=\"" + descAnchor.ToString() + "\"\n";
            if (font != DisplayFont.PixelOperator)
            {
                if (font == DisplayFont.EightBitoperator) text += "font=\"8bitoperator\"\n";
            }
            if (bgColor.r != 1 || bgColor.g != 1 || bgColor.b != 1) text += "bg-color={\"" + bgColor.r.ToString() + "\",\"" + bgColor.g.ToString() + "\",\"" + bgColor.b.ToString() + "\"}\n";
            if (bgColor.a != 0.1875f) text += "bg-alpha=\"" + bgColor.a.ToString() + "\"\n";
            string path = Path.Combine(FileLoader.DataRoot, "Mods/" + modDirName + "/" + MODINFO_FILE_NAME);
            try   { File.WriteAllText(path, text); }
            catch { return false; }
            return true;
        }

        public static bool DeleteFile(string modDirName)
        {
            string path = Path.Combine(FileLoader.DataRoot, "Mods/" + modDirName + "/" + MODINFO_FILE_NAME);
            try   { File.Delete(path); }
            catch { return false; }
            return true;
        }
    }
}
