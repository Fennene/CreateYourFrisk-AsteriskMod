using AsteriskMod.ModdingHelperTools.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class CMFEWindowManager : MonoBehaviour
    {
        internal static CMFEWindowManager Insatnce;

        private RectTransform self;

        internal enum ParameterWindows
        {
            TargetVersion,
            Title,
            Subtitle,
            Description,
            Language,
            File
        }

        private Text title;
        private Text description;

        private GameObject targetVersionEditor;
        private GameObject textEditor;
        private bool scriptChange;

        private Button Cancel;
        private Button Delete;
        private Button Revert;
        private Button Accept;

        private bool animationRequester;
        private bool openAnim;

        private void Awake()
        {
            self = GetComponent<RectTransform>();

            title       = transform.Find("Title").Find("TitleLabel") .GetComponent<Text>();
            description = transform.Find("Main") .Find("Description").GetComponent<Text>();

            targetVersionEditor = transform.Find("Main").Find("TargetVersionDropDown").gameObject;
            textEditor          = transform.Find("Main").Find("TextEditor")           .gameObject;
            scriptChange = false;

            Cancel = transform.Find("Cancel").GetComponent<Button>();
            Delete = transform.Find("Delete").GetComponent<Button>();
            Revert = transform.Find("Revert").GetComponent<Button>();
            Accept = transform.Find("Accept").GetComponent<Button>();

            animationRequester = false;
            openAnim = false;

            Insatnce = this;
        }

        private void CloseWindow()
        {
            if (AnimFrameCounter.Instance.IsRunningAnimation) return;
            openAnim = false;
            animationRequester = true;
            AnimFrameCounter.Instance.StartAnimation();
        }

        private void Start()
        {
            self.anchoredPosition = new Vector2(0, -480);
            //UnityButtonUtil.AddListener(Cancel, CloseWindow);

            AddListeners();
        }

        private void Update()
        {
            if (Insatnce == null) return;
            if (!AnimFrameCounter.Instance.IsRunningAnimation) return;
            if (!animationRequester) return;
            if (AnimFrameCounter.Instance.CurrentFrame == 16)
            {
                self.anchoredPosition = new Vector2(0, openAnim ? 0 : -480);
                AnimFrameCounter.Instance.EndAnimation();
                animationRequester = false;
                return;
            }
            self.anchoredPosition += new Vector2(0, openAnim ? 30 : -30);
        }

        internal static void Dispose() { Insatnce = null; }

        internal void OpenWindow(ParameterWindows target)
        {
            if (AnimFrameCounter.Instance.IsRunningAnimation) return;
            SetEditingParameter(target);
            openAnim = true;
            animationRequester = true;
            AnimFrameCounter.Instance.StartAnimation();
        }


        private int ConvertToDropDownValue(Asterisk.Versions version)
        {
            switch (version)
            {
                case Asterisk.Versions.TakeNewStepUpdate:
                    return 0;
                case Asterisk.Versions.QOLUpdate:
                    return 1;
                default:
                    return 2;
            }
        }

        private void AddListeners()
        {
            targetVersionEditor.GetComponent<Dropdown>().onValueChanged.RemoveAllListeners();
            targetVersionEditor.GetComponent<Dropdown>().onValueChanged.AddListener((value) =>
            {
                if (scriptChange)
                {
                    CMFEButtonManager.Instance.UpdateParameters();
                    return;
                }
                switch (value)
                {
                    case 1:
                        CMFEButtonManager.CurrentModInfo.targetVersion = Asterisk.Versions.QOLUpdate;
                        break;
                    case 2:
                        CMFEButtonManager.CurrentModInfo.targetVersion = Asterisk.Versions.UtilUpdate;
                        break;
                    default:
                        CMFEButtonManager.CurrentModInfo.targetVersion = Asterisk.Versions.TakeNewStepUpdate;
                        break;
                }
                CMFEButtonManager.Instance.UpdateParameters();
            });
        }

        private void SetEditingParameter(ParameterWindows parameter)
        {
            scriptChange = true;
            string t = "Error!";
            string d = "No Description";
            targetVersionEditor.SetActive(false);
            textEditor.SetActive(false);
            switch (parameter)
            {
                case ParameterWindows.TargetVersion:
                    t = "AsteriskMod's Target Version";
                    d = "Sets the mod's working AsteriskMod version.";
                    targetVersionEditor.SetActive(true);
                    targetVersionEditor.GetComponent<Dropdown>().value = ConvertToDropDownValue(CMFEButtonManager.CurrentModInfo.targetVersion);
                    UnityButtonUtil.AddListener(Cancel, () =>
                    {
                        CMFEButtonManager.CurrentModInfo.targetVersion = CMFEButtonManager.OriginalModInfo.targetVersion;
                        targetVersionEditor.GetComponent<Dropdown>().value = ConvertToDropDownValue(CMFEButtonManager.CurrentModInfo.targetVersion);
                        CloseWindow();
                    });
                    UnityButtonUtil.AddListener(Delete, () =>
                    {
                        CMFEButtonManager.CurrentModInfo.targetVersion = Asterisk.Versions.TakeNewStepUpdate;
                        targetVersionEditor.GetComponent<Dropdown>().value = ConvertToDropDownValue(CMFEButtonManager.CurrentModInfo.targetVersion);
                        CloseWindow();
                    });
                    UnityButtonUtil.AddListener(Revert, () =>
                    {
                        CMFEButtonManager.CurrentModInfo.targetVersion = CMFEButtonManager.OriginalModInfo.targetVersion;
                        targetVersionEditor.GetComponent<Dropdown>().value = ConvertToDropDownValue(CMFEButtonManager.CurrentModInfo.targetVersion);
                    });
                    UnityButtonUtil.AddListener(Accept, () =>
                    {
                        CMFEButtonManager.OriginalModInfo.targetVersion = CMFEButtonManager.CurrentModInfo.targetVersion;
                        targetVersionEditor.GetComponent<Dropdown>().value = ConvertToDropDownValue(CMFEButtonManager.CurrentModInfo.targetVersion);
                        CMFEButtonManager.Instance.WarningText.enabled = true;
                        CloseWindow();
                    });
                    break;
                case ParameterWindows.Title:
                    t = "Mod's Title";
                    break;
                case ParameterWindows.Subtitle:
                    t = "Mod's Subtitle";
                    break;
                case ParameterWindows.Description:
                    t = "Mod's Description";
                    break;
                case ParameterWindows.Language:
                    t = "Supporting Language Override";
                    break;
                case ParameterWindows.File:
                    t = "Accept Change or Manages File";
                    d = "Cancel: Closes this window.\nDelete: Deletes info.cyfmod\nRevert: Reverts all changes\nAccept: Save changes.\n\n!WARNING! commentout will be deleted when saves change.";
                    UnityButtonUtil.AddListener(Cancel, () =>
                    {
                        CloseWindow();
                    });
                    UnityButtonUtil.AddListener(Delete, () =>
                    {
                        LegacyModInfo.DeleteFile(FakeStaticInits.ENCOUNTER);
                        CMFEButtonManager.Instance.realModInfo = new LegacyModInfo();
                        CMFEButtonManager.Instance.realModInfo.targetVersion = Asterisk.Versions.TakeNewStepUpdate;
                        CMFEButtonManager.Instance.editing = CMFEButtonManager.Instance.realModInfo.Clone();
                        CMFEButtonManager.Instance.WarningText.enabled = false;
                        CMFEButtonManager.Instance.UpdateParameters();
                        CloseWindow();
                    });
                    UnityButtonUtil.AddListener(Revert, () =>
                    {
                        CMFEButtonManager.Instance.editing = CMFEButtonManager.Instance.realModInfo.Clone();
                        CMFEButtonManager.Instance.WarningText.enabled = false;
                        CMFEButtonManager.Instance.UpdateParameters();
                        CloseWindow();
                    });
                    UnityButtonUtil.AddListener(Accept, () =>
                    {
                        CMFEButtonManager.Instance.realModInfo.Write(FakeStaticInits.ENCOUNTER);
                        CMFEButtonManager.Instance.editing = CMFEButtonManager.Instance.realModInfo.Clone();
                        CMFEButtonManager.Instance.WarningText.enabled = false;
                        CMFEButtonManager.Instance.UpdateParameters();
                        CloseWindow();
                    });
                    break;
            }
            title.text = t;
            description.text = d;
            scriptChange = false;
        }
    }
}
