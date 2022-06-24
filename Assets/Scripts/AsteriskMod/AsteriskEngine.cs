using UnityEngine;

namespace AsteriskMod
{
    public class AsteriskEngine
    {
        internal static bool IsSimulator { get; set; }

        public static Asterisk.Versions ModTarget_AsteriskVersion { get; private set; }

        public static CodeStyle LuaCodeStyle { get; private set; }

        public static char AsteriskChar { get; set; }

        public static bool AutoRemoveProjectiles { get; set; }

        public static bool AutoResetStaticText { get; set; }

        public static class JapaneseStyleOption
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
                if (PlayerNameText.instance) PlayerNameText.instance.SetJP();
            }

            // Font

            // Options
            public static string JapaneseFontName { get; set; }

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
                //* AsteriskChar = active ? '＊' : '*';
                //* ArenaUtil.DialogTextMove(0, active ? -5 : 5);
            }

            // Auto set JP
            public static bool AutoJPFontEncounterText { get; set; }
            public static bool AutoJPFontBattleDialog { get; set; }
            public static bool AutoJPFontEnemySelect { get; set; }
            public static bool AutoJPFontActCommands { get; set; }
            public static bool AutoJPFontStateEditor { get; set; }

            public static void SetAutoJapaneseFontStyle(bool active)
            {
                AutoJPFontEncounterText = active;
                AutoJPFontBattleDialog = active;
                AutoJPFontEnemySelect = active;
                AutoJPFontActCommands = active;
                AutoJPFontStateEditor = active;
            }

            public static void SetJapaneseButton(bool active)
            {
                int index1 = active ? 2 : 0;
                int index2 = index1 + 1;
                UIController.ActionButtonManager.FIGHT.SetSprite("fightbt_" + index1, "fightbt_" + index2);
                UIController.ActionButtonManager.ACT  .SetSprite("actbt_"   + index1, "actbt_"   + index2);
                UIController.ActionButtonManager.ITEM .SetSprite("itembt_"  + index1, "itembt_"  + index2);
                UIController.ActionButtonManager.MERCY.SetSprite("mercybt_" + index1, "mercybt_" + index2);
            }

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
                SetJapaneseButton(active);
            }

            internal static string FontCommand { get { return "[font:" + JapaneseFontName + "]"; } }
        }

        internal static void Initialize()
        {
            IsSimulator = false;
            ModTarget_AsteriskVersion = Asterisk.Versions.Unknwon;
            LuaCodeStyle = new CodeStyle();
            AsteriskChar = '*';
            AutoRemoveProjectiles = true;
            AutoResetStaticText = false;
            JapaneseStyleOption.Initialize();
            PlayerUtil.Initialize();
        }

        internal static void PrepareMod(string modName)
        {
            ModInfo info = ModInfo.Get(modName);
            Debug.Log("AsteriskMod TargetVersion: " + Asterisk.ConvertFromModVersion(info.targetVersion));
            ModTarget_AsteriskVersion = info.targetVersion;
            LuaCodeStyle = CodeStyle.Get(modName);
            AsteriskChar = '*';
            AutoRemoveProjectiles = true;
            AutoResetStaticText = false;
            Lang.PrepareMod(modName);
        }

        internal static void AwakeMod()
        {
            CYFEngine.Initialize();
            UIController.InitalizeButtonManager();
            PlayerUtil.Initialize();
            ArenaUI.Initialize();
            JapaneseStyleOption.Reset();
        }

        internal static void Reset()
        {
            CYFEngine.Reset();
            Initialize();
            Lang.Reset();
        }
    }
}
