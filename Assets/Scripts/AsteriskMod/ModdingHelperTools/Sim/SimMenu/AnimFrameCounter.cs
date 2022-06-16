using UnityEngine;

namespace AsteriskMod.ModdingHelperTools
{
    internal class AnimFrameCounter : MonoBehaviour
    {
        /**
        private static bool _uniqueCheck;
        private void Awake()
        {
            if (_uniqueCheck) throw new Exception("AnimFrameCounterが複数存在します。");
            _uniqueCheck = true;
        }
        */

        private static bool _anim;
        internal static bool IsRunningAnimation { get { return _anim; } }

        private static int _frameCounter;
        internal static int CurrentFrame { get { return _frameCounter; } }

        private void Awake()
        {
            _frameCounter = 0;
            _anim = false;
        }

        internal static void StartAnimation()
        {
            _frameCounter = 0;
            _anim = true;
        }

        private void Update()
        {
            if (!_anim) return;
            _frameCounter++;
        }

        internal static void EndAnimation()
        {
            _anim = false;
            _frameCounter = 0;
        }
    }
}
