using AsteriskMod.FakeIniLoader;
using System;
using System.IO;
using UnityEngine;

namespace AsteriskMod
{
    public class ModInfo
    {
        public const string MODINFO_FILE_NAME = "info.cyfmod";

        public bool HasFile { get; private set; }
        public Asterisk.Versions TargetVersion { get; private set; }
        public string[] EncounterNames { get; private set; }
        public string[] ShowEncounters { get; private set; }
        public string[] HideEncounters { get; private set; }
        public Color BackgroundColor { get; private set; }
        public Color LaunchingBackgroundColor { get; private set; }
        public Font ScreenFont { get; private set; }
        public string TitleOverride { get; private set; }
        public string SubtitleOverride { get; private set; }
        [ToDo("will implemente")] internal bool RichText { get; private set; }
        public string Description { get; private set; }
        public TextAnchor DescriptionAlign { get; private set; }
        [ShouldAddToDocument] public Color EncounterBox { get; private set; }
        [ShouldAddToDocument] public Color[] EncounterButtons { get; private set; }
        [ShouldAddToDocument] public Color[] EncounterButtonsBorders { get; private set; }
        public bool[] SupportedLanguagesOverride { get; private set; }
        [ToDo("will implemente")] internal bool RetroMode { get; private set; }

        public ModInfo()
        {
            HasFile = false;
            TargetVersion = Asterisk.Versions.Unknwon;
            EncounterNames = null;
            ShowEncounters = null;
            HideEncounters = null;
            BackgroundColor = new Color(1f, 1f, 1f, 0.25f);
            LaunchingBackgroundColor = new Color(1f, 1f, 1f, 0.1875f);
            ScreenFont = AsteriskResources.PixelOperator;
            TitleOverride = null;
            SubtitleOverride = null;
            Description = null;
            DescriptionAlign = TextAnchor.UpperLeft;
            EncounterBox = new Color32(32, 32, 32, 128);
            EncounterButtons = null;
            EncounterButtonsBorders = null;
            SupportedLanguagesOverride = null;
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

        public static ModInfo GetFromFile(string path, bool isFullPath = false)
        {
            ModInfo info = new ModInfo();
            string fullPath = isFullPath ? path : Path.Combine(FileLoader.DataRoot, "Mods/" + path + "/" + MODINFO_FILE_NAME);
            if (AsteriskUtil.IsIgnoreFile(fullPath)) return info;

            info.HasFile = true;
            FakeIni ini = FakeIniFileLoader.Load(fullPath, true);
            string descFilePath = null;
            foreach (string realKey in ini.Main.ParameterNames)
            {
                string keyName = realKey.ToLower().Replace('_', '-');
                if (realKey != keyName && ini.Main.ParameterExists(keyName)) continue;
                switch (keyName)
                {
                    case "target-version":
                    case "target":
                    case "asterisk-version":
                        info.TargetVersion = Asterisk.ConvertToModVersion(ini.Main[realKey].String);
                        break;

                    case "encounter-names":
                    case "encounters-names":
                        info.EncounterNames = ini.Main[realKey].Array;
                        break;

                    case "show-encounters":
                    case "show":
                        info.ShowEncounters = ini.Main[realKey].Array;
                        break;

                    case "hide-encounters":
                    case "hide":
                        info.HideEncounters = ini.Main[realKey].Array;
                        break;

                    case "bg-color":
                    case "background-color":
                        info.BackgroundColor = TryConvertToColor(info.BackgroundColor, ini.Main[realKey].Array);
                        break;
                    case "bg-color32":
                    case "background-color32":
                        info.BackgroundColor = TryConvertToColor32(info.BackgroundColor, ini.Main[realKey].Array);
                        break;

                    case "bg-alpha":
                    case "background-alpha":
                        float alpha;
                        if (float.TryParse(ini.Main[realKey].String, out alpha))
                        {
                            info.BackgroundColor = new Color(info.BackgroundColor.r, info.BackgroundColor.g, info.BackgroundColor.b, alpha);
                        }
                        break;
                    case "bg-alpha32":
                    case "background-alpha32":
                        byte alpha32;
                        if (byte.TryParse(ini.Main[realKey].String, out alpha32))
                        {
                            Color32 color32 = (Color32)info.BackgroundColor;
                            info.BackgroundColor = new Color32(color32.r, color32.g, color32.b, alpha32);
                        }
                        break;

                    case "launch-bg-color":
                    case "launch-background-color":
                        info.LaunchingBackgroundColor = TryConvertToColor(info.LaunchingBackgroundColor, ini.Main[realKey].Array);
                        break;
                    case "launch-bg-color32":
                    case "launch-background-color32":
                        info.LaunchingBackgroundColor = TryConvertToColor32(info.LaunchingBackgroundColor, ini.Main[realKey].Array);
                        break;

                    case "launch-bg-alpha":
                    case "launch-background-alpha":
                        float alpha_;
                        if (float.TryParse(ini.Main[realKey].String, out alpha_))
                        {
                            info.LaunchingBackgroundColor = new Color(info.LaunchingBackgroundColor.r, info.LaunchingBackgroundColor.g, info.LaunchingBackgroundColor.b, alpha_);
                            info.LaunchingBackgroundColor = new Color(info.LaunchingBackgroundColor.r, info.LaunchingBackgroundColor.g, info.LaunchingBackgroundColor.b, alpha_);
                        }
                        break;
                    case "launch-bg-alpha32":
                    case "launch-background-alpha32":
                        byte alpha32_;
                        if (byte.TryParse(ini.Main[realKey].String, out alpha32_))
                        {
                            Color32 color32 = (Color32)info.LaunchingBackgroundColor;
                            info.LaunchingBackgroundColor = new Color32(color32.r, color32.g, color32.b, alpha32_);
                        }
                        break;

                    case "font":
                        string fontName = ini.Main[realKey].String.ToLower().Replace("-", " ").Replace("_", " ");
                        if (fontName == "8bit" || fontName == "8bitoperator" || fontName == "8bit operator" || fontName == "8bitoperator jve" || fontName == "8bit operator jve")
                        {
                            info.ScreenFont = AsteriskResources.EightBitoperator;
                        }
                        break;

                    case "title":
                    case "mod-title":
                    case "mod-name":
                        info.TitleOverride = ini.Main[realKey].String;
                        break;

                    case "subtitle":
                    case "sub-title":
                        info.SubtitleOverride = ini.Main[realKey].String;
                        break;
                    case "author":
                        info.SubtitleOverride = (ini.Main[realKey].String.StartsWith("by ") ? "" : "by ") + ini.Main[realKey].String;
                        break;

                    case "description":
                    case "desc":
                        info.Description = ini.Main[realKey].String;
                        break;
                    case "description-file":
                    case "desc-file":
                    case "descfile":
                        descFilePath = ini.Main[realKey].String;
                        break;

                    case "description-align":
                    case "desc-align":
                    case "align":
                    case "description-anchor":
                    case "anchor":
                        try   { info.DescriptionAlign = (TextAnchor)Enum.Parse(typeof(TextAnchor), ini.Main[realKey].String); }
                        catch { /* ignore */ }
                        break;

                    /*
                    case "encounter-box-bg-color":
                    case "encounter-box-bg":
                        info.EncounterBox = TryConvertToColor(info.EncounterBox, ini.Main[realKey].Array);
                        break;
                    case "encounter-box-bg-color32":
                        info.EncounterBox = TryConvertToColor32(info.EncounterBox, ini.Main[realKey].Array);
                        break;

                    case "encounter-box-button-color":
                        break;
                    case "encounter-box-button-color32":
                        break;
                    */

                    case "supported-languages":
                    case "languages":
                    case "localization":
                        info.SupportedLanguagesOverride = new bool[2];
                        foreach (string langText in ini.Main[realKey].Array)
                        {
                            Languages lang = AsteriskUtil.ConvertToLanguage(langText, false);
                            if (lang == Languages.Unknown)  continue;
                            if (lang == Languages.English)  info.SupportedLanguagesOverride[0] = true;
                            if (lang == Languages.Japanese) info.SupportedLanguagesOverride[1] = true;
                        }
                        break;
                }
            }
            if (descFilePath.IsNullOrWhiteSpace() || descFilePath.Contains("..") || isFullPath) return info;
            descFilePath = Path.Combine(FileLoader.DataRoot, "Mods/" + path + "/" + descFilePath);
            if (AsteriskUtil.IsIgnoreFile(descFilePath)) return info;
            try   { info.Description = File.ReadAllText(descFilePath); }
            catch { /* ignore */ }
            return info;
        }
    }
}
