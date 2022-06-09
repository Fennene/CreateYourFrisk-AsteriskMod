using MoonSharp.Interpreter;
using System;

namespace AsteriskMod
{
    /// <summary>Create Your Frisk - Asterisk Mod</summary>
    public class Asterisk
    {
        // --------------------------------------------------------------------------------
        //                          Asterisk Mod Modification
        // --------------------------------------------------------------------------------
        // --------------------------------------------------------------------------------

        public const string ModName = "Asterisk Mod";
        public const string ModVersion = "0.5.3";
        public const string ModAuthor = "Nil256";
        public const string ModAuthorJP = "にるにころ";
        public const string ModAuthorAlt = "FenneneProject";

        /// <summary>Asterisk Mod's Versions</summary>
        public enum Versions
        {
            /// <summary>v0.5: <see href="https://github.com/Fennene/CreateYourFrisk-AsteriskMod/releases/tag/v0.5"/><br/>v0.5.2: <see href="https://github.com/Fennene/CreateYourFrisk-AsteriskMod/releases/tag/v0.5.2"/></summary>
            InitialVersion,
            /// <summary>v0.5.2.7<br/><see href="https://github.com/Fennene/CreateYourFrisk-AsteriskMod/releases/tag/v0.5.2.7"/></summary>
            CustomStateUpdate,
            /// <summary>v0.5.2.8<br/><see href="https://github.com/Fennene/CreateYourFrisk-AsteriskMod/releases/tag/v0.5.2.8"/></summary>
            UtilUpdate,
            /// <summary>v0.5.2.9<br/><see href="https://github.com/Fennene/CreateYourFrisk-AsteriskMod/releases/tag/v0.5.2.9"/></summary>
            QOLUpdate, // v0.5.2.9
            /// <summary>v0.5.3<br/><see href="https://github.com/Fennene/CreateYourFrisk-AsteriskMod/releases/tag/v0.5.3"/></summary>
            TakeNewStepUpdate,
            /// <summary>Unknown Version<br/>This version is regarded to latest version.</summary>
            Unknwon
            // <summary>v0.5.3.5 ?</summary>
            //GMSUpdate,
            // <summary>v0.5.4</summary>
            //AsteriskMod
        }

        /// <summary>Whether "Experimental Features" is enabled or not</summary>
        public static bool experimentMode;
        /// <summary>In Mod Selection, whether always show mods' description or not</summary>
        public static bool alwaysShowDesc;
        /// <summary>Shows annoying dog</summary>
        public static bool showErrorDog;
        /// <summary>Engine's Target Language<br/>Currently always English</summary>
        public static Languages language;
        /// <summary>Whether mods prevent to change system option by <c>SetAlMightyGlobal()</c> or not</summary>
        public static bool optionProtecter;
        /// <summary>Whether show error or not if mods try to change system option by <c>SetAlMightyGlobal()</c></summary>
        public static bool reportProtecter;
        /// <summary>Whether replaces mod's name set from modders</summary>
        public static bool displayModInfo;

        internal const string OPTION_EXPERIMENT = "*CYF-Experiment";
        internal const string OPTION_DESC = "*CYF-Description";
        internal const string OPTION_DOG = "*CYF-ErrorDog";
        internal const string OPTION_LANG = "*CYF-Language";
        internal const string OPTION_PROTECT = "*CYF-ProtectOption";
        internal const string OPTION_PROTECT_ERROR = "*CYF-ProtectReport";
        internal const string OPTION_MODINFO = "*CYF-ModInfo";

        public const string WindowBasisName = "*Create Your Frisk";
        public const string WinodwBsaisNmae = "*Crate Your Frisk";

        public static readonly string[] nonOWScenes = { "NewMod", "MHTMenu", "MHTSim", "AsteriskOptions" };

