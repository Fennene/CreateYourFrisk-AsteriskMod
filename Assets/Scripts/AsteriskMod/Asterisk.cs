using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteriskMod
{
    public class Asterisk
    {
        public const string WindowBasisName = "*Create Your Frisk";
        public const string WinodwBsaisNmae = "*Crate Your Frisk";

        public const string ModName = "Asterisk Mod";
        public const string ModVersion = "v0.5";

        public static void Initialize()
        {
            ControlPanel.instance.WindowBasisName = WindowBasisName;
            ControlPanel.instance.WinodwBsaisNmae = WinodwBsaisNmae;
        }
    }
}
