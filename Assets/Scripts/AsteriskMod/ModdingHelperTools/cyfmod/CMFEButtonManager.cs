using AsteriskMod.ModdingHelperTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class CMFEButtonManager : MonoBehaviour
    {
        internal static CMFEButtonManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public Button ExitButton;
        public Button AcceptButton;

        public Button TargetVersionButton;
        public Text TargetVersionLabelShadow, TargetVersionLabel;

        private void Start()
        {
            UnityButtonUtil.AddListener(ExitButton, () =>
            {
                CMFEWindowManager.Dispose();
                Instance = null;
                AnimFrameCounter.Dispose();
                SceneManager.LoadScene("MHTMenu");
            });

            UnityButtonUtil.AddListener(TargetVersionButton, () => CMFEWindowManager.Insatnce.OpenWindow(CMFEWindowManager.ParameterWindows.TargetVersion));
        }

        internal void UpdateParameters()
        {
        }
    }
}
