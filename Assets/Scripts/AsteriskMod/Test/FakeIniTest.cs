using UnityEngine;

namespace AsteriskMod
{
    internal static class FakeIniTest
    {
        internal static void ToStringTest(FakeIni ini)
        {
            string log = "< FakeIni.ToString() Test >\n";
            log += "----------------------------------------------------------------------------------------------------------------------\n";
            log += ini.ToString();
            log += "\n----------------------------------------------------------------------------------------------------------------------";
            Debug.Log(log);
        }

        internal static void Log(string path)
        {
            FakeIni ini = FakeIniFileLoader.Load(path);
            string text = "";
            foreach (string parameterName in ini["$"].ParameterNames)
            {
                text += ini["$"].GetParameter(parameterName);
            }
        }
    }
}
