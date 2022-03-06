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
        public const string ModVersion = "v0.5.2.7";

        public static bool experimentMode;
        public static Languages language = Languages.English;

        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------

        public static void Initialize()
        {
            ControlPanel.instance.WindowBasisName = WindowBasisName;
            ControlPanel.instance.WinodwBsaisNmae = WinodwBsaisNmae;
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
            string optionID_lang = "*CYFLanguage";
            if (LuaScriptBinder.GetAlMighty(null, optionID_lang) != null && LuaScriptBinder.GetAlMighty(null, optionID_lang).Type == DataType.String)
            {
                string lang = LuaScriptBinder.GetAlMighty(null, optionID_lang).String;
                switch (lang)
                {
                    case "en":
                        language = Languages.English;
                        break;
                    case "jp":
                        language = Languages.Japanese;
                        break;
                }
            }
        }
    }
}
