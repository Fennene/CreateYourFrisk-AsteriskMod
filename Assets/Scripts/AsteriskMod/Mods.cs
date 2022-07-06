using MoonSharp.Interpreter;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AsteriskMod
{
    public static class Mods
    {
        public static List<Mod> realMods;
        public static ModPack[] modPacks;
        public static List<Mod> mods;

        public static bool Reset()
        {
            LoadModPacks();
            return LoadMods();
        }

        public static bool LoadMods()
        {
            // Load directory info
            DirectoryInfo di = new DirectoryInfo(Path.Combine(FileLoader.DataRoot, "Mods"));
            var modDirsTemp = di.GetDirectories();

            // Remove mods with 0 encounters and hidden mods from the list
            List<DirectoryInfo> modDirs = (from modDir in modDirsTemp
                                           where new DirectoryInfo(Path.Combine(FileLoader.DataRoot, "Mods/" + modDir.Name + "/Lua/Encounters")).Exists
                                           let hasEncounters = new DirectoryInfo(Path.Combine(FileLoader.DataRoot, "Mods/" + modDir.Name + "/Lua/Encounters")).GetFiles("*.lua").Any()
                                           where hasEncounters && (modDir.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden && !modDir.Name.StartsWith("@")
                                           select modDir).ToList();

            if (modDirs.Count == 0) return false;

            modDirs.Sort((a, b) => a.Name.CompareTo(b.Name));

            realMods = new List<Mod>(modDirs.Count);
            mods = new List<Mod>(modDirs.Count);
            for (var i = 0; i < modDirs.Count; i++)
            {
                realMods.Add(new Mod(modDirs[i]));
                if (Asterisk.TargetModPack == -1 || modPacks[Asterisk.TargetModPack].ShowingMods.Contains(realMods[i].RealName))
                {
                    mods.Add(realMods[i]);
                }
            }
            if (mods.Count <= 0)
            {
                Asterisk.TargetModPack = -1;
                LuaScriptBinder.SetAlMighty(null, Asterisk.OPTION_MODPACK, DynValue.NewNumber(Asterisk.TargetModPack), true);
                mods = new List<Mod>(realMods.Count);
                for (var i = 0; i < realMods.Count; i++)
                {
                    mods.Add(realMods[i]);
                }
            }
            return mods.Count > 0;
        }

        public static void LoadModPacks()
        {
            modPacks = ModPack.GetModPacks(true);

            /*
            string _ = "Load ModPacks\n";
            for (var i = 0; i < modPacks.Length; i++)
            {
                _ += "\n" + modPacks[i].FileName;
            }
            UnityEngine.Debug.Log(_);
            */

            if (Asterisk.TargetModPack < -1 || Asterisk.TargetModPack >= modPacks.Length)
            {
                Asterisk.TargetModPack = -1;
                LuaScriptBinder.SetAlMighty(null, Asterisk.OPTION_MODPACK, DynValue.NewNumber(Asterisk.TargetModPack), true);
            }
        }

        public static int FindIndex(string modDirName) { return mods.FindIndex(mod => mod.RealName == modDirName); }

        public static int FindModPackIndex(string modPackName) { return modPacks.ToList().FindIndex(modPack => modPack.FileName == modPackName); }
    }
}
