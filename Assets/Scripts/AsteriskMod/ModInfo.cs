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
        public string title;
        public string subtitle;
        public string description;
        public TextAnchor descAnchor;
        public DisplayFont font;
        public Color bgColor;
        public Asterisk.Versions targetVersion;

        public ModInfo()
        {
            title = "";
            subtitle = "";
            description = "";
            descAnchor = TextAnchor.UpperLeft;
            font = DisplayFont.PixelOperator;
            bgColor = new Color32(255, 255, 255, 64);
            targetVersion = Asterisk.Versions.Unknwon;
        }

        private static bool IgnoreFile(string fullpath)
        {
            return !File.Exists(fullpath) || new FileInfo(fullpath).Length > 1024 * 1024; // 1MB
        }

        public static ModInfo LoadFile(string modName, string path)
        {
            ModInfo info = new ModInfo();
            path = Path.Combine(FileLoader.DataRoot, "Mods/" + modName + "/" + path);
            if (IgnoreFile(path)) return info;
            string descFilePath = "";
            try
            {
                foreach (string l in File.ReadAllLines(path))
                {
                    string line = l.Replace("\r", "").Replace("\n", "");
                    if (line.StartsWith(";")) continue;
                    if (!line.Contains("=")) continue;
                    string[] _ = line.Split(new char[1] { '=' }, 2);
                    string key = _[0].ToLower().Replace("_", "-").Trim();
                    string parameter = _[1].Replace("\"", "").Trim();
                    switch (key)
                    {
                        case "title":
                        case "name":
                            info.title = parameter;
                            break;
                        case "subtitle":
                        case "sub-title":
                            info.subtitle = parameter;
                            break;
                        case "description":
                        case "desc":
                            info.description = parameter;
                            break;
                        case "description-file":
                        case "desc-file":
                        case "descfile":
                            descFilePath = parameter;
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
                        case "target-version":
                        case "target":
                            info.targetVersion = Asterisk.ConvertToModVersion(parameter);
                            break;
                    }
                }
            }
            catch { /* ignore */ }
            if (descFilePath == "" || descFilePath.Contains("..")) return info;
            descFilePath = Path.Combine(FileLoader.DataRoot, "Mods/" + modName + "/" + descFilePath);
            if (IgnoreFile(descFilePath)) return info;
            try
            {
                info.description = File.ReadAllText(descFilePath);
            }
            catch { /* ignore */ }
            return info;
        }
    }
}
