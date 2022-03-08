using System;
using System.IO;
using UnityEngine;

namespace AsteriskMod
{
    public struct ModData
    {
        public string title;
        public string subitle;
        public string description;
        public TextAnchor descAnchor;

        public static ModData Empty
        {
            get
            {
                ModData _ = new ModData();
                _.title = "";
                _.subitle = "";
                _.description = "";
                _.descAnchor = TextAnchor.UpperLeft;
                return _;
            }
        }

        private static bool ShouldOpenCheck(string path)
        {
            return File.Exists(path) && new FileInfo(path).Length <= 1024 * 1024; // 1MB
        }

        public static ModData GetCurrentModData()
        {
            ModData result = ModData.Empty;
            string info_cyfmod_path = FileLoader.pathToModFile("info.cyfmod");
            if (!ShouldOpenCheck(info_cyfmod_path))
                return result;
            string description_file_path = "";
            foreach (string l in File.ReadAllLines(info_cyfmod_path))
            {
                string line = l.Replace("\r", "").Replace("\t", "").Replace(" ", "");
                if (line.StartsWith(";"))
                    continue;
                if (!line.Contains("="))
                    continue;
                string[] _ = line.Split(new char[1] { '=' }, 2);
                string key = _[0];
                string parameter = _[1];
                parameter = parameter.Replace("\"", "");
                switch (key.ToLower())
                {
                    case "title":
                        result.title = parameter;
                        break;
                    case "subtitle":
                        result.subitle = parameter;
                        break;
                    case "descfile":
                        description_file_path = parameter;
                        break;
                    case "description":
                        result.description = parameter;
                        break;
                    case "align":
                    case "anchor":
                        try
                        {
                            result.descAnchor = (TextAnchor)Enum.Parse(typeof(TextAnchor), parameter); ;
                        }
                        catch { }
                        break;
                }
            }
            if (description_file_path == "" || description_file_path.Contains(".."))
                return result;
            description_file_path = FileLoader.pathToModFile(description_file_path);
            if (!ShouldOpenCheck(description_file_path))
                return result;
            result.description = File.ReadAllText(description_file_path);
            return result;
        }
    }
}
