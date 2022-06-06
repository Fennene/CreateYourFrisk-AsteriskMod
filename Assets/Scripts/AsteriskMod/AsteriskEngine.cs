using UnityEngine;

namespace AsteriskMod
{
    public class AsteriskEngine
    {
        public static Asterisk.Versions ModTarget_AsteriskVersion { get; private set; }

        public static CodeStyle LuaCodeStyle { get; private set; }

        public static char AsteriskChar { get; set; }

        public static bool AutoRemoveProjectiles { get; set; }

        public class JapaneseStyleOption
        {
            // Name
            private static bool _jpName;

            public static bool JPName
            {
                get { return _jpName; }
                set { SetJapaneseNameActive(value); }
            }

            public static void SetJapaneseNameActive(bool active)
            {
                if (_jpName == active) return;
                _jpName = active;
                PlayerNameText.instance.SetJP();
            }
            public static void SetJPName(bool active) { SetJapaneseNameActive(active); }

            // Font

            // Options
            public static string JapaneseFontName { get; set; }
            public static string JPFontName
            {
                get { return JapaneseFontName; }
                set { JapaneseFontName = value; }
            }

            private static bool _autoFontCoordinating;
            public static bool AutoFontCoordinating
            {
                get { return _autoFontCoordinating; }
                set { SetAutoFontCoordinatingActive(value); }
            }

            public static void SetAutoFontCoordinatingActive(bool active)
            {
                if (_autoFontCoordinating == active) return;
                _autoFontCoordinating = active;
                AsteriskChar = active ? '＊' : '*';
            }

            // Auto set JP
            public static bool AutoJPFontEncounterText { get; set; }
            public static bool AutoJPFontBattleDialog { get; set; }
            public static bool AutoJPFontActCommands { get; set; }
            public static bool AutoJPFontStateEditor { get; set; }

            public static void SetAutoJapaneseFontStyle(bool active)
            {
                AutoJPFontEncounterText = active;
                AutoJPFontBattleDialog = active;
                AutoJPFontActCommands = active;
                AutoJPFontStateEditor = active;
            }
            public static void SetAutoJPFont(bool active) { SetAutoJapaneseFontStyle(active); }

            // System

            internal static void Initialize()
            {
                _jpName = false;
                JapaneseFontName = "jp";
                _autoFontCoordinating = false;
                SetAutoJapaneseFontStyle(false);
            }

            internal static void Reset()
            {
                Initialize();
                SetJapaneseNameActive(Asterisk.language == Languages.Japanese);
            }

            public static void SetActive(bool active)
            {
                SetJapaneseNameActive(active);
                SetAutoFontCoordinatingActive(active);
                SetAutoJapaneseFontStyle(active);
            }

            internal static string FontCommand { get { return "[font:" + JapaneseFontName + "]"; } }
        }

        internal static void Initialize()
        {
            ModTarget_AsteriskVersion = Asterisk.Versions.Unknwon;
            LuaCodeStyle = new CodeStyle();
            AsteriskChar = '*';
            AutoRemoveProjectiles = true;
            JapaneseStyleOption.Initialize();
        }

        internal static void PrepareMod(ModInfo modInfo, CodeStyle codeStyle)
        {
            Debug.Log("AsteriskMod TargetVersion: " + Asterisk.ConvertFromModVersion(modInfo.targetVersion));
            ModTarget_AsteriskVersion = modInfo.targetVersion;
            LuaCodeStyle = codeStyle;
            AsteriskChar = '*';
            AutoRemoveProjectiles = true;
        }

        internal static void AwakeMod()
        {
            CYFEngine.Initialize();
            UIController.InitalizeButtonManager();
            ArenaUI.Initialize();
            JapaneseStyleOption.Reset();
        }

        internal static void Reset()
        {
            CYFEngine.Reset();
            Initialize();
        }
    }
}
