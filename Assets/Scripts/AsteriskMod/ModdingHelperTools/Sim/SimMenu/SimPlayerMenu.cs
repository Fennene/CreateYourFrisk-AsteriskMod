using AsteriskMod.ModdingHelperTools.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SimPlayerMenu : MonoBehaviour
    {
        internal Button BackButton;

        internal void Awake()
        {
            BackButton = transform.Find("MenuNameLabel").Find("BackButton").GetComponent<Button>();
        }

        internal void Start()
        {
            UnityButtonUtil.AddListener(BackButton, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.PlayerStatus, SimMenuWindowManager.DisplayingSimMenu.Main);
            });
        }

        // --------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------

        // --------------------------------------------------------------------------------
    }
}
