using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class Mod
    {
        private DirectoryInfo _rawData;
        private ModInfo _modInfo;

        public Mod(DirectoryInfo dir)
        {
            _rawData = dir;
            _modInfo = ModInfo.Get(_rawData.Name);

            DirectoryName = _rawData.Name;
            Title = _rawData.Name;
            SubTitle = null;

            if (Asterisk.alwaysShowDesc)
            {
                if (string.IsNullOrEmpty(_modInfo.title))
                {
                }
            }
        }

        public string DirectoryName { get; private set; }
        public string Title { get; private set; }
        public string SubTitle { get; private set; }
        public string[] Encounters { get; private set; }
    }
}
