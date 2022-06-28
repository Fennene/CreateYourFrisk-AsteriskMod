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
        private bool scriptChange;

        internal void Awake()
        {
            BackButton = transform.Find("MenuNameLabel").Find("BackButton").GetComponent<Button>();
            AwakePlayerStatus(transform.Find("PlayerStatusWindow"));
            Transform objControlParent = transform.Find("ObjControllerWindow").Find("View");
            AwakeHPUIPos(objControlParent.Find("HPUIPos"));
            AwakeHPTextPos(objControlParent.Find("HPTextPos"));
            scriptChange = false;
        }

        internal void Start()
        {
            UnityButtonUtil.AddListener(BackButton, () =>
            {
                if (AnimFrameCounter.Instance.IsRunningAnimation) return;
                SimMenuWindowManager.Instance.ChangePage(SimMenuWindowManager.DisplayingSimMenu.PlayerStatus, SimMenuWindowManager.DisplayingSimMenu.Main);
            });
            StartPlayerStatus();
            StartHPUIPos();
            StartHPTextPos();
        }

        // --------------------------------------------------------------------------------

        private CYFInputField LV;
        private CYFInputField HP;
        private CYFInputField MaxHP;
        private Toggle HPBarLimited;

        private void AwakePlayerStatus(Transform valueParent)
        {
            LV = valueParent.Find("LV").Find("lv").GetComponent<CYFInputField>();
            Transform hpParent = valueParent.Find("HP");
            HP           = hpParent.Find("hp")     .GetComponent<CYFInputField>();
            MaxHP        = hpParent.Find("maxhp")  .GetComponent<CYFInputField>();
            HPBarLimited = hpParent.Find("limited").GetComponent<Toggle>();
        }

        private void StartPlayerStatus()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(LV   , (value) => CYFInputFieldUtil.ShowInputError(LV   , ParseUtil.CanParseInt(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(HP   , (value) => CYFInputFieldUtil.ShowInputError(HP   , ParseUtil.CanParseInt(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(MaxHP, (value) => CYFInputFieldUtil.ShowInputError(MaxHP, ParseUtil.CanParseInt(value)));
            CYFInputFieldUtil.AddListener_OnEndEdit(LV, (value) =>
            {
                if (!ParseUtil.CanParseInt(value))
                {
                    LV.InputField.text = SimInstance.BattleSimulator.PlayerLV.ToString();
                    return;
                }
                SimInstance.BattleSimulator.PlayerLV = ParseUtil.GetInt(value);
                MaxHP.InputField.text = SimInstance.BattleSimulator.PlayerMaxHP.ToString();
                HP.InputField.text = SimInstance.BattleSimulator.PlayerHP.ToString();
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(HP, (value) =>
            {
                if (!ParseUtil.CanParseInt(value))
                {
                    HP.InputField.text = SimInstance.BattleSimulator.PlayerHP.ToString();
                    return;
                }
                SimInstance.BattleSimulator.PlayerHP = ParseUtil.GetInt(value);
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(MaxHP, (value) =>
            {
                if (!ParseUtil.CanParseInt(value))
                {
                    MaxHP.InputField.text = SimInstance.BattleSimulator.PlayerMaxHP.ToString();
                    return;
                }
                SimInstance.BattleSimulator.PlayerMaxHP = ParseUtil.GetInt(value);
                HP.InputField.text = SimInstance.BattleSimulator.PlayerHP.ToString();
            });
            UnityToggleUtil.AddListener(HPBarLimited, (value) => { FakePlayerHPStat.instance.LifeBar.limited = value; });
        }

        // --------------------------------------------------------------------------------

        private CYFInputField HPUIPos_x;
        private CYFInputField HPUIPos_y;

        private void AwakeHPUIPos(Transform hpUIPosParent)
        {
            HPUIPos_x = hpUIPosParent.Find("x").GetComponent<CYFInputField>();
            HPUIPos_y = hpUIPosParent.Find("y").GetComponent<CYFInputField>();
        }

        private void StartHPUIPos()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(HPUIPos_x, (value) => CYFInputFieldUtil.ShowInputError(HPUIPos_x, ParseUtil.CanParseFloat(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(HPUIPos_y, (value) => CYFInputFieldUtil.ShowInputError(HPUIPos_y, ParseUtil.CanParseFloat(value)));
            CYFInputFieldUtil.AddListener_OnEndEdit(HPUIPos_x, (value) =>
            {
                if (!ParseUtil.CanParseFloat(value))
                {
                    HPUIPos_x.InputField.text = FakePlayerUtil.Instance.hpx.ToString();
                    return;
                }
                FakePlayerUtil.Instance.hpx = ParseUtil.GetFloat(value);
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(HPUIPos_y, (value) =>
            {
                if (!ParseUtil.CanParseFloat(value))
                {
                    HPUIPos_y.InputField.text = FakePlayerUtil.Instance.hpy.ToString();
                    return;
                }
                FakePlayerUtil.Instance.hpy = ParseUtil.GetFloat(value);
            });
        }

        // --------------------------------------------------------------------------------

        private Text HPTextPos_xLabel;
        private CYFInputField HPTextPos_x;
        private Text HPTextPos_yLabel;
        private CYFInputField HPTextPos_y;
        private Toggle HPTextPos_Abs;
        private Toggle HPTextPos_Override;
        private Button HPTextPos_UpdatePos;

        private void AwakeHPTextPos(Transform hpTextPosParent)
        {
            HPTextPos_xLabel    = hpTextPosParent.Find("xLabel")         .GetComponent<Text>();
            HPTextPos_x         = hpTextPosParent.Find("x")              .GetComponent<CYFInputField>();
            HPTextPos_yLabel    = hpTextPosParent.Find("yLabel")         .GetComponent<Text>();
            HPTextPos_y         = hpTextPosParent.Find("y")              .GetComponent<CYFInputField>();
            HPTextPos_Abs       = hpTextPosParent.Find("Abs")            .GetComponent<Toggle>();
            HPTextPos_Override  = hpTextPosParent.Find("ControlOverride").GetComponent<Toggle>();
            HPTextPos_UpdatePos = hpTextPosParent.Find("UpdatePos")      .GetComponent<Button>();
        }

        private void StartHPTextPos()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(HPTextPos_x, (value) => CYFInputFieldUtil.ShowInputError(HPTextPos_x, ParseUtil.CanParseInt(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(HPTextPos_y, (value) => CYFInputFieldUtil.ShowInputError(HPTextPos_y, ParseUtil.CanParseInt(value)));
            CYFInputFieldUtil.AddListener_OnEndEdit(HPTextPos_x, (value) =>
            {
                if (!ParseUtil.CanParseInt(value))
                {
                    HPTextPos_x.InputField.text = (HPTextPos_Abs.isOn ? FakePlayerHPStat.instance.LifeTextMan.absx : FakePlayerHPStat.instance.LifeTextMan.x).ToString();
                    return;
                }
                if (HPTextPos_Abs.isOn)
                {
                    FakePlayerHPStat.instance.LifeTextMan.absx = ParseUtil.GetInt(value);
                }
                else
                {
                    FakePlayerHPStat.instance.LifeTextMan.x = ParseUtil.GetInt(value);
                }
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(HPTextPos_y, (value) =>
            {
                if (!ParseUtil.CanParseInt(value))
                {
                    HPTextPos_y.InputField.text = (HPTextPos_Abs.isOn ? FakePlayerHPStat.instance.LifeTextMan.absy : FakePlayerHPStat.instance.LifeTextMan.y).ToString();
                    return;
                }
                if (HPTextPos_Abs.isOn)
                {
                    FakePlayerHPStat.instance.LifeTextMan.absy = ParseUtil.GetInt(value);
                }
                else
                {
                    FakePlayerHPStat.instance.LifeTextMan.y = ParseUtil.GetInt(value);
                }
            });
            UnityToggleUtil.AddListener(HPTextPos_Abs, (value) =>
            {
                HPTextPos_xLabel.text = value ? "absx:" : "x:";
                HPTextPos_yLabel.text = value ? "absy:" : "y:";
                HPTextPos_x.InputField.text = (value ? FakePlayerHPStat.instance.LifeTextMan.absx : FakePlayerHPStat.instance.LifeTextMan.x).ToString();
                HPTextPos_y.InputField.text = (value ? FakePlayerHPStat.instance.LifeTextMan.absy : FakePlayerHPStat.instance.LifeTextMan.y).ToString();
            });
            UnityToggleUtil.AddListener(HPTextPos_Override, (value) => { FakePlayerHPStat.instance.textOverride = value; });
            UnityButtonUtil.AddListener(HPTextPos_UpdatePos, () => { FakePlayerHPStat.instance.SetTextPosition(true); });
        }
    }
}
