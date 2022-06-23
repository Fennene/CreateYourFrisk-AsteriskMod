using UnityEngine;

namespace AsteriskMod.ModdingHelperTools
{
    internal class AnimFrameCounter : MonoBehaviour
    {
        internal static AnimFrameCounter Instance;

        private bool _anim;
        internal bool IsRunningAnimation { get { return _anim; } }

        private int _frameCounter;
        internal int CurrentFrame { get { return _frameCounter; } }

        private void Awake()
        {
            _frameCounter = 0;
            _anim = false;

            Instance = this;
        }

        internal void StartAnimation()
        {
            _frameCounter = 0;
            _anim = true;
        }

        private void Update()
        {
            if (!_anim) return;
            _frameCounter++;
        }

        internal void EndAnimation()
        {
            _anim = false;
            _frameCounter = 0;
        }

        internal static void Dispose()
        {
            Instance = null;
        }
    }
}
