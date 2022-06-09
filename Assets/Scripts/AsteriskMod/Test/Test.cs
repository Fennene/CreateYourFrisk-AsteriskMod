using System.IO;

namespace AsteriskMod
{
    internal class Test
    {
        public static void Tset()
        {
            FakeIni ini = FakeIniFileLoader.Load(Path.Combine(FileLoader.DataRoot, "test.cyfmod"));
            FakeIniTest.ToStringTest(ini);
            /*
            FakeIni ini = new FakeIni();
            ini.Main.SetParameter("target-version", new FakeIniParameter("v0.5.3"));
            ini.SetParameter("General", "Love", new FakeIniParameter("1.000000"));
            ini.SetParameter("General", "Kills", new FakeIniParameter("0.000000"));
            ini.SetParameter("General", "", new FakeIniParameter("True"));
            ini.SetParameter("Sans", "Met1", new FakeIniParameter("1.000000"));
            ini.SetParameter("Sans", "F", new FakeIniParameter("1.000000"));
            ini.SetParameter("Flowey", "Met1", new FakeIniParameter("2.000000"));
            ini.SetSection("$", new FakeIniSection());
            ini.SetParameter("$", "Lang", new FakeIniParameter("en"));
            ini.SetParameter("$", "supported-language", new FakeIniParameter(new[] { "en", "jp" }));
            FakeIniTest.ToStringTest(ini);
            */
        }
    }
}
