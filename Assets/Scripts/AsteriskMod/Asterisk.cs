using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod
{
    public class Asterisk
    {
        public const string WindowBasisName = "*Create Your Frisk";
        public const string WinodwBsaisNmae = "*Crate Your Frisk";

        public const string ModName = "Asterisk Mod";
        public const string ModVersion = "v0.5";

        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------

        public static void Initialize()
        {
            ControlPanel.instance.WindowBasisName = WindowBasisName;
            ControlPanel.instance.WinodwBsaisNmae = WinodwBsaisNmae;
            Misc.WindowName = GlobalControls.crate ? ControlPanel.instance.WinodwBsaisNmae : ControlPanel.instance.WindowBasisName;
        }

        public static string GetVersionDisplay()
        {
            return " *" + ModVersion;
        }
    }
}
