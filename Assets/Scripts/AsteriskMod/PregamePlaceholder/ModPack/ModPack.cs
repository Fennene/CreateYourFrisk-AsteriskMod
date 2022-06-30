using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AsteriskMod.ModPack
{
    public class ModPack
    {
        public string fileName;
        public string[] packs;

        public ModPack(string path)
        {
            if (AsteriskUtil.IsIgnoreFile(path)) throw new CYFException("");
            fileName = Path.GetFileNameWithoutExtension(path);
            packs = File.ReadAllLines(path);
        }
    }
}