        /// <summary>
        /// Initialize AsteriskMod's variables and Modify CYF's global variables<br/>
        /// AsteriskModの変数を初期化し、一部のCYFの変数の値を変更します。
        /// </summary>
        public static void Initialize()
        {
            ControlPanel.instance.WindowBasisName = WindowBasisName;
            ControlPanel.instance.WinodwBsaisNmae = WinodwBsaisNmae;

            int index = GlobalControls.nonOWScenes.Length;
            Array.Resize(ref GlobalControls.nonOWScenes, index + nonOWScenes.Length);
            for (var i = 0; i < nonOWScenes.Length; i++)
            {
                GlobalControls.nonOWScenes[index + i] = nonOWScenes[i];
            }

            experimentMode = false;
            alwaysShowDesc = true;
            showErrorDog = true;
            language = Languages.English;
            optionProtecter = true;
            reportProtecter = true;
            displayModInfo = true;

            AsteriskEngine.Initialize();
        }

        /// <summary>AsteriskModのオプションを読み込みます。</summary>
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
                language = AsteriskUtil.ConvertToLanguage(lang);
            }
            if (LuaScriptBinder.GetAlMighty(null, OPTION_PROTECT) != null && LuaScriptBinder.GetAlMighty(null, OPTION_PROTECT).Type == DataType.Boolean)
            {
                optionProtecter = LuaScriptBinder.GetAlMighty(null, OPTION_PROTECT).Boolean;
            }
            if (LuaScriptBinder.GetAlMighty(null, OPTION_PROTECT_ERROR) != null && LuaScriptBinder.GetAlMighty(null, OPTION_PROTECT_ERROR).Type == DataType.Boolean)
            {
                reportProtecter = LuaScriptBinder.GetAlMighty(null, OPTION_PROTECT_ERROR).Boolean;
            }
            if (LuaScriptBinder.GetAlMighty(null, OPTION_MODINFO) != null && LuaScriptBinder.GetAlMighty(null, OPTION_MODINFO).Type == DataType.Boolean)
            {
                displayModInfo = LuaScriptBinder.GetAlMighty(null, OPTION_MODINFO).Boolean;
            }

#if UNITY_EDITOR
            //Test.Tset();
#endif
        }

        /// <summary>"Experimental Features"オプションが有効になっているかどうか調べます。<br/>有効になっていない場合はエラーを発生させます。</summary>
        /// <param name="funcName">関数名 ()は不要。</param>
        /// <param name="showError"><c>false</c>の場合エラーが発生しません。</param>
        /// <returns>"Experiment Features"オプションが有効になっているかどうかを返します。</returns>
        public static bool RequireExperimentalFeature(string funcName, bool showError = true)
        {
            if (experimentMode) return true;
            if (!showError)     return false;
            throw new CYFException(funcName + "() is experimental feature. You need to enable \"Experimental Features\" in AsteriskMod's option.");
        }

        public static Versions ConvertToModVersion(string versionName)
        {
            versionName = versionName.ToLower();
            if (versionName.Contains(".") && !versionName.StartsWith("v")) versionName = "v" + versionName;
            switch (versionName)
            {
                case "v0.5":
                case "v0.5.2":
                    return Versions.InitialVersion;
                case "v0.5.2.4":
                case "v0.5.2.5":
                case "v0.5.2.6":
                case "v0.5.2.7":
                    return Versions.CustomStateUpdate;
                case "v0.5.2.8":
                    return Versions.UtilUpdate;
                case "v0.5.2.9":
                    return Versions.QOLUpdate;
                case "v0.5.3":
                    return Versions.TakeNewStepUpdate;
            }
            return Versions.Unknwon;
        }

        public static string ConvertFromModVersion(Versions version)
        {
            switch (version)
            {
                case Versions.InitialVersion:    return "v0.5";
                case Versions.CustomStateUpdate: return "v0.5.2.7";
                case Versions.UtilUpdate:        return "v0.5.2.8";
                case Versions.QOLUpdate:         return "v0.5.2.9";
                case Versions.TakeNewStepUpdate: return "v0.5.3";
            }
            return "Unknwon";
        }
    }
}
