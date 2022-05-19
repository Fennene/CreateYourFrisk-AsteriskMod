using System.IO;
using UnityEngine;

namespace AsteriskMod
{
    public class CodeStyle
    {
        public string buttonUtilName;
        public string playerUtilName;
        public string arenaUtilName;
        public bool moreUtil;

        public CodeStyle()
        {
            buttonUtilName = "ButtonUtil";
            playerUtilName = "PlayerUtil";
            arenaUtilName = "ArenaUtil";
            moreUtil = false;
        }

        private static bool IgnoreFile(string fullpath)
        {
            return !File.Exists(fullpath) || new FileInfo(fullpath).Length > 1024 * 1024; // 1MB
        }

        internal static CodeStyle Load()
        {
            CodeStyle codeStyle = new CodeStyle();
            string path = Path.Combine(FileLoader.ModDataPath, "Lua/codestyle.cyfmod");
            if (IgnoreFile(path)) return codeStyle;
            Debug.Log("CodeStyle is found.");
            try
            {
                foreach (string l in File.ReadAllLines(path))
                {
                    string line = l.Replace("\r", "").Replace("\n", "");
                    if (line.StartsWith(";")) continue;
                    if (!line.Contains("=")) continue;
                    string[] _ = line.Split(new char[1] { '=' }, 2);
                    string key = _[0].Replace("_", "-").Trim();
                    string parameter = _[1].Replace("\"", "").Trim();
                    switch (key)
                    {
                        case "ButtonUtil":
                            codeStyle.buttonUtilName = parameter;
                            break;
                        case "PlayerUtil":
                            codeStyle.playerUtilName = parameter;
                            break;
                        case "ArenaUtil":
                            codeStyle.arenaUtilName = parameter;
                            break;
                        case "MoreUtil":
                            if (parameter.ToLower() == "true")
                                codeStyle.moreUtil = true;
                            break;
                    }
                }
            }
            catch { /* ignore */ }
            return codeStyle;
        }
    }
}
