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
        public const string ModVersion = "0.5.3.0.5";
        public const string ModAuthor = "Nil256";
        public const string ModAuthorJP = "にるにころ";
        public const string ModAuthorAlt = "FenneneProject";
        //public const bool DevEdition = true;

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
            /// <summary>v0.5.3.0.1<br/><see href="https://github.com/Fennene/CreateYourFrisk-AsteriskMod/releases/tag/v0.5.3.0.1"/></summary>
            GlobalsScriptsAddition,
            /// <summary>v0.5.3.0.2<br/><see href="https://github.com/Fennene/CreateYourFrisk-AsteriskMod/releases/tag/v0.5.3.0.2"/></summary>
            BeAddedShaderAndAppData,
            /// <summary>v0.5.3.0.4<br/><see href="https://github.com/Fennene/CreateYourFrisk-AsteriskMod/releases/tag/v0.5.3.0.4"/></summary>
            DynamicEnumAddition,
            /// <summary>v0.5.3.0.5<br/><see href="https://github.com/Fennene/CreateYourFrisk-AsteriskMod/releases/tag/v0.5.3.0.5"/></summary>
            LayerActivator,
            // <summary>v0.5.3.1 ?</summary>
            //GlobalScriptUpdate,
            // <summary>v0.5.3.2 ?</summary>
            //GMSUpdate,
            // <summary>v0.5.4</summary>
            //AsteriskMod
            /// <summary>Unknown Version<br/>This version is regarded to latest version.</summary>
            Unknwon
        }

        /// <summary>Whether "Experimental Features" is enabled or not<br/>実験的機能が有効かどうか</summary>
        public static bool experimentMode;
        /// <summary>In Mod Selection, whether always show mods' description or not<br/>Mod選択画面においてModの説明を常に表示するかどうか</summary>
        public static bool alwaysShowDesc;
        /// <summary>Engine's Displaying Language<br/>エンジンの表示言語</summary>
        public static Languages language;
        /// <summary>Whether mods prevent to change system option by <c>SetAlMightyGlobal()</c> or not<br/></summary>
        public static bool optionProtecter;
        /// <summary>Whether show error or not if mods try to change system option by <c>SetAlMightyGlobal()</c><br/></summary>
        public static bool reportProtecter;
        /// <summary>Whether replaces mod's name set from modders<br/>Mod製作者が設定したMod名に置き換えるかどうか</summary>
        public static bool displayModInfo;
        /// <summary>Whether change UIs with user's set language<br/>ユーザーの設定した言語に応じてUIを自動で変えるかどうか</summary>
        public static bool changeUIwithLanguage;
        /// <summary>Target ModPack's index</summary>
        public static int TargetModPack;

        internal const string OPTION_EXPERIMENT = "*CYF-Experiment";
        internal const string OPTION_DESC = "*CYF-Description";
        internal const string OPTION_LANG = "*CYF-Language";
        internal const string OPTION_PROTECT = "*CYF-ProtectOption";
        internal const string OPTION_PROTECT_ERROR = "*CYF-ProtectReport";
        internal const string OPTION_MODINFO = "*CYF-ModInfo";
        internal const string OPTION_UIWITHLANG = "*CYF-UIChangedWithLanguage";
        internal const string OPTION_MODPACK = "*CYF-ModPack";

        public const string WindowBasisName = "*Create Your Frisk";
        public const string WinodwBsaisNmae = "*Crate Your Frisk";

        public static readonly string[] nonOWScenes = { "AlternativeModSelect", "ModPack", "NewMod", "MHTMenu", "CyfModFileEditor", "MHTSim", "AsteriskOptions" };

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
            language = Languages.English;
            optionProtecter = true;
            reportProtecter = true;
            displayModInfo = true;
            changeUIwithLanguage = true;
            //ModPackDatas = new ModPack[0];
            TargetModPack = -1;

            AsteriskEngine.Initialize();
            Lang.Initialize();
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
            if (LuaScriptBinder.GetAlMighty(null, OPTION_UIWITHLANG) != null && LuaScriptBinder.GetAlMighty(null, OPTION_UIWITHLANG).Type == DataType.Boolean)
            {
                changeUIwithLanguage = LuaScriptBinder.GetAlMighty(null, OPTION_UIWITHLANG).Boolean;
            }
            if (LuaScriptBinder.GetAlMighty(null, OPTION_MODPACK) != null && LuaScriptBinder.GetAlMighty(null, OPTION_MODPACK).Type == DataType.Number)
            {
                TargetModPack = (int)LuaScriptBinder.GetAlMighty(null, OPTION_MODPACK).Number;
            }
        }

        /// <summary>"Experimental Features"オプションが有効になっているかどうか調べます。<br/>有効になっていない場合はエラーを発生させます。</summary>
        /// <param name="funcName">関数名 ()は不要。</param>
        /// <param name="showError"><c>false</c>の場合エラーが発生しません。</param>
        /// <returns>"Experiment Features"オプションが有効になっているかどうかを返します。</returns>
        public static bool RequireExperimentalFeature(string funcName, bool showError = true)
        {
            if (experimentMode) return true;
            if (!showError)     return false;
            string errorMessage = funcName + "() is experimental feature. You need to enable \"Experimental Features\" in AsteriskMod's option.";
            if (language == Languages.Japanese) errorMessage = funcName + "()は実験的機能です。Asterisk Mod Optionから\"Experimental Features\"(実験的機能)を有効にしてください。";
            throw new CYFException(errorMessage);
        }

        [ToDo] public static Versions NewConvertToModVersion(string versionName)
        {
            versionName = versionName.ToLower();
            if (!versionName.Contains(".")) return Versions.Unknwon;
            if (versionName.StartsWith("v"))
            {
                versionName = versionName.Substring(1);
            }
            string[] _ = versionName.Split('.');
            int parseNum = 6;
            if (_.Length < 6)
            {
                parseNum = _.Length;
            }
            /*
            for (var i = parseNum; i > 0; i--)
            {
            }
            */
            int[] versionNumbers = new int[parseNum];
            for (var i = 0; i < parseNum; i++)
            {
                try   { versionNumbers[i] = int.Parse(_[i], System.Globalization.NumberStyles.None); }
                catch { return Versions.Unknwon; }
            }
            if (versionNumbers[0] != 0) return Versions.Unknwon;
            if (versionNumbers.Length <= 1) return Versions.Unknwon;
            if (versionNumbers[1] != 5) return Versions.Unknwon;
            if (versionNumbers.Length <= 2) return Versions.InitialVersion;
            if (versionNumbers[2] < 2)
            {
                return Versions.InitialVersion;
            }
            else if (versionNumbers[2] == 2)
            {
                if (versionNumbers.Length <= 3) return Versions.InitialVersion;
                if (versionNumbers[3] <= 0)
                {
                }

            }
            else if (versionNumbers[2] == 3)
            {
                if (versionNumbers.Length <= 3) return Versions.TakeNewStepUpdate;
            }
            return Versions.Unknwon;
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
                case "v0.5.3.0.1":
                    return Versions.GlobalsScriptsAddition;
                case "v0.5.3.0.2":
                    return Versions.BeAddedShaderAndAppData;
                case "v0.5.3.0.3":
                case "v0.5.3.0.4":
                    return Versions.DynamicEnumAddition;
                case "v0.5.3.0.5":
                    return Versions.LayerActivator;
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
                case Versions.GlobalsScriptsAddition:  return "v0.5.3.0.1";
                case Versions.BeAddedShaderAndAppData: return "v0.5.3.0.2";
                case Versions.DynamicEnumAddition:     return "v0.5.3.0.4";
                case Versions.LayerActivator:          return "v0.5.3.0.5";
            }
            return "Unknwon";
        }

        public static void FakeNotFoundError(string className, string memberName, bool isAsteriskMod = true)
        {
            throw new CYFException("cannot access field " + memberName + " of userdata<" + (isAsteriskMod ? "AsteriskMod." : "") + className + ">");
        }
    }
}
