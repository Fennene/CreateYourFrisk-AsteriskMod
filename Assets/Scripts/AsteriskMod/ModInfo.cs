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
        public string[] environmentPathes;

        public string title;
        public string subtitle;
        public string description;
        public TextAnchor descAnchor;
        public DisplayFont font;
        public Color bgColor;

        public const string MODINFO_FILE_NAME = "info.cyfmod";

        public ModInfo()
        {
            targetVersion = Asterisk.Versions.Unknwon;
            supportedLanguages = new bool[2] { false, false };
            showEncounters = new string[0];
            hideEncounters = new string[0];
            retroMode = null;
            environmentPathes = new string[0];

            title = "";
            subtitle = "";
            description = "";
            descAnchor = TextAnchor.UpperLeft;
            font = DisplayFont.PixelOperator;
            bgColor = new Color32(255, 255, 255, 64);
        }

        public static ModInfo Get(string modDirName)
        {
            return Load(modDirName, MODINFO_FILE_NAME);
        }

        private static bool IgnoreFile(string fullpath)
        {
            return !File.Exists(fullpath) || new FileInfo(fullpath).Length > 1024 * 1024; // 1MB
        }

        private static string[] ConvertToArray(string text)
        {
            if (!text.StartsWith("{")) return new string[0];
            if (!text.EndsWith("}"))   return new string[0];
            string[] ret = text.TrimStart('{').TrimEnd('}').Split(',');
            for (var i = 0; i < ret.Length; i++)
            {
                ret[i] = ret[i].Trim().Replace("\"", "");
            }
            return ret;
        }

        private static bool? ConvertToNullableBoolean(string text)
        {
            text = text.ToLower();
            if (text == "true") return true;
            if (text == "false") return false;
            return null;
        }

        private static ModInfo Load(string modDirName, string filePath)
        {
            ModInfo info = new ModInfo();
            string path = Path.Combine(FileLoader.DataRoot, "Mods/" + modDirName + "/" + filePath);
            if (IgnoreFile(path)) return info;
            string descriptionFilePath = "";
            try
            {
                foreach (string l in File.ReadAllLines(path))
                {
                    string line = l.Replace("\r", "").Replace("\n", "");
                    if (line.StartsWith(";")) continue;
                    if (!line.Contains("=")) continue; // throw new Exception("Illegal info.cyfmod");
                    string[] _ = line.Split(new char[1] { '=' }, 2);
                    string key = _[0].ToLower().Replace("_", "-").Trim();
                    string parameter = _[1].Replace("\"", "").Trim();
                    switch (key)
                    {
                        case "target-version":
                        case "target":
                            info.targetVersion = Asterisk.ConvertToModVersion(parameter);
                            break;
                        case "supported-languages":
                        //case "supported-language":
                        case "languages":
                        //case "language":
                        case "localization":
                            foreach (string langText in ConvertToArray(parameter))
                            {
                                Languages lang = AsteriskUtil.ConvertToLanguage(langText, false);
                                if (lang == Languages.Unknown) continue;
                                if (lang == Languages.English)  info.supportedLanguages[0] = true;
                                if (lang == Languages.Japanese) info.supportedLanguages[1] = true;
                            }
                            break;
                        case "show":
                        case "show-encounters":
                            //case "show-encounter":
                            if (info.hideEncounters.Length > 0) info.hideEncounters = new string[0];
                            info.showEncounters = ConvertToArray(parameter);
                            break;
                        case "hide":
                        case "hide-encounters":
                        //case "hide-encounter":
                            if (info.showEncounters.Length > 0) continue; // Showキーの優先
                            info.hideEncounters = ConvertToArray(parameter);
                            break;
                        case "retro-mode":
                        case "retro":
                            info.retroMode = ConvertToNullableBoolean(parameter);
                            break;
                        case "env-path":
                        case "env-pathes":
                        //case "environment-path":
                        case "environment-pathes":
                        case "lib-path":
                        case "lib-pathes":
                        //case "library-path":
                        case "library-pathes":
                            info.environmentPathes = ConvertToArray(parameter);
                            break;

                        case "title":
                        case "name":
                        case "mod-name":
                            info.title = parameter;
                            break;
                        case "subtitle":
                        case "sub-title":
                            info.subtitle = parameter;
                            break;
                        case "author":
                            info.subtitle = (parameter.StartsWith("by ") ? "" : "by ") + parameter;
                            break;
                        case "description":
                        case "desc":
                            info.description = parameter;
                            break;
                        case "description-file":
                        case "desc-file":
                        case "descfile":
                            descriptionFilePath = parameter;
                            break;
                        case "description-align":
                        case "align":
                        case "description-anchor":
                        case "anchor":
                            try
                            {
                                info.descAnchor = (TextAnchor)Enum.Parse(typeof(TextAnchor), parameter);
                            }
                            catch { /* ignore */ }
                            break;
                        case "font":
                            string fontName = parameter.ToLower().Replace("-", " ").Replace("_", " ");
                            if (fontName == "8bit" || fontName == "8bitoperator" || fontName == "8bit operator" || fontName == "8bitoperator jve" || fontName == "8bit operator jve")
                            {
                                info.font = DisplayFont.EightBitoperator;
                            }
                            break;
                        case "bg-alpha":
                        case "background-alpha":
                        case "alpha":
                            float alpha;
                            if (float.TryParse(parameter, out alpha))
                                info.bgColor = new Color(1f, 1f, 1f, alpha);
                            break;
                        case "bg-alpha32":
                        case "background-alpha32":
                        case "alpha32":
                            byte alpha32;
                            if (byte.TryParse(parameter, out alpha32))
                                info.bgColor = new Color32(255, 255, 255, alpha32);
                            break;
                    }
                }
            }
            catch { /* ignore */ }
            if (descriptionFilePath == "" || descriptionFilePath.Contains("..")) return info;
            descriptionFilePath = Path.Combine(FileLoader.DataRoot, "Mods/" + modDirName + "/" + descriptionFilePath);
            if (IgnoreFile(descriptionFilePath)) return info;
            try
            {
                info.description = File.ReadAllText(descriptionFilePath);
            }
            catch { /* ignore */ }
            return info;
        }
    }
}
