using System.IO;

namespace AsteriskMod
{
    internal static class FakeIniFileLoader
    {
        public static FakeIni Load(string path)
        {
            FakeIni ini = new FakeIni();
            string[] fileLines = File.ReadAllLines(path);
            if (fileLines[0].StartsWith("!") || fileLines[0].StartsWith("#")) fileLines[0] = ";" + fileLines[0];
            string nowSection = "$Main";
            foreach (string fileLine in fileLines)
            {
                string line = fileLine.Trim();
                if (line.StartsWith(";")) continue;
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    string sectionName = line.TrimOnce('[', ']');
                    if (!FakeIniUtil.IsValidSectionName(sectionName)) continue;
                    nowSection = sectionName;
                    ini.SetSection(nowSection, new FakeIniSection());
                }
                if (nowSection.IsNullOrWhiteSpace()) continue;
                if (!line.Contains("=")) continue;
                string[] parameterData = line.Split(new[] { '=' }, 2);
                parameterData[0] = parameterData[0].Trim();
                parameterData[1] = parameterData[1].Trim();
                if (!FakeIniUtil.IsValidParameterName(parameterData[0])) continue;
                FakeIniParameter parameter;
                if (FakeIniUtil.TryGetParameter(parameterData[1], out parameter)) ini.SetParameter(nowSection, parameterData[0], parameter);
            }
            return ini;
        }
    }
}
