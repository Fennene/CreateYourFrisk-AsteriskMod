using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AsteriskMod
{
    public class ModPack
    {
        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        public string[] ShowingMods { get; private set; }

        public ModPack(string fullPath)
        {
            FilePath = fullPath;
            FileName = Path.GetFileNameWithoutExtension(FilePath);
            ShowingMods = File.ReadAllLines(FileName);
        }

        public static string ModPackDirectory { get { return Path.Combine(Application.persistentDataPath, "ModPack").Replace('\\', '/'); } }

        public static ModPack[] GetModPacks(bool init = false)
        {
            string modPackDir = ModPackDirectory;
            if (!Directory.Exists(modPackDir))
            {
                if (init) Directory.CreateDirectory(modPackDir);
                return new ModPack[0];
            }
            string[] filePathes;
            try   { filePathes = Directory.GetFiles(modPackDir, "*.modpack", SearchOption.TopDirectoryOnly); }
            catch { return new ModPack[0]; }
            List<ModPack> modPacks = new List<ModPack>(filePathes.Length);
            for (var i = 0; i < filePathes.Length; i++)
            {
                ModPack modPack;
                try   { modPack = new ModPack(filePathes[i]); }
                catch { continue; }
                modPacks.Add(modPack);
            }
            return modPacks.ToArray();
        }
    }
}
