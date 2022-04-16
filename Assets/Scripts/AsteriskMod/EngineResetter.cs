using UnityEngine;

namespace AsteriskMod
{
    public class EngineResetter
    {
        public static Asterisk.Versions ModTarget_AsteriskVersion { get; private set; }

        public static void SetTargetAsteriskVersion(Asterisk.Versions version)
        {
            Debug.Log("AsteriskMod TargetVersion: " + Asterisk.ConvertToString(version));
            ModTarget_AsteriskVersion = version;
        }

        public static void Initialize()
        {
            CYFEngine.Initialize();
            UIController.InitalizeButtonManager();
        }

        public static void Revert()
        {
            CYFEngine.Reset();
        }
    }
}
