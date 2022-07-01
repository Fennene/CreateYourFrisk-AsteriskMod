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

        internal LegacyModInfo realModInfo;
        internal LegacyModInfo editing;
        //internal bool notAccepted;

        internal static LegacyModInfo OriginalModInfo { get { return Instance.realModInfo; } }
        internal static LegacyModInfo CurrentModInfo { get { return Instance.editing; } }

        private void Awake()
        {
            realModInfo = LegacyModInfo.Get(FakeStaticInits.MODFOLDER);

            if (!LegacyModInfo.Exists(FakeStaticInits.MODFOLDER))
            {
                if (LegacyModInfo.CreateFile(FakeStaticInits.MODFOLDER))
                {
                    realModInfo = LegacyModInfo.Get(FakeStaticInits.MODFOLDER);
                }
                else
                {
                    realModInfo = new LegacyModInfo();
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
            UnityButtonUtil.AddListener(AcceptButton, () =>CMFEWindowManager.Insatnce.OpenWindow(CMFEWindowManager.ParameterWindows.File));

            UnityButtonUtil.AddListener(TargetVersionButton, () => CMFEWindowManager.Insatnce.OpenWindow(CMFEWindowManager.ParameterWindows.TargetVersion));
        }

        internal void UpdateParameters()
        {
            TargetVersionLabel.text = "Target Version:\n" + Asterisk.ConvertFromModVersion(CurrentModInfo.targetVersion);
            TargetVersionLabelShadow.text = TargetVersionLabel.text;
        }
    }
}
