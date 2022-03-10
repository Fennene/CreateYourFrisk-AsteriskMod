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
        public const string ModVersion = "v0.5.2.8";

        public static bool experimentMode;
        public static Languages language;
        public static bool showErrorDog;

        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------

        public static void Initialize()
        {
            ControlPanel.instance.WindowBasisName = WindowBasisName;
            ControlPanel.instance.WinodwBsaisNmae = WinodwBsaisNmae;
            language = Languages.English;
            showErrorDog = true;
        }

        public static void LoadOption()
        {
            string optionID = "*CYFExperiment";
            if (LuaScriptBinder.GetAlMighty(null, optionID) != null && LuaScriptBinder.GetAlMighty(null, optionID).Type == DataType.Boolean)
            {
                experimentMode = LuaScriptBinder.GetAlMighty(null, optionID).Boolean;
            }
            optionID = "*CYFLanguage";
            if (LuaScriptBinder.GetAlMighty(null, optionID) != null && LuaScriptBinder.GetAlMighty(null, optionID).Type == DataType.String)
            {
                string lang = LuaScriptBinder.GetAlMighty(null, optionID).String;
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
            optionID = "*CYFErrorDog";
            if (LuaScriptBinder.GetAlMighty(null, optionID) != null && LuaScriptBinder.GetAlMighty(null, optionID).Type == DataType.Boolean)
            {
                showErrorDog = LuaScriptBinder.GetAlMighty(null, optionID).Boolean;
            }
        }
    }
}
