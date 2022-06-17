using AsteriskMod.ModdingHelperTools.UI;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace AsteriskMod.ModdingHelperTools
{
    internal static class SPControllerUI
    {
        [ToDo] private static Image DisabledCover;
        private static bool scriptChange;

        internal static void Awake(Transform controllerView)
        {
            scriptChange = false;

            AwakeSprite(controllerView.Find("Sprite"));
            AwakePosition(controllerView.Find("Pos"));
        }

        internal static void Start()
        {
            StartSprite();
            StartPosition();
        }

        internal static void UpdateParameters()
        {
            if (SPTargetDelUI.TargetIndex < 0
            || (!SPTargetDelUI.IsTargetBullet && SPTargetDelUI.TargetIndex >= SimSprProjSimMenu.SpriteLength)
            || (SPTargetDelUI.IsTargetBullet && SPTargetDelUI.TargetIndex >= SimSprProjSimMenu.BulletLength))
            {
                return;
            }
            scriptChange = true;
            UpdateSpriteParameter();
            UpdatePositionParameter();
            scriptChange = false;
        }

        // --------------------------------------------------------------------------------

        private static bool SpriteExists(string value)
        {
            FileInfo fi = new FileInfo(FakeFileLoader.pathToModFile("Sprites/" + value + ".png"));
            if (!fi.Exists) fi = new FileInfo(FakeFileLoader.pathToDefaultFile("Sprites/" + value + ".png"));
            return fi.Exists;
        }

        private static bool CanFloatParse(string value) { float _; return float.TryParse(value, out _); }

        // --------------------------------------------------------------------------------

        private static CYFInputField Sprite_Set;
        private static Text Sprite_width;
        private static Text Sprite_height;

        private static void AwakeSprite(Transform parent)
        {
            Sprite_Set = parent.Find("Set").GetComponent<CYFInputField>();
            Sprite_width  = parent.Find("width") .Find("Text").GetComponent<Text>();
            Sprite_height = parent.Find("height").Find("Text").GetComponent<Text>();
        }

        private static void StartSprite()
        {
            CYFInputFieldUtil.AddListener_OnValueChanged(Sprite_Set, (value) =>
            {
                if (!scriptChange)
                {
                    CYFInputFieldUtil.ShowInputError(Sprite_Set, SpriteExists(value));
                    return;
                }

                Sprite_width.text = ((float)SimSprProjSimMenu.GetFromTarget(
                    (sprite) => { return sprite.width; },
                    (bullet) => { return bullet.sprite.width; }
                )).ToString();
                Sprite_height.text = ((float)SimSprProjSimMenu.GetFromTarget(
                    (sprite) => { return sprite.height; },
                    (bullet) => { return bullet.sprite.height; }
                )).ToString();

                CYFInputFieldUtil.ShowInputError(Sprite_Set, true);
            });
            CYFInputFieldUtil.AddListener_OnEndEdit(Sprite_Set, (value) =>
            {
                // if (scriptChange) return;

                if (!SpriteExists(value))
                {
                    scriptChange = true;
                    Sprite_Set.InputField.text = ((string)SimSprProjSimMenu.GetFromTarget(
                        (sprite) => { return sprite.spritename; },
                        (bullet) => { return bullet.sprite.spritename; }
                    )).ToString();
                    scriptChange = false;
                    return;
                }

                SimSprProjSimMenu.ActionToTarget((sprite) => { sprite.Set(value); }, (bullet) => { bullet.sprite.Set(value); });

                Sprite_width.text = ((float)SimSprProjSimMenu.GetFromTarget(
                    (sprite) => { return sprite.width; },
                    (bullet) => { return bullet.sprite.width; }
                )).ToString();
                Sprite_height.text = ((float)SimSprProjSimMenu.GetFromTarget(
                    (sprite) => { return sprite.height; },
                    (bullet) => { return bullet.sprite.height; }
                )).ToString();

                SPTargetDelUI.UpdateTargetDropDown();
            });
        }

        private static void UpdateSpriteParameter()
        {
            Sprite_Set.InputField.text = ((string)SimSprProjSimMenu.GetFromTarget(
                (sprite) => { return sprite.spritename; },
                (bullet) => { return bullet.sprite.spritename; }
            )).ToString();
        }

        // --------------------------------------------------------------------------------

        private static Text Position_xLabel;
        private static CYFInputField Position_x;
        private static Text Position_yLabel;
        private static CYFInputField Position_y;
        private static Toggle Position_Abs;

        private static void AwakePosition(Transform parent)
        {
            Position_xLabel = parent.Find("xLabel").GetComponent<Text>();
            Position_x      = parent.Find("x")     .GetComponent<CYFInputField>();
            Position_yLabel = parent.Find("yLabel").GetComponent<Text>();
            Position_y      = parent.Find("y")     .GetComponent<CYFInputField>();
            Position_Abs    = parent.Find("Abs")   .GetComponent<Toggle>();
        }

        private static void StartPosition()
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
            UnityToggleUtil.AddListener(Position_Abs, (value) =>
            {
                scriptChange = true;
                Position_xLabel.text = value ? "absx:" : "x:";
                Position_yLabel.text = value ? "absy:" : "y:";
                Position_x.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget(
                    (sprite) => { return value ? sprite.absx : sprite.x; },
                    (bullet) => { return value ? bullet.absx : bullet.x; }
                )).ToString();
                Position_y.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget(
                    (sprite) => { return value ? sprite.absy : sprite.y; },
                    (bullet) => { return value ? bullet.absy : bullet.y; }
                )).ToString();
                scriptChange = false;
            });
        }

        private static void UpdatePositionParameter()
        {
            Position_x.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget(
                (sprite) => { return Position_Abs.isOn ? sprite.absx : sprite.x; },
                (bullet) => { return Position_Abs.isOn ? bullet.absx : bullet.x; }
                )).ToString();
            Position_y.InputField.text = ((float)SimSprProjSimMenu.GetFromTarget(
                (sprite) => { return Position_Abs.isOn ? sprite.absy : sprite.y; },
                (bullet) => { return Position_Abs.isOn ? bullet.absy : bullet.y; }
                )).ToString();
        }
    }
}
