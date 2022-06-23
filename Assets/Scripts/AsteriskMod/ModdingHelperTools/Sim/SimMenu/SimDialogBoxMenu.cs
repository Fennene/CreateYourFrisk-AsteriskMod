using AsteriskMod.ModdingHelperTools.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal class SimDialogBoxMenu : MonoBehaviour
    {
        internal static SimDialogBoxMenu Instance;

        internal Button BackButton;

        private void Awake()
        {
            BackButton = transform.Find("MenuNameLabel").Find("BackButton").GetComponent<Button>();
            AwakeCover();
            AwakePosition(transform.Find("Pos"));
            AwakeSize(transform.Find("Size"));
            Instance = this;
        }

        private void Start()
        {
            UnityButtonUtil.AddListener(BackButton, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.DialogBox, SimMenuWindowManager.DisplayingSimMenu.Main);
            });
            StartCover();
            StartPosition();
            StartSize();
        }

        // --------------------------------------------------------------------------------

        private Image DisabledCover;

        private void AwakeCover() { DisabledCover = transform.Find("Cover").GetComponent<Image>(); }

        private void StartCover() { DisabledCover.enabled = true; }

        internal void UpdateState()
        {
            Position_x.InputField.text = FakeArenaManager.instance.desiredX.ToString();
            Position_y.InputField.text = FakeArenaManager.instance.desiredY.ToString();
            Size_width.InputField.text = FakeArenaManager.instance.desiredWidth.ToString();
            Size_height.InputField.text = FakeArenaManager.instance.desiredHeight.ToString();
            DisabledCover.enabled = (SimInstance.BattleSimulator.CurrentState != UIController.UIState.DEFENDING);
        }

        // --------------------------------------------------------------------------------

        private CYFInputField Position_x;
        private CYFInputField Position_y;
        private Toggle Position_immediate;
        private Button Position_Run;
        //private Image Position_RunButton_Image;

        private void AwakePosition(Transform parent)
        {
            Position_x               = parent.Find("x")        .GetComponent<CYFInputField>();
            Position_y               = parent.Find("y")        .GetComponent<CYFInputField>();
            Position_immediate       = parent.Find("Immediate").GetComponent<Toggle>();
            Position_Run             = parent.Find("MoveTo")   .GetComponent<Button>();
            //Position_RunButton_Image = parent.Find("MoveTo")   .GetComponent<Image>();
        }

        private void StartPosition()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Position_x, (value) => CYFInputFieldUtil.ShowInputError(Position_x, ParseUtil.CanParseFloat(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(Position_y, (value) => CYFInputFieldUtil.ShowInputError(Position_y, ParseUtil.CanParseFloat(value)));
            CYFInputFieldUtil.AddListener_OnEndEdit(Position_x, (value) =>
            {
                if (ParseUtil.CanParseFloat(value)) return;
                Position_x.InputField.text = FakeArenaManager.instance.desiredX.ToString();
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Position_y, (value) =>
            {
                if (ParseUtil.CanParseFloat(value)) return;
                Position_y.InputField.text = FakeArenaManager.instance.desiredY.ToString();
            });
            UnityButtonUtil.AddListener(Position_Run, () =>
            {
                if (FakeArenaManager.instance.isMoveInProgress() || FakeArenaManager.instance.isResizeInProgress()) return;
                float x = ParseUtil.GetFloat(Position_x.InputField.text);
                float y = ParseUtil.GetFloat(Position_y.InputField.text);
                if (!Position_immediate.isOn)
                {
                    FakeArenaManager.instance.MoveTo(x, y, false);
                }
                else
                {
                    FakeArenaManager.instance.MoveToImmediate(x, y, false);
                }
                Position_x.InputField.text = FakeArenaManager.instance.desiredX.ToString();
                Position_y.InputField.text = FakeArenaManager.instance.desiredY.ToString();
            });
            Position_x.InputField.text = FakeArenaManager.instance.desiredX.ToString();
            Position_y.InputField.text = FakeArenaManager.instance.desiredY.ToString();
        }

        // --------------------------------------------------------------------------------

        private CYFInputField Size_width;
        private CYFInputField Size_height;
        private Toggle Size_immediate;
        private Button Size_Run;

        private void AwakeSize(Transform parent)
        {
            Size_width     = parent.Find("width")    .GetComponent<CYFInputField>();
            Size_height    = parent.Find("height")   .GetComponent<CYFInputField>();
            Size_immediate = parent.Find("Immediate").GetComponent<Toggle>();
            Size_Run       = parent.Find("Resize")   .GetComponent<Button>();
        }

        private void StartSize()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Size_width,  (value) => CYFInputFieldUtil.ShowInputError(Size_width,  ParseUtil.CanParseFloat(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(Size_height, (value) => CYFInputFieldUtil.ShowInputError(Size_height, ParseUtil.CanParseFloat(value)));
            CYFInputFieldUtil.AddListener_OnEndEdit(Size_width, (value) =>
            {
                if (ParseUtil.CanParseFloat(value)) return;
                Size_width.InputField.text = FakeArenaManager.instance.desiredWidth.ToString();
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Size_height, (value) =>
            {
                if (ParseUtil.CanParseFloat(value)) return;
                Size_height.InputField.text = FakeArenaManager.instance.desiredHeight.ToString();
            });
            UnityButtonUtil.AddListener(Size_Run, () =>
            {
                if (FakeArenaManager.instance.isMoveInProgress() || FakeArenaManager.instance.isResizeInProgress()) return;
                float width  = ParseUtil.GetFloat(Size_width .InputField.text);
                float height = ParseUtil.GetFloat(Size_height.InputField.text);
                if (!Size_immediate.isOn)
                {
                    FakeArenaManager.instance.Resize(width, height);
                }
                else
                {
                    FakeArenaManager.instance.ResizeImmediate(width, height);
                }
                Size_width .InputField.text = FakeArenaManager.instance.desiredWidth .ToString();
                Size_height.InputField.text = FakeArenaManager.instance.desiredHeight.ToString();
            });
            Size_width.InputField.text = FakeArenaManager.instance.desiredWidth.ToString();
            Size_height.InputField.text = FakeArenaManager.instance.desiredHeight.ToString();
        }

        // --------------------------------------------------------------------------------

        internal static void Dispose()
        {
            Instance = null;
        }
    }
}
