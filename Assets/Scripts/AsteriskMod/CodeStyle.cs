using System.IO;
using UnityEngine;

namespace AsteriskMod
{
    public class CodeStyle
    {
        public bool useGeneralUtil;
        public string buttonUtilName;
        public string playerUtilName;
        public string arenaUtilName;

        public CodeStyle()
        {
            useGeneralUtil = false;
            buttonUtilName = "ButtonUtil";
            playerUtilName = "PlayerUtil";
            arenaUtilName = "ArenaUtil";
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
                    //Debug.Log("Line: \"" + line + "\"");
                    if (line.StartsWith(";")) continue;
                    if (!line.Contains("=")) continue;
                    //Debug.Log("Line: \"" + line + "\"");
                    string[] _ = line.Split(new char[1] { '=' }, 2);
                    string key = _[0].Replace("_", "-").Trim();
                    string parameter = _[1].Replace("\"", "").Trim();
                    //Debug.Log("Key: \"" + key+ "\", parameter: \"" + parameter + "\"");
                    switch (key)
                    {
                        case "CYFUtil":
                            //Debug.Log("CYFUtil is found. parameter: \"" + parameter + "\"");
                            if (parameter.ToLower() == "true")
                                codeStyle.useGeneralUtil = true;
                            break;
                        case "ButtonUtil":
                            codeStyle.buttonUtilName = parameter;
                            break;
                        case "PlayerUtil":
                            codeStyle.playerUtilName = parameter;
                            break;
                        case "ArenaUtil":
                            codeStyle.arenaUtilName = parameter;
                            break;
                    }
                }
            }
            catch { /* ignore */ }
            return codeStyle;
        }
    }
}
