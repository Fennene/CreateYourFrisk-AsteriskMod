using MoonSharp.Interpreter;
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
        public const string ModVersion = "v0.5.2.4";

        public static bool experimentMode;
        public static Languages languages = Languages.English;

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

        public static void LoadOption()
        {
            string optionID_experiment = "*CYFExperiment";
            if (LuaScriptBinder.GetAlMighty(null, optionID_experiment) != null && LuaScriptBinder.GetAlMighty(null, optionID_experiment).Type == DataType.Boolean)
            {
                experimentMode = LuaScriptBinder.GetAlMighty(null, optionID_experiment).Boolean;
            }
        }
    }
}
