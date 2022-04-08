using MoonSharp.Interpreter;

namespace AsteriskMod
{
    /// <summary>Create Your Frisk - Asterisk Mod</summary>
    public class Asterisk
    {
        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------
        // *Mod-4

        public const string ModName = "Asterisk Mod";
        public const string ModVersion = "0.5.2.9";
        public const string ModAuthor = "Nil256";
        public const string ModAuthorJP = "にるにころ";
        public const string ModAuthorAlt = "FenneneProject";

        /// <summary>Asterisk Mod's Versions</summary>
        public enum Versions
        {
            /// <summary>Unknown, Debug or Beta Version</summary>
            Unknwon,
            /// <summary>v0.5: <see href="https://github.com/Fennene/CreateYourFrisk-AsteriskMod/releases/tag/v0.5"/><br/>v0.5.2: <see href="https://github.com/Fennene/CreateYourFrisk-AsteriskMod/releases/tag/v0.5.2"/></summary>
            InitialVersion,
            /// <summary>v0.5.2.7<br/><see href="https://github.com/Fennene/CreateYourFrisk-AsteriskMod/releases/tag/v0.5.2.7"/></summary>
            CustomStateUpdate,
            /// <summary>v0.5.2.8<br/><see href="https://github.com/Fennene/CreateYourFrisk-AsteriskMod/releases/tag/v0.5.2.8"/></summary>
            UtilUpdate,
            /// <summary>v0.5.2.9<br/><see href="https://github.com/Fennene/CreateYourFrisk-AsteriskMod/releases/tag/v0.5.2.9"/></summary>
            QOLUpdate, // v0.5.2.9
            /// <summary>v0.5.3</summary>
            GMSUpdate,
            /// <summary>v0.5.4</summary>
            AsteriskMod
        }

        /// <summary>Whether "Experimental Features" is enabled or not</summary>
        public static bool experimentMode;
        /// <summary>In Mod Selection, whether always show mods' description or not</summary>
        public static bool alwaysShowDesc;
        /// <summary>Remove annoying dog</summary>
        public static bool showErrorDog;
        /// <summary>Engine's Target Language<br/>Currently always English</summary>
        public static Languages language;

        internal const string OPTION_EXPERIMENT = "*CYF-Experiment";
        internal const string OPTION_DESC = "*CYF-Description";
        internal const string OPTION_DOG = "*CYF-ErrorDog";
        internal const string OPTION_LANG = "*CYF-Language";

        /// <summary>A<br/>p<br/>r<br/>i<br/>l<br/><br/>f<br/>o<br/>o<br/>l<br/><br/><br/>[Insert description here]</summary>
        public static bool active; // *Mod-4
        internal const string OPTION_JOKE_ACTIVE = "*CYF-AprilFool-Joke-Active"; // *Mod-4

        public const string WindowBasisName = "*Create Your Frisk";
        public const string WinodwBsaisNmae = "*Crate Your Frisk";

        /// <summary>Initialize AsteriskMod's variables and Modify CYF's global variables</summary>
        public static void Initialize()
        {
            ControlPanel.instance.WindowBasisName = WindowBasisName;
            ControlPanel.instance.WinodwBsaisNmae = WinodwBsaisNmae;
            experimentMode = false;
            alwaysShowDesc = true;
            showErrorDog = true;
            language = Languages.English;

            active = true;
        }

        /// <summary>Load</summary>
        public static void Load()
        {
            if (LuaScriptBinder.GetAlMighty(null, OPTION_EXPERIMENT) != null && LuaScriptBinder.GetAlMighty(null, OPTION_EXPERIMENT).Type == DataType.Boolean)
            {
                experimentMode = LuaScriptBinder.GetAlMighty(null, OPTION_EXPERIMENT).Boolean;
            }
            if (LuaScriptBinder.GetAlMighty(null, OPTION_DESC) != null && LuaScriptBinder.GetAlMighty(null, OPTION_DESC).Type == DataType.Boolean)
            {
                alwaysShowDesc = LuaScriptBinder.GetAlMighty(null, OPTION_DESC).Boolean;
            }
            if (LuaScriptBinder.GetAlMighty(null, OPTION_DOG) != null && LuaScriptBinder.GetAlMighty(null, OPTION_DOG).Type == DataType.Boolean)
            {
                showErrorDog = LuaScriptBinder.GetAlMighty(null, OPTION_DOG).Boolean;
            }
            if (LuaScriptBinder.GetAlMighty(null, OPTION_LANG) != null && LuaScriptBinder.GetAlMighty(null, OPTION_LANG).Type == DataType.String)
            {
                string lang = LuaScriptBinder.GetAlMighty(null, OPTION_LANG).String;
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

            // *Mod-4
            if (LuaScriptBinder.GetAlMighty(null, OPTION_JOKE_ACTIVE) != null && LuaScriptBinder.GetAlMighty(null, OPTION_JOKE_ACTIVE).Type == DataType.Boolean)
            {
                active = LuaScriptBinder.GetAlMighty(null, OPTION_JOKE_ACTIVE).Boolean;
            }
        }

        public static Versions ConvertFromString(string versionName)
        {
            if (!versionName.StartsWith("v")) versionName = "v" + versionName;
            if (versionName == "v0.5" || versionName == "v0.5.2")
                return Versions.InitialVersion;
            if (versionName == "v0.5.2.4" || versionName == "v0.5.2.5" || versionName == "v0.5.2.6" || versionName == "v0.5.2.7")
                return Versions.CustomStateUpdate;
            if (versionName == "v0.5.2.8")
                return Versions.UtilUpdate;
            if (versionName == "v0.5.2.9")
                return Versions.QOLUpdate;
            /*
            if (versionName == "v0.5.3")
                return Versions.GMSUpdate;
            if (versionName == "v0.5.4")
                return Versions.AsteriskMod;
            */
            return Versions.Unknwon;
        }

        public static bool RequireExperimentalFeature(string funcName, bool showError = true)
        {
            if (experimentMode) return true;
            if (!showError)     return false;
            throw new CYFException(funcName + "() is experimental feature. You need to enable \"Experimental Features\" in AsteriskMod's option.");
        }

        public static bool AprilFooooooooooooooooooooooooooooooool_CheckActive(string className, string fieldName, bool showError = true/*, ScriptWrapper caller = null*/)
        {
            if (active) return true;
            if (!showError) return false;
            string mess = "cannot access field " + fieldName + " of userdata<" + className + ">";
            //if (caller != null) UnitaleUtil.DisplayLuaError(caller.scriptname, mess);
            throw new CYFException(mess);
        }
    }
}
