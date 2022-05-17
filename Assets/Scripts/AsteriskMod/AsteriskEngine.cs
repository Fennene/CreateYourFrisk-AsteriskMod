using UnityEngine;

namespace AsteriskMod
{
    public class AsteriskEngine
    {
        public static Asterisk.Versions ModTarget_AsteriskVersion { get; private set; }

        public static void SetTargetAsteriskVersion(Asterisk.Versions version)
        {
            Debug.Log("AsteriskMod TargetVersion: " + Asterisk.ConvertFromModVersion(version));
            ModTarget_AsteriskVersion = version;
        }

        public static char AsteriskChar { get; set; }

        public class JapaneseStyleOption
        {
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
            public static void SetJPNameActive(bool active) { SetJapaneseNameActive(active); }

            private static bool _autoJPfont;
            public static bool AutoJPFont
            {
                get { return _autoJPfont; }
                set { SetAutoJapaneseFontActive(value); }
            }
            public static void SetAutoJapaneseFontActive(bool active)
            {
                if (_autoJPfont == active) return;
                _autoJPfont = active;
                AsteriskChar = active ? '＊' : '*';
            }

            /**
            private static bool _autoFullWidthAsterisk;
            public static bool AsteriskChanged
            {
                get { return _autoFullWidthAsterisk; }
                set { ChangeAsterisk(value); }
            }
            public static void ChangeAsterisk(bool active)
            {
                if (_autoFullWidthAsterisk == active) return;
                _autoFullWidthAsterisk = active;
                AsteriskChar = active ? '＊' : '*';
            }
            */

            internal static void Reset()
            {
                _jpName = false;
                _autoJPfont = false;
                //_autoFullWidthAsterisk = false;
            }
        }

        public static void SetJapaneseMode(bool active)
        {
            JapaneseStyleOption.SetJapaneseNameActive(active);
            JapaneseStyleOption.SetAutoJapaneseFontActive(active);
        }

        public static bool AutoRemoveProjectiles { get; set; }

        public static void Initialize()
        {
            CYFEngine.Initialize();
            UIController.InitalizeButtonManager();
            AsteriskChar = '*';
            AutoRemoveProjectiles = true;
            JapaneseStyleOption.Reset();
            if (Asterisk.language == Languages.Japanese) SetJapaneseMode(true);
        }

        public static void Revert()
        {
            CYFEngine.Reset();
            AsteriskChar = '*';
            AutoRemoveProjectiles = true;
            //SetJapaneseMode(false);
            JapaneseStyleOption.Reset();
        }
    }
}
