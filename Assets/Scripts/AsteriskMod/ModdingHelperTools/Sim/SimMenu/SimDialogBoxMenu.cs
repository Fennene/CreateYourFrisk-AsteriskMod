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
            AwakePosition(transform.Find("PosLabel"), transform.Find("Pos"));
            AwakeSize(transform.Find("SizeLabel"), transform.Find("Size"));
            Instance = this;
        }

        private void Start()
        {
            UnityButtonUtil.AddListener(BackButton, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.DialogBox, SimMenuWindowManager.DisplayingSimMenu.Main);
            });
            StartPosition();
            StartSize();
        }

        internal void UpdateState()
        {
            if (SimInstance.BattleSimulator.CurrentState == UIController.UIState.DEFENDING)
            {
                AddPositionListenersAsArena();
                AddSizeListenersAsArena();
            }
            else
            {
                AddPositionListenersAsArenaUtil();
                AddSizeListenersAsArenaUtil();
            }
        }

        private Text Position_Label_Main;
        private Text Position_Label_Over;
        private CYFInputField Position_x;
        private CYFInputField Position_y;
        private Toggle Position_immediate;
        private Button Position_Run;
        //private Image Position_RunButton_Image;

        private void AwakePosition(Transform textParent, Transform mainParent)
        {
            Position_Label_Main = textParent.Find("Text")    .GetComponent<Text>();
            Position_Label_Over = textParent.Find("TextOver").GetComponent<Text>();
            Position_x               = mainParent.Find("x")        .GetComponent<CYFInputField>();
            Position_y               = mainParent.Find("y")        .GetComponent<CYFInputField>();
            Position_immediate       = mainParent.Find("Immediate").GetComponent<Toggle>();
            Position_Run             = mainParent.Find("MoveTo")   .GetComponent<Button>();
            //Position_RunButton_Image = mainParent.Find("MoveTo")   .GetComponent<Image>();
        }

        private void StartPosition()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Position_x, (value) => CYFInputFieldUtil.ShowInputError(Position_x, ParseUtil.CanParseFloat(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(Position_y, (value) => CYFInputFieldUtil.ShowInputError(Position_y, ParseUtil.CanParseFloat(value)));
            AddPositionListenersAsArenaUtil();
        }

        private void AddPositionListenersAsArena()
        {
            Position_Label_Main.text = "Position\n[Arena.MoveTo()]";
            Position_Label_Over.text = "Position\n";
            Position_Run.GetComponentInChildren<Text>().text = "MoveTo";
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

        private void AddPositionListenersAsArenaUtil()
        {
            Position_Label_Main.text = "Position (offset)\nArenaUtil.SetOffset()";
            Position_Label_Over.text = "Position (offset)\n";
            Position_Run.GetComponentInChildren<Text>().text = "SetOffset";
            CYFInputFieldUtil.AddListener_OnEndEdit(Position_x, (value) =>
            {
                if (ParseUtil.CanParseFloat(value)) return;
                Position_x.InputField.text = FakeArenaUtil.Instance.ArenaOffset.x.ToString();
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Position_y, (value) =>
            {
                if (ParseUtil.CanParseFloat(value)) return;
                Position_y.InputField.text = FakeArenaUtil.Instance.ArenaOffset.y.ToString();
            });
            UnityButtonUtil.AddListener(Position_Run, () =>
            {
                if (FakeArenaManager.instance.isMoveInProgress() || FakeArenaManager.instance.isResizeInProgress()) return;
                float x = ParseUtil.GetFloat(Position_x.InputField.text);
                float y = ParseUtil.GetFloat(Position_y.InputField.text);
                FakeArenaUtil.Instance.ArenaOffset = new Vector2(x, y);
                Position_x.InputField.text = FakeArenaUtil.Instance.ArenaOffset.x.ToString();
                Position_y.InputField.text = FakeArenaUtil.Instance.ArenaOffset.y.ToString();
            });
            Position_x.InputField.text = FakeArenaUtil.Instance.ArenaOffset.x.ToString();
            Position_y.InputField.text = FakeArenaUtil.Instance.ArenaOffset.y.ToString();
        }

        // --------------------------------------------------------------------------------

        private Text Size_Label_Main;
        private Text Size_Label_Over;
        private CYFInputField Size_width;
        private CYFInputField Size_height;
        private Toggle Size_immediate;
        private Button Size_Run;

        private void AwakeSize(Transform textParent, Transform mainParent)
        {
            Size_Label_Main = textParent.Find("Text")    .GetComponent<Text>();
            Size_Label_Over = textParent.Find("TextOver").GetComponent<Text>();
            Size_width     = mainParent.Find("width")    .GetComponent<CYFInputField>();
            Size_height    = mainParent.Find("height")   .GetComponent<CYFInputField>();
            Size_immediate = mainParent.Find("Immediate").GetComponent<Toggle>();
            Size_Run       = mainParent.Find("Resize")   .GetComponent<Button>();
        }

        private void StartSize()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Size_width,  (value) => CYFInputFieldUtil.ShowInputError(Size_width,  ParseUtil.CanParseFloat(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(Size_height, (value) => CYFInputFieldUtil.ShowInputError(Size_height, ParseUtil.CanParseFloat(value)));
            AddSizeListenersAsArenaUtil();
        }

        private void AddSizeListenersAsArena()
        {
            Size_Label_Main.text = "Size\n[Arena.Resize()]";
            Size_Label_Over.text = "Size\n";
            Size_Run.GetComponentInChildren<Text>().text = "Resize";
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
            Size_width .InputField.text = FakeArenaManager.instance.desiredWidth .ToString();
            Size_height.InputField.text = FakeArenaManager.instance.desiredHeight.ToString();
        }

        private void AddSizeListenersAsArenaUtil()
        {
            Size_Label_Main.text = "RelativeSize\nArenaUtil.SetRelative...";
            Size_Label_Over.text = "RelativeSize\n";
            Size_Run.GetComponentInChildren<Text>().text = "SetRelativeSize";
            CYFInputFieldUtil.AddListener_OnEndEdit(Size_width, (value) =>
            {
                if (ParseUtil.CanParseFloat(value)) return;
                Size_width.InputField.text = FakeArenaUtil.Instance.ArenaOffsetSize.x.ToString();
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Size_height, (value) =>
            {
                if (ParseUtil.CanParseFloat(value)) return;
                Size_height.InputField.text = FakeArenaUtil.Instance.ArenaOffsetSize.y.ToString();
            });
            UnityButtonUtil.AddListener(Size_Run, () =>
            {
                if (FakeArenaManager.instance.isMoveInProgress() || FakeArenaManager.instance.isResizeInProgress()) return;
                float width  = ParseUtil.GetFloat(Size_width .InputField.text);
                float height = ParseUtil.GetFloat(Size_height.InputField.text);
                FakeArenaUtil.Instance.SetArenaOffsetSize(width, height, Size_immediate.isOn);
                Size_width .InputField.text = FakeArenaUtil.Instance.ArenaOffsetSize.x.ToString();
                Size_height.InputField.text = FakeArenaUtil.Instance.ArenaOffsetSize.y.ToString();
            });
            Size_width .InputField.text = FakeArenaUtil.Instance.ArenaOffsetSize.x.ToString();
            Size_height.InputField.text = FakeArenaUtil.Instance.ArenaOffsetSize.y.ToString();
        }

        // --------------------------------------------------------------------------------

        internal static void Dispose()
        {
            Instance = null;
        }
    }
}
