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

        internal ModInfo realModInfo;
        internal ModInfo editing;
        //internal bool notAccepted;

        internal static ModInfo OriginalModInfo { get { return Instance.realModInfo; } }
        internal static ModInfo CurrentModInfo { get { return Instance.editing; } }

        private void Awake()
        {
            realModInfo = ModInfo.Get(FakeStaticInits.MODFOLDER);

            if (!ModInfo.Exists(FakeStaticInits.MODFOLDER))
            {
                if (ModInfo.CreateFile(FakeStaticInits.MODFOLDER))
                {
                    realModInfo = ModInfo.Get(FakeStaticInits.MODFOLDER);
                }
                else
                {
                    realModInfo = new ModInfo();
                    realModInfo.targetVersion = Asterisk.Versions.TakeNewStepUpdate;
                }
            }

            if (realModInfo.targetVersion == Asterisk.Versions.Unknwon) realModInfo.targetVersion = Asterisk.Versions.TakeNewStepUpdate;

            editing = realModInfo.Clone();
            //notAccepted = false;

            Instance = this;

            UpdateParameters();
        }

        public Button ExitButton;
        public Button AcceptButton;
        public Text WarningText;

        public Button TargetVersionButton;
        public Text TargetVersionLabelShadow, TargetVersionLabel;

        private void Start()
        {
            WarningText.enabled = false;

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
            TargetVersionLabel.text = "Target Version:\n" + Asterisk.ConvertFromModVersion(CurrentModInfo.targetVersion);
            TargetVersionLabelShadow.text = TargetVersionLabel.text;
        }
    }
}
