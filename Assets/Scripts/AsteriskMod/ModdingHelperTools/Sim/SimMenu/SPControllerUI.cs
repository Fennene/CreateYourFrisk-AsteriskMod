using AsteriskMod.ModdingHelperTools.UI;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal static class SPControllerUI
    {
        private static Text Position_xLabel;
        private static CYFInputField Position_x;
        private static Text Position_yLabel;
        private static CYFInputField Position_y;
        private static Toggle Position_Abs;

        private static bool scriptChange;

        internal static void Awake(Transform controllerView)
        {
            Transform temp = controllerView.Find("Pos");
            Position_xLabel = temp.Find("xLabel").GetComponent<Text>();
            Position_x      = temp.Find("x")     .GetComponent<CYFInputField>();
            Position_yLabel = temp.Find("yLabel").GetComponent<Text>();
            Position_y      = temp.Find("y")     .GetComponent<CYFInputField>();
            Position_Abs    = temp.Find("Abs")   .GetComponent<Toggle>();

            scriptChange = false;
        }

        private static bool CanFloatParse(string value) { float _; return float.TryParse(value, out _); }

        internal static void Start()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Position_x, (value) => CYFInputFieldUtil.ShowInputError(Position_x, CanFloatParse(value)));
            CYFInputFieldUtil.AddListener_OnValueChanged(Position_y, (value) => CYFInputFieldUtil.ShowInputError(Position_y, CanFloatParse(value)));
            CYFInputFieldUtil.AddListener_OnEndEdit(Position_x, (value) =>
            {
                if (scriptChange) return;
                if (!CanFloatParse(value))
                {
                    scriptChange = true;
                    Position_x.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget(
                        (sprite) => { return Position_Abs.isOn ? sprite.absx : sprite.x; },
                        (bullet) => { return Position_Abs.isOn ? bullet.absx : bullet.x; }
                        )).ToString();
                    scriptChange = false;
                    return;
                }
                SimSprProjSimMenu.ActionToTarget(
                    (sprite) =>
                    {
                        if (Position_Abs.isOn) sprite.absx = ParseUtil.GetFloat(value);
                        else                   sprite.x =    ParseUtil.GetFloat(value);
                    },
                    (bullet) =>
                    {
                        if (Position_Abs.isOn) bullet.absx = ParseUtil.GetFloat(value);
                        else                   bullet.x =    ParseUtil.GetFloat(value);
                    }
                );
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Position_y, (value) =>
            {
                if (scriptChange) return;
                if (!CanFloatParse(value))
                {
                    scriptChange = true;
                    Position_y.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget(
                        (sprite) => { return Position_Abs.isOn ? sprite.absy : sprite.y; },
                        (bullet) => { return Position_Abs.isOn ? bullet.absy : bullet.y; }
                        )).ToString();
                    scriptChange = false;
                    return;
                }
                SimSprProjSimMenu.ActionToTarget(
                    (sprite) =>
                    {
                        if (Position_Abs.isOn) sprite.absy = ParseUtil.GetFloat(value);
                        else                   sprite.y =    ParseUtil.GetFloat(value);
                    },
                    (bullet) =>
                    {
                        if (Position_Abs.isOn) bullet.absy = ParseUtil.GetFloat(value);
                        else                   bullet.y =    ParseUtil.GetFloat(value);
                    }
                );
            });
        }

        internal static void UpdateData()
        {
            scriptChange = true;
            Position_x.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget(
                (sprite) => { return Position_Abs.isOn ? sprite.absx : sprite.x; },
                (bullet) => { return Position_Abs.isOn ? bullet.absx : bullet.x; }
                )).ToString();
            Position_y.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget(
                (sprite) => { return Position_Abs.isOn ? sprite.absy : sprite.y; },
                (bullet) => { return Position_Abs.isOn ? bullet.absy : bullet.y; }
                )).ToString();
            scriptChange = false;
        }
    }
}
