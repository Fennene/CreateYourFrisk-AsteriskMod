using UnityEngine;

namespace AsteriskMod
{
    public class EngineResetter
    {
        public static Asterisk.Versions ModTarget_AsteriskVersion { get; private set; }

        public static bool Japanese { get; private set; }

        public static void SetTargetAsteriskVersion(Asterisk.Versions version)
        {
            Debug.Log("AsteriskMod TargetVersion: " + Asterisk.ConvertToString(version));
            ModTarget_AsteriskVersion = version;
        }

        public static void SetJapaneseMode(bool active)
        {
            if (Japanese == active) return;
            Japanese = active;
            PlayerNameText.instance.SetJP(active);
        }

        public static void Initialize()
        {
            CYFEngine.Initialize();
            UIController.InitalizeButtonManager();
            if (Asterisk.language == Languages.Japanese) Japanese = true;
        }

        public static void Revert()
        {
            CYFEngine.Reset();
            Japanese = false;
        }
    }
}
