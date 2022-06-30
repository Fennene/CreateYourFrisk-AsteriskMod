using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.UnityUI
{
    [DefaultExecutionOrder(-100)]
    public class FrameCounter : MonoBehaviour
    {
        private bool _counting;
        private uint _frame;

        private void Awake()
        {
            if (Instance != null) throw new DuplicateUIException("１つのシーンに複数のFrameCounterコンポーネントが存在します。");
            _counting = false;
            _frame = 0;
            Instance = this;
        }

        private void Update()
        {
            if (!_counting) return;
            _frame++;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private static FrameCounter Instance;

        public static bool Exists { get { return Instance != null; } }

        public static bool IsCounting { get { return Instance._counting; } }

        public static uint CurrentFrame { get { return Instance._frame; } }

        public static void StartCount()
        {
            Instance._frame = 0;
            Instance._counting = true;
        }

        public static void StopCount()
        {
            Instance._counting = false;
            Instance._frame = 0;
        }
    }
}
