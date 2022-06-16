using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SimScreenMenu : MonoBehaviour
    {
        private static bool _uniqueCheck;

        internal static Button BackButton;

        private void Awake()
        {
            if (_uniqueCheck) throw new Exception("SimScreenMenuが複数存在します。");
            _uniqueCheck = true;

            BackButton = transform.Find("MenuNameLabel").Find("BackButton").GetComponent<Button>();
        }

        private void Start()
        {
            UnityButtonUtil.AddListener(BackButton, () =>
            {
                if (AnimFrameCounter.IsRunningAnimation) return;
                SimMenuWindowManager.ChangePage(SimMenuWindowManager.DisplayingSimMenu.Screen, SimMenuWindowManager.DisplayingSimMenu.Main);
            });
        }
    }
}
